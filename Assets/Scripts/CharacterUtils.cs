public static class CharacterUtils
{
    public static CharacterState GetCharacterState(GameEntity character)
    {
        return character.hasCharacterStateTransition
            ? character.characterStateTransition.value
            : character.character.state;
    }

    public static bool IsNotDead(GameEntity character)
    {
        return !IsDead(character);
    }

    public static bool IsDead(GameEntity character)
    {
        var characterState = GetCharacterState(character);
        return characterState == CharacterState.Dead || characterState == CharacterState.BeginDying;
    }

    public static bool IsIdle(GameEntity character)
    {
        return GetCharacterState(character) == CharacterState.Idle;
    }
}