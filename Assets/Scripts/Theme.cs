using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Theme")]
public class Theme : ScriptableObject
{
    public List<Zone> zones = new List<Zone>();
    public GameObject[] curvEnvAssets;
    public GameObject[] rightCurvEnvAssets;
    [Header("Obstracles")]
    public GameObject Truck;
    public GameObject BusIdle;
    public GameObject busMove;
    public GameObject carIdle;
    public GameObject carMove;
    public GameObject HudleJump;
    public GameObject HudleDow;
    public GameObject HudleTwoWay;
    public GameObject Blocker;
}
