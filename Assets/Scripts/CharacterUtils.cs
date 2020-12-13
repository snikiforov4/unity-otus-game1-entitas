using System.Collections.Generic;
using System.Linq;

public static class CharacterUtils
{
    public static CharacterState GetCharacterState(GameEntity character)
    {
        return character.characterState.value;
    }

    public static bool IsNotDead(GameEntity character)
    {
        return !IsDead(character);
    }

    public static bool IsDead(GameEntity character)
    {
        var characterState = character.characterState.value;
        return characterState == CharacterState.Dead || characterState == CharacterState.BeginDying;
    }

    public static bool IsIdle(GameEntity character)
    {
        return character.characterState.value == CharacterState.Idle;
    }
    
    public static GameEntity FirstAliveCharacter(IEnumerable<GameEntity> characters)
    {
        return characters.FirstOrDefault(IsNotDead);
    }
    
    public static List<GameEntity> FindAll(IEnumerable<GameEntity> characters, CharacterType type)
    {
        return characters.Where(e => e.character.type == type).ToList();
    }
 
    public static List<GameEntity> FindBadGuys(IEnumerable<GameEntity> characters)
    {
        return characters.AsEnumerable().Where(e => e.character.type == CharacterType.BadGuy).ToList();
    }
 
    public static List<GameEntity> FindGoodGuys(IEnumerable<GameEntity> characters)
    {
        return characters.AsEnumerable().Where(e => e.character.type == CharacterType.GoodGuy).ToList();
    }

    public static CharacterType GetOppositeCharacterType(CharacterType type)
    {
        return type == CharacterType.BadGuy 
            ? CharacterType.GoodGuy 
            : CharacterType.BadGuy; 
    }
}