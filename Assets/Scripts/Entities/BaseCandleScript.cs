using System.Collections;
using UnityEngine;

public abstract class BaseCandleScript : MonoBehaviour
{
    private PlayerCharacterController _characterController;
    private Animator _animator;
    private Vector3 _velocity = Vector3.zero;
    private float _velocity_x_smoothing;

    [SerializeReference]
    public float MoveSpeed = 6f;

    [SerializeReference]
    public GameObject ProjectilePrefab;

    [SerializeReference]
    public float ProjectileVelocity = 1f;

    private void Awake()
    {
        _characterController = GetComponent<PlayerCharacterController>();
        _animator = GetComponent<Animator>();

        StartCoroutine(WindUp());
    }

    private IEnumerator WindUp()
    {
        while (true)
        {
            _animator.SetBool("WindingUp", true);
            yield return new WaitForSeconds(Random.Range(3, 6));
        }
    }

    public void Move(Vector2 input)
    {
        float targetX = input.x * MoveSpeed;
        float accelerationTime = .1f;
        _velocity.x = targetX == 0 ? 0 : Mathf.SmoothDamp(_velocity.x, targetX, ref _velocity_x_smoothing, accelerationTime);
        _velocity.y += -2 * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    public abstract void Fire();
}
