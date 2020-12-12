using UnityEngine;

public class CharacterAnimationEvents : MonoBehaviour
{
    private GameEntity _entity;

    void Start()
    {
        _entity = GetComponent<EntitasEntity>().entity;
    }

    void ShootEnd()
    {
        _entity.AddCharacterStateTransition(CharacterState.Idle);
    }

    void AttackEnd()
    {
        _entity.AddCharacterStateTransition(CharacterState.RunningFromEnemy);
    }

    void Damage()
    {
        _entity.isCharacterHit = true;
    }
}