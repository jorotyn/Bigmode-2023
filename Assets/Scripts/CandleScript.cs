using UnityEngine;

public class CandleScript : MonoBehaviour
{
    private PlayerCharacterController _characterController;
    private Vector3 _velocity = Vector3.zero;
    private float _velocity_x_smoothing;
    [SerializeField] private float MoveSpeed = 6f;

    private void Awake()
    {
        _characterController = GetComponent<PlayerCharacterController>();
    }

    // Update is called once per frame
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

}
