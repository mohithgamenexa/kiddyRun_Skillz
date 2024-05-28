﻿
using UnityEngine;

public class ObstraclePiece : MonoBehaviour
{   
    public GameObject[] meshes,crMeshes,beachMeshes;
    public GameObject cityObj, CRObj,beachObj,forestObj;
    public GameObject obstacle;
    int c = 0;
    void Awake()
    {
        if(TrackManager._instance.currentZone == 0 && cityObj == null && meshes.Length > 0)
        {

            //cityObj = Instantiate(meshes[Random.Range(0, meshes.Length)], transform);
            cityObj = Instantiate(meshes[c], transform);
            c++;
            if (c > meshes.Length)
                c = 0;
            cityObj.SetActive(true);
        }
        if (CRObj == null && crMeshes.Length > 0)
        {
            CRObj = Instantiate(crMeshes[Random.Range(0, crMeshes.Length)], transform);
            CRObj.SetActive(false);
        }
        if(TrackManager._instance.currentZone == 1 && beachObj == null && beachMeshes.Length > 0)
        {
            Debug.Log("Beach Theme Obstacle");
            beachObj = Instantiate(beachMeshes[Random.Range(0, beachMeshes.Length)], transform);
            obstacle.SetActive(false);

            beachObj.SetActive(true);
        }
        if (cityObj == null && meshes.Length == 0)
        {
            Debug.LogError("Meshes are empy............");
        }
    }
    public void ChangeZone()
    {
        int zone = TrackManager._instance.currentZone;
        if (cityObj != null)
            cityObj.SetActive(false);
        if (CRObj != null)
            CRObj.SetActive(false);
        if (beachObj != null)
            beachObj.SetActive(false);
        if (forestObj != null)
            forestObj.SetActive(false);
        if(zone == 0)
        {
            if (cityObj != null)
                cityObj.SetActive(true);
        }
        else if(zone == 1)
        {
            if (beachObj != null)
                beachObj.SetActive(true);
        }
        else if(zone == 2)
        {
            if (CRObj != null)
                CRObj.SetActive(true);
        }
        else if(zone == 3)
        {
            if (forestObj != null)
                forestObj.SetActive(true);
        }
    }
}
