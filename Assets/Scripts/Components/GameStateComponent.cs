using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique]
public class GameStateComponent : IComponent
{
    public GameState state;
}