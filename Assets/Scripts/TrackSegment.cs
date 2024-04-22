using System.Collections.Generic;
using UnityEngine;
using BezierSolution;

public class TrackSegment : MonoBehaviour
{
    public BezierSpline mSpline;
    public bool isFixedPath;
    List<GameObject> mapList = new List<GameObject>();
    public bool isYjun;
    [HideInInspector]
    public bool isFirstMap;
    public void Start()
    {
        int prvNo = -1;
        if (!isFixedPath)
        {
            if (mSpline.pathType == PATHTYPE.STRIGHT || mSpline.pathType == PATHTYPE.JUNCTION || mSpline.pathType == PATHTYPE.TUNNEL)
            {
                
                int forword = TrackManager._instance.inverse == true ? 1 : 0;
                ResetMaplist();
               GameObject[] envAssets = getEnvAssets(0);
               for (int i = 0; i < 4; i++)
               {
                  Transform map = null;
                 if (mSpline.pathType == PATHTYPE.JUNCTION && i == 2)
                 {
                        continue;
                 }
                 else
                 {
                    int mapNo = SkillzCrossPlatform.Random.Range(0, envAssets.Length);
                        while(mapNo == prvNo)
                        {
                            mapNo = SkillzCrossPlatform.Random.Range(0, envAssets.Length);
                        }
                        prvNo = mapNo;
                    map = Instantiate(envAssets[mapNo]).transform;
                 }
                    map.transform.SetParent(this.transform);
                    if (forword == 1)
                    {
                        map.localEulerAngles = Vector3.up*180f;
                    }
                    else
                    {
                        map.localEulerAngles = Vector3.zero;
                    }
                    map.localPosition = Vector3.forward * (i+forword) * 20;
                    mapList.Add(map.gameObject);
               }
            }
        }
    }
    public void MakeMapVisible(bool visibel)
    {
       mapList[0].SetActive(visibel);    
    }
    void ResetMaplist()
    {
        for(int i= 0; i <mapList.Count; i++)
        {
            Destroy(mapList[i]);
        }
        mapList.Clear();
    }
    GameObject[] getEnvAssets(int dir)
    {
        Theme theme = Resources.Load("Data/" + TrackManager._instance.themeType.ToString()) as Theme;
        if(dir == 0)
        {
            return theme.zones[TrackManager._instance.currentZone].envAssets;
        }
        if(dir == 1)
        {
            return theme.rightCurvEnvAssets;
        }
        else
        {
            return theme.curvEnvAssets;
        }
    }


    void MakeMapForLeftOrRight(int dir)
    {
        GameObject[] curvedAssets = getEnvAssets(dir);
        for (int i = 0; i < 4; i++)
        {
            Transform map = Instantiate(curvedAssets[SkillzCrossPlatform.Random.Range(0, curvedAssets.Length)]).transform;
            map.transform.SetParent(this.transform);
           // map.transform.localScale = new Vector3(-dir, 1f, 1f);
            Vector3 pos = Vector3.zero;
            Vector3 eAngles = Vector3.up*(22.5f *dir*i);
            map.localEulerAngles = eAngles;
            if (i == 1)
            {
                pos.x = 3.8768f*dir;
                pos.z = 19.49f;
            }
            else if (i == 2)
            {
                pos.x = 14.917f*dir;
                pos.z = 36.013f;
            }
            else if (i == 3)
            {
                pos.x = 31.44f*dir;
                pos.z = 47.053f;
            }
            map.localPosition = pos;
            mapList.Add(map.gameObject);
        }
    }
}
