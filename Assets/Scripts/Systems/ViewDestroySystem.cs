using Entitas;
using UnityEngine;

public class ViewDestroySystem : IInitializeSystem, ITearDownSystem
{
    IGroup<GameEntity> group;

    public ViewDestroySystem(Contexts contexts)
    {
        group = contexts.game.GetGroup(GameMatcher.View);
    }

    public void Initialize()
    {
        group.OnEntityRemoved += OnViewRemoved;
    }

    public void TearDown()
    {
        group.OnEntityRemoved -= OnViewRemoved;
    }

    void OnViewRemoved(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        var view = (ViewComponent)component;
        Object.Destroy(view.gameObject);
    }
}
