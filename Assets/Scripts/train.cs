using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class train : MonoBehaviour
{
    Transform mTransform, player_;
    void Start()
    {
        mTransform = transform;
        player_ = GameObject.FindWithTag("Player").transform;
    }
    void Update()
    {
        if (TrackManager._instance._gameState == GameState.PLAYING)
        {
            float dis = Vector3.Distance(player_.position, mTransform.position);
            if (dis < 100)
            {
              mTransform.position += mTransform.forward * 13 * Time.deltaTime;
            }
        }
    }
}
