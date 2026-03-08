
using UnityEngine;

public class ColliderCleaner : MonoBehaviour
{

    // Call this to remove all MeshColliders under this GameObject
    [ContextMenu("Remove MeshColliders")]
    public void RemoveMeshColliders()
    {
        // Get all MeshColliders in this GameObject and its children
        MeshCollider[] colliders = GetComponentsInChildren<MeshCollider>(includeInactive: true);

        int count = 0;
        foreach (MeshCollider col in colliders)
        {
            DestroyImmediate(col);
            count++;
        }

        Debug.Log($"Removed {count} MeshCollider(s) from {gameObject.name} and its children.");
    }
}
