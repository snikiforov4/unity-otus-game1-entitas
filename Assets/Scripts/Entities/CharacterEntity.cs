using UnityEngine;

public class CharacterEntity : AbstractEntity
{
    public GameObject characterPrefab;
    public CharacterType characterType;
    public Weapon weapon;
    public float health;

    protected override void Start()
    {
        base.Start();
        entity.AddPosition(transform.position);
        entity.AddRotation(transform.rotation);
        entity.AddCharacter(characterType, CharacterState.Idle, weapon);
        entity.AddPrefab(characterPrefab);
        entity.AddHealth(health);
        entity.isCurrentTarget = false;
    }
}
