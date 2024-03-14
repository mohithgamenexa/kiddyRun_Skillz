
using UnityEngine;
using System.Collections;

public class ScaleObs : MonoBehaviour
{
    Transform target,mTrns;  
    public float Scale = 1.0f;
    bool once;
    public bool gate;
    void Awake()
    {
        target = TrackManager._instance.playerScript.transform;
        mTrns = transform;
    }
    void Update()
    {
        float distance = Vector3.Distance(mTrns.position, target.position);
        if(distance < 30 && !once)
        {
            if (!gate)
            {
                StartCoroutine(Move());
            }
            else
            {
                StartCoroutine(Rotate());
            }
            once = true;
        }
    }

    IEnumerator Move()
    {
        float startScale = mTrns.localScale.y;
        float progress = 0.0f;
        while (progress < 1.0f)
        {
            progress += Time.deltaTime*7;
            float t = Mathf.Lerp(startScale, Scale, progress);
            Vector3 Lscale = mTrns.localScale;
            Lscale.y = t;
            mTrns.localScale = Lscale; 
            yield return null;
        }
    }


    IEnumerator Rotate()
    {
        float startScale = mTrns.localEulerAngles.z;
        float progress = 0.0f;
        while (progress < 1.0f)
        {
            progress += Time.deltaTime*7;
            float t = Mathf.Lerp(startScale, 0.0f, progress);
            Vector3 LEuler = mTrns.localEulerAngles;
            LEuler.z = t;
            mTrns.localEulerAngles = LEuler;
            yield return null;
        }
    }
    void OnEnable()
    {
        once = false;
        if (!gate)
        {
            mTrns.localScale = new Vector3(1, 0, 1);
        }
        else
        {
            mTrns.localEulerAngles = new Vector3(mTrns.localEulerAngles.x,mTrns.localEulerAngles.y,90);
        }
    }
}
