using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class themeiconChange : MonoBehaviour
{
    // Start is called before the first frame update
    public Texture2D[] themicons;
    void Start()
    {
        GetComponent<Renderer>().material.mainTexture = themicons[TrackManager._instance.NextZone];
        if(Random.Range(0,10) < 5)
        {
            Vector3 pos = transform.parent.localPosition;
            pos.x = 2.5f;
            transform.parent.localPosition = pos;
        }
    }

}
