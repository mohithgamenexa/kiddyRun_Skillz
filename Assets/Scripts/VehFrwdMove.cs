using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehFrwdMove : MonoBehaviour
{
    Transform player_;
    Transform mTransform;
    public float speed = 8;
    public GameObject[] meshes;
    void Start()
    {
        mTransform = transform;
        player_ = GameObject.FindWithTag("Player").transform;
        if (meshes.Length > 0)
        {
            GameObject mesh = Instantiate(meshes[Random.Range(0, meshes.Length)], mTransform);
            mesh.transform.localEulerAngles = new Vector3(0, 180, 0);
            mesh.transform.localPosition = Vector3.forward * 7.5f;
        }
    }
    void Update()
    {
        if (TrackManager._instance._gameState == GameState.PLAYING)
        {
            float dis = Vector3.Distance(player_.position, mTransform.position);
            if (dis < 40)
            {
                Vector3 relativePoint = mTransform.InverseTransformDirection(player_.forward);
                if (Mathf.Abs(relativePoint.x) < 0.2f)
                {
                    mTransform.position += mTransform.forward * speed * Time.deltaTime;
                }
            }
        }
    }
}
