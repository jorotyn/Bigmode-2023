using static PieConstants;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Layers.Death ||
	        collision.CompareTag(Tags.Player))
        {
            Destroy(gameObject);
        }
    }

    public void Fire(Vector2 direction)
    {
        _rigidBody.velocity = direction;
    }
}
