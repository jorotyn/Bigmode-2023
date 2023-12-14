using System.Collections;
using UnityEngine;

public class CandleScript : MonoBehaviour
{
    private PlayerCharacterController _characterController;
    private Animator _animator;
    private Vector3 _velocity = Vector3.zero;
    private float _velocity_x_smoothing;

    [SerializeField] private float MoveSpeed = 6f;
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private float ProjectileVelocity = 1f;

    private void Awake()
    {
        _characterController = GetComponent<PlayerCharacterController>();
        _animator = GetComponent<Animator>();
        StartCoroutine(WindUp());
    }

    IEnumerator WindUp()
    {
        yield return new WaitForSeconds(3);
        _animator.SetBool("WindingUp", true);
    }

    void Update()
    {
        HandleMovement((Vector2)_velocity);
    }

    private void HandleMovement(Vector2 input)
    {
        float targetX = input.x * MoveSpeed;
        float accelerationTime = .1f;
        _velocity.x = targetX == 0 ? 0 : Mathf.SmoothDamp(_velocity.x, targetX, ref _velocity_x_smoothing, accelerationTime);
        _velocity.y += -2 * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    public void Fire()
    {
        DoCircleAttack();
    }

    private void DoCircleAttack()
    {
        const int radius = 2;

        var numberOfProjectiles = 8;

        var startPoint = (Vector2)transform.position;
        float angleStep = 360f / numberOfProjectiles;
        float angle = 0f;

        for (int i = 0; i <= numberOfProjectiles - 1; i++)
        {
            var projectileDirXPosition = startPoint.x + Mathf.Cos((angle * Mathf.PI) / 180) * radius;
            var projectileDirYPosition = startPoint.y + Mathf.Sin((angle * Mathf.PI) / 180) * radius;

            var projectileVector = new Vector2(projectileDirXPosition, projectileDirYPosition);
            var projectileMoveDirection = (projectileVector - startPoint).normalized * ProjectileVelocity;

            var tmpObj = Instantiate(ProjectilePrefab, startPoint, Quaternion.identity);
            tmpObj.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);

            angle += angleStep;
        }
    }
}
