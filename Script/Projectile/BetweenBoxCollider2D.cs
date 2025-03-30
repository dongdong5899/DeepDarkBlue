using UnityEngine;

public class BetweenBoxCollider2D : BetweenCollider2D
{
    public Vector2 size = Vector2.one;
    public Vector2 offset;

    public override bool CheckCollision(LayerMask whatIsTarget, out RaycastHit2D[] hits, Vector2 moveTo = default)
    {
        hits = Physics2D.BoxCastAll(transform.position + transform.rotation * offset,
                            size, transform.rotation.z, moveTo.normalized, moveTo.magnitude, whatIsTarget);

        return hits.Length > 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(offset, size);
    }
}
