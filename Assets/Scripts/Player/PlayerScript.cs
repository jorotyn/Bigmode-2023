using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerCharacterController))]
public class PlayerScript : MonoBehaviour
{
    public PlayerCharacterController CharacterController;
    public PieAudioManager audioPieManager;
    public float JumpHeight = 3.5f;
    public float JumpTimeToApex = .4f;
    public float MoveSpeed = 6f;
    public float AccelerationTimeAir = .2f;
    public float AccelerationTimeGround = .1f;

    private float _jumpVelocity = 8f;
    private float _gravity = -20;
    private int _jumpCount = 0;
    private const int MaxJumpCount = 2;

    private Vector3 _velocity;

    private float _velocity_x_smoothing;// don't touch this, it's for SmoothDamp to keep track of

    private bool _canTakeDamage = true;

    private SpriteRenderer _spriteRenderer;
    private PlayerAbilities _playerAbilities;

    public void Start()
    {
        CharacterController = GetComponent<PlayerCharacterController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerAbilities = GetComponent<PlayerAbilities>();

        _gravity = -(2 * JumpHeight) / Mathf.Pow(JumpTimeToApex, 2);
        _jumpVelocity = Mathf.Abs(_gravity) * JumpTimeToApex;
    }

    public void Update()
    {
        if (CharacterController.CurrentCollisions.Above || CharacterController.CurrentCollisions.Below)
        {
            _velocity.y = 0;
            if (CharacterController.CurrentCollisions.Below)
            {
                _jumpCount = 0; // Reset jump count when player touches the ground
            }
        }

        var input = InputManager.CurrentDirectionalInput();

        // Wall jump logic
        if ((CharacterController.CurrentCollisions.WallLeft || CharacterController.CurrentCollisions.WallRight) &&
            !CharacterController.CurrentCollisions.Below && // Ensure the player is not on the ground
            InputManager.JumpPressed() && _playerAbilities.CurrentAbility == AbilityEnum.WallJump)
        {
            _velocity.y = _jumpVelocity; // Wall jump velocity
            _velocity.x = CharacterController.CurrentCollisions.WallLeft ? MoveSpeed : -MoveSpeed; // Push off the wall
            _jumpCount = 1; // Reset the jump count to allow for another jump after wall jump
                            // Additional logic to ensure wall jump is correctly handled
        }

        // Jump logic
        else if (InputManager.JumpPressed() && _jumpCount < MaxJumpCount &&
                (_playerAbilities.CurrentAbility == AbilityEnum.DoubleJump || CharacterController.CurrentCollisions.Below))
        {
            // Check if the jump button is pressed and the player has not exceeded the maximum number of jumps
            _velocity.y = _jumpVelocity;
            _jumpCount++;
        }

        float targetX = input.x * MoveSpeed;
        float accelerationTime = CharacterController.CurrentCollisions.Below ? AccelerationTimeGround : AccelerationTimeAir;
        _velocity.x = targetX == 0 ? 0 : Mathf.SmoothDamp(_velocity.x, targetX, ref _velocity_x_smoothing, accelerationTime);
        _velocity.y += _gravity * Time.deltaTime;
        CharacterController.Move(_velocity * Time.deltaTime);

        HandleSpriteFlip();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))
        {
            HandleSpikeDamage();
        }
    }

    private void HandleSpikeDamage()
    {
        if (!_canTakeDamage) return;
        StartCoroutine(DisableDamage());
        audioPieManager.Hurt();
    }

    private IEnumerator DisableDamage(float seconds = 1.5f)
    {
        _canTakeDamage = false;
        StartCoroutine(ShowInvincibility());
        yield return new WaitForSeconds(seconds);
        _canTakeDamage = true;
    }

    private IEnumerator ShowInvincibility()
    {
        while (!_canTakeDamage)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
        }
        _spriteRenderer.enabled = true;
    }

    private void HandleSpriteFlip()
    {
        // Flip the player sprite based on movement direction
        if (_velocity.x > 0)
        {
            // Player is moving right, flip the sprite to face right
            _spriteRenderer.flipX = false;
        }
        else if (_velocity.x < 0)
        {
            // Player is moving left, flip the sprite to face left
            _spriteRenderer.flipX = true;
        }
    }
}
