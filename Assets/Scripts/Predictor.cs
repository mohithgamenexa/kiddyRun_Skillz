using System.Collections;
using UnityEngine;

public class Predictor : MonoBehaviour
{   
    public Transform _player;
    Transform mTransform;
    BoxCollider mCol;
    void Start()
    {
        mTransform = transform;
        mCol = GetComponent<BoxCollider>();
    }
    void Update()
    {
        mTransform.position = _player.position;
        mTransform.rotation = _player.rotation;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "die" && TrackManager._instance._gameState == GameState.PLAYING)
        {
            TrackManager._instance.canTackeCollision = false;
            mCol.enabled = false;
            StartCoroutine(Reset());
        }
    }
    IEnumerator Reset()
    {
        yield return new WaitForSeconds(0.5f);
        TrackManager._instance.canTackeCollision = true;
        mCol.enabled = true;
    }
}
