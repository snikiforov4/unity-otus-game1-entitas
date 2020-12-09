using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEvents : MonoBehaviour
{
    Character character;

    void Start()
    {
        character = GetComponentInParent<Character>();
    }

    void ShootEnd()
    {
        character.SetState(CharacterState.Idle);
    }

    void AttackEnd()
    {
        character.SetState(CharacterState.RunningFromEnemy);
    }

    void PunchEnd()
    {
        character.SetState(CharacterState.RunningFromEnemy);
    }

    void DoDamage()
    {
        Character targetCharacter = character.target.GetComponent<Character>();
        targetCharacter.DoDamage();
    }
}
