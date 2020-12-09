public class CharacterUtils
{
    public static CharacterState GetCharacterState(GameEntity character)
    {
        return character.hasCharacterStateTransition
            ? character.characterStateTransition.value
            : character.character.state;
    }
}