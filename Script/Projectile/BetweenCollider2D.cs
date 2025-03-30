using UnityEngine;

public abstract class BetweenCollider2D : MonoBehaviour
{
    public abstract bool CheckCollision(LayerMask whatIsTarget, out RaycastHit2D[] hits, Vector2 moveTo = default);
}
