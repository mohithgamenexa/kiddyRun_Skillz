
using UnityEngine;

public class visualRandomiser : MonoBehaviour
{  
    public GameObject[] meshes;
    public GameObject[] CRmeshes;
    public GameObject colonyMesh, CRMesh;
    void Start()
    {
        if(meshes.Length > 0)
        {
            for (int i = 0; i < meshes.Length; i++)
            {
                meshes[i].SetActive(false);
            }
            meshes[Random.Range(0, meshes.Length)].SetActive(true);
        }       
    }
    public void SetTheme()
    {
        bool cr = TrackManager._instance.currentZone == 2 ? true : false;
        CRMesh.SetActive(cr);
        colonyMesh.SetActive(!cr);
    }  
}
