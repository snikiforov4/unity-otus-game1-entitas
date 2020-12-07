using System.Collections.Generic;
using Entitas;

public class PrefabInstantiateSystem : ReactiveSystem<GameEntity>
{
    private Contexts _contexts;

    public PrefabInstantiateSystem(Contexts contexts)
        : base(contexts.game)
    {
        this._contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Health);
        // return context.CreateCollector(GameMatcher.Prefab);
    }

    protected override bool Filter(GameEntity entity)
    {
        return false;
        // return entity.hasPrefab && !entity.hasView;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities) {
            // var obj = GameObject.Instantiate(e.prefab.prefab);
            // if (obj.TryGetComponent<EntitasEntity>(out var ee))
            //     ee.entity = e;
            // else
            //     obj.AddComponent<EntitasEntity>().entity = e;
            // e.AddView(obj);
        }
    }
}
