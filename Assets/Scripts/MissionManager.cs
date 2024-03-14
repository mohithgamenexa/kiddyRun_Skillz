using UnityEngine;
public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;
    public MissionData data;
    int jumpCount;
    int rollCount;
    int powerupCount;
    int magentCount;
    int wordCount;
    int cycleCount;
    int usageBoardCount;
    int keyCount;
    int jetPackCount;
    int boxCount;
    public int CoinCount;
    public float runCount;
    public int scoreCount;

    public int mCoin, mRun, mJump, mPow, mKey, mJetPack, mBox, mRoll, mWordHunt, mMagnet, mCycle, mScore, mPurShield;

    public MissionHolder[] missionArray, pauseMission;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void OnMissionPnlAppear()
    {
        if (CurrentSet < data.missionSet.Length)
        {
            for (int i = 0; i < missionArray.Length; i++)
            {
                missionArray[i].Reset();
            }
            uimanager.instance.sMulTxtMission.text = CurrentSet + 1 + "X";
            missionCmpltdLbl.SetActive(false);
        }
        else
        {
            missionCmpltdLbl.SetActive(true);
        }

    }
    public void OnPausePnlAppear()
    {

        if (CurrentSet < data.missionSet.Length)
        {
            for (int i = 0; i < pauseMission.Length; i++)
            {
                pauseMission[i].Reset();
            }
            missionCmpltdLblPause.SetActive(false);
        }
        else
        {
            missionCmpltdLblPause.SetActive(true);
        }

    }

    public int isMissionCmpltd(int mNo)
    {
        return PlayerPrefs.GetInt("sM" + mNo);
    }
    public void SetMissionCmpltd(int mNo, bool animate = true)
    {
        if (animate)
            StartCoroutine(uimanager.instance.ShowInfo("Mission Completed", 0.0f));
        PlayerPrefs.SetInt("sM" + mNo, 1);
        missionArray[mNo].Reset();
        pauseMission[mNo].Reset();
        CheckForAllMissions();
    }
    public GameObject missionCmpltdLbl, missionCmpltdLblPause;
    public void CheckForAllMissions()
    {
        for (int i = 0; i < missionArray.Length; i++)
        {
            if (isMissionCmpltd(i) == 0)
            {
                return;
            }
        }
        if (CurrentSet < data.missionSet.Length)
        {
            CurrentSet = CurrentSet + 1;
        }
        else
        {
            missionCmpltdLbl.SetActive(true);
        }
        uimanager.instance.SetScoreMul(CurrentSet + 1);
        Invoke("ChangeMissionSet", 1f);

//       FirebaseEvents.instance.SetUserProperty("PROGRESS", "" + (CurrentSet / 60) * 100);
#if GOOGLEPLAYGAMES
        // SET ACHIEVEMENT COMPLETED.
        //if (CurrentSet == 10)
        //{
        //   PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQBA");
        //}
        //else if (CurrentSet == 20)
        //{
        //  PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQBQ");
        //}
        //else if (CurrentSet == 60)
        //{
        //  PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQBg");
        //}
#endif
    }
    void ChangeMissionSet()
    {
        if (CurrentSet < data.missionSet.Length)
        {
            for (int i = 0; i < missionArray.Length; i++)
            {
                PlayerPrefs.SetInt("sM" + i, 0);
            }
        }
        DeletePrefValues();
        SetMissionValues();
        OnMissionPnlAppear();
        OnPausePnlAppear();
    }
    public void SetMissionValues()
    {
        ResetMissionVal();
        for (int i = 0; i < data.missionSet[CurrentSet]._missionSet.Length; i++)
        {
            if (isMissionCmpltd(i) == 0)
            {
                Mission m = data.missionSet[CurrentSet]._missionSet[i];
                if (m.missionType == MissionType.GOLD)
                {
                    if (m.inOneRun)
                    {
                        mCoin = m.targetCount;
                    }
                    else
                    {
                        mCoin = GetReminder(m);
                    }
                }
                else if (m.missionType == MissionType.RUN)
                {
                    if (m.inOneRun)
                    {
                        mRun = m.targetCount;
                    }
                    else
                    {
                        mRun = GetReminder(m);
                    }
                }
                else if (m.missionType == MissionType.JUMP)
                {
                    if (m.inOneRun)
                    {
                        mJump = m.targetCount;
                    }
                    else
                    {
                        mJump = GetReminder(m);
                    }
                }
                else if (m.missionType == MissionType.POWERUP)
                {
                    if (m.inOneRun)
                    {
                        mPow = m.targetCount;
                    }
                    else
                    {
                        mPow = GetReminder(m);
                    }
                }
                else if (m.missionType == MissionType.ROLL)
                {
                    if (m.inOneRun)
                    {
                        mRoll = m.targetCount;
                    }
                    else
                    {
                        mRoll = GetReminder(m);
                    }
                }
                else if (m.missionType == MissionType.WORDHUNT)
                {
                    if (m.inOneRun)
                    {
                        mWordHunt = m.targetCount;
                    }
                    else
                    {
                        mWordHunt = GetReminder(m);
                    }
                }
                else if (m.missionType == MissionType.MAGNET)
                {
                    if (m.inOneRun)
                    {
                        mMagnet = m.targetCount;
                    }
                    else
                    {
                        mMagnet = GetReminder(m);
                    }
                }
                else if (m.missionType == MissionType.Bicycle)
                {
                    if (m.inOneRun)
                    {
                        mCycle = m.targetCount;
                    }
                    else
                    {
                        mCycle = GetReminder(m);
                    }
                }
                else if (m.missionType == MissionType.SCORE)
                {
                    if (m.inOneRun)
                    {
                        mScore = m.targetCount;
                    }
                    else
                    {
                        mScore = GetReminder(m);
                    }
                }
                else if (m.missionType == MissionType.BicyclePurchase)
                {
                    if (m.inOneRun)
                    {
                        mPurShield = m.targetCount;
                    }
                    else
                    {
                        mPurShield = GetReminder(m);
                    }
                }
                else if (m.missionType == MissionType.JETPACK)
                {
                    if (m.inOneRun)
                    {
                        mJetPack = m.targetCount;
                    }
                    else
                    {
                        mJetPack = GetReminder(m);
                    }
                }
                else if (m.missionType == MissionType.KEY)
                {
                    if (m.inOneRun)
                    {
                        mKey = m.targetCount;
                    }
                    else
                    {
                        mKey = GetReminder(m);
                    }
                }
                else if (m.missionType == MissionType.MYSTERYBOX)
                {
                    if (m.inOneRun)
                    {
                        mBox = m.targetCount;
                    }
                    else
                    {
                        mBox = GetReminder(m);
                    }
                }
            }
        }
    }
    void ResetMissionVal()
    {
        mCoin = 0;
        mRun = 0;
        mJump = 0;
        mPow = 0;
        mRoll = 0;
        mWordHunt = 0;
        mMagnet = 0;
        mCycle = 0;
        mPurShield = 0;
        mScore = 0;
        mJetPack = 0;
        mBox = 0;
        mKey = 0;
        CoinCount = 0;
        runCount = 0;
        scoreCount = 0;

        jumpCount = 0;
        rollCount = 0;
        powerupCount = 0;
        magentCount = 0;
        wordCount = 0;
        cycleCount = 0;
        usageBoardCount = 0;
        keyCount = 0;
        jetPackCount = 0;
        boxCount = 0;
    }
    void DeletePrefValues()
    {
        GetCoin = 0;
        GetRun = 0;
        GetScore = 0;
        GetJumpCount = 0;
        GetRollCount = 0;
        GetWordCount = 0;
        GetPowerupCount = 0;
        GetMangnet = 0;
        GetShield = 0;
        getBoard = 0;
        GetJetPack = 0;
        GetKey = 0;
        GetMysteryBox = 0;
    }

    public void MissionTrigger(MissionType mType)
    {
        for (int i = 0; i < missionArray.Length; i++)
        {
            if (isMissionCmpltd(i) == 0)
            {
                if (missionArray[i].myMission.missionType == mType)
                {
                    SetMissionCmpltd(i);
                }
            }
        }
    }
    public void CoinTrigger()
    {
        if (mCoin > 0)
        {
            CoinCount += 1;
            if (CoinCount >= mCoin)
            {
                MissionTrigger(MissionType.GOLD);
            }
        }
    }
    public void MagnetTrigger()
    {
        if (mMagnet > 0)
        {
            magentCount += 1;
            if (magentCount >= mMagnet)
            {
                MissionTrigger(MissionType.MAGNET);
            }
        }
    }
    public void ShieldTrigger()
    {
        if (mCycle > 0)
        {
            cycleCount += 1;
            if (cycleCount >= mCycle)
            {
                MissionTrigger(MissionType.Bicycle);
            }
        }
    }
    public void WordHuntTrigger()
    {
        if (mWordHunt > 0)
        {
            wordCount += 1;
            if (wordCount >= mWordHunt)
            {
                MissionTrigger(MissionType.WORDHUNT);
            }
        }
    }
    public void JumpTrigger()
    {
        if (mJump > 0)
        {
            jumpCount += 1;
            if (jumpCount >= mJump)
            {
                MissionTrigger(MissionType.JUMP);
            }
        }
    }
    public void RollTrigger()
    {
        if (mRoll > 0)
        {
            rollCount += 1;
            if (rollCount >= mRoll)
            {
                MissionTrigger(MissionType.ROLL);
            }
        }
    }
    public void PowerUpTrigger()
    {
        if (mPow > 0)
        {
            powerupCount += 1;
            if (powerupCount >= mPow)
            {
                MissionTrigger(MissionType.POWERUP);
            }
        }
    }
    public void KeyTrigger()
    {
        if (mKey > 0)
        {
            keyCount += 1;
            if (keyCount >= mKey)
            {
                MissionTrigger(MissionType.KEY);
            }
        }
    }
    public void JetPackTrigger()
    {
        if (mJetPack > 0)
        {
            jetPackCount += 1;
            if (jetPackCount >= mJetPack)
            {
                MissionTrigger(MissionType.JETPACK);
            }
        }
    }
    public void MysteryBoxTrigger()
    {
        if (mBox > 0)
        {
            boxCount += 1;
            if (boxCount >= mBox)
            {
                MissionTrigger(MissionType.JETPACK);
            }
        }
    }
    public void ShieldUsageTrigger()
    {
        if (mPurShield > 0)
        {
            usageBoardCount += 1;
            if (usageBoardCount >= mPurShield)
            {
                MissionTrigger(MissionType.BicyclePurchase);
            }
        }
    }

    public void GameOver(int coins, int run, int score)
    {
        if (mCoin > 0)
        {
            GetCoin += coins;
        }
        if (mRun > 0)
        {
            GetRun += run;
        }
        if (mScore > 0)
        {
            GetScore += score;
        }
        if (mJump > 0)
        {
            GetJumpCount += jumpCount;
        }
        if (mRoll > 0)
        {
            GetRollCount += rollCount;
        }
        if (mWordHunt > 0)
        {
            GetWordCount += wordCount;
        }
        if (mPow > 0)
        {
            GetPowerupCount += powerupCount;
        }
        if (mMagnet > 0)
        {
            GetMangnet += magentCount;
        }
        if (mCycle > 0)
        {
            GetShield += cycleCount;
        }
        if (mPurShield > 0)
        {
            getBoard += usageBoardCount;
        }
        if (mJetPack > 0)
        {
            GetJetPack += jetPackCount;
        }
        if (mKey > 0)
        {
            GetKey += keyCount;
        }
        if (mBox > 0)
        {
            GetMysteryBox += boxCount;
        }
    }

    public int CurrentSet
    {
        get
        {
            return PlayerPrefs.GetInt("mSetNo");
        }
        set
        {
            PlayerPrefs.SetInt("mSetNo", value);
        }
    }
    public int GetReminder(Mission m)
    {
        int actualCount = m.targetCount;
        int reminder = 0;
        if (m.missionType == MissionType.GOLD)
        {
            reminder = actualCount - GetCoin;
        }
        else if (m.missionType == MissionType.RUN)
        {
            reminder = actualCount - GetRun;
        }
        else if (m.missionType == MissionType.JUMP)
        {
            reminder = actualCount - GetJumpCount;
        }
        else if (m.missionType == MissionType.POWERUP)
        {
            reminder = actualCount - GetPowerupCount;
        }
        else if (m.missionType == MissionType.ROLL)
        {
            reminder = actualCount - GetRollCount;
        }
        else if (m.missionType == MissionType.WORDHUNT)
        {
            reminder = actualCount - GetWordCount;
        }
        else if (m.missionType == MissionType.MAGNET)
        {
            reminder = actualCount - GetMangnet;
        }
        else if (m.missionType == MissionType.Bicycle)
        {
            reminder = actualCount - GetShield;
        }
        else if (m.missionType == MissionType.SCORE)
        {
            reminder = actualCount - GetScore;
        }
        else if (m.missionType == MissionType.BicyclePurchase)
        {
            reminder = actualCount - getBoard;
        }
        else if (m.missionType == MissionType.JETPACK)
        {
            reminder = actualCount - GetJetPack;
        }
        else if (m.missionType == MissionType.KEY)
        {
            reminder = actualCount - GetKey;
        }
        else if (m.missionType == MissionType.MYSTERYBOX)
        {
            reminder = actualCount - GetMysteryBox;
        }
        return reminder;
    }
    public int GetGamePlayRem(Mission m)
    {
        int reminder = 0;
        if (m.missionType == MissionType.GOLD)
        {
            reminder = mCoin - CoinCount;
        }
        else if (m.missionType == MissionType.RUN)
        {
            reminder = mRun - (int)runCount;
        }
        else if (m.missionType == MissionType.JUMP)
        {
            reminder = mJump - jumpCount;
        }
        else if (m.missionType == MissionType.POWERUP)
        {
            reminder = mPow - powerupCount;
        }
        else if (m.missionType == MissionType.ROLL)
        {
            reminder = mRoll - rollCount;
        }
        else if (m.missionType == MissionType.WORDHUNT)
        {
            reminder = mWordHunt - wordCount;
        }
        else if (m.missionType == MissionType.MAGNET)
        {
            reminder = mMagnet - magentCount;
        }
        else if (m.missionType == MissionType.Bicycle)
        {
            reminder = mCycle - cycleCount;
        }
        else if (m.missionType == MissionType.SCORE)
        {
            reminder = mScore - scoreCount;
        }
        else if (m.missionType == MissionType.BicyclePurchase)
        {
            reminder = mPurShield - usageBoardCount;
        }
        else if (m.missionType == MissionType.JETPACK)
        {
            reminder = mJetPack - jetPackCount;
        }
        else if (m.missionType == MissionType.MYSTERYBOX)
        {
            reminder = mBox - boxCount;
        }
        else if (m.missionType == MissionType.KEY)
        {
            reminder = mKey - keyCount;
        }

        return reminder;
    }
    int GetCoin
    {
        get
        {
            return PlayerPrefs.GetInt("mCoin");
        }
        set
        {
            PlayerPrefs.SetInt("mCoin", value);
        }
    }
    int GetJumpCount
    {
        get
        {
            return PlayerPrefs.GetInt("mJump");
        }
        set
        {
            PlayerPrefs.SetInt("mJump", value);
        }
    }
    int GetRollCount
    {
        get
        {
            return PlayerPrefs.GetInt("mRoll");
        }
        set
        {
            PlayerPrefs.SetInt("mRoll", value);
        }
    }
    int GetPowerupCount
    {
        get
        {
            return PlayerPrefs.GetInt("mPow");
        }
        set
        {
            PlayerPrefs.SetInt("mPow", value);
        }
    }
    int GetRun
    {
        get
        {
            return PlayerPrefs.GetInt("mRun");
        }
        set
        {
            PlayerPrefs.SetInt("mRun", value);
        }
    }
    int GetScore
    {
        get
        {
            return PlayerPrefs.GetInt("mScore");
        }
        set
        {
            PlayerPrefs.SetInt("mScore", value);
        }
    }
    int getBoard
    {
        get
        {
            return PlayerPrefs.GetInt("uBoard");
        }
        set
        {
            PlayerPrefs.SetInt("uBoard", value);
        }
    }
    int GetJetPack
    {
        get
        {
            return PlayerPrefs.GetInt("mJPck");
        }
        set
        {
            PlayerPrefs.SetInt("mJPck", value);
        }
    }
    int GetKey
    {
        get
        {
            return PlayerPrefs.GetInt("mKy");
        }
        set
        {
            PlayerPrefs.SetInt("mKy", value);
        }
    }
    int GetMysteryBox
    {
        get
        {
            return PlayerPrefs.GetInt("mMstBox");
        }
        set
        {
            PlayerPrefs.SetInt("mMstBox", value);
        }
    }
    int GetWordCount
    {
        get
        {
            return PlayerPrefs.GetInt("wHunt");
        }
        set
        {
            PlayerPrefs.SetInt("wHunt", value);
        }
    }
    int GetMangnet
    {
        get
        {
            return PlayerPrefs.GetInt("mgnt");
        }
        set
        {
            PlayerPrefs.SetInt("mgnt", value);
        }
    }
    int GetShield
    {
        get
        {
            return PlayerPrefs.GetInt("shld");
        }
        set
        {
            PlayerPrefs.SetInt("shld", value);
        }
    }
}
