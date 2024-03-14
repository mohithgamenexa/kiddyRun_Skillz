using System.Collections;
using UnityEngine;
using BezierSolution;

public class CameraController : MonoBehaviour
{

    
    public BezierWalkerWithSpeed playerScript;
    Transform player_;

    public Vector3 offsetPosition;

    
    public float rotationDamping;
    
    public float xDamping;
    public Transform tCamera,flashcamera;
    public bool canFollow;
    Vector3 cameraPos,mPos,camRot;    
    public GameObject introScene,headBoost,doorL,doorR;
    public void Restart()
    {
        canFollow = false;
        mTransform.position = mPos;
        mTransform.eulerAngles = Vector3.right * 14f;
        tCamera.localPosition = new Vector3(6f,-4.5f,-3.5f);
        tCamera.localEulerAngles = new Vector3(-9f, -77.5f, 12f);
        tCamera.GetComponent<Camera>().fieldOfView = 60f;
        offsetPosition = new Vector3(0, 4.5f, -5.5f);
        playerScript.canSwipe = false;
        TrackManager._instance.strightPathList[0].MakeMapVisible(false);
    }
    IEnumerator pullCamPos()
    {
        float progress = 0.0f;
        Vector3 initialPos = tCamera.localPosition;
        Quaternion initialRot = tCamera.localRotation;
        while (progress < 1)
        {
            progress += Time.deltaTime*1.5f;
            tCamera.localPosition = Vector3.Lerp(initialPos, Vector3.zero, progress);
            tCamera.localRotation = Quaternion.Lerp(initialRot, Quaternion.identity, progress);
            /////////
            flashcamera.localPosition = Vector3.Lerp(initialPos, Vector3.zero, progress);
            flashcamera.localRotation = Quaternion.Lerp(initialRot, Quaternion.identity, progress);
            yield return null;
        }
        tCamera.localPosition = Vector3.zero;
        tCamera.localRotation = Quaternion.identity;
        doorL.transform.localEulerAngles = Vector3.zero;
        doorR.transform.localEulerAngles = Vector3.zero;
        playerScript.anim.SetBool("idlexit", false);
        uimanager.instance.inGameMenu.SetActive(true);
        yield return new WaitForSeconds(2f);
        introScene.SetActive(false);
        TrackManager._instance.strightPathList[0].MakeMapVisible(true);
    }


    private void LateUpdate()
    {
        if (TrackManager._instance._gameState == GameState.PLAYING)
        {
            if (canFollow)
            {
                 Refresh();
                //FollowPlayer();
            }else
            {
                if (player_.position.z >= mTransform.position.z + Mathf.Abs(offsetPosition.z))
                {
                    canFollow = true;
                    StartCoroutine(pullCamPos());
                    if(DataManager.instance.tutorialPlyd == 1)
                        playerScript.canSwipe = true;
                    if(DataManager.instance.tutorialPlyd!=0 && DataManager.instance.GetHeadBoost > 0)
                        headBoost.SetActive(true);
                    playerScript.targetXOffset = 0.0f;

                }
                else
                {
                    playerScript.targetXOffset = Mathf.Lerp(playerScript.targetXOffset, 0.0f, Time.deltaTime*2);
                    Vector3 lookPos = player_.position + new Vector3(0,2.5f,0.6f);
                    lookPos.x = 0;
                    tCamera.LookAt(lookPos);
                }
            }
        }
    }
    Transform mTransform;
    Vector3 currentPos;
    Quaternion rot;

    void Awake()
    {
        mTransform = transform;
        player_ = playerScript.transform;
        tCamera = transform.GetChild(0);
        mPos = mTransform.position;
        cameraPos = tCamera.localPosition;
        camRot = tCamera.localEulerAngles;
        playerScript.canSwipe = false;
    }

    public static float XOffSet = 0.2f;

    public void Refresh()
    {       
            float targetX = -playerScript.targetXOffset * 0.2f;
            offsetPosition.x = Mathf.Lerp(offsetPosition.x, targetX, xDamping * Time.deltaTime);
            Vector3 wantedPos = player_.TransformPoint(offsetPosition);
            wantedPos.y = Mathf.Lerp(mTransform.position.y, wantedPos.y, playerScript.camHeightDamping * Time.deltaTime);
            mTransform.position = wantedPos;

            Quaternion rot_ = Quaternion.Slerp(mTransform.localRotation, player_.localRotation, Time.deltaTime * xDamping);
            mTransform.localRotation = rot_;
            mTransform.localEulerAngles = new Vector3(14, mTransform.localEulerAngles.y, 0);            
    }
    private void FollowPlayer()
    {
        // Calculate the current rotation angles
        float wantedRotationAngle = player_.eulerAngles.y;
        float wantedHeight = player_.position.y + offsetPosition.y;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, playerScript.camHeightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        transform.position = player_.position;
        transform.position -= currentRotation * Vector3.forward * offsetPosition.z;

        // Set the height of the camera
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // Always look at the target
        transform.rotation = currentRotation;
        //transform.LookAt(target);
    }

    public IEnumerator Shake()
    {
        float ShakeTime = 1.0f;
        Vector3 newPos = Vector3.zero;
        while(ShakeTime > 0)
        {
            ShakeTime -= Time.deltaTime*2;
            if (Vector3.Distance(newPos,tCamera.localPosition) < 0.1f)
            {
                newPos = Random.insideUnitSphere * 0.25f;
                newPos.z = 0;
            }
            tCamera.localPosition = Vector3.Lerp(tCamera.localPosition, newPos, Time.deltaTime * 15);
            yield return null;
        }
        tCamera.localPosition = Vector3.zero;
    }
}
