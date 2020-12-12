using System;
using Entitas;
using UnityEngine;

public class CharacterUpdateSystem : IExecuteSystem
{
    private const float RunSpeed = 0.1f;
    private const float DistanceFromEnemy = 0.5f;

    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int MeleeAttack = Animator.StringToHash("MeleeAttack");
    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int Punch = Animator.StringToHash("Punch");
    private static readonly int Death = Animator.StringToHash("Death");

    private readonly IGroup<GameEntity> _allCharacters;

    public CharacterUpdateSystem(Contexts contexts)
    {
        _allCharacters = contexts.game.GetGroup(GameMatcher.Character);
    }

    public void Execute()
    {
        foreach (var character in _allCharacters)
        {
            UpdateCharacter(character);
        }
    }

    private void UpdateCharacter(GameEntity entity)
    {
        switch (CharacterUtils.GetCharacterState(entity))
        {
            case CharacterState.Idle:
                if (entity.hasTarget) entity.RemoveTarget();
                entity.ReplaceRotation(entity.originalRotation.value);
                GetAnimator(entity).SetFloat(Speed, 0.0f);
                break;

            case CharacterState.RunningToEnemy:
                GetAnimator(entity).SetFloat(Speed, RunSpeed);
                if (RunTowards(entity, entity.target.value.position.value, DistanceFromEnemy))
                {
                    switch (entity.character.weapon)
                    {
                        case Weapon.Bat:
                            UpdateState(entity, CharacterState.BeginAttack);
                            break;

                        case Weapon.Fist:
                            UpdateState(entity, CharacterState.BeginPunch);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException($"unexpected weapon={entity.character.weapon}");
                    }
                }

                break;

            case CharacterState.BeginAttack:
                GetAnimator(entity).SetTrigger(MeleeAttack);
                UpdateState(entity, CharacterState.Attack);
                break;

            case CharacterState.Attack:
                break;

            case CharacterState.BeginShoot:
                GetAnimator(entity).SetTrigger(Shoot);
                UpdateState(entity, CharacterState.Shoot);
                break;

            case CharacterState.Shoot:
                break;

            case CharacterState.BeginPunch:
                GetAnimator(entity).SetTrigger(Punch);
                UpdateState(entity, CharacterState.Punch);
                break;

            case CharacterState.Punch:
                break;

            case CharacterState.RunningFromEnemy:
                GetAnimator(entity).SetFloat(Speed, RunSpeed);
                if (RunTowards(entity, entity.originalPosition.value, 0.0f))
                    UpdateState(entity, CharacterState.Idle);
                break;

            case CharacterState.BeginDying:
                GetAnimator(entity).SetTrigger(Death);
                UpdateState(entity, CharacterState.Dead);
                break;

            case CharacterState.Dead:
                break;
        }
    }

    private static Animator GetAnimator(GameEntity entity)
    {
        return entity.view.gameObject.GetComponentInChildren<Animator>();
    }

    private void UpdateState(GameEntity entity, CharacterState state)
    {
        entity.ReplaceCharacterStateTransition(state);
    }

    private bool RunTowards(GameEntity character, Vector3 targetPosition, float distanceFromTarget)
    {
        Vector3 distance = targetPosition - character.position.value;
        if (distance.magnitude < 0.00001f)
        {
            character.ReplacePosition(targetPosition);
            return true;
        }

        Vector3 direction = distance.normalized;
        character.ReplaceRotation(Quaternion.LookRotation(direction));

        targetPosition -= direction * distanceFromTarget;
        distance = targetPosition - character.position.value;

        Vector3 step = direction * RunSpeed;
        if (step.magnitude < distance.magnitude)
        {
            character.ReplacePosition(character.position.value + step);
            return false;
        }

        character.ReplacePosition(targetPosition);
        return true;
    }
}