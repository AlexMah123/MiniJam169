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
            CombineInstance[] combine = new CombineInstance[meshFilters.Length - 1]; //ignore parent
            
            //seperate index, only increments when filter is not null
            int index = 0;
            foreach (var filter in meshFilters)
            {
                if (filter.gameObject == this.gameObject || filter == null)
                {
                    continue;      
                }
                
                combine[index].mesh = filter.sharedMesh;
                combine[index].transform = filter.transform.localToWorldMatrix;
                
                filter.gameObject.SetActive(false);
                index++;
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