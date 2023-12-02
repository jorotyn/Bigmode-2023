using UnityEngine;

[RequireComponent(typeof(PlayerCharacterController))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerScript : MonoBehaviour
{
    #region Serialized Fields
    [Header("Abilities")]
    [SerializeField] private AbilityEnum InitialAbility = AbilityEnum.None;

    [Header("Movement")]
    [SerializeField] private float MoveSpeed = 6f;
    [SerializeField] private float JumpHeight = 3.5f;
    [SerializeField] private float JumpTimeToApex = .4f;
    [SerializeField] private float AccelerationTimeAir = .2f;
    [SerializeField] private float AccelerationTimeGround = .1f;

    [Header("Wall Sliding")]
    [SerializeField] private float WallSlideSpeedMax = 3f;
    [SerializeField] private bool LimitWallJumps = true;
    [SerializeField] private bool RequireWallJumpAbilityForWallSlide = true;
    #endregion

    #region Private Fields
    private float _gravity;
    private float _jumpVelocity;
    public int _jumpCount;
    private const int MaxJumpCount = 2;
    private int _wallJumpCount;
    private const int MaxWallJumpCount = 1;
    private Vector3 _velocity;
    private float _velocity_x_smoothing;
    private SpriteRenderer _spriteRenderer;
    private PlayerAbilities _playerAbilities;
    private PlayerCharacterController _characterController;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        _characterController = GetComponent<PlayerCharacterController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerAbilities = GetComponent<PlayerAbilities>();

        _gravity = -(2 * JumpHeight) / Mathf.Pow(JumpTimeToApex, 2);
        _jumpVelocity = Mathf.Abs(_gravity) * JumpTimeToApex;
        _jumpCount = 0;
        _playerAbilities.CurrentAbility = InitialAbility;
    }

    private void Update()
    {
        HandleCollisions();

        var input = InputManager.CurrentDirectionalInput();

        HandleWallJump(input);
        HandleRegularJump(input);
        HandleMovement(input);
        HandleSpriteFlip();
        HandleWallSlide();
    }
    #endregion

    #region Private Methods
    private void HandleCollisions()
    {
        if (_characterController.CurrentCollisions.Above || _characterController.CurrentCollisions.Below)
        {
            _velocity.y = 0;
            if (_characterController.CurrentCollisions.Below)
            {
                _jumpCount = 0;
                _wallJumpCount = 0;
            }
        }
    }

    private void HandleWallJump(Vector2 input)
    {
        if ((_characterController.CurrentCollisions.WallLeft || _characterController.CurrentCollisions.WallRight) &&
            !_characterController.CurrentCollisions.Below &&
            InputManager.JumpPressed() &&
            _playerAbilities.CurrentAbility == AbilityEnum.WallJump &&
            (!LimitWallJumps || _wallJumpCount < MaxWallJumpCount))
        {
            _velocity.y = _jumpVelocity;
            _velocity.x = _characterController.CurrentCollisions.WallLeft ? MoveSpeed : -MoveSpeed;
            if (LimitWallJumps)
            {
                _wallJumpCount++;
            }
        }
    }

    private void HandleRegularJump(Vector2 input)
    {
        if (InputManager.JumpPressed() && _jumpCount < MaxJumpCount &&
            (_playerAbilities.CurrentAbility == AbilityEnum.DoubleJump || _characterController.CurrentCollisions.Below))
        {
            _velocity.y = _jumpVelocity;
            _jumpCount++;
        }
    }

    private void HandleMovement(Vector2 input)
    {
        float targetX = input.x * MoveSpeed;
        float accelerationTime = _characterController.CurrentCollisions.Below ? AccelerationTimeGround : AccelerationTimeAir;
        _velocity.x = targetX == 0 ? 0 : Mathf.SmoothDamp(_velocity.x, targetX, ref _velocity_x_smoothing, accelerationTime);
        _velocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void HandleSpriteFlip()
    {
        if (_velocity.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_velocity.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
    }

    private void HandleWallSlide()
    {
        if ((_characterController.CurrentCollisions.WallLeft || _characterController.CurrentCollisions.WallRight) &&
            !_characterController.CurrentCollisions.Below &&
            (!RequireWallJumpAbilityForWallSlide || _playerAbilities.CurrentAbility == AbilityEnum.WallJump))
        {
            if (_velocity.y < -WallSlideSpeedMax)
            {
                _velocity.y = -WallSlideSpeedMax; // Apply wall slide speed
            }
        }
    }
    #endregion
}
