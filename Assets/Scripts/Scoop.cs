using UnityEngine;

public class Scoop : MonoBehaviour
{
    public enum Position { bottom = 1, middle, top }

    private Rigidbody2D _rigidbody;
    private Position _position;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void AttachToPlayer(Transform playerTransform, Position position)
    {
        transform.parent = playerTransform;
        _position = position;
        _rigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (transform.parent != null)
        {
            var newPos = transform.parent.position;
            transform.position = new Vector2(newPos.x,
                                             newPos.y + (0.5f * (int)_position));
        }
    }
}