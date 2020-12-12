public class GameStateEntity : AbstractEntity
{
    protected override void Start()
    {
        base.Start();
        entity.AddGameState(GameState.NewRound);
        entity.isWaitingUserInput = true;
        entity.isTargetSwitch = true;
    }
}