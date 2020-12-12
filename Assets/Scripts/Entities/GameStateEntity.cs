public class GameStateEntity : AbstractEntity
{

    protected override void Start()
    {
        base.Start();
        entity.isGameState = true;
        entity.isWaitingUserInput = true;
        entity.isTargetSwitch = true;
    }
}