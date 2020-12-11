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
    
    Character FirstAliveCharacter(Character[] characters)
    {
        foreach (var character in characters)
        {
            if (!character.IsDead())
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
                if (!player.IsDead())
                {
                    currentTarget = FirstAliveCharacter(enemyCharacter);
                    if (currentTarget == null)
                        break;

                    currentTarget.targetIndicator.gameObject.SetActive(true);
                    // Utility.SetCanvasGroupEnabled(gameControlsCanvasGroup, true);

                    waitingForInput = true;
                    while (waitingForInput)
                        yield return null;

                    // Utility.SetCanvasGroupEnabled(gameControlsCanvasGroup, false);
                    currentTarget.targetIndicator.gameObject.SetActive(false);

                    player.target = currentTarget.transform;
                    player.AttackEnemy();

                    while (!player.IsIdle())
                        yield return null;

                    break;
                }
            }

            foreach (var enemy in enemyCharacter)
            {
                if (!enemy.IsDead())
                {
                    Character target = FirstAliveCharacter(playerCharacter);
                    if (target == null)
                        break;

                    enemy.target = target.transform;
                    enemy.AttackEnemy();

                    while (!enemy.IsIdle())
                        yield return null;

                    break;
                }
            }
        }
    }

    void Start()
    {
        // StartCoroutine(GameLoop());
    }
}