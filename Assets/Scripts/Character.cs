using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum Weapon
    {
        Pistol,
        Bat,
        Fist,
    }

    public Weapon weapon;
    public float runSpeed;
    public float distanceFromEnemy;
    public Transform target;
    public TargetIndicator targetIndicator;
    CharacterState _characterState;
    Animator animator;
    Vector3 originalPosition;
    Quaternion originalRotation;
    Health health;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<Health>();
        targetIndicator = GetComponentInChildren<TargetIndicator>(true);
        _characterState = CharacterState.Idle;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public bool IsIdle()
    {
        return _characterState == CharacterState.Idle;
    }

    public bool IsDead()
    {
        return _characterState == CharacterState.BeginDying || _characterState == CharacterState.Dead;
    }

    public void SetState(CharacterState newCharacterState)
    {
        if (IsDead())
            return;

        _characterState = newCharacterState;
    }

    public void DoDamage()
    {
        if (IsDead())
            return;

        health.ApplyDamage(1.0f); // FIXME: захардкожено
        if (health.current <= 0.0f)
            _characterState = CharacterState.BeginDying;
    }

    [ContextMenu("Attack")]
    public void AttackEnemy()
    {
        if (IsDead())
            return;

        Character targetCharacter = target.GetComponent<Character>();
        if (targetCharacter.IsDead())
            return;

        switch (weapon) {
            case Weapon.Bat:
                _characterState = CharacterState.RunningToEnemy;
                break;

            case Weapon.Fist:
                _characterState = CharacterState.RunningToEnemy;
                break;

            case Weapon.Pistol:
                _characterState = CharacterState.BeginShoot;
                break;
        }
    }

    bool RunTowards(Vector3 targetPosition, float distanceFromTarget)
    {
        Vector3 distance = targetPosition - transform.position;
        if (distance.magnitude < 0.00001f) {
            transform.position = targetPosition;
            return true;
        }

        Vector3 direction = distance.normalized;
        transform.rotation = Quaternion.LookRotation(direction);

        targetPosition -= direction * distanceFromTarget;
        distance = (targetPosition - transform.position);

        Vector3 step = direction * runSpeed;
        if (step.magnitude < distance.magnitude) {
            transform.position += step;
            return false;
        }

        transform.position = targetPosition;
        return true;
    }

    void FixedUpdate()
    {
        switch (_characterState) {
            case CharacterState.Idle:
                transform.rotation = originalRotation;
                animator.SetFloat("Speed", 0.0f);
                break;

            case CharacterState.RunningToEnemy:
                animator.SetFloat("Speed", runSpeed);
                if (RunTowards(target.position, distanceFromEnemy)) {
                    switch (weapon) {
                        case Weapon.Bat:
                            _characterState = CharacterState.BeginAttack;
                            break;

                        case Weapon.Fist:
                            _characterState = CharacterState.BeginPunch;
                            break;
                    }
                }
                break;

            case CharacterState.BeginAttack:
                animator.SetTrigger("MeleeAttack");
                _characterState = CharacterState.Attack;
                break;

            case CharacterState.Attack:
                break;

            case CharacterState.BeginShoot:
                animator.SetTrigger("Shoot");
                _characterState = CharacterState.Shoot;
                break;

            case CharacterState.Shoot:
                break;

            case CharacterState.BeginPunch:
                animator.SetTrigger("Punch");
                _characterState = CharacterState.Punch;
                break;

            case CharacterState.Punch:
                break;

            case CharacterState.RunningFromEnemy:
                animator.SetFloat("Speed", runSpeed);
                if (RunTowards(originalPosition, 0.0f))
                    _characterState = CharacterState.Idle;
                break;

            case CharacterState.BeginDying:
                animator.SetTrigger("Death");
                _characterState = CharacterState.Dead;
                break;

            case CharacterState.Dead:
                break;
        }
    }
}