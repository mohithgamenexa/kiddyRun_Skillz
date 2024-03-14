using UnityEngine;
using BezierSolution;
using System.Collections.Generic;

public class MovingBus : MonoBehaviour {
    Transform player_;
    Transform mTransform;
    public float speed = 18;
    BezierWalkerWithSpeed playerScript;
	void Awake () {
        mTransform = transform;      
    }
    void Start()
    {
        player_ = GameObject.FindWithTag("Player").transform;
        playerScript = player_.GetComponent<BezierWalkerWithSpeed>();
    }
    void Update () {
        
        if (TrackManager._instance._gameState == GameState.PLAYING)
        {
            float dis = Vector3.Distance(player_.position, mTransform.position);
            if (dis < 70)
            {
                if (!check)
                {
                    check = true;
                    WaitAndGetCoin();
                }
                Vector3 relativePoint = mTransform.InverseTransformDirection(player_.forward);
                if (Mathf.Abs(relativePoint.x) < 0.2f)
                {
                    mTransform.position -= mTransform.forward * playerScript.m_Speed*0.6f * Time.deltaTime;
                }                
                if (coins.Count > 0)
                {
                    for (int i = 0; i < coins.Count; i++)
                    {
                        coins[i].position = mTransform.position + offSet[i];
                    }
                }
            }
        }
    }
    List<Transform> coins = new List<Transform>();
    Vector3[] offSet;
    public Vector3 boxOr;
    public Vector3 boxBoarder;
    public bool check = false;
    void WaitAndGetCoin()
    {
        Vector3 center = mTransform.TransformPoint(boxOr);
        Collider[] cols = Physics.OverlapBox(center, boxBoarder,mTransform.rotation);       
        coins = new List<Transform>();
        for (int i = 0; i < cols.Length; i++)
        {
            if(cols[i].tag == "coin")
            {
                coins.Add(cols[i].transform);
            }
        }
        offSet = new Vector3[coins.Count];
        for (int i = 0; i < coins.Count; i++)
        {
            offSet[i] = coins[i].position - mTransform.position;
        }
    }
}
