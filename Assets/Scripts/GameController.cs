using System.Collections;
using Entitas;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Character[] playerCharacter;
    public Character[] enemyCharacter;
    Character currentTarget;
    bool waitingForInput;

    private Systems _systems;

    void Awake()
    {
        var context = Contexts.sharedInstance;

        _systems = new Systems();
        _systems.Add(new PrefabInstantiateSystem(context));
        _systems.Add(new TransformApplySystem(context));
        _systems.Add(new HitTrackerSystem(context));
        _systems.Add(new DamageApplySystem(context));
        _systems.Add(new CharacterHealthIndicatorSystem(context));
        _systems.Add(new TargetSwitchSystem(context));
        _systems.Add(new CharacterTargetIndicatorSystem(context));
        _systems.Add(new DeathTrackerSystem(context));
        _systems.Add(new FinishGameTrackerSystem(context));
        _systems.Add(new CharacterNewStateApplySystem(context));
        _systems.Initialize();
    }

    void OnDestroy()
    {
        _systems.TearDown();
    }

    void Update()
    {
        _systems.Execute();
        _systems.Cleanup();
    }

    private Character FirstAliveCharacter(Character[] characters)
    {
        foreach (var character in characters)
        {
            if (!CharacterUtils.IsDead(character.GetEntity()))
                return character;
        }

        return null;
    }

    IEnumerator GameLoop()
    {
        yield return null;
        // while (!CheckEndGame())
        while (true)
        {
            foreach (var player in playerCharacter)
            {
                if (!CharacterUtils.IsDead(player.GetEntity()))
                {
                    currentTarget = FirstAliveCharacter(enemyCharacter);
                    if (currentTarget == null)
                        break;

                    // currentTarget.targetIndicator.gameObject.SetActive(true);
                    // Utility.SetCanvasGroupEnabled(gameControlsCanvasGroup, true);

                    waitingForInput = true;
                    while (waitingForInput)
                        yield return null;

                    // Utility.SetCanvasGroupEnabled(gameControlsCanvasGroup, false);
                    // currentTarget.targetIndicator.gameObject.SetActive(false);

                    player.target = currentTarget.transform;
                    AttackEnemy(player);

                    while (!CharacterUtils.IsIdle(player.GetEntity()))
                        yield return null;

                    break;
                }
            }

            foreach (var enemy in enemyCharacter)
            {
                if (!CharacterUtils.IsDead(enemy.GetEntity()))
                {
                    Character target = FirstAliveCharacter(playerCharacter);
                    if (target == null)
                        break;

                    enemy.target = target.transform;
                    AttackEnemy(enemy);

                    while (!CharacterUtils.IsIdle(enemy.GetEntity()))
                        yield return null;

                    break;
                }
            }
        }
    }

    private void AttackEnemy(Character character)
    {
        var entity = character.GetEntity();
        if (CharacterUtils.IsDead(entity))
            return;

        if (CharacterUtils.IsDead(GetTarget()))
            return;

        switch (entity.character.weapon)
        {
            case Weapon.Bat:
            case Weapon.Fist:
                entity.AddCharacterStateTransition(CharacterState.RunningToEnemy);
                break;

            case Weapon.Pistol:
                entity.AddCharacterStateTransition(CharacterState.BeginShoot);
                break;
        }
    }
    
    private GameEntity GetTarget()
    {
        // todo implement me
        return null;
    }
    
}