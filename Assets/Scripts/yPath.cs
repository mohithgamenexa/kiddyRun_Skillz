
using UnityEngine;
using BezierSolution;

public class yPath : MonoBehaviour
{
    BezierWalkerWithSpeed playerScript;
    public BezierSpline mySpline;
    void Start()
    {
        playerScript = TrackManager._instance.playerScript;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !playerScript.mFlying)
        {
            playerScript.nextSpline = mySpline;
            Transform t = playerScript.advanceSpline.transform.parent;
            t.position = mySpline.endPoints[mySpline.endPoints.Count - 1].transform.position;
            t.rotation = mySpline.endPoints[mySpline.endPoints.Count - 1].transform.rotation;
        }
    }
}
