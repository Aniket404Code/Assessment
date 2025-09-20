using UnityEngine;

public class RemoveAllEdgeColliders : MonoBehaviour
{
    [ContextMenu("Remove All EdgeColliders")]
    void RemoveColliders()
    {
        EdgeCollider2D[] colliders = GetComponents<EdgeCollider2D>();
        foreach (var col in colliders)
        {
            DestroyImmediate(col);
        }
    }
}
