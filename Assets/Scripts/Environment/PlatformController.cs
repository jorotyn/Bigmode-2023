using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PlatformController : RaycastMovementController
{
    public LayerMask PassengerMask;
    public Vector2 Move;

    private List<PassengerMovement> _passengerMovements;
    private Dictionary<int, PlayerCharacterController> _passengerDictionary = new();

    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        UpdateRaycastOrigins();
        var velocity = Move * Time.deltaTime;

        MovePassengers(true);
        CalculatePassengerMovement(velocity);
        MovePassengers(false);
        transform.Translate(velocity);
    }

    private void MovePassengers(bool beforeMovePlatform)
    {
        foreach (var pm in _passengerMovements.Where(i => i.MoveBeforePlatform == beforeMovePlatform))
        {
            var transformId = pm.Transform.GetInstanceID();
            if (!_passengerDictionary.ContainsKey(transformId))
            {
                _passengerDictionary.Add(transformId, pm.Transform.GetComponent<PlayerCharacterController>());
            }
            _passengerDictionary[transformId].Move(pm.Velocity, pm.StandingOnPlatform);
        }
    }

    private void CalculatePassengerMovement(Vector2 velocity)
    {
        var movedPassengers = new HashSet<int>();
        _passengerMovements = new();

        var directionX = Mathf.Sign(velocity.x);
        var directionY = Mathf.Sign(velocity.y);

        // vertically moving platform
        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + SkinWidth;
            for (int i = 0; i < VerticalRayCount; i++)
            {
                var rayOrigin = directionY == -1 ? CastOrigins.BottomLeft : CastOrigins.TopLeft;
                rayOrigin += Vector2.right * (VerticalRaySpacing * i);

                var hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, PassengerMask);
                if (hit)
                {
                    var transformId = hit.transform.GetInstanceID();
                    if (!movedPassengers.Contains(transformId))
                    {
                        movedPassengers.Add(transformId);
                        var pushX = directionY == Vector2.up.y ? velocity.x : 0;
                        var pushY = velocity.y - (hit.distance - SkinWidth) * directionY;

                        _passengerMovements.Add(new PassengerMovement(
                            hit.transform,
                            new Vector2(pushX, pushY),
                            directionY == Vector2.up.y,
                            true
                        ));
                    }
                }
            }
        }

        // horizontal moving platform
        if (velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + SkinWidth;
            for (int i = 0; i < HorizontalRayCount; i++)
            {
                var rayOrigin = directionX == Vector2.left.x ? CastOrigins.BottomLeft : CastOrigins.BottomRight;
                rayOrigin += Vector2.up * (HorizontalRaySpacing * i);

                var hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, PassengerMask);
                if (hit)
                {
                    var transformId = hit.transform.GetInstanceID();
                    if (!movedPassengers.Contains(transformId))
                    {
                        movedPassengers.Add(transformId);
                        var pushX = velocity.x - (hit.distance - SkinWidth) * directionX;
                        var pushY = -SkinWidth;

                        _passengerMovements.Add(new PassengerMovement(
                            hit.transform,
                            new Vector2(pushX, pushY),
                            false,
                            true
                        ));
                    }
                }
            }
        }

        // passenger is on top of horizontally moving platform
        // OR a downward moving platform
        if (directionY == Vector2.down.y || (velocity.y == 0 && velocity.x != 0))
        {
            float rayLength = SkinWidth * 2;
            for (int i = 0; i < VerticalRayCount; i++)
            {
                var rayOrigin = CastOrigins.TopLeft + Vector2.right * (VerticalRaySpacing * i);

                var hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, PassengerMask);
                if (hit)
                {
                    var transformId = hit.transform.GetInstanceID();
                    if (!movedPassengers.Contains(transformId))
                    {
                        movedPassengers.Add(transformId);
                        var pushX = directionY == Vector2.up.y ? velocity.x : 0;
                        var pushY = velocity.y - (hit.distance - SkinWidth) * directionY;

                        _passengerMovements.Add(new PassengerMovement(
                            hit.transform,
                            new Vector2(pushX, pushY),
                            true,
                            false
                        ));
                    }
                }
            }
        }
    }

    struct PassengerMovement
    {
        public Transform Transform;
        public Vector2 Velocity;
        public bool StandingOnPlatform;
        public bool MoveBeforePlatform;

        public PassengerMovement(Transform transform, Vector2 velocity, bool standingOnPlatform, bool moveBeforePlatform)
        {
            Transform = transform;
            Velocity = velocity;
            StandingOnPlatform = standingOnPlatform;
            MoveBeforePlatform = moveBeforePlatform;
        }
    }
}