using UnityEngine;

public class Scoop : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    private SpriteRenderer _parentRenderer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void AttachToPlayer(Transform playerTransform)
    {
        transform.parent = playerTransform;
        _rigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (transform.parent != null)
        {
            var newPos = transform.parent.position;
            transform.position = new Vector2(newPos.x,
                                             newPos.y - 0.15f);
            _animator.SetBool("IsFalling", false);

            if(_parentRenderer != null)
            {
                _spriteRenderer.flipX = _parentRenderer.flipX;
	        }
        }
        else
        { 
            _animator.SetBool("IsFalling", true);
	    }
    }

    private void OnTransformParentChanged()
    {
        if (transform.parent != null)
        {
            _parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
        }
        else
	    {
            _parentRenderer = null;
        }
    }
}