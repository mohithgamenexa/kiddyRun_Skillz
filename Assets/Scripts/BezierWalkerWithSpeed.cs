using UnityEngine;
using UnityEngine.Events;
using TMPro;
namespace BezierSolution
{
    public class BezierWalkerWithSpeed : MonoBehaviour
    {
        private Transform cachedTransform;
        private Transform mMesh;
        [HideInInspector]
        public BezierSpline currentSpline;
        [HideInInspector]
        public BezierSpline nextSpline;
        [HideInInspector]
        public BezierSpline advanceSpline;


        public TextMeshProUGUI metersTxt, scoreTxt, highScoreTxt;
        public float m_Speed = 5f;
        public float rProgress = 0f;

        public Transform rayPos;
        public Transform shadowT;
        public bool mFlying = false;
        public float flyLength = 90.0f;
        public int scoreMul;
        public int powMul = 1;
        public GameObject glider, flyingPrtcl,gliderSlideOff;
        public float NormalizedT
        {
            get { return rProgress; }
            set { rProgress = value; }
        }

        [Range(0f, 0.06f)]
        public float relaxationAtEndPoints = 0.01f;

        //public float movementLerpModifier = 10f;
        public float rotationLerpModifier = 10f;

        public bool lookForward = true;


        public UnityEvent onPathCompleted = new UnityEvent();
        public float currentAngle;
        public bool canSwipe = true;
        [Header("tutorialAssets")]
        public GameObject tutorialPnl;
        public RectTransform tutAnim;
        public TextMeshProUGUI swipeTxt;
        int highScore;
        public GameObject slideParticle;
        public GameObject[] slideParticles;
        int floorMask;
        public bool railRun;
        public GameObject railVehicle;

        void Awake()
        {
            cachedTransform = transform;
            mMesh = transform.Find("mesh");
            mBody = GetComponent<Rigidbody>();
            mCollieder = GetComponent<CapsuleCollider>();
            floorMask = LayerMask.GetMask("ground");
            slideParticle = slideParticles[0];
        }
        bool canChange = false;

        public void changePath()
        {
            BezierSpline wantedPath = nextSpline;
            BezierSpline nextPath_ = advanceSpline;
            BezierSpline oldPath = currentSpline;

            currentSpline = wantedPath;
            nextSpline = nextPath_;
            advanceSpline = oldPath;

            if (currentSpline.pathType == PATHTYPE.LEFT || currentSpline.pathType == PATHTYPE.RIGHT)
            {
                float y = currentSpline.pathType == PATHTYPE.LEFT ? -10f : 10f;
                StartCoroutine(TrackManager._instance.roateCamera(y));
                SfxManager.instance.PlayLaugh();
                anim.SetTrigger("backrun");
            }
            if (currentSpline.isYjun)
            {
                StopFlying();
            }
          //  TrackManager._instance.cLvlTxt.text = "" + currentSpline.mapNo;
        }
        public void ResetPos()
        {
            targetXOffset = 0;
            m_CurrentLane = 0;
            m_PrevieousDir = 0;
        }
        public void Play()
        {
            targetXOffset = -2.5f;
            camHeightDamping = 5.0f;
            m_CurrentLane = 1;
            m_PrevieousDir = 1;
            m_TotalWorldDistance = 0;
            nextTargetDistance = 1000;
            m_Sliding = false;
            m_Jumping = false;
            mFlying = false;
            mWallRun = false;
            landing = false;
            inAir = false;
            m_IsSwiping = false;
            canChange = false;
            rProgress = 0.0f;
            maxSpeed = DataManager.instance.HighSpeed;
            if (maxSpeed < 40)
            {
                maxSpeed = 40;
            }
            offSet = new Vector3(-2.5f, 0, 0);
            anim.transform.localPosition = Vector3.zero;
            anim.transform.localRotation = Quaternion.identity;
            m_Speed = 15;
            anim.enabled = true;
            TrackManager._instance._gameState = GameState.PLAYING;
            scoreMul = MissionManager.instance.CurrentSet + 1;
            scoreMul += DataManager.instance.ScoreMul;
            DataManager.instance.ScoreMul = 0;
            uimanager.instance.scoreMul.text = scoreMul + "X";
            glider.SetActive(false);
            flyingPrtcl.SetActive(false);
            cachedTransform.position = Vector3.right * -2.5f;
            cachedTransform.eulerAngles = Vector3.zero;
            TrackManager._instance.sesstionCount += 1;
            highScore = DataManager.instance.HighScore;
            slideParticle = slideParticles[TrackManager._instance.currentZone];
        }
        int tCount;
        bool tutorialPld = false;
        public void OnTutorialPlay()
        {
            Time.timeScale = 0.001f;
            if (tCount == 0)
            {
                swipeTxt.text = "Swipe Left";
                tutAnim.rotation = Quaternion.AngleAxis(180, Vector3.forward);
            }
            else if (tCount == 1)
            {
                swipeTxt.text = "Swipe Right";
                tutAnim.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            }
            else if (tCount == 2)
            {
                swipeTxt.text = "Swipe Up";
                tutAnim.rotation = Quaternion.AngleAxis(90, Vector3.forward);
            }
            else if (tCount == 3)
            {
                swipeTxt.text = "Swipe Down";
                tutAnim.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
            }
            canSwipe = true;
            tutorialPld = true;
            tutorialPnl.gameObject.SetActive(true);
        }
        void onSwipeTut()
        {
            tutorialPld = false;
            Time.timeScale = 1f;
            tCount += 1;
            if (tCount < 4)
            {
                canSwipe = false;
            }
            else
            {
                DataManager.instance.tutorialPlyd = 1;
                TrackManager._instance.tutorialPld = true;
            }
            tutorialPnl.gameObject.SetActive(false);
        }
        public void HitOnTurorial()
        {
            rProgress -= 0.1f;
        }
        void Update()
        {
            if (TrackManager._instance._gameState == GameState.PLAYING)
            {
                if (canSwipe)
                {
                    CheckInput();
                }
                MoveInPath();
                AdjustPosWithControlls();
                CalculateSpeed();
            }
            else if (TrackManager._instance._gameState == GameState.TUTORIAL)
            {
                CheckInput();
            }
        }

        int m_CurrentLane = 1;
        int m_PrevieousDir = 1;
        public float laneOffset;
        public float targetXOffset = 0.0f;
        public Animator anim;
        public float camHeightDamping = 5.0f;
        public float jumpHeight;
        int wallDirection = 0;
        void wallRun(int wallDir)
        {
            mWallRun = true;
            wallDirection = wallDir;
            anim.SetBool("cyclerun", false);
            anim.SetBool("stickrun", false);
            anim.SetBool("wallrun", true);
            uimanager.instance.powerupArray[4].PlrRepObj.SetActive(false);
            uimanager.instance.powerupArray[5].PlrRepObj.SetActive(false);
        }
        void ChangeLane(int direction)
        {
            SfxManager.instance.PlaySwipe();
            if (tutorialPld)
            {
                if (direction == -1)
                {
                    if (tCount != 0)
                        return;
                }
                else if (direction == 1)
                {
                    if (tCount != 1)
                        return;
                }
                onSwipeTut();
            }
            Vector3 rayPos = cachedTransform.position + cachedTransform.forward * 4;
            rayPos.y = 2.0f;
            if (Physics.Raycast(rayPos, cachedTransform.right * direction, 2f, 1 << 10))
            {
                wallRun(direction);
            }
            else
            {
                int targetLane = m_CurrentLane + direction;
                if (targetLane < 0 || targetLane > 2)
                    return;

                m_CurrentLane = targetLane;
                targetXOffset = (m_CurrentLane - 1) * laneOffset;
                m_PrevieousDir = direction;
                string s = "right";
                if (mFlying)
                {
                    s = direction == 1 ? "gliderRight" : "gliderLeft";
                }
                else if (railRun)
                {
                    s = direction == 1 ? "RailRight" : "RailLeft";
                }
                else if (uimanager.instance.powerupArray[4].isActivated)
                {
                    s = direction == 1 ? "CycleRight" : "CycleLeft";
                }
                else if (uimanager.instance.powerupArray[5].isActivated)
                {
                    s = direction == 1 ? "PogoStickRight" : "PoostickLeft";
                }

                else
                {
                    s = direction == 1 ? "right" : "left";
                }
                anim.SetBool("jump", false);
                anim.SetBool("slide", false);
                anim.Play(s);
            }
        }
        public void ResetLane(bool zonechange = false)
        {
            if(!zonechange)
                rProgress = 0.05f;
             m_CurrentLane = 1;
             m_PrevieousDir = 1;
            targetXOffset = 0.0f;
            offSet = Vector3.zero;
        }
        public void BackToPrevLane()
        {
            ChangeLane(-m_PrevieousDir);
        }
        bool m_Jumping;
        bool m_Sliding;
        public float jumpLength;
        public float minSpeed;
        public float maxSpeed;
        public float laneChangeSpeed;
        public float Acceleration;
        float speedRatio { get { return (m_Speed - minSpeed) / (maxSpeed - minSpeed); } }
        private float m_JumpStart;
        public float m_TotalWorldDistance;
        public int m_Score;
        private float _Y;
        Rigidbody mBody;
        float flyRatio;
        float m_FlyStart;

        public void DoFly()
        {
            uimanager.instance.StopCyclingStickRu();
            mFlying = true;
            m_FlyStart = m_TotalWorldDistance;
            anim.SetBool("glider", true);
            TrackManager._instance.GenerateCoinToFly(rProgress);
            glider.SetActive(true);
            flyingPrtcl.SetActive(true);
            Time.timeScale = 1.5f;
        }

        void Jump()
        {
            if (tutorialPld)
            {
                if (tCount != 2)
                    return;
                onSwipeTut();
            }
            if (!m_Jumping && !inAir && !mFlying)
            {
                SfxManager.instance.PlayJump();
                if (m_Sliding)
                    StopSliding();
                anim.SetInteger("jumpno", Random.Range(0, 3));
                m_JumpStart = m_TotalWorldDistance;
                _Y = cachedTransform.position.y;
                //  mBody.useGravity = false;
                anim.SetBool("jump", true);
                m_Jumping = true;
                camHeightDamping = 0.0f;
                //MissionManager.instance.JumpTrigger();
                uimanager.instance.enemyAnim.SetTrigger("jump");
            }
        }
        void Slide()
        {
            if (tutorialPld)
            {
                if (tCount != 3)
                    return;
                onSwipeTut();
            }
            if (!m_Sliding && !inAir && !mFlying)
            {
                SfxManager.instance.PlaySlide();
                if (m_Jumping)
                    StopJumping();
                float correctSlideLength = slideLength * (1.0f + speedRatio);
                m_SlideStart = m_TotalWorldDistance;
                // mCollieder.radius = 0.2f;
                mCollieder.height = 0.2f;
                mCollieder.center = new Vector3(0, 0.3f, 0);
                m_Sliding = true;
                anim.SetBool("slide", true);
                //MissionManager.instance.RollTrigger();
                if (cachedTransform.position.y < 1&&TrackManager._instance.currentZone!=4)
                    slideParticle.SetActive(true);
            }
        }
        void StopSliding()
        {
            if (m_Sliding)
            {
                anim.SetBool("slide", false);
                m_Sliding = false;
                // mCollieder.radius = 0.3f;
                mCollieder.height = 1.75f;
                mCollieder.center = new Vector3(0, 0.75f, 0);
                slideParticle.SetActive(false);
            }
        }
        public void DoRailRun(bool On)
        {
            anim.SetBool("railrun", On);
            railRun = On;
            railVehicle.SetActive(On);
        }
        public float slideLength;
        private float m_SlideStart;
        private CapsuleCollider mCollieder;
        public void StopJumping()
        {
            if (m_Jumping)
            {
                anim.SetBool("jump", false);
                anim.SetBool("inair", false);
                m_Jumping = false;
                camHeightDamping = 5.0f;
            }
        }
        public void DoCycling(bool ON)
        {
            anim.SetBool("cyclerun", ON);
            float h = ON == true ? 4.0f : 4.5f;
            float s = ON == true ? m_Speed + 3.0f : m_Speed - 3.0f;
            m_Speed = s;
            uimanager.instance.camCntrl.offsetPosition.y = h;
            if (!mFlying)
                flyingPrtcl.SetActive(ON);
        }
        public void DoStickRun(bool ON)
        {
            anim.SetBool("stickrun", ON);
        }

        public void StopFlying()
        {
            Time.timeScale = 1f;
            mFlying = false;
            anim.SetBool("glider", false);
            glider.SetActive(false);
            flyingPrtcl.SetActive(false);
            if (uimanager.instance.powerupArray[4].isActivated)
            {
                uimanager.instance.powerupArray[4].PlrRepObj.SetActive(true);
            }
            Instantiate(gliderSlideOff, cachedTransform.position,cachedTransform.rotation);
        }
        public void EnterDie()
        {
            StopJumping();
            StopSliding();
            anim.SetBool("saveme", false);

            anim.SetBool("inair", false);
            anim.SetBool("right", false);
            anim.SetBool("left", false);
            anim.SetBool("glider", false);
            anim.SetBool("cyclerun", false);
            anim.SetBool("stickrun", false);
            anim.SetBool("die", true);
            
            if (!railRun)
                anim.SetTrigger("dead");
            else
                anim.SetTrigger("raildead");
            slideParticle.SetActive(false);
            for (int i = 0; i < slideParticles.Length; i++)
            {
                slideParticles[i].SetActive(false);
            }
        }
        public void ScoreMulONOFF(bool ON)
        {
            if (ON)
            {
                powMul = 2;
            }
            else
            {
                powMul = 1;
            }
            lastRunMeters = (int)m_TotalWorldDistance;
        }

        float nextTargetDistance = 2000;
        int lastRunMeters = 0;
        void CalculateSpeed()
        {
            float scaledSpeed = m_Speed * Time.deltaTime;
            m_TotalWorldDistance += scaledSpeed;
            if (m_Speed < minSpeed)
                m_Speed += Acceleration * Time.deltaTime;

            else if (m_TotalWorldDistance > nextTargetDistance)
            {
                m_Speed += 2.0f;
                int round = (int)m_TotalWorldDistance / 1000;
                nextTargetDistance += 500 * round;
            }
            if (m_Speed > maxSpeed)
                m_Speed = maxSpeed;
            int rMeters = (int)m_TotalWorldDistance;
            metersTxt.text = "" + rMeters;

            m_Score = rMeters * scoreMul;
            if(powMul != 1)
            {
                int r = rMeters - lastRunMeters;
                r *= 2;
                m_Score += r;
            }
            scoreTxt.text = "" + m_Score;
            int hScore = highScore - m_Score;
            if (hScore < 0)
            {
                hScore = 0;
            }
            highScoreTxt.text = hScore.ToString("000");
            int missionRun = MissionManager.instance.mRun;
            if(missionRun > 0)
            {
                float curRun = MissionManager.instance.runCount += scaledSpeed;
                if(curRun >= missionRun)
                {
                    MissionManager.instance.MissionTrigger(MissionType.RUN);
                    MissionManager.instance.mRun = 0;
                }
            }
            int missionScore = MissionManager.instance.mScore;
            if (missionScore > 0)
            {
                int curScore = MissionManager.instance.scoreCount += (int)(scaledSpeed * scoreMul);
                if(curScore >= missionScore)
                {
                    MissionManager.instance.MissionTrigger(MissionType.SCORE);
                }
            }
        }

        Vector3 offSet = Vector3.zero;
        RaycastHit hit;
        bool mWallRun;
        void AdjustPosWithControlls()
        {
            offSet.x = Mathf.MoveTowards(offSet.x, targetXOffset, laneChangeSpeed * Time.deltaTime);
            cachedTransform.position = cachedTransform.TransformPoint(offSet);
            if (m_Jumping)
            {
                Vector3 jumpVector = cachedTransform.position;
                float _jumpHeight = jumpHeight;
                if (uimanager.instance.powerupArray[5].isActivated)
                {
                    _jumpHeight += 2;
                }
                float correctJumpLength = jumpLength * (1.0f + speedRatio);
                float ratio = (m_TotalWorldDistance - m_JumpStart) / correctJumpLength;
                // _jumpRatio += Time.deltaTime * 1.75f;// *jumpLength;
                if (ratio < 1.0f)
                {
                    jumpVector.y = _Y + (Mathf.Sin(ratio * Mathf.PI) * _jumpHeight);
                    cachedTransform.position = Vector3.MoveTowards(cachedTransform.position, jumpVector, laneChangeSpeed * Time.deltaTime);
                }
                else
                {
                    StopJumping();
                }
            }
            if (m_Sliding)
            {
                float correctSlideLength = slideLength * (1.0f + speedRatio);
                float ratio = (m_TotalWorldDistance - m_SlideStart) / correctSlideLength;

                if (ratio >= 1.0f)
                {
                    StopSliding();
                }
            }
            if (mWallRun)
            {
                Vector3 childPos = anim.transform.localPosition;
                if (Physics.Raycast(transform.position, transform.right * wallDirection, 2f, 1 << 10))
                {
                    childPos.x = Mathf.MoveTowards(childPos.x, wallDirection * 0.5f, Time.deltaTime * 10f);
                    anim.transform.localPosition = childPos;
                }
                else
                {
                    mWallRun = false;
                    anim.transform.localPosition = Vector3.zero;
                    anim.SetBool("wallrun", false);
                }
            }
        }

        protected Vector2 m_StartingTouch;
        [HideInInspector]
        public bool m_IsSwiping = false;
        #if !UNITY_EDITOR
        bool swipeEnd = false;
        float swipeTimer = 0.0f;
        #endif
        void CheckInput()
        {
            #if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ChangeLane(-1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ChangeLane(1);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Slide();
            }
#else
            // Use touch input on mobile
            if (Input.touchCount == 1)
            {
                if (m_IsSwiping)
                {
                    if (swipeEnd)
                    {
                        swipeTimer += Time.deltaTime;
                        if (swipeTimer > 0.5f)
                        {
                            swipeTimer = 0.0f;
                            swipeEnd = false;
                            m_StartingTouch = Input.GetTouch(0).position;
                        }
                    }
                    else
                    {
                        Vector2 diff = Input.GetTouch(0).position - m_StartingTouch;

                        // Put difference in Screen ratio, but using only width, so the ratio is the same on both
                        // axes (otherwise we would have to swipe more vertically...)
                        diff = new Vector2(diff.x / Screen.width, diff.y / Screen.width);

                        if (diff.magnitude > 0.035f) //we set the swip distance to trigger movement to 1% of the screen width
                        {
                            if (Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
                            {
                                if (diff.y < 0)
                                {
                                    Slide();
                                }
                                else
                                {
                                    Jump();
                                }
                            }
                            else
                            {
                                if (diff.x < 0)
                                {
                                    ChangeLane(-1);
                                }
                                else
                                {
                                    ChangeLane(1);
                                }
                            }
                            swipeEnd = true;
                        }
                    }
                }

                // Input check is AFTER the swip test, that way if TouchPhase.Ended happen a single frame after the Began Phase
                // a swipe can still be registered (otherwise, m_IsSwiping will be set to false and the test wouldn't happen for that began-Ended pair)
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    m_StartingTouch = Input.GetTouch(0).position;
                    m_IsSwiping = true;
                    swipeEnd = false;
                    if (Input.GetTouch(0).tapCount == 2)
                    {
                        if (DataManager.instance.Boards > 0&& !uimanager.instance.powerupArray[4].isActivated)
                        {
                            DataManager.instance.Boards = DataManager.instance.Boards - 1;
                            uimanager.instance.PowerUpCollected(4);
                            MissionManager.instance.ShieldUsageTrigger();
                        }
                    }
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    m_IsSwiping = false;
                    swipeEnd = false;
                    swipeTimer = 0.0f;
                }
            }
#endif
        }
        bool landing = false;
        bool inAir = false;
        void MoveInPath()
        {
            if (!currentSpline)
                return;
            Vector3 targetPos;
            targetPos = currentSpline.MoveAlongSpline(ref rProgress, m_Speed * Time.deltaTime);
            Vector3 shadowPos = cachedTransform.position;
            if (!mWallRun && !mFlying)
            {
                if (Physics.Raycast(rayPos.position, -Vector3.up, out hit, 20.0f, floorMask))
                {
                    float distanceToGround = hit.distance;
                    shadowPos.y = hit.point.y + 0.01f;
                    if (!m_Jumping)
                    {
                        if (distanceToGround > 1.25f && !landing)
                        {
                            anim.SetBool("inair", true);
                            landing = true;
                        }
                        else if (landing && distanceToGround <= 1.25f)
                        {
                            landing = false;
                            anim.SetBool("inair", false);
                        }

                        inAir = distanceToGround > 1.15f ? true : false;

                        if (inAir)
                            targetPos.y = Mathf.MoveTowards(cachedTransform.position.y, hit.point.y, Time.deltaTime * 7);// - mCollider.bounds.extents.y;
                        else
                            targetPos.y = hit.point.y;
                    }
                    else
                    {
                        if (distanceToGround < 0.8f)
                        {
                            targetPos.y = hit.point.y;
                            StopJumping();
                        }
                        else
                        {
                            targetPos.y = cachedTransform.position.y;
                            shadowPos.y = shadowT.position.y;
                        }
                    }
                }
                else
                {
                    targetPos.y = cachedTransform.position.y;
                    shadowPos.y = shadowT.position.y;
                }
            }
            else if (mWallRun)
            {
                targetPos.y = Mathf.MoveTowards(cachedTransform.position.y, 2f, Time.deltaTime * 10f);
            }
            else if (mFlying)
            {
                targetPos.y = Mathf.MoveTowards(cachedTransform.position.y, 10f, Time.deltaTime * 5f);
            }
            cachedTransform.position = targetPos;
            Quaternion targetRotation = Quaternion.LookRotation(currentSpline.GetTangent(rProgress));
            cachedTransform.rotation = Quaternion.Lerp(cachedTransform.rotation, targetRotation, rotationLerpModifier * Time.deltaTime);
            shadowT.position = shadowPos;
            if (rProgress >= 1f)
            {
                rProgress = 0;
                changePath();
                canChange = true;
            }
            else if (canChange && rProgress > 0.1f)
            {
                if (advanceSpline.pathType == PATHTYPE.STRIGHT && !advanceSpline.isDestroyble)
                {
                    advanceSpline.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    Destroy(advanceSpline.transform.parent.gameObject);
                }
                TrackManager._instance.PathCompleted();
                canChange = false;
            }
        }
    }
}