using System.Collections.Generic;
using Entitas;

 public class DeathSystem : IExecuteSystem
{
    IGroup<GameEntity> entities;
    List<Entity> deadEntities = new List<Entity>();

    public DeathSystem(Contexts contexts)
    {
        entities = contexts.game.GetGroup(GameMatcher.Health);
    }

    public void Execute()
    {
        deadEntities.Clear();
        foreach (var e in entities) {
            if (e.health.value <= 0)
                deadEntities.Add(e);
        }

        foreach (var e in deadEntities)
            e.Destroy();
    }
}
