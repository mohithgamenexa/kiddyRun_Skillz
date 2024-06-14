using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JungleSpikes : MonoBehaviour
{
    public bool isSpike;
    Transform target, mTrns;
    bool once = false;

    public bool stone;
    public bool trunk;
    public bool swing;

    void Start()
    {
        target = TrackManager._instance.playerScript.transform;
        mTrns = transform;
        if(!isSpike && Random.value < 0.5f)
        {
            print(Random.value);
            mTrns.localEulerAngles = Vector3.up * 180;
        }
    }
    void LateUpdate()
    {
        float distance = Vector3.Distance(mTrns.position, target.position);
        if (distance < 30 && !once)
        {
            once = true;
            if (!isSpike)
            {
                if(stone)
                {
                    GetComponent<Animation>()["StonefallRight"].speed = 1;
                    GetComponent<Animation>().Play("StonefallRight");
                }
                else if(trunk)
                {
                    GetComponent<Animation>()["NewTrunk"].speed = 1;
                    GetComponent<Animation>().Play("NewTrunk");
                }
                else if(swing)
                {
                    GetComponent<Animation>()["HookAnimation"].speed = 1;
                    GetComponent<Animation>().Play("HookAnimation");
                }
            }else
                GetComponent<Animation>().Play();
        } 
        else if(once && distance > 30)
        {
            once = false;
            if (!isSpike)
            {
                if(stone)
                {
                    GetComponent<Animation>()["StonefallRight"].speed = -1;
                    GetComponent<Animation>().Play("StonefallRight");
                }
                else if (trunk)
                {
                    GetComponent<Animation>()["NewTrunk"].speed = -1;
                    GetComponent<Animation>().Play("NewTrunk");
                }
                else if (swing)
                {
                    GetComponent<Animation>()["HookAnimation"].speed = -1;
                    GetComponent<Animation>().Play("HookAnimation");
                }
            }          
        }
    }
}
