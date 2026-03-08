#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class MergeAndSaveMeshColliders : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] private GameObject sourceRoot;

    [Header("Options")]
    [SerializeField] private bool removeOriginalColliders = false;
    [SerializeField] private bool saveAssetToFile = true;
    [SerializeField] private bool setMeshConvex = true;

    [Tooltip("Only used if 'saveAssetToFile' is true.")]
    [SerializeField] private string savePath = "Assets/Meshes/CombinedMeshCollider.asset";

    [ContextMenu("Merge MeshColliders")]
    public void MergeAndOptionallySave()
    {
        if (sourceRoot == null)
        {
            Debug.LogError("SourceRoot must be assigned.");
            return;
        }

        MeshCollider[] meshColliders = sourceRoot.GetComponentsInChildren<MeshCollider>();

        if (meshColliders.Length == 0)
        {
            Debug.LogWarning("No MeshColliders found.");
            return;
        }

        CombineInstance[] combine = new CombineInstance[meshColliders.Length];

        for (int i = 0; i < meshColliders.Length; i++)
        {
            MeshCollider mc = meshColliders[i];

            if (mc.sharedMesh == null)
                continue;

            combine[i] = new CombineInstance
            {
                mesh = mc.sharedMesh,
                transform = mc.transform.localToWorldMatrix
            };

            if (removeOriginalColliders)
                DestroyImmediate(mc);
        }

        Mesh combinedMesh = new Mesh
        {
            indexFormat = UnityEngine.Rendering.IndexFormat.UInt32,
            name = "CombinedColliderMesh"
        };
        combinedMesh.CombineMeshes(combine);

        // Assign to this object's MeshCollider
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = combinedMesh;
        meshCollider.convex = setMeshConvex;

#if UNITY_EDITOR
        if (saveAssetToFile)
        {
            AssetDatabase.CreateAsset(combinedMesh, savePath);
            AssetDatabase.SaveAssets();
            Debug.Log($"Saved combined collider mesh to {savePath}");
        }
        else
        {
            Debug.Log("Combined mesh assigned but not saved as asset.");
        }
#else
        Debug.LogWarning("Saving mesh as asset is only supported in the Unity Editor.");
#endif
    }
}
