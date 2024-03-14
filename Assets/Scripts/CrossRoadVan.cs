
using UnityEngine;

public class CrossRoadVan : MonoBehaviour
{
    Transform player_;
    Transform mTransform;
    public float speed = 22;   
    public Transform tire1, tire2;
    Vector3 tgtPos;
    void Start()
    {
        player_ = GameObject.FindWithTag("Player").transform;
        mTransform = transform;
        mTransform.GetChild(0).gameObject.SetActive(true);
        tgtPos = Vector3.zero;
        int t = Random.Range(-1, 2);
        if(t == -1)
        {
            tgtPos.x = -3.5f;
        }else if(t == 1)
        {
            tgtPos.x = 2.5f;
        }
    }
    void Update()
    {
        float dis = Vector3.Distance(player_.position, mTransform.position);
        if(dis < 60)
        {
            float d = Vector3.Distance(mTransform.localPosition, tgtPos);
            if(d >= 0.1f)
            {
                mTransform.localPosition = Vector3.MoveTowards(mTransform.localPosition, tgtPos, Time.deltaTime * speed);
                tire1.Rotate(Vector3.right * speed * 0.5f);
                tire2.Rotate(Vector3.right * speed * 0.5f);
            }               
        }

    }
}
