using UnityEngine;

public class CharacterEntity : AbstractEntity
{
    public GameObject characterPrefab;
    public float health;

    protected override void Start()
    {
        base.Start();
        entity.isCharacter = true;
        entity.AddPrefab(characterPrefab);
        entity.AddHealth(health);
    }
}
