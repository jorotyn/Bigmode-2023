using UnityEngine;

public class Spike : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Fire(Vector2 direction)
    {
        _rigidBody.velocity = direction;
    }
}
