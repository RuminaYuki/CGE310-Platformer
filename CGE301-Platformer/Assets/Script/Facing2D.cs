using UnityEngine;

public class Facing2D
{
    private readonly Transform targetTransform;
    private readonly SpriteRenderer spriteRenderer;
    private readonly bool useScaleFlip;

    public int FacingDirection { get; private set; } = 1;

    public Facing2D(Transform targetTransform, SpriteRenderer spriteRenderer = null, bool useScaleFlip = true)
    {
        this.targetTransform = targetTransform;
        this.spriteRenderer = spriteRenderer;
        this.useScaleFlip = useScaleFlip;
        FacingDirection = targetTransform != null && targetTransform.localScale.x < 0f ? -1 : 1;
    }

    public void SetFacing(int direction)
    {
        if (targetTransform == null)
        {
            return;
        }

        if (direction == 0)
        {
            return;
        }

        FacingDirection = direction > 0 ? 1 : -1;

        if (useScaleFlip)
        {
            Vector3 scale = targetTransform.localScale;
            scale.x = Mathf.Abs(scale.x) * FacingDirection;
            targetTransform.localScale = scale;
            return;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = FacingDirection < 0;
        }
    }

    public void SetFacingByDirectionX(float directionX)
    {
        if (Mathf.Abs(directionX) < 0.001f)
        {
            return;
        }

        SetFacing(directionX > 0f ? 1 : -1);
    }
}
