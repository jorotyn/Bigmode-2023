using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerCharacterController))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerScript : MonoBehaviour
{
    public PlayerCharacterController CharacterController;
    public PieAudioManager audioPieManager;
    public float JumpHeight = 3.5f;
    public float JumpTimeToApex = .4f;
    public float MoveSpeed = 6f;
    public float AccelerationTimeAir = .2f;
    public float AccelerationTimeGround = .1f;
    public AbilityEnum InitialAbility = AbilityEnum.None;

    private float _gravity;
    private float _jumpVelocity;
    private int _jumpCount;
    private const int MaxJumpCount = 2;
    private Vector3 _velocity;
    private float _velocity_x_smoothing;
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
        _jumpCount = 0;
        _playerAbilities.CurrentAbility = InitialAbility;
    }

    public void Update()
    {
        HandleCollisions();

        var input = InputManager.CurrentDirectionalInput();

        HandleWallJump(input);
        HandleRegularJump(input);

        float targetX = input.x * MoveSpeed;
        float accelerationTime = CharacterController.CurrentCollisions.Below ? AccelerationTimeGround : AccelerationTimeAir;
        _velocity.x = targetX == 0 ? 0 : Mathf.SmoothDamp(_velocity.x, targetX, ref _velocity_x_smoothing, accelerationTime);
        _velocity.y += _gravity * Time.deltaTime;
        CharacterController.Move(_velocity * Time.deltaTime);

        HandleSpriteFlip();
    }

    private void HandleCollisions()
    {
        if (CharacterController.CurrentCollisions.Above || CharacterController.CurrentCollisions.Below)
        {
            _velocity.y = 0;
            if (CharacterController.CurrentCollisions.Below)
            {
                _jumpCount = 0; // Reset jump count when player touches the ground
            }
        }
    }

    private void HandleWallJump(Vector2 input)
    {
        if ((CharacterController.CurrentCollisions.WallLeft || CharacterController.CurrentCollisions.WallRight) &&
            !CharacterController.CurrentCollisions.Below &&
            InputManager.JumpPressed() && _playerAbilities.CurrentAbility == AbilityEnum.WallJump)
        {
            _velocity.y = _jumpVelocity;
            _velocity.x = CharacterController.CurrentCollisions.WallLeft ? MoveSpeed : -MoveSpeed;
            _jumpCount = 1;
        }
    }

    private void HandleRegularJump(Vector2 input)
    {
        if (InputManager.JumpPressed() && _jumpCount < MaxJumpCount &&
            (_playerAbilities.CurrentAbility == AbilityEnum.DoubleJump || CharacterController.CurrentCollisions.Below))
        {
            _velocity.y = _jumpVelocity;
            _jumpCount++;
        }
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
        if (_velocity.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_velocity.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
    }
}
