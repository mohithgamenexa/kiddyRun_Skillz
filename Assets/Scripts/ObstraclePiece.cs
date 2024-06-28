using System.Collections.Generic;
using UnityEngine;

public class ObstraclePiece : MonoBehaviour
{   
    public GameObject[] meshes,crMeshes,beachMeshes;
    public GameObject cityObj, CRObj,beachObj,forestObj;
    public List<GameObject> forestObjs;
    public bool fallingObjects;

    public int forestobjNum = 0;

    void Awake()
    {
        Debug.Log("obstacle Awake");
        forestobjNum = SkillzCrossPlatform.Random.Range(0, forestObjs.Count);
        if(cityObj == null && meshes.Length > 0)
        {

            //cityObj = Instantiate(meshes[Random.Range(0, meshes.Length)], transform);
            cityObj = Instantiate(meshes[Random.Range(0, meshes.Length)], transform);
            /*c++;
            if (c > meshes.Length)
                c = 0;*/
            cityObj.SetActive(true);
        }
        if (CRObj == null && crMeshes.Length > 0)
        {
            CRObj = Instantiate(crMeshes[Random.Range(0, crMeshes.Length)], transform);
            CRObj.SetActive(false);
        }
        if(beachObj == null && beachMeshes.Length > 0)
        {
            Debug.Log("Beach Theme Obstacle");
            beachObj = Instantiate(beachMeshes[Random.Range(0, beachMeshes.Length)], transform);
            beachObj.SetActive(false);
        }
        if (cityObj == null && meshes.Length == 0)
        {
            Debug.LogError("Meshes are empy............");
        }
    }
    public void ChangeZone()
    {

        int zone = TrackManager._instance.currentZone;
        Debug.Log("Zone::" + gameObject.name + "==============" + zone);

        if (cityObj != null)
            cityObj.SetActive(false);
        if (CRObj != null)
            CRObj.SetActive(false);
        if (beachObj != null)
            beachObj.SetActive(false);
        if (forestObj != null)
        {
            if(fallingObjects)
            {
                foreach (var item in forestObjs)
                {
                    item.SetActive(false);
                }
            }
            else
                forestObj.SetActive(false);
        }
        Debug.Log("all objstacles disabled::");
        if(zone == 0)
        {
            if (cityObj != null)
                cityObj.SetActive(true);
        }
        else if(zone == 1)
        {
            if (beachObj != null)
            {
                Debug.Log("in zone 1--111111");

                beachObj.SetActive(true);
            }
        }
        else if(zone == 2)
        {
            if (CRObj != null)
            {
                Debug.Log("in zone 2--111111");
                CRObj.SetActive(true);
            }
        }
        else if(zone == 3)
        {
            Debug.Log("in zone 3--111111");

            if (forestObj != null)
            {
                Debug.Log("in zone 3--222222");

                if (fallingObjects)
                {
                    Debug.Log("in zone 3--33333");
                   /* if (forestobjNum > forestObjs.Count)
                        forestobjNum = 0;
                    forestobjNum++;*/

                    forestObjs[forestobjNum].SetActive(true);
                    

                }
                else
                {
                    Debug.Log("in zone 3--55555");

                    forestObj.SetActive(true);
                }
            }
        }
    }
}
