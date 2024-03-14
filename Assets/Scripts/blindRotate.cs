
using System.Collections;
using UnityEngine;

public class blindRotate : MonoBehaviour
{
    Transform rTransform;
    public Transform coin1;
    public Transform coin2;
    Animation anim;
    void Start()
    {
        rTransform = transform;
        anim = GetComponent<Animation>();
    }
    Vector3 vel = Vector3.zero;
    bool inRange = false;
    void Update()
    {
        if(uimanager.instance.powerupArray[0].isActivated)
        {
            Vector3 plr = TrackManager._instance.playerScript.transform.position+Vector3.up*0.5f;
            plr.y = rTransform.position.y;
            float distance = Vector3.Distance(rTransform.position, plr);
            if(distance <= 7 && !inRange)
            {
                inRange = true;
                StartCoroutine(moveToPlayer());
            }
            if(distance > 12)
            {
                inRange = false;
            }
        }

        rTransform.Rotate(0, 50f * Time.deltaTime, 0);
    }

    IEnumerator moveToPlayer()
    {

        Vector3 targetpos = TrackManager._instance.playerScript.transform.position + Vector3.up * 0.5f;
        float _distance = Vector3.Distance(rTransform.position, targetpos);
        while (_distance > 0.3f)
        {
            targetpos = TrackManager._instance.playerScript.transform.position + Vector3.up * 0.5f;
            _distance = Vector3.Distance(rTransform.position, targetpos);
            Vector3 _dir = targetpos - rTransform.position;
            _dir = _dir.normalized;
            rTransform.position += _dir * 25.0f * Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);

    }

    public void Split(bool ON)
    {
        if (gameObject.activeInHierarchy)
        {
            string _anim = ON == true ? "SplitAnim" : "SplitBack";
            anim.Play(_anim);
        }
        else
        {
            coin2.gameObject.SetActive(ON);
            if (ON)
            {
                coin1.localPosition = new Vector3(-0.25f, 0.5f, 0);
                coin2.localPosition = new Vector3(0.25f, 0.5f, 0);
            }
            else
            {
                coin1.localPosition = new Vector3(0, 0.5f, 0);
                coin2.localPosition = new Vector3(0, 0.5f, 0);
            }
        }
    }

}
