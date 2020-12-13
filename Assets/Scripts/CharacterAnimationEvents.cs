using UnityEngine;

public class CharacterAnimationEvents : MonoBehaviour
{
    private GameEntity _entity;

    void Start()
    {
        _entity = GetComponentInParent<EntitasEntity>()?.entity;
    }

    void ShootEnd()
    {
        _entity.ReplaceCharacterState(CharacterState.ShootEnd);
    }

    void AttackEnd()
    {
        _entity.ReplaceCharacterState(CharacterState.RunningFromEnemy);
    }

    void Damage()
    {
        _entity.isCharacterHit = true;
    }
}