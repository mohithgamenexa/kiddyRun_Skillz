using System.Collections;
using UnityEngine;
using BezierSolution;

public class PlayerCollisionCntrl : MonoBehaviour
{
    BezierWalkerWithSpeed playerScript;
    string s;
    Transform anim;
    Transform mTransform;   
    public ParticleSystem coinPrtCle, PowCol;
    void Start()
    {
        playerScript = GetComponent<BezierWalkerWithSpeed>();
        anim = playerScript.anim.transform;
        mTransform = transform;
    }

   void OnTriggerEnter(Collider other)
   {
        if(TrackManager._instance._gameState == GameState.PLAYING)
        {
            s = other.tag;
            if (s == "coin")
            {
                coinPrtCle.Play();
                other.gameObject.SetActive(false);
                uimanager.instance.UpdateCoin();
                PowerupHandler.instance.FillSectionBonusBar();
            }else if(s == "powerup")
            {
                SfxManager.instance.PlayPowerUp();
                uimanager.instance.PowerUpCollected((int)other.GetComponent<powerupCntrl>().powerUpType);
                MissionManager.instance.PowerUpTrigger();
                PowCol.Play();
                Destroy(other.gameObject);
            }
            else if(s == "word")
            {
                SfxManager.instance.PlayPowerUp();
                TrackManager._instance.WordCollected();
                Destroy(other.gameObject);
            }
            else if (s == "die" || s == "die2")
            {
                if (uimanager.instance.powerupArray[3].isActivated || playerScript.mFlying)
                {
                    return;
                   // TrackManager._instance._gameState = GameState.SAVEME;
                }else if (uimanager.instance.powerupArray[4].isActivated)
                {
                    uimanager.instance.StopCycling();
                }
                else
                {
                    if (uimanager.instance.inChase)
                    {
                        PlayDeathScene(mTransform.position);
                    }
                    else
                    {
                        Vector3 rayPos = mTransform.position + mTransform.forward * -2f;
                        rayPos.y = 1.5f;
                        RaycastHit hit;
                        if (Physics.Raycast(rayPos, mTransform.forward, out hit, 3f))
                        {
                            if (hit.collider != null)
                            {
                                PlayDeathScene(hit.point);
                            }
                        }
                        else
                        {
                            SfxManager.instance.PlayUps();
                            uimanager.instance.StartChase();
                        }
                    }                                          
                }
                StartCoroutine(uimanager.instance.camCntrl.Shake());
            }
            else if(s == "ship" && transform.position.y < 5f)
            {
                if (uimanager.instance.powerupArray[3].isActivated)
                {
                    return;
                }
                if (uimanager.instance.powerupArray[4].isActivated)
                {
                    uimanager.instance.StopCycling();
                }
               SfxManager.instance.PlayUps();
               playerScript.BackToPrevLane();
               uimanager.instance.StartChase();              
            }else if(s == "tutr")
            {
                playerScript.OnTutorialPlay();
            }
            else if(s == "gldr")
            {
                playerScript.StopFlying();
                Destroy(other.gameObject);
            }else if(s == "cave" && !playerScript.mFlying)
            {
                //  TrackManager._instance._gameState = GameState.ZONECHANGE;
                StartCoroutine(TrackManager._instance.OnTunnelEnter());
                other.gameObject.SetActive(false);
            }else if(s == "lava")
            {
                PlayDeathScene(mTransform.position,true);
            }
        }
   }
    public void PlayDeathScene(Vector3 hitPoint,bool lava = false)
    {
        SfxManager.instance.PlayDeathSound();
        TrackManager._instance._gameState = GameState.PREGAMEOVER;
        playerScript.EnterDie();
        uimanager.instance.PreGameOver();
        if(!playerScript.railRun)
            StartCoroutine(movePlayer(hitPoint + (mTransform.forward * -1.5f)));
    }

    IEnumerator movePlayer(Vector3 target)
    {
        target.y = 1;
        float progress = 0.0f;
        while(progress < 1)
        {
            progress += Time.deltaTime * 3;
            mTransform.position = Vector3.Lerp(mTransform.position, target, progress);
            yield return null;
        }
    }

    IEnumerator KissTheCamera(Transform t_)
    {
        Transform target = Camera.main.transform;
        Vector3 targetPos = target.position - target.localPosition;
        float progress = 0.0f;
        while (progress < 1.0f)
        {
            progress += Time.deltaTime * 5;
            t_.position = Vector3.Lerp(t_.position, targetPos, progress);
            yield return null;
        }
    }
}
