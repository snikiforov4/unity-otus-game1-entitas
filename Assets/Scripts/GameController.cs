using Entitas;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Systems _systems;

    void Awake()
    {
        var context = Contexts.sharedInstance;

        _systems = new Systems();
        _systems.Add(new PrefabInstantiateSystem(context));
        _systems.Add(new ViewDestroySystem(context));
        _systems.Add(new MainSystem(context));
        _systems.Add(new CharacterStateUpdateSystem(context));
        _systems.Add(new TargetPickerSystem(context));
        _systems.Add(new CharacterRoundMoveStarterSystem(context));
        _systems.Add(new HitTrackerSystem(context));
        _systems.Add(new DamageApplySystem(context));
        _systems.Add(new CharacterHealthIndicatorSystem(context));
        _systems.Add(new TargetSwitchSystem(context));
        _systems.Add(new CharacterTargetIndicatorSystem(context));
        _systems.Add(new DeathTrackerSystem(context));
        _systems.Add(new FinishGameTrackerSystem(context));
        _systems.Add(new TransformApplySystem(context));
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
}