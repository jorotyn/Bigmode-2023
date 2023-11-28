using UnityEngine;

public class PlayerCharacterController : RaycastMovementController
{
    public int MaxClimbAngle = 70;// in degrees
    public int MaxDescendAngle = 75;// in degrees

    public CollisionInfo CurrentCollisions;

    public void Move(Vector2 velocity, bool standingOnPlatform = false)
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

        if (standingOnPlatform)
        {
            CurrentCollisions.Below = true;
        }
    }

    private Vector2 VerticalCollisions(Vector2 velocity)
    {
        var directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + SkinWidth;
        for (int i = 0; i < VerticalRayCount; i++)
        {
            var rayOrigin = directionY == -1 ? CastOrigins.BottomLeft : CastOrigins.TopLeft;
            rayOrigin += Vector2.right * (VerticalRaySpacing * i + velocity.x);

            var hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, CollisionMask);
            if (hit)
            {
                velocity.y = (hit.distance - SkinWidth) * directionY;
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
            rayLength = Mathf.Abs(velocity.x) + SkinWidth;
            var rayOrigin = (directionX == Vector2.left.x ? CastOrigins.BottomLeft : CastOrigins.BottomRight) + Vector2.up * velocity.y;
            var hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);
            if (hit)
            {
                var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != CurrentCollisions.SlopeAngle)
                {
                    velocity.x = (hit.distance - SkinWidth) * directionX;
                    CurrentCollisions.SlopeAngle = slopeAngle;
                }
            }
        }

        return velocity;
    }

    private Vector2 HorizontalCollisions(Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + SkinWidth;

        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? CastOrigins.BottomLeft : CastOrigins.BottomRight;
            rayOrigin += Vector2.up * (HorizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);

            if (hit)
            {
                if (hit.distance == 0)
                {
                    continue;
                }

                var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (i == 0 && slopeAngle <= MaxClimbAngle)
                {
                    var distanceToSlopeStart = 0f;
                    if (slopeAngle != CurrentCollisions.SlopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - SkinWidth;
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

                    velocity.x = (hit.distance - SkinWidth) * directionX;
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

    private Vector2 ClimbSlope(Vector2 velocity, float slopeAngle)
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

    private Vector2 DescendSlope(Vector2 velocity)
    {
        var directionX = Mathf.Sign(velocity.x);
        var rayOrigin = directionX == Vector2.left.x ? CastOrigins.BottomRight : CastOrigins.BottomLeft;
        var hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, CollisionMask);

        if (hit)
        {
            var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= MaxDescendAngle &&
                Mathf.Sign(hit.normal.x) == directionX &&
                hit.distance - SkinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
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

    public struct CollisionInfo
    {
        public bool Above, Below, Left, Right;
        public bool ClimbingSlope, DescendingSlope;
        public float SlopeAngle, SlopeAngleOld;

        public Vector2 VelocityOld;

        public void Reset()
        {
            Above = Below = Left = Right = false;
            ClimbingSlope = DescendingSlope = false;
            SlopeAngleOld = SlopeAngle;
            SlopeAngle = 0;
        }
    }
}

