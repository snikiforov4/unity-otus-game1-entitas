using UnityEngine;

public class CharacterEntity : AbstractEntity
{
    public GameObject characterPrefab;
    public CharacterType characterType;
    public float health;

    protected override void Start()
    {
        base.Start();
        entity.AddCharacter(characterType, CharacterState.Idle);
        entity.AddPrefab(characterPrefab);
        entity.AddHealth(health);
    }
}
