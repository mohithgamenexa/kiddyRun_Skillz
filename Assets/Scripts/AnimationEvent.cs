
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public void OnAnimFinished()
    {
        if(TrackManager._instance._gameState != GameState.ZONECHANGE)
            gameObject.SetActive(false);
    }
    public void DestroyOnFinish()
    {
        Destroy(gameObject);
    }
    public void OnIntroAnim()
    {
        uimanager.instance.Play();
    }
    public void StartChase()
    {
        uimanager.instance.StartChase();
    }
    public void PreGameOver()
    {
        uimanager.instance.PlayCountDown();
    }
    public void Resume()
    {
        gameObject.SetActive(false);
        MusicPlayer.instance.Pause(false);
        TrackManager._instance._gameState = GameState.PLAYING;
        Timer.instance.startTimer = true;
        uimanager.instance.enemyAnim.enabled = true;
    }
}
