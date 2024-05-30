using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;
using System;
using TMPro;
using UnityEngine.UI;

public class ObstracleClass
{
    public ObstracleType _obstracleType;
    public CoinType _coinType;
}
public class TrackManager : MonoBehaviour
{
    public static TrackManager _instance;
    public BezierWalkerWithSpeed playerScript;
    
    public GameObject busThrible,CR_Gate,rhino,elephant;
    public List<TrackSegment> strightPathList = new List<TrackSegment>();
  
    float laneOffSet = 2.5f;
    bool isGeneratingMap = false;
    int currentLevel = 1;
    const int rows = 80;
    const int colums = 3;
    ObstracleClass[] obstracleArray = new ObstracleClass[240];
    Theme currentTheme;
    
    /// <summary>
    /// lists for pooling obstracles
    /// </summary>
    List<GameObject> TruckList = new List<GameObject>();
    List<GameObject> busIdleList = new List<GameObject>();
    List<GameObject> busMoveList = new List<GameObject>();
    List<GameObject> carIdleList = new List<GameObject>();
    List<GameObject> carMoveList = new List<GameObject>();
    List<GameObject> hudleJumpList = new List<GameObject>();
    List<GameObject> hudleDowList = new List<GameObject>();
    List<GameObject> hudleTwoWayList = new List<GameObject>();
    List<GameObject> poleList = new List<GameObject>();
    List<GameObject> coinList = new List<GameObject>();
    List<GameObject> busThribleList = new List<GameObject>();
    List<GameObject> mvFwdBusList = new List<GameObject>();
    List<GameObject> Cr_GateList = new List<GameObject>();

    Vector3 posToPlace = Vector3.zero;
    [Header("splineMaps")]    
    public TrackSegment StrightMap;    
    public TrackSegment LeftPath;   
    public TrackSegment RightPath;  
    public TrackSegment yJun;
    public TrackSegment mapJunction;
    public TrackSegment mapMetro;
    public TrackSegment mapCrane;
    public TrackSegment beachExt;
    public TrackSegment forestSp1;
    public TrackSegment tunnelMap;
    public TrackSegment[] railRushMaps;
    [Header("coins/Particles")]
    public GameObject coinPrefab;    
    public GameObject magnet, scoreMul, coinMul, shield, giftBox, key,wordPrefab,glider,goldBag,Bicycle,PogoStick,tutorialPrefab,endGlider,flashScene,fadeImg;
    public GameObject blastParticle;
    POWERUPTYPE _powerupType;
    public bool canTackeCollision = true;
    GameState gameState;
    public ThemeType themeType;
    [HideInInspector]
    public bool inverse;

    int strightMapLength;
    float lastRunLength = 0;
    int laneLength;
    bool staticMap = true;
    public int currentZone = 0;
    // daily bonus assets \\
    public bool canGiveWrd;
    public DailyBonus dailyBonus;
    public string dayWord;
    public int dayWordIndx;    
    public TextMeshProUGUI WrdTxt;    
    public GameObject boardAnim;
    // ----------------------- \\
    public bool tutorialPld = true;
    public AudioClip inGameClip,menuClip, gameOverClip;
    public int sesstionCount = 0;
    [Header("SkyBoxThings")]
    public Material citySky;
    public Material beachSky;
    public Material CRSky;
    public Material RailMat;
    public Color cityFog,beachFog,CRFog,RailFog;
    public TextMeshProUGUI cLvlTxt;
    public Image dipFade;
    public int NextZone;
    Transform SpawnedObjects;

    public TMP_Text levelNo;

    public List<int> powerupCount;
    public int MapCount = 1;

    public GameState _gameState
    {
        get
        {
            return gameState;
        }
        set
        {
            gameState = value;
            if(gameState == GameState.PAUSE)
            {
                playerScript.anim.enabled = false;
            }else if(gameState == GameState.PLAYING)
            {
                playerScript.anim.enabled = true;
                playerScript.anim.SetBool("play", true);
                playerScript.m_IsSwiping = false;
                MusicPlayer.instance.SetTheme(0, inGameClip);
            }else if(gameState == GameState.HOME)
            {   if (idleroutine != null)
                {
                    StopCoroutine(idleroutine);
                }
                idleroutine = StartCoroutine(PlayIdle());
                uimanager.instance.vaultMenu.GetComponent<GiftBoxCntrl>().doclaimavail();
                MusicPlayer.instance.SetTheme(0, menuClip);
            }
            else if(gameState == GameState.GAMEOVER)
            {
                GameOver();
                MusicPlayer.instance.SetTheme(0, gameOverClip);
             //  FirebaseEvents.instance.LogMulFirebaseEvent("GamveOver", new Firebase.Analytics.Parameter[] { new Firebase.Analytics.Parameter("scoreMultiplier", playerScript.scoreMul), new Firebase.Analytics.Parameter("Score", playerScript.m_TotalWorldDistance) });
            }
            else if(gameState == GameState.SAVEME)
            {
                StartCoroutine(SaveMe());
            }else if(gameState == GameState.FLASH)
            {
                uimanager.instance.camCntrl.tCamera.gameObject.SetActive(false);
                uimanager.instance.camCntrl.flashcamera.gameObject.SetActive(true);
                flashScene.SetActive(true);
                StartCoroutine(FlashDown());
              //  playerScript.ResetPos();
            }else if(gameState == GameState.ZONECHANGE)
            {
              //  StartCoroutine(OnTunnelEnter());
            }
        }
    }
    Coroutine idleroutine;

    IEnumerator PlayIdle()
    {
        PlayerPrefs.SetInt("key", 500);
        int prvNo = 0;
        int timer = 0;
        int targetTime = 20;
        while (true)
        {
            timer += 1;
            if(timer > targetTime)
            {
                int rNo = UnityEngine.Random.Range(0, 4);
                while(rNo == prvNo)
                {
                    rNo = UnityEngine.Random.Range(0, 4);
                }
                prvNo = rNo;
                targetTime = UnityEngine.Random.Range(15, 20);
                timer = 0;
                playerScript.anim.SetInteger("idleno", rNo);   
                
            }
            yield return new WaitForSeconds(0.5f);
            if (prvNo == 1)
            {
                playerScript.anim.SetInteger("idleno", 0);
            }
            if (gameState != GameState.HOME)
            {
                yield break;
            }
        }
    }
    int[] mapArray = new int[] { 1, 2,3,4};
    int mapIndx = 0;
    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        laneOffSet = playerScript.laneOffset;
        mapArray = ShuffleArray(mapArray);
        SpawnedObjects = new GameObject("SpawnedObjects").transform;
    }
    int[] ShuffleArray(int[] arr)
    {
        int size = arr.Length;

        for (int i = 0; i < size; i++)
        {
            int indexToSwap = SkillzCrossPlatform.Random.Range(i, size);
            int oldValue = arr[i];
            arr[i] = arr[indexToSwap];
            arr[indexToSwap] = oldValue;
        }
        return arr;
    }
    public void WordCollected()
    {
        char[] cArray = dayWord.ToCharArray();
        string word = "";
        for(int i= 0; i < cArray.Length; i++)
        {
            if (dayWordIndx >= i)
            {
                word += "<color=green>";
            }
            else
            {
                word += "<color=white>";
            }
            word += cArray[i];
            word += "</color>";
        }
        WrdTxt.text = word;
        uimanager.instance.inGameWordTxt.text = word;
        boardAnim.SetActive(true);
        dayWordIndx += 1;
        if (dayWordIndx >= cArray.Length)
        {
            dailyBonus.WordCompleted();
            canGiveWrd = false;
            //MissionManager.instance.WordHuntTrigger();
        }
    }

    IEnumerator FlashDown()
    {
        currentZone = 0;
        NextZone = 0;
        float animTime = 4.0f;
        yield return StartCoroutine(OnTunnelEnter(false));
        while(animTime > 0)
        {
            animTime -= Time.deltaTime;
            float prg = 1 - (animTime / 4.0f);
           
            float mtrs = 1000 * prg;
            int rMeters = (int)mtrs;
            int m_Score = rMeters * playerScript.scoreMul;
            playerScript.scoreTxt.text = "" + m_Score;
            yield return null;
        }
        playerScript.m_TotalWorldDistance = 1000;
        yield return new WaitForEndOfFrame();
        uimanager.instance.camCntrl.tCamera.gameObject.SetActive(true);
        uimanager.instance.camCntrl.flashcamera.gameObject.SetActive(false);
        flashScene.SetActive(false);
    }

    IEnumerator SaveMe()
    {

        if (!playerScript.railRun)
        {
            Transform _cam = uimanager.instance.camCntrl.tCamera;
            GameObject particle = Instantiate(blastParticle, _cam.transform.position + _cam.transform.forward * 7f, _cam.transform.rotation);
            Collider[] hitColliders = Physics.OverlapSphere(playerScript.transform.position, 35f);
            int i = 0;
            while (i < hitColliders.Length)
            {
                string s = hitColliders[i].tag;
                if (s == "die" || s == "coin")
                    hitColliders[i].gameObject.SetActive(false);
                i++;
            }
            yield return new WaitForSeconds(0.2f);
            Destroy(particle);
        }
        _gameState = GameState.PLAYING;
    }

    public void _SaveMe()
    {
        if (!playerScript.railRun)
        {
            Collider[] hitColliders = Physics.OverlapSphere(playerScript.transform.position, 35f);
            int i = 0;
            while (i < hitColliders.Length)
            {
                string s = hitColliders[i].tag;
                if (s == "die" || s == "coin")
                    hitColliders[i].gameObject.SetActive(false);
                i++;
            }
           // playerScript.anim.SetBool("die", false);
            if (playerScript.currentSpline.name == "cranemap")
            {
                playerScript.rProgress = 0.9f;
                hitColliders = Physics.OverlapSphere(playerScript.transform.position, 25f);
                i = 0;
                while (i < hitColliders.Length)
                {
                    string s = hitColliders[i].tag;
                    if (s == "die" || s == "coin")
                        hitColliders[i].gameObject.SetActive(false);
                    i++;
                }
            }
        }
        else
        {
            playerScript.ResetLane();
        }
        StartCoroutine(WaitAndSaveMe());
    }
    IEnumerator WaitAndSaveMe()
    {
        Transform _cam = uimanager.instance.camCntrl.tCamera;
        GameObject particle = Instantiate(blastParticle, _cam.transform.position + _cam.transform.forward * 5f, _cam.transform.rotation);
        yield return new WaitForSeconds(0.2f);
        _gameState = GameState.PLAYING;
        if (playerScript.railRun)
        {
            playerScript.railVehicle.transform.localPosition = Vector3.zero;
            playerScript.railVehicle.transform.localEulerAngles  = new Vector3(0f,0f,-7.2f);
            playerScript.railVehicle.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        playerScript.anim.SetBool("saveme",true);
        Destroy(particle);

            Time.timeScale = 0.1f;
            while (Time.timeScale < 1)
            {
                Time.timeScale += 0.005f;
                yield return null;
            }
            Time.timeScale = 1;

    }
    void Start()
    {
        Shuffle(powerupCount);
        tutorialPld = DataManager.instance.tutorialPlyd == 1 ? true : false;
        currentTheme = Resources.Load("Data/" + themeType.ToString()) as Theme;
        LoadAsssets();
        AssaignSplineInStart();
        //uimanager.instance.inGameWordTxt.text = dailyBonus.dailyWords[PlayerPrefs.GetInt("wrd")];
    }
    public float progress = 0;
    IEnumerator WaitAndLoadAllObs()
    { 
        yield return(WaitAndLoadObjects(currentTheme.Truck, TruckList, 7));
        progress = 5.0f;
        yield return (WaitAndLoadObjects(currentTheme.BusIdle, busIdleList, 20));
        progress = 10.0f;
        yield return (WaitAndLoadObjects(currentTheme.busMove, busMoveList, 10));
        progress = 15.0f;
        yield return (WaitAndLoadObjects(currentTheme.carIdle, carIdleList, 10));
        progress = 20.0f;
        yield return (WaitAndLoadObjects(currentTheme.carMove, carMoveList, 5));
        progress = 25.0f;
        yield return (WaitAndLoadObjects(currentTheme.HudleJump, hudleJumpList, 15));
        progress = 35.0f;
        yield return (WaitAndLoadObjects(currentTheme.HudleDow, hudleDowList, 15));
        progress = 45.0f; 
        yield return (WaitAndLoadObjects(currentTheme.HudleTwoWay, hudleTwoWayList, 7));
        progress = 50.0f;
        yield return (WaitAndLoadObjects(coinPrefab, coinList, 120));
        progress = 65.0f;
        yield return (WaitAndLoadObjects(currentTheme.Blocker, poleList, 15));
        progress = 70.0f;
        yield return (WaitAndLoadObjects(busThrible, busThribleList, 4));
        progress = 75.0f;
        yield return (WaitAndLoadObjects(busFwd, mvFwdBusList, 8));
        yield return (WaitAndLoadObjects(CR_Gate, Cr_GateList, busThribleList.Count));
        StartCoroutine(WaitPlaceObsInStart());
    }
    IEnumerator WaitPlaceObsInStart()
    {
        //comment by Dharma
        /*canGiveWrd = dailyBonus.CanDoDailyBattle();
        dayWord = dailyBonus.dailyWords[PlayerPrefs.GetInt("wrd")];*/
      /*  if (canGiveWrd)
        {
            uimanager.instance.inGameWordTxt.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            uimanager.instance.inGameWordTxt.transform.parent.gameObject.SetActive(false);
        }*/
        //LoadLevel(playerScript.currentSpline, false, true);
        // yield return new WaitUntil(() => isGeneratingMap == false);
        progress = 90.0f;
        if (tutorialPld)
        {
            LoadLevel(playerScript.nextSpline, false);
            Timer.instance.totalTime = 185;
            Timer.instance.startTimer = true;

        }
        else
        {
           Instantiate(tutorialPrefab, SpawnedObjects);
        }
        yield return new WaitForSeconds(1f);
        progress = 100;
    }
    IEnumerator WaitAndLoadObjects(GameObject prefab, List<GameObject> vehicleList, int count)
    {
        if (prefab != null)
        {
            GameObject g = new GameObject("" + prefab.name);
            for (int i = 0; i < count; i++)
            {
                GameObject pr = Instantiate(prefab, g.transform);
                pr.SetActive(false);
                vehicleList.Insert(i, pr);
                yield return new WaitForFixedUpdate();
            }
        }
    }
    public IEnumerator AnimateAllCoins(bool ON)
    {
        int i = 0;
        while(i < coinList.Count)
        {
            coinList[i].GetComponent<blindRotate>().Split(ON);
            yield return null;
            i++;
        }
    }
    int pathIndex;
    void LoadAsssets()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 pos = -Vector3.forward * 100;
            strightPathList.Add(Instantiate(StrightMap, pos, Quaternion.identity));
            strightPathList[i].gameObject.SetActive(false);
        }
        pathIndex = 3;
        StartCoroutine(WaitAndLoadAllObs());
    }
    void AssaignSplineInStart()
    {
        TrackSegment a = strightPathList[0];
        a.transform.position = Vector3.zero;
        a.transform.rotation = Quaternion.identity;
        playerScript.currentSpline = a.mSpline;

        TrackSegment b = strightPathList[1];
        b.transform.position = Vector3.forward * 80;
        b.transform.rotation = Quaternion.identity;
        playerScript.nextSpline = b.mSpline;

        TrackSegment c = strightPathList[2];
        c.transform.position = Vector3.forward * 160;
        c.transform.rotation = Quaternion.identity;
        playerScript.advanceSpline = c.mSpline;

        a.gameObject.SetActive(true);
        b.gameObject.SetActive(true);
        c.gameObject.SetActive(true);
        pathIndex = 3;
        strightMapLength = 3;
       // a.ma
    }
    public void GameOver()
    {
        if(levelGenerator != null)
        {
            isGeneratingMap = false;
            StopCoroutine(levelGenerator);
        }
        playerScript.DoRailRun(false);
        MakeAllVehicleReset(TruckList, ref truckIndx);
        MakeAllVehicleReset(busIdleList, ref busIndx);
        MakeAllVehicleReset(busMoveList, ref busIndx);
        MakeAllVehicleReset(carIdleList, ref carIdleIndx);
        MakeAllVehicleReset(hudleJumpList, ref jumpIndx);
        MakeAllVehicleReset(hudleDowList, ref dowIndx);
        MakeAllVehicleReset(hudleTwoWayList, ref twowayIndx);
        MakeAllVehicleReset(carMoveList, ref carMoveIndx);
        MakeAllVehicleReset(poleList, ref poleIndx);
        MakeAllVehicleReset(coinList, ref coinIndex);
        MakeAllVehicleReset(busThribleList, ref busThribleIndx);
        MakeAllVehicleReset(mvFwdBusList, ref busFwdIndx);
        MakeAllVehicleReset(Cr_GateList, ref busThribleIndx);

        for (int i = 0; i < strightPathList.Count; i++)
        {
            Vector3 pos = -Vector3.forward * 100;
            strightPathList[i].gameObject.SetActive(false);
        }
        strightPathList[0].MakeMapVisible(false);
        strightPathList[0].gameObject.SetActive(true);
        strightPathList[1].gameObject.SetActive(true);
        uimanager.instance.camCntrl.introScene.SetActive(true);
        ChangeZoneOnGameOver();
        strightMapLength = 0;
        lastRunLength = 0;
        laneLength = 0;
        powerupFreequency = 150.0f;
        staticMap = true;
        isGeneratingMap = false;
        uimanager.instance.camCntrl.Restart();
        playerScript.transform.position = new Vector3(-2.5f,0.2f,0);
        playerScript.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
        playerScript.shadowT.position = new Vector3(-2.5f, 0.15f, 0);
        playerScript.anim.SetBool("play", false);
        playerScript.anim.SetBool("die", false);
        playerScript.anim.SetBool("railrun", false);
        playerScript.railRun = false;
        AssaignSplineInStart();
        foreach (Transform child in SpawnedObjects)
        {
            Destroy(child.gameObject);
        }
        GC.Collect();
        StartCoroutine(WaitPlaceObsInStart());
    }
    void ChangeZoneOnGameOver()
    {
        NextZone = 0;
        if(currentZone != 0)
        {
            currentZone = 0;
            StartCoroutine(ChangeZone()); 
            StartCoroutine(WaitChangeZoneObs());
            ChangeSky();
        }
        mapIndx = 0;
        mapArray = ShuffleArray(mapArray);
    }
    void MakeAllVehicleReset(List<GameObject> vehList,ref int t_Index)
    {
        for (int i = 0; i < vehList.Count; i++)
        {
            vehList[i].SetActive(false);
        }
        t_Index = 0;
    }
    public IEnumerator OnTunnelEnter(bool flash = true)
    {
        if (isGeneratingMap && levelGenerator != null)
        {
            isGeneratingMap = false;
            StopCoroutine(levelGenerator);
        }
        float progress = 0.0f;
        if (flash)
        {
            dipFade.gameObject.SetActive(true);
            while (progress < 1.0f)
            {
                progress += Time.deltaTime * 3;
                dipFade.color = Color.Lerp(new Color(1, 1, 1, 0), Color.cyan, progress);
                yield return null;
            }
        }
        gameState = GameState.ZONECHANGE;
  
        playerScript.DoRailRun(false);
        MakeAllVehicleReset(TruckList, ref truckIndx);
        yield return new WaitForEndOfFrame();
        MakeAllVehicleReset(busIdleList, ref busIndx);
        yield return new WaitForEndOfFrame();
        MakeAllVehicleReset(busMoveList, ref busIndx);
        yield return new WaitForEndOfFrame();
        MakeAllVehicleReset(carIdleList, ref carIdleIndx);
        yield return new WaitForEndOfFrame();
        MakeAllVehicleReset(hudleJumpList, ref jumpIndx);
        yield return new WaitForEndOfFrame();
        MakeAllVehicleReset(hudleDowList, ref dowIndx);
        yield return new WaitForEndOfFrame();
        MakeAllVehicleReset(hudleTwoWayList, ref twowayIndx);
        yield return new WaitForEndOfFrame();
        MakeAllVehicleReset(carMoveList, ref carMoveIndx);
        yield return new WaitForEndOfFrame();
        MakeAllVehicleReset(poleList, ref poleIndx);
        yield return new WaitForEndOfFrame();
        MakeAllVehicleReset(coinList, ref coinIndex);
        yield return new WaitForEndOfFrame();
        MakeAllVehicleReset(busThribleList, ref busThribleIndx);
        yield return new WaitForEndOfFrame();
        MakeAllVehicleReset(mvFwdBusList, ref busFwdIndx);
        yield return new WaitForEndOfFrame();
        MakeAllVehicleReset(Cr_GateList, ref busThribleIndx);
        yield return new WaitForEndOfFrame();
        SetZoneValue();
        yield return new WaitForEndOfFrame();
        foreach (Transform t in SpawnedObjects)
        {
            if (t.tag == "powerup" || t.tag == "word" || t.tag == "selfdeath" || t.tag == "die")
            {
                Destroy(t.gameObject);
            }
        }
        yield return new WaitForEndOfFrame();
        ChangeSky();
        playerScript.rProgress = 0.9f;
        playerScript.ResetLane(true);
        TrackSegment trckSeg = playerScript.currentSpline.transform.parent.GetComponent<TrackSegment>();
        if (trckSeg)
        {
            trckSeg.Start();
        }
        yield return new WaitForEndOfFrame();
        gameState = GameState.PLAYING;
        progress = 0.0f;
        if (flash)
        {
            while (progress < 1.0f)
            {
                progress += Time.deltaTime * 2;
                dipFade.color = Color.Lerp(Color.cyan, new Color(1, 1, 1, 0), progress);//  new Color(1, 1, 1, Mathf.Lerp(1.0f, 0.0f, progress));
                yield return null;
            }
            dipFade.gameObject.SetActive(false);
        }
    }

    public void PathCompleted()
    {
        Transform endPoint = playerScript.nextSpline.endPoints[playerScript.nextSpline.endPoints.Count - 1].transform;
        TrackSegment nextElement = GetTrack();

        nextElement.transform.position = endPoint.position;
        nextElement.transform.rotation = endPoint.rotation;
        playerScript.advanceSpline = nextElement.mSpline;
        nextElement.gameObject.SetActive(true);
        
        LoadLevel(playerScript.nextSpline, nextElement.isYjun);
    }
    int lastZone;
    public void SetZoneValue()
    {
        if(currentZone == 4)
        {
            if (playerScript.nextSpline.pathType == PATHTYPE.FIXED)
            {
                GameObject g = playerScript.nextSpline.transform.parent.gameObject;
                Destroy(g);
            }
            if (playerScript.advanceSpline.pathType == PATHTYPE.FIXED)
            {
                GameObject g = playerScript.advanceSpline.transform.parent.gameObject;
                Destroy(g);
            }

            TrackSegment map = strightPathList[0];
            playerScript.nextSpline = map.mSpline;
            Transform endPoint = playerScript.currentSpline.endPoints[playerScript.currentSpline.endPoints.Count - 1].transform;
            map.transform.position = endPoint.position;
            map.transform.rotation = endPoint.rotation;
            map.gameObject.SetActive(true);

            TrackSegment map2 = strightPathList[1];
            playerScript.advanceSpline = map2.mSpline;
            endPoint = playerScript.nextSpline.endPoints[playerScript.nextSpline.endPoints.Count - 1].transform;
            map2.transform.position = endPoint.position;
            map2.transform.rotation = endPoint.rotation;
            map2.gameObject.SetActive(true);
            pathIndex = 2;
        }
        // set currentzone
        currentZone = NextZone;
        ////////TestCase
        //currentZone = 4;
        if (currentZone != 4)
        {
            StartCoroutine(ChangeZone());
            StartCoroutine(WaitChangeZoneObs());
        }
        else
        {
            playerScript.nextSpline.transform.parent.gameObject.SetActive(false);
            TrackSegment map = Instantiate(railRushMaps[0],SpawnedObjects);
            playerScript.nextSpline = map.mSpline;
            Transform endPoint = playerScript.currentSpline.endPoints[playerScript.currentSpline.endPoints.Count - 1].transform;
            map.transform.position = endPoint.position;
            map.transform.rotation = endPoint.rotation;
            playerScript.advanceSpline.transform.parent.gameObject.SetActive(false);
            TrackSegment map2 = Instantiate(railRushMaps[UnityEngine.Random.Range(1, railRushMaps.Length)],SpawnedObjects);
            playerScript.advanceSpline = map2.mSpline;
            endPoint = playerScript.nextSpline.endPoints[playerScript.nextSpline.endPoints.Count - 1].transform;
            map2.transform.position = endPoint.position;
            map2.transform.rotation = endPoint.rotation;
            uimanager.instance.StopCyclingStickRu();
            playerScript.DoRailRun(true);
        }
        playerScript.slideParticle.SetActive(false);
        playerScript.slideParticle = playerScript.slideParticles[currentZone];
        lastZone = currentZone;
    }
    public void ChangeSky()
    {
        RenderSettings.fogStartDistance = 50;
        RenderSettings.fogEndDistance = 60;
        GetComponent<WorldCurver>().curveStrength = 0.001f;

        if (currentZone == 0)
        {
            RenderSettings.skybox = citySky;
            RenderSettings.fogColor = cityFog;
        }else if(currentZone == 1 || currentZone == 3)
        {
            RenderSettings.skybox = beachSky;
            RenderSettings.fogColor = beachFog;
        }else if(currentZone == 2)
        {
            RenderSettings.skybox = CRSky;
            RenderSettings.fogColor = CRFog;
        }else if(currentZone == 4)
        {
            RenderSettings.fogStartDistance = 0;
            RenderSettings.fogEndDistance = 55;
            RenderSettings.skybox = RailMat;
            RenderSettings.fogColor = RailFog;
            GetComponent<WorldCurver>().curveStrength = 0.0f;
        }
        GetComponent<WorldCurver>().OnEnable();
    }

    IEnumerator WaitChangeZoneObs()
    {
        ChangeZoneObs(poleList);
        yield return new WaitForFixedUpdate();
        ChangeZoneObs(busIdleList);
        yield return new WaitForFixedUpdate();
        ChangeZoneObs(busMoveList);
        yield return new WaitForFixedUpdate();
        ChangeZoneObs(hudleDowList);
        yield return new WaitForFixedUpdate();
        ChangeZoneObs(hudleTwoWayList);
        yield return new WaitForFixedUpdate();
        ChangeZoneObs(hudleJumpList);
        yield return new WaitForFixedUpdate();
        ChangeZoneObs(carIdleList);
        yield return new WaitForFixedUpdate();
        ChangeZoneObs(carMoveList);
       /* if(currentZone == 3)
        {
            for(int i = 0; i < Cr_GateList.Count; i++)
            {
                Cr_GateList[i].transform.GetChild(0).gameObject.SetActive(true);
                Cr_GateList[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        else if(currentZone == 2)
        {
            for (int i = 0; i < Cr_GateList.Count; i++)
            {
                Cr_GateList[i].transform.GetChild(0).gameObject.SetActive(false);
                Cr_GateList[i].transform.GetChild(1).gameObject.SetActive(true);
            }
        }*/
    }
    void ChangeZoneObs(List<GameObject> obsL)
    {
        for (int i = 0; i < obsL.Count; i++)
        {
            ObstraclePiece OP = obsL[i].GetComponent<ObstraclePiece>();
            if (OP)
                OP.ChangeZone();
        }
    }
    IEnumerator ChangeZone()
    {
        for (int i = 0; i < strightPathList.Count; i++)
        {
            strightPathList[i].Start();
            yield return new WaitForFixedUpdate();
        }
    }
    void setNextZone()
    {
        if (mapIndx >= mapArray.Length)
        {
            mapIndx = 0;
            NextZone = 0;
            mapArray = ShuffleArray(mapArray);
        } 
        else
        {
            NextZone = mapArray[mapIndx];
            mapIndx++;
        }
        
    }
    TrackSegment GetTrack()
    {
        TrackSegment nextTrack = null;
        if (lastRunLength >= currentTheme.zones[currentZone].Length)
        {
            lastRunLength = 0;
            Debug.Log("create Tunnel Here");
            //commented by dharma here instantiate tunnel
            nextTrack = Instantiate(tunnelMap,SpawnedObjects);
            strightMapLength = 0;
            setNextZone();
            while (NextZone == currentZone)
            {
                setNextZone();
            }
        }
        else if (currentZone == 4)
        {
            nextTrack = Instantiate(railRushMaps[UnityEngine.Random.Range(0, railRushMaps.Length)],SpawnedObjects);
        }
        else
        {
            if (strightMapLength > 7 && currentZone != 4 && currentZone != 1 && currentZone != 3)
            {             
                strightMapLength = 0;
                if(currentZone == 0)
                {
                    //change to skillz random here
                    float rand = SkillzCrossPlatform.Random.Range(0f,1f);
                    //float rand = 0.4f;

                    if (rand < 0.25f)
                    {
                        nextTrack = Instantiate(mapJunction, SpawnedObjects);
                    }
                    else if (rand <= 0.5f)
                    {
                        nextTrack = Instantiate(mapMetro, SpawnedObjects);
                    }
                    else if(rand <= 0.75f)
                    {
                        nextTrack = Instantiate(LeftPath, SpawnedObjects);
                    }else if(rand <= 1.0f)
                    {
                        nextTrack = Instantiate(RightPath, SpawnedObjects);
                    }
                    else
                    {
                        nextTrack = Instantiate(yJun, SpawnedObjects);
                    }
                }
                else if(currentZone == 2)
                {
                    nextTrack = Instantiate(mapCrane, SpawnedObjects);
                }else if(currentZone == 3)
                {
                    nextTrack = Instantiate(forestSp1,SpawnedObjects);
                }
            }
            else
            {
                if (pathIndex >= strightPathList.Count)
                {
                    pathIndex = 0;
                }
                nextTrack = strightPathList[pathIndex];
                pathIndex++;
                strightMapLength++;
            }
        }
        lastRunLength += 80;
        return nextTrack;
    }

    int pLvl = 0;
    int c = 80;
    int SelectLevel()
    {
        firstRunObs = false;
        int lvlNo = 0;

        if (currentZone == 1)
        {
            lvlNo = SkillzCrossPlatform.Random.Range(43, 58);
        }
        else if(currentZone == 2)
        {
            lvlNo = SkillzCrossPlatform.Random.Range(58, 81);
        }else if(currentZone == 3)
        {
            lvlNo = SkillzCrossPlatform.Random.Range(81, 104);
        }else if(currentZone == 0)
        {
            lvlNo = SkillzCrossPlatform.Random.Range(1, 104);

            /*lvlNo = c;
            c++;
            if (c >= 104)
                c = 80;*/

            /*if (laneLength >= 700)
            {
                staticMap = !staticMap;
                if (!staticMap)
                    firstRunObs = true;
                laneLength = 0;
            }
            if (staticMap)
            {
                if(playerScript.m_TotalWorldDistance < 1000)
                {
                    lvlNo = SkillzCrossPlatform.Random.Range(1, 7);
                }
                else
                {
                    lvlNo = SkillzCrossPlatform.Random.Range(1, 31);
                }
            }
            else
            {
                lvlNo = SkillzCrossPlatform.Random.Range(31, 43);
            }
            laneLength += 80;
            while (lvlNo == pLvl)
            {
                if (staticMap)
                {
                    lvlNo = SkillzCrossPlatform.Random.Range(1, 31);
                }
                else
                {
                    lvlNo = SkillzCrossPlatform.Random.Range(31, 43);
                }
            }*/
        }
        Debug.Log("NextLevelNo::" + lvlNo);
        levelNo.text = lvlNo.ToString();
        pLvl = lvlNo;
        return lvlNo;
    }
    bool firstRunObs = false;
    Coroutine levelGenerator;
    void LoadLevel(BezierSpline spline,bool isYjun)
    {
        if (isGeneratingMap && levelGenerator != null)
        {
            isGeneratingMap = false;
            StopCoroutine(levelGenerator);
        }
        if (spline.isTunnel || playerScript.mFlying || !tutorialPld)
            return;
        else if (spline.pathType != PATHTYPE.STRIGHT && spline.pathType != PATHTYPE.JUNCTION)
        {
            if ((spline.pathType == PATHTYPE.LEFT || spline.pathType == PATHTYPE.RIGHT) && !spline.isYjun)
            {
                StartCoroutine(_MapToTurn(spline));
            }
            return;
        }
        isGeneratingMap = true;
        currentLevel = SelectLevel();
        if (currentLevel == 0)
            currentLevel = 1;
        LoadDataFromLocal(currentLevel);
        bool halfPart = false;
        if (playerScript.currentSpline.pathType == PATHTYPE.JUNCTION && !staticMap)
        {
            halfPart = true;
        }
        halfPart = halfPart || firstRunObs;
        levelGenerator = StartCoroutine(GenerateObstracles(spline,isYjun,halfPart));
       // spline.mapNo = currentLevel;
    }
    
    public void LoadDataFromLocal(int currentLevel)
    {
        //Read data from text file
        TextAsset mapText = Resources.Load("Levels/" + currentLevel) as TextAsset;
        if (mapText == null)
        {
            mapText = Resources.Load("Levels/" + 1) as TextAsset;
        }
        ProcessGameDataFromString(mapText.text);
    }
    void ProcessGameDataFromString(string mapText)
    {
        string[] lines = mapText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        int mapLine = 0;
        obstracleArray = new ObstracleClass[rows * colums];
        for (int i = 0; i < obstracleArray.Length; i++)
        {
            ObstracleClass obs = new ObstracleClass();
            obs._obstracleType = ObstracleType.NONE;
            obstracleArray[i] = obs;
        }
        foreach (string line in lines)
        {
            string[] st = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < st.Length; i++)
            {
                string _value = st[i].ToString();
                obstracleArray[mapLine * colums + i]._coinType = (CoinType)int.Parse(_value[0].ToString());
                obstracleArray[mapLine * colums + i]._obstracleType = (ObstracleType)int.Parse(_value.Substring(1, _value.Length - 1).ToString());
            }
            mapLine++;
        }
    }
    GameObject GetPoweUp()
    {
        GameObject p = null;
        float mul = canGiveWrd == true ? 1.5f : 1f;      
        float rand = UnityEngine.Random.value;
        rand = rand * mul;
            if (rand <= .15f)
                p = Instantiate(magnet);
            else if (rand <= .2f)
                p = Instantiate(coinMul);
            else if (rand <= .3f)
                p = Instantiate(scoreMul);
            else if (rand <= .5f)
                p = Instantiate(Bicycle);
            else if (rand <= .55f)
                p = Instantiate(PogoStick);
            else if (rand <= .6f)
                p = Instantiate(key);
            else if (rand <= .65f)
                p = Instantiate(shield);
            else if (rand <= .7f)
                p = Instantiate(giftBox);
            else if (rand <= .9f)
            {
                p = Instantiate(glider);
            }
            else if (rand <= 1.0f)
                p = Instantiate(goldBag);
            else
                p = Instantiate(wordPrefab);
        p.transform.SetParent(SpawnedObjects);
        return p;
    }
    float xPos;
    IEnumerator GenerateObstracles(BezierSpline currentSpline,bool isYjun,bool halfPart)
    {
        float incrementParts = 1.0f / 80.0f;   // 80 is the array count for obstracle that is fixed for any level
        float progress = 0.0f;
        for (int row = 0; row < rows; row++)
        {
            if (progress >= 1)
                break;
            if (currentSpline.pathType == PATHTYPE.JUNCTION && progress >= 0.3f)
                break;
            if (isYjun && progress > 0.7f)
                break;        
            xPos = -laneOffSet;          
            for (int col = 0; col < colums; col++)
            {
                if (halfPart && progress < 0.5f)
                {
                    continue;
                }
                posToPlace = currentSpline.GetPoint(progress);
                Quaternion targetRotation = Quaternion.LookRotation(currentSpline.GetTangent(progress));
                ObstracleClass obs = obstracleArray[row * colums + col];
                if (!halfPart && obs._coinType != CoinType.None)
                {
                    SpawnCoinPrefab(obs, posToPlace, targetRotation, xPos);
                }
                if (obs._obstracleType != ObstracleType.NONE)
                {
                    int no = (int)obstracleArray[row * colums + col]._obstracleType;
                    GameObject pref = getSpwanPrefab(no);
                    if (pref != null)
                    {
                        pref.transform.position = posToPlace;
                        pref.transform.rotation = targetRotation;
                        pref.transform.position = pref.transform.position + pref.transform.right * xPos;
                        
                        pref.SetActive(true);
                    }
                    yield return new WaitForFixedUpdate();
                }
                xPos += laneOffSet;
            }
            progress += incrementParts;
           // posToPlace.z += 1;
        }
        isGeneratingMap = false;
    }
    float powerupFreequency = 150;
    void SpawnCoinPrefab(ObstracleClass obs,Vector3 position,Quaternion rotation,float _xPos)
    {
        Vector3 posSpawn = position;
        if(obs._coinType == CoinType.PowerUp)
        {
            if(playerScript.m_TotalWorldDistance > powerupFreequency)
            {
                /*GameObject poweUp = GetPoweUp();
                if(poweUp != null)
                {
                    poweUp.transform.position = position;
                    poweUp.transform.rotation = rotation;
                    poweUp.transform.position = poweUp.transform.position + poweUp.transform.right * xPos;
                    if (playerScript.m_TotalWorldDistance < 2000)
                        powerupFreequency = playerScript.m_TotalWorldDistance + UnityEngine.Random.Range(450, 550);
                    else 
                        powerupFreequency = playerScript.m_TotalWorldDistance + UnityEngine.Random.Range(700, 900);
                } */           
            }           
        }  
        else if (obs._coinType == CoinType.Up)
        {
            posSpawn.y = 3.3f;
            _spawnCoin(posSpawn,rotation,_xPos);
        }else if(obs._coinType == CoinType.Down)
        {
            posSpawn.y = 0.1f;
            _spawnCoin(posSpawn, rotation, _xPos);
        }else if(obs._coinType == CoinType.OnCar)
        {
            posSpawn.y = 2f;
            _spawnCoin(posSpawn, rotation, _xPos);
        }
        if(obs._coinType == CoinType.CurveDown || obs._coinType == CoinType.CurveUp)
        {
            if(obs._coinType == CoinType.CurveUp)
                posSpawn.y = 3.3f;         
            for (int i= 0; i<9; i++)
            {
                _spawnCoin(posSpawn, rotation, _xPos, i);
                if (i < 4)
                {
                    posSpawn.y += 0.6f;
                }else if(i >= 4)
                {
                    posSpawn.y -= 0.6f;
                }                
            }
        } 
    }
    int coinIndex = 0;
    void _spawnCoin(Vector3 posSpawn,Quaternion rotation,float _xPos,int _zPos = 0)
    {
        Transform tCoin = coinList[coinIndex].transform;
        tCoin.position = posSpawn;
        tCoin.rotation = rotation;
        tCoin.position = tCoin.position + tCoin.right * xPos;
        if (_zPos != 0)
            tCoin.position = tCoin.position + tCoin.forward * _zPos*2f;
        tCoin.gameObject.SetActive(true);
        coinIndex++;
        if (coinIndex >= coinList.Count-1)
            coinIndex = 0;
    }
    public void GenerateCoinToFly(float rProgress)
    {
        StartCoroutine(_MapToFly(rProgress));
    }
    IEnumerator _MapToFly(float rProgress)
    {
        BezierSpline curSpline = playerScript.currentSpline;
        BezierSpline nextSp = playerScript.nextSpline;
        BezierSpline advSp = playerScript.advanceSpline;
        float incrementParts = (1.0f / 80.0f);
        incrementParts += incrementParts;
        float length = 0.0f;
        float targetLength = playerScript.flyLength / 100;
        int coinCount = 0;
        float targetX = 0.0f;
        float curX = targetX;
        float startX = curX;
        float parts_ = 0.0f;
        rProgress += 0.4f;
        Vector3 pos = Vector3.zero;
        Transform tCoin = null;
        Quaternion targetRotation = Quaternion.identity;
        while (length < targetLength)
        {
            if (coinCount % 15 == 0)
            {
                if (UnityEngine.Random.Range(0, 10) < 6)
                {
                    targetX += 2.5f;
                }
                else
                {
                    targetX -= 2.5f;
                }
                if (targetX < -2.5f || targetX > 2.5f)
                {
                    targetX = 0.0f;
                }
            }
            rProgress += incrementParts;
            length += incrementParts;
            if (rProgress > 1.0f)
            {
                if (curSpline == nextSp)
                {
                    yield return new WaitUntil(() => (playerScript.nextSpline == advSp && playerScript.rProgress > 0.4f));
                    nextSp = playerScript.nextSpline;
                    advSp = playerScript.advanceSpline;
                }
                curSpline = nextSp;
                rProgress = rProgress - 1.0f;
            }
            tCoin = coinList[coinIndex].transform;
            pos = curSpline.GetPoint(rProgress);
            targetRotation = Quaternion.LookRotation(curSpline.GetTangent(rProgress));
            pos.y = 10.0f;
            coinCount += 1;

            tCoin.position = pos;
            tCoin.rotation = targetRotation;
            tCoin.position = tCoin.position + tCoin.right * curX;
            tCoin.gameObject.SetActive(true);
            if (curX != targetX)
            {
                parts_ += 0.25f;
                if (parts_ >= 1)
                {
                    curX = targetX;
                    parts_ = 0.0f;
                    startX = targetX;
                }
                curX = Mathf.Lerp(startX, targetX, parts_);
            }

            coinIndex++;
            if (coinIndex >= coinList.Count - 1)
                coinIndex = 0;
            yield return new WaitForFixedUpdate();
        }
        Instantiate(endGlider, tCoin.position, tCoin.rotation);
    }
    IEnumerator _MapToTurn(BezierSpline spline)
    {
        float incrementParts = (1.0f / 80.0f);
        incrementParts += incrementParts; 
        float _x = 0.0f;

        Transform tCoin = null;
        int coinCount = 0;
        float progress = 0.0f;
        while (progress < 1)
        {
            if (coinCount % 8 == 0)
            {
                float pX = _x;
                while (pX == _x)
                {
                    int r = UnityEngine.Random.Range(-1, 2);
                    _x = r * 2.5f;
                }
                progress += 0.05f;
            }
            progress += incrementParts;

            tCoin = coinList[coinIndex].transform;
            Vector3 pos = spline.GetPoint(progress);
            Quaternion targetRotation = Quaternion.LookRotation(spline.GetTangent(progress));
            pos.y = 0.0f;
            coinCount += 1;
            tCoin.position = pos;
            tCoin.rotation = targetRotation;
            tCoin.position = tCoin.position + tCoin.right * _x;
            tCoin.gameObject.SetActive(true);
            coinIndex++;
            if (coinIndex >= coinList.Count - 1)
                coinIndex = 0;
            yield return new WaitForFixedUpdate();
        }
    }
    int truckIndx,busIndx,jumpIndx,dowIndx,twowayIndx,carIdleIndx,poleIndx,carMoveIndx,busMoveIndx,busThribleIndx,busFwdIndx = 0;
    public GameObject busFwd;
    GameObject getSpwanPrefab(int no)
    {
        GameObject spawnPrefab = null;
        if (no == 0)
        {
            return null;
        }
        else if (no == 1)
        {
            spawnPrefab = TruckList[truckIndx];
            truckIndx++;
            if (truckIndx >= TruckList.Count)
            {
                truckIndx = 0;
            }

        }
        else if (no == 2)
        {
            spawnPrefab = busIdleList[busIndx];
            busIndx++;
            if (busIndx >= busIdleList.Count)
            {
                busIndx = 0;
            }
        }
        else if (no == 3)
        {
            if(currentZone == 3)
            {
                if (elephant)
                {
                    spawnPrefab = Instantiate(elephant, SpawnedObjects);
                }
            }
            else
            {
                spawnPrefab = busMoveList[busMoveIndx];
                busMoveIndx++;
                if (busMoveIndx >= busMoveList.Count)
                    busMoveIndx = 0;
                MovingBus bus = spawnPrefab.GetComponent<MovingBus>();
                if (bus)
                {
                    bus.check = false;
                }
            }      
        }
        else if (no == 4)
        {
            spawnPrefab = carIdleList[carIdleIndx];
            carIdleIndx++;
            if (carIdleIndx >= carIdleList.Count)
                carIdleIndx = 0;
        }
        else if (no == 5)
        {
            if(currentZone == 3)
            {
                if (rhino)
                {
                    spawnPrefab = Instantiate(rhino, SpawnedObjects);
                }
            }
            else
            {
                spawnPrefab = carMoveList[carMoveIndx];
                carMoveIndx++;
                if (carMoveIndx >= carMoveList.Count)
                    carMoveIndx = 0;
                MovingBus bus = spawnPrefab.GetComponent<MovingBus>();
                if (bus)
                {
                    bus.check = false;
                }
            }
            
        }
        else if (no == 6)
        { 
            spawnPrefab = hudleJumpList[jumpIndx];
            jumpIndx++;
            if (jumpIndx >= hudleJumpList.Count)
            {
                jumpIndx = 0;
            }
        }
        else if (no == 7)
        {
            spawnPrefab = hudleDowList[dowIndx];
            dowIndx++;
            if (dowIndx >= hudleDowList.Count)
            {
                dowIndx = 0;
            }
        }
        else if (no == 8)
        {
            spawnPrefab = hudleTwoWayList[twowayIndx];
            twowayIndx++;
            if (twowayIndx >= hudleTwoWayList.Count)
            {
                twowayIndx = 0;
            }
        }
        else if (no == 10)
        {

            spawnPrefab = poleList[poleIndx];
            poleIndx++;
            if (poleIndx >= poleList.Count)
                poleIndx = 0;
        }else if (no == 11)
        {
            spawnPrefab = Cr_GateList[busThribleIndx];
            busThribleIndx++;
            if (busThribleIndx >= busThribleList.Count)
            {
                busThribleIndx = 0;
            }
        }
        else if (no == 13)
        {
            spawnPrefab = mvFwdBusList[busFwdIndx];
            busFwdIndx++;
            if (busFwdIndx >= mvFwdBusList.Count)
                busFwdIndx = 0;
        }
        else if (no == 14)
        {
            spawnPrefab = busThribleList[busThribleIndx];
            busThribleIndx++;
            if (busThribleIndx >= busThribleList.Count)
            {
                busThribleIndx = 0;
            }
            MovingBus bus = spawnPrefab.GetComponent<MovingBus>();
            if (bus)
            {
                bus.check = false;
            }
        }
        return spawnPrefab;
    }
    
    public IEnumerator roateCamera(float targetY)
    {
        Transform _cam = uimanager.instance.camCntrl.tCamera;
        Quaternion startRotation = _cam.localRotation;
        Quaternion endRotation = Quaternion.Euler(0, targetY, 0);
        float camX = CameraController.XOffSet;
        float rotationProgress = 0;
        while (rotationProgress <= 1.0f)
        {
            rotationProgress += Time.deltaTime *playerScript.m_Speed/25.0f;
            _cam.localRotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress);
            CameraController.XOffSet = Mathf.Lerp(camX, 0f, rotationProgress);
            yield return null;
        }
        rotationProgress = 0.0f;
        yield return new WaitForSeconds(3f - playerScript.m_Speed /25.0f);
        startRotation = _cam.localRotation;
        endRotation = Quaternion.identity;
        while (rotationProgress <= 1.0f)
        {
            rotationProgress += Time.deltaTime * playerScript.m_Speed / 25.0f;
            _cam.localRotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress);
            CameraController.XOffSet = Mathf.Lerp(camX, 0.3f, rotationProgress);
            yield return null;
        }
        CameraController.XOffSet = 0.3f;
        _cam.localRotation = Quaternion.identity;
        _cam.localPosition = Vector3.zero;
    }
    public static void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
public enum ObstracleType
{
  NONE = 0,
  Truck,
  BusIdle,
  busMove,
  carIdle,
  carMove,
  HudleJump,
  HudleDow,
  HudleTwoWay,
  Block,
  Blocker,
  Gate,
  CaveLeft,
  BusFwd,
  BusThrible
}
public enum CoinType
{
    None,
    Down,
    Up,
    CurveDown,
    CurveUp,
    PowerUp,
    Clear,
    OnCar
}
public enum POWERUPTYPE
{
    MAGNET,
    DOUBLETHECOIN,
    DOUBLETHESCORE,
    SPOOKYMODE,
    BICYCLE,
    POGOSTICK,
    GIFTBOX,
    key,
    FLIGHT,
    GOLDBAG,
}
public enum RewardVideoType
{
    SaveMe,
    DoubleIt,
    BuyVault,
    decVaultTime
}
public enum GameState
{
    HOME,
    REPLAY,
    PLAYING,
    PAUSE,
    PREGAMEOVER,
    GAMEOVER,
    HIGHSOCRE,
    SAVEME,
    TUTORIAL,
    FLASH,
    ZONECHANGE
}
[Serializable]
public class Zone
{
    public string name;
    public int Length;
    public bool isContineous = false;
    public GameObject[] envAssets;
}
public enum ThemeType
{
    CITY,
    FOREST,
    DESERT,
}
public enum MapType
{
    COLONY,
    BEACH,
}



