using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastMovementController : MonoBehaviour
{
    public LayerMask CollisionMask;
	public int HorizontalRayCount = 4;
    public int VerticalRayCount = 4;

    protected const float SkinWidth = .015f;

    protected float HorizontalRaySpacing;
    protected float VerticalRaySpacing;

    protected BoxCollider2D Collider;
    protected RaycastOrigins CastOrigins;

    public virtual void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    protected void UpdateRaycastOrigins()
    {
        var bounds = Collider.bounds;
        bounds.Expand(SkinWidth * -2);

        CastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        CastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
        CastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
        CastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    protected void CalculateRaySpacing()
    {
        var bounds = Collider.bounds;
        bounds.Expand(SkinWidth * -2);

        HorizontalRayCount = Mathf.Clamp(HorizontalRayCount, 2, int.MaxValue);
        VerticalRayCount = Mathf.Clamp(VerticalRayCount, 2, int.MaxValue);

        HorizontalRaySpacing = bounds.size.y / (HorizontalRayCount - 1);
        VerticalRaySpacing = bounds.size.x / (VerticalRayCount - 1);
    }

    protected struct RaycastOrigins
    {
        public Vector2 TopLeft, TopRight;
        public Vector2 BottomLeft, BottomRight;
    }
}