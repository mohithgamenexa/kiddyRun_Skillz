
using UnityEngine;

public class DesttoySelf : MonoBehaviour
{
    void FixedUpdate()
    {
        if(TrackManager._instance._gameState == GameState.GAMEOVER)
        {
            Destroy(gameObject);
        }       
    }
}
