
using UnityEngine;

public class blindFollower : MonoBehaviour {   
    public Transform target;
    Transform mTransform;
    float OffsetZ = -25.0f;
    void Start()
    {
        mTransform = transform;
    }
	void Update () {
        Vector3 pos = target.TransformPoint(new Vector3(0, 0, OffsetZ));
        pos.y = 10;
        mTransform.position = pos;
        mTransform.rotation = target.rotation;
	}

    void OnTriggerEnter(Collider other)
    {
        string tag_ = other.tag;
        if(tag_ == "die" || tag_ == "coin" )
        {
            other.gameObject.SetActive(false);
        }else if(tag_ == "selfdeath" || tag_ == "powerup")
        {
            Destroy(other.gameObject);
        }
    }
}
