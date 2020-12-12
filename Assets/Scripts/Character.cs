using UnityEngine;

public class Character : MonoBehaviour
{
    private const float RunSpeed = 0.1f;
    private const float DistanceFromEnemy = 0.5f;

    public Transform target;
    private Animator _animator;
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private GameEntity entity;

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
        entity = GetEntity();
    }

    public GameEntity GetEntity()
    {
        return GetComponent<EntitasEntity>().entity;
    }

    void FixedUpdate()
    {
        switch (entity.character.state)
        {
            case CharacterState.Idle:
                transform.rotation = _originalRotation;
                _animator.SetFloat("Speed", 0.0f);
                break;

            case CharacterState.RunningToEnemy:
                _animator.SetFloat("Speed", RunSpeed);
                if (RunTowards(target.position, DistanceFromEnemy))
                {
                    switch (entity.character.weapon)
                    {
                        case Weapon.Bat:
                            UpdateState(CharacterState.BeginAttack);
                            break;

                        case Weapon.Fist:
                            UpdateState(CharacterState.BeginPunch);
                            break;
                    }
                }

                break;

            case CharacterState.BeginAttack:
                _animator.SetTrigger("MeleeAttack");
                UpdateState(CharacterState.Attack);
                break;

            case CharacterState.Attack:
                break;

            case CharacterState.BeginShoot:
                _animator.SetTrigger("Shoot");
                UpdateState(CharacterState.Shoot);
                break;

            case CharacterState.Shoot:
                break;

            case CharacterState.BeginPunch:
                _animator.SetTrigger("Punch");
                UpdateState(CharacterState.Punch);
                break;

            case CharacterState.Punch:
                break;

            case CharacterState.RunningFromEnemy:
                _animator.SetFloat("Speed", RunSpeed);
                if (RunTowards(_originalPosition, 0.0f))
                    UpdateState(CharacterState.Idle);
                break;

            case CharacterState.BeginDying:
                _animator.SetTrigger("Death");
                UpdateState(CharacterState.Dead);
                break;

            case CharacterState.Dead:
                break;
        }
    }

    private void UpdateState(CharacterState state)
    {
        entity.AddCharacterStateTransition(state);
    }

    private bool RunTowards(Vector3 targetPosition, float distanceFromTarget)
    {
        Vector3 distance = targetPosition - transform.position;
        if (distance.magnitude < 0.00001f)
        {
            transform.position = targetPosition;
            return true;
        }

        Vector3 direction = distance.normalized;
        transform.rotation = Quaternion.LookRotation(direction);

        targetPosition -= direction * distanceFromTarget;
        distance = (targetPosition - transform.position);

        Vector3 step = direction * RunSpeed;
        if (step.magnitude < distance.magnitude)
        {
            transform.position += step;
            return false;
        }

        transform.position = targetPosition;
        return true;
    }
}