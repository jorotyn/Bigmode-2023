using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerCharacterController : MonoBehaviour
{
    public int HorizontalRayCount = 4;
    public int VerticalRayCount = 4;
    public int MaxClimbAngle = 70;// in degrees
    public int MaxDescendAngle = 75;// in degrees
    public LayerMask CollisionMask;

    public CollisionInfo CurrentCollisions;

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
        CurrentCollisions.Reset();
        CurrentCollisions.VelocityOld = velocity;
        if (velocity.y < 0)
        {
            velocity = DescendSlope(velocity);
        }
        if (velocity.x != 0)
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

            var hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, CollisionMask);
            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if (CurrentCollisions.ClimbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(CurrentCollisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                CurrentCollisions.Above = directionY == Vector2.up.y;
                CurrentCollisions.Below = directionY == Vector2.down.y;
            }
        }

        if (CurrentCollisions.ClimbingSlope)
        {
            var directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            var rayOrigin = (directionX == Vector2.left.x ? _raycastOrigins.BottomLeft : _raycastOrigins.BottomRight) + Vector2.up * velocity.y;
            var hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);
            if (hit)
            {
                var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != CurrentCollisions.SlopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    CurrentCollisions.SlopeAngle = slopeAngle;
                }
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

            if (hit)
            {
                var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (i == 0 && slopeAngle <= MaxClimbAngle)
                {
                    var distanceToSlopeStart = 0f;
                    if (slopeAngle != CurrentCollisions.SlopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    velocity = ClimbSlope(velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                if (!CurrentCollisions.ClimbingSlope || slopeAngle > MaxClimbAngle)
                {
                    if (CurrentCollisions.DescendingSlope)
                    {
                        CurrentCollisions.DescendingSlope = false;
                        velocity = CurrentCollisions.VelocityOld;
                    }

                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (CurrentCollisions.ClimbingSlope)
                    {
                        velocity.y = Mathf.Tan(CurrentCollisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    CurrentCollisions.Left = directionX == Vector2.left.x;
                    CurrentCollisions.Right = directionX == Vector2.right.x;
                }
            }
        }

        return velocity;
    }

    private Vector3 ClimbSlope(Vector3 velocity, float slopeAngle)
    {
        var moveDistance = Mathf.Abs(velocity.x);
        var climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            CurrentCollisions.Below = true;
            CurrentCollisions.ClimbingSlope = true;
            CurrentCollisions.SlopeAngle = slopeAngle;
        }
        return velocity;
    }

    private Vector3 DescendSlope(Vector3 velocity)
    {
        var directionX = Mathf.Sign(velocity.x);
        var rayOrigin = directionX == Vector2.left.x ? _raycastOrigins.BottomRight : _raycastOrigins.BottomLeft;
        var hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, CollisionMask);

        if (hit)
        {
            var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= MaxDescendAngle &&
                Mathf.Sign(hit.normal.x) == directionX &&
                hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
            {
                var moveDistance = Mathf.Abs(velocity.x);
                var descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                velocity.y -= descendVelocityY;

                CurrentCollisions.SlopeAngle = slopeAngle;
                CurrentCollisions.DescendingSlope = true;
                CurrentCollisions.Below = true;
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

    public struct CollisionInfo
    {
        public bool Above, Below, Left, Right;
        public bool ClimbingSlope, DescendingSlope;
        public float SlopeAngle, SlopeAngleOld;

        public Vector3 VelocityOld;

        public void Reset()
        {
            Above = Below = Left = Right = false;
            ClimbingSlope = DescendingSlope = false;
            SlopeAngleOld = SlopeAngle;
            SlopeAngle = 0;
        }
    }
}

