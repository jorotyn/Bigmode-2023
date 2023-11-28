using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerCharacterController : MonoBehaviour
{
    public int HorizontalRayCount = 4;
    public int VerticalRayCount = 4;
    public LayerMask CollisionMask;

    private float horizontalRaySpacing, verticalRaySpacing;

    private const float skinWidth = 0.015f;

    private BoxCollider2D _collider;
    private RaycastOrigins _raycastOrigins;

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        if(velocity.x != 0)
        {
            velocity = HorizontalCollisions(velocity);
	    }
        if (velocity.y != 0)
        {
            velocity = VerticalCollisions(velocity);
        }
        transform.Translate(velocity);
    }

    private Vector3 VerticalCollisions(Vector3 velocity)
    {
        var directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        for (int i = 0; i < VerticalRayCount; i++)
        {
            var rayOrigin = directionY == -1 ? _raycastOrigins.BottomLeft : _raycastOrigins.TopLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

            Debug.DrawRay(_raycastOrigins.BottomLeft + Vector2.right * verticalRaySpacing * i, Vector2.up * -2, Color.red);

            var hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, CollisionMask);
            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
            }
        }
        return velocity;
    }

    private Vector3 HorizontalCollisions(Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? _raycastOrigins.BottomLeft : _raycastOrigins.BottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;
            }
        }
        return velocity;
    }

    private void UpdateRaycastOrigins()
    {
        var bounds = _collider.bounds;
        bounds.Expand(skinWidth * -2);

        _raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        _raycastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
        _raycastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
        _raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    private void CalculateRaySpacing()
    {
        var bounds = _collider.bounds;
        bounds.Expand(skinWidth * -2);

        HorizontalRayCount = Mathf.Max(2, HorizontalRayCount);
        VerticalRayCount = Mathf.Max(2, VerticalRayCount);

        horizontalRaySpacing = bounds.size.y / (HorizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (VerticalRayCount - 1);
    }

    struct RaycastOrigins
    {
        public Vector2 TopLeft, TopRight, BottomLeft, BottomRight;
    }
}

