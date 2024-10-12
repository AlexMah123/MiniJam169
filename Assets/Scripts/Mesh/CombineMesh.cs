using UnityEngine;

namespace Mesh
{
    
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class CombineMesh : MonoBehaviour
    {
        void Start()
        {
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];


            for (int i = 0; i < meshFilters.Length; i++)
            {
                if (meshFilters[i].gameObject == this.gameObject || meshFilters[i] == null)
                {
                    continue;      
                }
                
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);
            }
  
            
            var meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = new UnityEngine.Mesh();
            meshFilter.mesh.CombineMeshes(combine);
            GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;
            transform.gameObject.SetActive(true);
            
            transform.localScale = Vector3.one;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }
    }
}