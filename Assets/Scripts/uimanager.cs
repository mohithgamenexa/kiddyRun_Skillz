using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using TMPro;
//using Assets.SimpleAndroidNotifications;

public class uimanager : MonoBehaviour
{
    public static uimanager instance;
    public GameObject gameoverMenu, HighScoreMenu, preGameOveMenu, pauseMenu, lodingMenu,leaderboardpanel, header, resumeMenu, lessCoinMenu, infoMenu, btnContinue, storePnl, settingsPnl, missionPnl, dailyChallengeMenu, vaultMenu,characterspanel;
    public GameObject OfferIappanel,offeriapbtn1, offeriapbtn1_0, offeriapbtn2, doubleItBtn,removeadsIapBtn,ratepanel;

    [Header("Remotepack")]
    public GameObject RemotePack;


    public Animation doubleItCoinIcon;
    public TextMeshProUGUI specialpack2, specialpack1, specialpack1B;
    public Image loadingBar;
    public GameObject mainMenu,fader;
    public GameObject inGameMenu;
    Coroutine lastRoutine;
    public int coinsCltd = 0;
    int deathCount;
    public ShopManager shop;
    public Animation hedrCoinobj, hedrkeyobj;
    [Header("Text Sources")]
    public TextMeshProUGUI inGameCoinTxt;
    public TextMeshProUGUI hedrKeyTxt, scoreOnGameOver, coinOnGameOver, keysTxtPGo, hedrCoinTxt, infoTxt, flyTxt;
    
    public TextMeshProUGUI sMulTxtMission, scoreMul, sMulTxtGameOver;
    public TextMeshProUGUI pGKeyTxt;
    public TextMeshProUGUI coinMul;
    public TextMeshProUGUI inGameWordTxt;
    public TextMeshProUGUI[] remoteiaptextlabels,RemoteIap2textlabels;
    public TextMeshProUGUI freeclaimcoins;

    public PowerUp[] powerupArray;
    [Header("MysteryBoxItems")]
    public GameObject giftboxMenu;
    public Animation claimnotif;

    public MainCharacter chMesh, gameOverMesh,flashMesh;
    int m_powerstrength;
    DateTime GameOpenTime;
    public int intersitialCount = 0;
    public int rewardVideoCount = 0;
    public CharacterSelection chCam;
    public ScrollRect storePnlScrollview;
    public bool inChase;
    public GameObject plrStarParticle;

    void Awake()
    {
        if (instance == null)
            instance = this;
        GameOpenTime = DateTime.Now;
        Input.multiTouchEnabled = false;
        /*regexCurrencyVal("€1,00,000.76");
        regexCurrencyVal("1,00,000.76€");
        Debug.Log("_________");
        regexCurrencySymbol("€1,00,000.76");
        regexCurrencySymbol("1,00,000.76€");*/
       
    }

    IEnumerator Start()
    {
        TrackManager._instance._gameState = GameState.HOME;
        MusicChangeOnEnable(DataManager.instance.GetMusic == 1 ? true : false);
        SfxChangeOnEnable(DataManager.instance.GetSFX == 1 ? true : false);
        notifiToggle.isOn = DataManager.instance.GetSFX == 1 ? true : false;
        header.SetActive(false);
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        preGameOveMenu.SetActive(false);
        gameoverMenu.SetActive(false);
        lodingMenu.SetActive(true);
        yield return new WaitUntil(() => !DataManager.instance.storyPnl.activeInHierarchy);
        float fillAmount = 0.0f;
        while (fillAmount < 1)
        {
            float rP = (float)TrackManager._instance.progress / 100.0f;
            fillAmount = Mathf.MoveTowards(fillAmount, rP, Time.deltaTime);
            loadingBar.fillAmount = fillAmount;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        hedrCoinTxt.text = "" + DataManager.instance.Coins;
        hedrKeyTxt.text = "" + DataManager.instance.Keys;

        for (int i = 0; i < powerupArray.Length; i++)
        {
            int upgradeNo = DataManager.instance.GetPowUpNo(i);
            powerupArray[i].UpgradeNo = upgradeNo;
            powerupArray[i].upgradeFillImg.fillAmount = (float)upgradeNo / 6.0f;
            if (upgradeNo < 6)
            {
                powerupArray[i].upgradePrice.text = "" + 750 * Math.Pow(2, upgradeNo);
            }
            else
            {
                powerupArray[i].upgradePrice.text = "Upgraded";
            }
            powerupArray[i].UpgradedVlue.text = upgradeNo * 1000 + " /6000";
        }
        m_powerstrength = Shader.PropertyToID("_powerstrength");
        TrackManager._instance.strightPathList[0].MakeMapVisible(false);
        lodingMenu.SetActive(false);

        //camCntrl.tCamera.GetComponent<Animation>().Play();
        mainMenu.SetActive(true);
        header.SetActive(true);

        if (DataManager.instance.lastPlayedDate != "")
        {
            DateTime lasplayeddate = Convert.ToDateTime(DataManager.instance.lastPlayedDate);
            if (lasplayeddate.Date != DateTime.Now.Date)
            {
                DataManager.instance.daysplayed += 1;
               //FirebaseEvents.instance.SetUserProperty("DAYSCOUNT", DataManager.instance.daysplayed.ToString());
                DataManager.instance.lastPlayedDate = DateTime.Now.ToString();
            }
        }
       // if (NotificationExample.Instance != null)
       // {
       //     NotificationExample.Instance.CancelAll();
       //     NotificationExample.Instance.ScheduleNormal(3000, LocalnotifTitles[UnityEngine.Random.Range(0, LocalnotifTitles.Length)], Localnotifdescs[UnityEngine.Random.Range(0, Localnotifdescs.Length)]); ///callback user on quiting notification
       // }
       // if (FirebaseRemoteConfiguration.instance != null)
       // {
       //     FirebaseRemoteConfiguration.instance.SetRemoteLibonUI();
       // }
       //FirebaseEvents.instance.LogFirebaseEvent("Screen1", "MainMenu", "UIManager.Start()");
    }


    public void openSettingsMenu(GameObject dummyHome)
    {
        SfxManager.instance.PlayButtonClick();
        storePnl.SetActive(false);
        if(TrackManager._instance._gameState == GameState.HOME)
        {
            dummyHome.SetActive(false);
        }
        else
        {
            dummyHome.SetActive(true);
        }
        settingsPnl.SetActive(true);
        //FirebaseEvents.instance.LogFirebaseEvent("Screen2", "SecondaryScreens", "InSettingspanel");

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&!giftboxMenu.activeInHierarchy)
        {
            if (TrackManager._instance._gameState == GameState.PLAYING)
            {
                Pause(true);
            }
            else if (storePnl.activeInHierarchy)
            {
                CloseStorePanel();
            }
            else if (TrackManager._instance._gameState == GameState.PREGAMEOVER)
            {
                CheckAd();
            }

            else if (lessCoinMenu.activeInHierarchy)
            {
                lessCoinMenu.SetActive(false);
            }
            else if (settingsPnl.activeInHierarchy)
            {
                settingsPnl.SetActive(false);
            }
            else if (leaderboardpanel.activeInHierarchy)
            {
                leaderboardpanel.SetActive(false);
            }
            else if (missionPnl.activeInHierarchy)
            {
                missionPnl.SetActive(false);
            }
            else if (shop.gameObject.activeInHierarchy)
            {
                shop.CloseShop();
            }
            else if (characterspanel.activeInHierarchy)
            {
                characterspanel.SetActive(false);
                chCam.gameObject.SetActive(false);
            }
            else if (vaultMenu.activeInHierarchy && !giftboxMenu.activeInHierarchy)
            {
                vaultMenu.SetActive(false);
            }
            else if (dailyChallengeMenu.activeInHierarchy)
            {
                dailyChallengeMenu.SetActive(false);
            }
        }
    }

    public void onLBOPEN(GameObject LBPanel)
    {
                SfxManager.instance.PlayButtonClick();
                LBPanel.SetActive(true);
             // FirebaseEvents.instance.LogFirebaseEvent("Screen2", "SecondaryScreens", "InLeaderboardpanel");
    }

    public void HomeforSettingsInapp(GameObject thisobj)
    {
        thisobj.SetActive(false);
        leaderboardpanel.SetActive(false);
        lessCoinMenu.SetActive(false);
        missionPnl.SetActive(false);
        shop.CloseShop();
        vaultMenu.SetActive(false);
        dailyChallengeMenu.SetActive(false);
        characterspanel.SetActive(false);
        chCam.gameObject.SetActive(false);
    }

    public void UpdateHeader()
    {
        hedrCoinTxt.text = "" + DataManager.instance.Coins;
        hedrKeyTxt.text = "" + DataManager.instance.Keys;
    }
    public void UpgradePowerUp(int no)
    {
        SfxManager.instance.PlayButtonClick();
        int upgradeNo = DataManager.instance.GetPowUpNo(no);
        if (upgradeNo < 6)
        {
            int requiredCoins = (int)(750 * Math.Pow(2, upgradeNo));
            if (DataManager.instance.haveEnoughCoins(requiredCoins))
            {
                PlayCoinParticle();
                DataManager.instance.AddCoin(-requiredCoins);
                hedrCoinTxt.text = "" + DataManager.instance.Coins;
                uimanager.instance.hedrCoinobj.Play();

                DataManager.instance.UpdagradePowrup(no);
                upgradeNo = DataManager.instance.GetPowUpNo(no);
                powerupArray[no].UpgradeNo = upgradeNo;
                powerupArray[no].upgradeFillImg.fillAmount = (float)upgradeNo / 6.0f;
                powerupArray[no].UpgradedVlue.text = upgradeNo * 1000 + " /6000";
            // FirebaseEvents.instance.LogFirebaseEvent("PowerupUpgraded", "PowerupNo: " + no, "UpgradeCount: " + upgradeNo);

                upgradeNo = DataManager.instance.GetPowUpNo(no);
                requiredCoins = (int)(750 * Math.Pow(2, upgradeNo));
                powerupArray[no].upgradePrice.text = "" + requiredCoins;
                if (upgradeNo >= 6)
                {
                    powerupArray[no].upgradePrice.text = "Upgraded";
                }
                #if UNITY_ANDROID
                  //  PGGC_Manager._instance.SaveToCloud();
                #endif
            }
            else
            {
                ShowNotEnoughCoin();
            }
        }
        else
        {
            powerupArray[no].upgradePrice.text = "Upgraded";
        }

    }
    public void OnIAPSuccess(int no)
    {
        if (no == 1)
        {
            DataManager.instance.AddKey(10);
            uimanager.instance.hedrkeyobj.Play();

        }
        else if (no == 2)
        {
            DataManager.instance.AddKey(100);
            uimanager.instance.hedrkeyobj.Play();

        }
        else if (no == 3)
        {
            DataManager.instance.AddKey(500);
            uimanager.instance.hedrkeyobj.Play();

        }
        else if (no == 4)
        {
            DataManager.instance.AddCoin(10000);
            uimanager.instance.hedrCoinobj.Play();

        }
        else if (no == 5)
        {
            DataManager.instance.AddCoin(25000);
            uimanager.instance.hedrCoinobj.Play();

        }
        else if (no == 6)
        {
            DataManager.instance.AddCoin(65000);
            uimanager.instance.hedrCoinobj.Play();

        }
        else if (no == 7)
        {
            DataManager.instance.AddCoin(25000);
            uimanager.instance.hedrCoinobj.Play();

            DataManager.instance.AddKey(150);
            uimanager.instance.hedrkeyobj.Play();

        }
        else if (no == 8)
        {
            DataManager.instance.AddCoin(25000);
            uimanager.instance.hedrCoinobj.Play();

            DataManager.instance.AddKey(250);
            uimanager.instance.hedrkeyobj.Play();

        }
        else if (no == 9)
        {
            DataManager.instance.AddKey(200);
            uimanager.instance.hedrkeyobj.Play();
        }

        else if (no == 10)
        {
            DataManager.instance.AddCoin(105000);
            uimanager.instance.hedrCoinobj.Play();
        }
        hedrCoinTxt.text = "" + DataManager.instance.Coins;
        hedrKeyTxt.text = "" + DataManager.instance.Keys;

        pGKeyTxt.text = "" + DataManager.instance.Keys;
       //FirebaseEvents.instance.LogFirebaseEvent("IAP_Panel", "Success", "");
      // FirebaseEvents.instance.SetUserProperty("IAPDONE", "IAPProductNo" + no);
        DataManager.instance.IAPSuccessCount += 1;
       //FirebaseEvents.instance.SetUserProperty("NOOFIAPS", "IAPCOUNT" + DataManager.instance.IAPSuccessCount);
      // PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQCQ");
    }

    public void OnIAPSuccessCalAmt(UnityEngine.Purchasing.IAPButton BtnP)
    {
        float purchasedvalue = regexCurrencyVal(BtnP.priceText.text);
        string currencytype = regexCurrencySymbol(BtnP.priceText.text);
        PlayerPrefs.SetFloat("iappurchasevalue", PlayerPrefs.GetFloat("iappurchasevalue")+ purchasedvalue);
       //FirebaseEvents.instance.LogFirebaseEvent("ProductPurchased", currencytype, purchasedvalue.ToString());
    }

    float regexCurrencyVal(string pricevalue)
    {
        Debug.Log(float.Parse(Regex.Replace(pricevalue, @"^\W|\W$", "")) + "______");
        return float.Parse(Regex.Replace(pricevalue, @"^\W|\W$", ""));
    }


    string regexCurrencySymbol(string pricevalue)
    {

        string _sym = (Regex.Replace(pricevalue, @"\d+", ""));
        _sym = Regex.Replace(_sym, @"[,*|.*]","");
        Debug.Log(_sym);
        return _sym;
    }

    public void OnInAppOpen()
    {
      // FirebaseEvents.instance.LogFirebaseEvent("IAP_Panel", "Opened", null);
    }
    public void Shop()
    {
        SfxManager.instance.PlayButtonClick();
        shop.gameObject.SetActive(true);
        shop.OnShopButtonPress();
       //FirebaseEvents.instance.LogFirebaseEvent("Screen2", "SecondaryScreens", "InShop");
    }
    public GameObject coinParticle;
    public void PlayCoinParticle()
    {
        var mousePos = Input.mousePosition;
       // mousePos.z = 10;
        GameObject par = Instantiate(coinParticle, transform);
        par.transform.position = mousePos;
        Destroy(par, 2.0f);
    }
    public void IntroPlay(Animation intro)
    {
        SfxManager.instance.PlayButtonClick();
        intro.Play();
        header.SetActive(false);
        mainMenu.SetActive(false);
       TrackManager._instance.playerScript.anim.SetBool("idlexit", true);
      // FirebaseEvents.instance.LogFirebaseEvent("Screen3", "ThirdScreen", "Home-Gameplay");
    }
    public void Play()
    {
        MissionManager.instance.SetMissionValues();
        TrackManager._instance.playerScript.Play();
        DataManager.instance.GamesPlayedOL += 1;
        GIftBoxOPen.rewardCount = 0;
    }
    public void Restart()
    {
        SfxManager.instance.PlayButtonClick();
        coinsCltd = 0;
        doubleItCoinIcon.clip = doubleItCoinIcon.GetClip("gameovercoinsidle");
        doubleItCoinIcon.GetComponent<Animation>().Play();
        gameoverMenu.SetActive(false);
        header.SetActive(false);
        gameOverMesh.transform.parent.gameObject.SetActive(false);
        inGameCoinTxt.text = "" + 0;
        Play();
        TrackManager._instance.playerScript.anim.Play("backrun");
        StartChase();
       // FirebaseEvents.instance.LogFirebaseEvent("Screen3", "ThirdScreen", "Gameover-Gameplay");
    }
    public void StopChase()
    {
        if (chaseScene != null)
        {
            StopCoroutine(chaseScene);
            camCntrl.offsetPosition.z = -5.5f;
            camCntrl.offsetPosition.y = 4.5f;
            enemyAnim.transform.parent.gameObject.SetActive(false);
            inChase = false;
        }
    }
    public void ME()
    {
        SfxManager.instance.PlayButtonClick();
        characterspanel.SetActive(true);
        chCam.gameObject.SetActive(true);
        chCam.OnAppear();
        if (TrackManager._instance._gameState == GameState.GAMEOVER)
        {
            gameOverMesh.transform.parent.gameObject.SetActive(false);
        }
      //FirebaseEvents.instance.LogFirebaseEvent("Screen2", "SecondaryScreens", "InCharacersPanel");
    }
    public void StartChase()
    {
        if (TrackManager._instance.playerScript.mFlying)
        {
            StopChase();
            return;
        }
        if (chaseScene != null)
            StopCoroutine(chaseScene);
        chaseScene = StartCoroutine(EnemyChase());
    }
    public void NotificationChange(Toggle toggle)
    {
        bool notfi = toggle.isOn;
        if (notfi)
        {
            DataManager.instance.GetNotification = 1;
        }
        else
        {
            DataManager.instance.GetNotification = 0;
           // NotificationExample.Instance.CancelAll();
        }
    }

    public Toggle musicToggle, sfxToggle, notifiToggle;
    public void MusicChangeOnEnable(bool ON)
    {
        if (ON)
        {
            MusicPlayer.instance.ChangeMusicVolume(1.0f);
        }
        else
        {
            MusicPlayer.instance.ChangeMusicVolume(-80.0f);
        }
        musicToggle.isOn = ON;
    }
    public void SfxChangeOnEnable(bool ON)
    {
        if (ON)
        {
            MusicPlayer.instance.ChangeSfxVolume(1.0f);
        }
        else
        {
            MusicPlayer.instance.ChangeSfxVolume(-80.0f);
        }
        sfxToggle.isOn = ON;
    }
    public void OnMusicChange(Toggle toggle)
    {
        SfxManager.instance.PlayButtonClick();
        bool musicOff = toggle.isOn;
        musicOff = !musicOff;
        if (musicOff)
        {
            MusicPlayer.instance.ChangeMusicVolume(-80.0f);
            DataManager.instance.GetMusic = 0;
        }
        else
        {
            MusicPlayer.instance.ChangeMusicVolume(1.0f);
            DataManager.instance.GetMusic = 1;
        }
    }
    public void OnSfxChange(Toggle toggle)
    {
        SfxManager.instance.PlayButtonClick();
        bool sfxOff = toggle.isOn;
        sfxOff = !sfxOff;
        if (sfxOff)
        {
            MusicPlayer.instance.ChangeSfxVolume(-80.0f);
            DataManager.instance.GetSFX = 0;
        }
        else
        {
            MusicPlayer.instance.ChangeSfxVolume(1.0f);
            DataManager.instance.GetSFX = 1;
        }
    }
    public void DelayOnWordCmpltd(string info)
    {
        StartCoroutine(ShowInfo(info, 3.0f));

    }
    public IEnumerator ShowInfo(string txt, float delay)
    {
        yield return new WaitForSeconds(delay);
        SfxManager.instance.PlayNotification();
        infoTxt.text = txt;
        infoMenu.SetActive(true);
    }
    public void BuyBoard()
    {
        SfxManager.instance.PlayButtonClick();
        int coins = DataManager.instance.Coins;
        int rCoins = 500;
        if (coins >= rCoins)
        {
            DataManager.instance.AddCoin(-rCoins);
            hedrCoinTxt.text = "" + DataManager.instance.Coins;
            uimanager.instance.hedrCoinobj.Play();

            int b = DataManager.instance.Boards;
            b += 1;
            DataManager.instance.Boards = b;
            #if UNITY_ANDROID
               // PGGC_Manager._instance.SaveToCloud();
            #endif
        }
    }

   
    Coroutine chaseScene;
    public void PreGameOver()
    {     
        CountDownFiller.fillAmount = 1;
        preGameOveMenu.SetActive(true);
        pGKeyTxt.text = "" + DataManager.instance.Keys;
        int needKeys = 1; //(int)Mathf.Pow(2, deathCount);
        keysTxtPGo.text = "" + needKeys;
        inChase = false;
        if(!TrackManager._instance.playerScript.railRun)
            StartCoroutine(EnterDie());

        btnContinue.SetActive(false);

        //mo
        //if (AdsManager_Mediation.Instance != null && AdsManager_Mediation.Instance._videoAvailable())
        //{
        //    btnContinue.SetActive(true);
        //}
        //else
        //{
        //    btnContinue.SetActive(false);
        //}
        //mo
        // ResetAllPowerUps();  
        /// FirebaseEvents.instance.LogFirebaseEvent("Screen4", "FourthScreen", "Gameplay-Revive");
        GC.Collect();
    }
    public void PlayCountDown()
    {
        lastRoutine = StartCoroutine(CountDown());
    }
    public Image CoundownImage;
    public Image CountDownFiller;
    public Sprite[] countDownSprites;
    public bool _isadplaying = false;
    IEnumerator CountDown()
    {
        int countdownno = 5;
        float fillAmt = 1;
        CountDownFiller.fillAmount = fillAmt;
        while (fillAmt > 0)
        {
            fillAmt -= Time.deltaTime * 0.3f;
            CountDownFiller.fillAmount = fillAmt;
            countdownno -= 1;
            
            if(fillAmt >= .8f)
            {
                CoundownImage.sprite = countDownSprites[0];
            }else if(fillAmt >= .6f)
            {
                CoundownImage.sprite = countDownSprites[1];
            }
            else if (fillAmt >= .4f)
            {
                CoundownImage.sprite = countDownSprites[2];
            }
            else if (fillAmt >= .2f)
            {
                CoundownImage.sprite = countDownSprites[3];
            }
            else
            {
                CoundownImage.sprite = countDownSprites[4];
            }
            // yield return new WaitForSeconds(1f);
            yield return new WaitUntil(() => storePnl.activeInHierarchy == false);
            yield return new WaitUntil(() => _isadplaying == false);
        }
        CheckAd();
    }
    public void CheckAd()
    {
        if (DataManager.instance.GetAdsON == 1 && DataManager.instance.removeAdsIap != 1)
        {
            GameOver();
            //mo
            //if (AdsManager_Mediation.Instance.ShowAd())
            //{
            //    AdsManager_Mediation.Instance.ShowInterstitial();
            //    AdsManager_Mediation.Instance.interstitial.OnAdClosed += AdsManager_Mediation.Instance.HandleAdclosed;
            //}
            //else
            //{
            //    GameOver();
            //}
            //mo
        }
        else
        {
            GameOver();
        }

    }



    Coroutine magnet,coinRoutine,scoreRoutine,shieldRoutine,cycleRoutine,stickroutine;

    public GameObject dummyHome;
    public void OpenMissionPnl(ScrollRect rect)
    {
        SfxManager.instance.PlayButtonClick();
        missionPnl.SetActive(true);
        rect.verticalNormalizedPosition = 1.0f;
     //  FirebaseEvents.instance.LogFirebaseEvent("Screen2", "SecondaryScreens", "InMissionpanel");
    }

    public void OpenStorePanel(bool t)
    {
            SfxManager.instance.PlayButtonClick();
            storePnlScrollview.verticalNormalizedPosition = 1.0f;
            storePnl.SetActive(true);
            header.SetActive(true);
            if (TrackManager._instance._gameState != GameState.HOME)
            {
                dummyHome.SetActive(true);
            }
            else
            {
                dummyHome.SetActive(false);
            }
            settingsPnl.SetActive(t);
         // FirebaseEvents.instance.LogFirebaseEvent("Screen2", "SecondaryScreens", "InStore");
    }

    public void CloseStorePanel()
    {
        SfxManager.instance.PlayButtonClick();
        storePnl.SetActive(false);
        if(TrackManager._instance._gameState == GameState.PREGAMEOVER)
        {
            header.SetActive(false);
        }

    }

    public void PowerUpCollected(int powerupNo)
    {
        if(powerupNo < 6)
        {
            PowerUp p = powerupArray[powerupNo];
            if (powerupNo == 0)
            {
                if (magnet != null)
                    StopCoroutine(magnet);
                magnet = StartCoroutine(PowerupDown(p, powerupNo));
                MissionManager.instance.MagnetTrigger();
            }
            else if (powerupNo == 1)
            {
                if (coinRoutine != null)
                    StopCoroutine(coinRoutine);
                coinRoutine = StartCoroutine(PowerupDown(p, powerupNo));
            }
            else if (powerupNo == 2)
            {
                if (scoreRoutine != null)
                    StopCoroutine(scoreRoutine);
                scoreRoutine = StartCoroutine(PowerupDown(p, powerupNo));
            }
            else if (powerupNo == 3)
            {
                if (shieldRoutine != null)
                    StopCoroutine(shieldRoutine);
                shieldRoutine = StartCoroutine(PowerupDown(p, powerupNo));
                MissionManager.instance.ShieldTrigger();
            }
            else if (powerupNo == 4)
            {
                if (cycleRoutine != null)
                    StopCoroutine(cycleRoutine);
                cycleRoutine = StartCoroutine(PowerupDown(p, powerupNo));
                StopChase();
               // MissionManager.instance.ShieldTrigger();
            }
            else if(powerupNo == 5)
            {
                if (stickroutine != null)
                    StopCoroutine(stickroutine);
                stickroutine = StartCoroutine(PowerupDown(p, powerupNo));
            }
        }
        
        else if (powerupNo == 6)
        {
            // collectedBoxes += 1;
            DataManager.instance.GiftBoxCount += 1;
            GIftBoxOPen.rewardCount += 1;
            MissionManager.instance.MysteryBoxTrigger();
        }
        else if (powerupNo == 7)
        {
            hedrKeyTxt.text = "" + DataManager.instance.AddKey(1);
            uimanager.instance.hedrkeyobj.Play();

            MissionManager.instance.KeyTrigger();
        }
        else if(powerupNo == 8)
        {
            powerupArray[4].PlrRepObj.SetActive(false);
            powerupArray[5].PlrRepObj.SetActive(false);
            TrackManager._instance.playerScript.DoFly();
            MissionManager.instance.JetPackTrigger();
            StopChase();
        }
        else if (powerupNo == 9)
        {
            DataManager.instance.AddCoin(150);
            flyTxt.text = "+150";
            flyTxt.gameObject.SetActive(true);
        }
    }

    IEnumerator PowerupDown(PowerUp p,int no)
    {
        PowerupOnOff(p, no,true);
        float time = p.time;
        time += p.UpgradeNo * p.upgradeMul;
        float animationTime = time;
        while (animationTime > 0 && p.isActivated)
        {
           
            if (TrackManager._instance._gameState == GameState.PAUSE)
                yield return new WaitUntil(() => (TrackManager._instance._gameState == GameState.PLAYING || TrackManager._instance._gameState == GameState.HOME));
            if (TrackManager._instance._gameState == GameState.PREGAMEOVER || TrackManager._instance._gameState == GameState.HOME)
            {
                PowerupOnOff(p, no, false);
                yield break;
            }
            animationTime -= Time.deltaTime;
            p.fillImg.fillAmount = animationTime / time;
            if (no == 3 && animationTime < 2)
            {
                Shader.SetGlobalFloat(m_powerstrength, UnityEngine.Random.value);
            }
            yield return null;
        }
        PowerupOnOff(p,no,false);
    }
    public void StopCycling()
    {
        if (cycleRoutine != null)
            StopCoroutine(cycleRoutine);
        PowerupOnOff(powerupArray[4], 4, false);
        TrackManager._instance._gameState = GameState.SAVEME;
    }
    public void StopCyclingStickRu()
    {
        if (cycleRoutine != null)
        {
            StopCoroutine(cycleRoutine);
            PowerupOnOff(powerupArray[4], 4, false);
        }
        if (stickroutine != null)
        {
            StopCoroutine(stickroutine);
            PowerupOnOff(powerupArray[5], 5, false);
        }
        if(coinRoutine != null)
        {
            StopCoroutine(coinRoutine);
            PowerupOnOff(powerupArray[1], 1, false);
        }
    }
    void PowerupOnOff(PowerUp p,int no,bool ON)
    {
        p.isActivated = ON;
        p.UIRepresentObj.SetActive(ON);
        p.PlrRepObj.SetActive(ON);
        switch (no)
        {
            case 0:
                break;
            case 1:
                coinMul.gameObject.SetActive(ON);
                StartCoroutine(TrackManager._instance.AnimateAllCoins(ON));
                break;
            case 2:
                TrackManager._instance.playerScript.ScoreMulONOFF(ON);
                int mP = ON == true ? 2 : 1;
                int m = TrackManager._instance.playerScript.scoreMul * TrackManager._instance.playerScript.powMul;
                scoreMul.text = m + "X";
                scoreMul.GetComponent<Animation>().Play();
                break;
            case 3:
                chMesh._invisble(ON);
                float nP = ON == true ? 1f : 2.0f;
                camCntrl.tCamera.GetComponent<Camera>().nearClipPlane = nP;
                Shader.SetGlobalFloat(m_powerstrength, 0.8f);
                break;
            case 4:
                TrackManager._instance.playerScript.DoCycling(ON);
                break;
            case 5:
                TrackManager._instance.playerScript.anim.SetBool("stickrun", ON);
                break;
        }
    }
    public void SetScoreMul(int mul)
    {
        TrackManager._instance.playerScript.scoreMul = mul;
        scoreMul.text = mul + "X";
        scoreMul.GetComponent<Animation>().Play();
    }

    public void openprivacypolicy(string ppurl)
    {
        Application.OpenURL(ppurl);
    }
    public void GameOver(bool pause=false)
    {
        if(TrackManager._instance._gameState == GameState.PREGAMEOVER||pause)
        {
            if( coinsCltd >0){
                //if (AdsManager_Mediation.Instance._videoAvailable())
                //{
                //    doubleItBtn.SetActive(true);
                //}
            }
            else
            {
                doubleItBtn.SetActive(false);
            }
            SfxManager.instance.PlayGameOver();
            MissionManager.instance.GameOver(coinsCltd, (int)TrackManager._instance.playerScript.m_TotalWorldDistance,TrackManager._instance.playerScript.m_Score);
            if(coinsCltd == 2019)
            {
              // PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQCg"); 
            }
            UpdateTextValues();
            if (lastRoutine != null)
            {
                StopCoroutine(lastRoutine);
            }
            StopChase();
            header.SetActive(true);
            preGameOveMenu.SetActive(false);
            inGameMenu.SetActive(false);
            if (GIftBoxOPen.rewardCount <= 0)
            {
                gameoverMenu.SetActive(true);
            }
            else
            {
                giftboxMenu.SetActive(true);
            }

            gameOverMesh.transform.parent.gameObject.SetActive(true);
            //sMulTxtGameOver.text = "X" + TrackManager._instance.playerScript.scoreMul;
            if(TrackManager._instance.playerScript.m_Score > DataManager.instance.HighScore)
            {
                DataManager.instance.HighScore = TrackManager._instance.playerScript.m_Score;
                HighScoreMenu.SetActive(true);
              // PGGC_Manager._instance.OnAddScoretoLeaderBoard(DataManager.instance.HighScore);
            }
            else
            {
                HighScoreMenu.SetActive(false);
            }
            TrackManager._instance._gameState = GameState.GAMEOVER;
           //FirebaseEvents.instance.LogFirebaseEvent("Screen5", "FifthScreen", "Revive-gameover");
                #if UNITY_ANDROID
                  //  PGGC_Manager._instance.SaveToCloud();
                #endif
        }

    }

    public void StoryTellingBtn(TextMeshProUGUI btnTxt)
    {
        Animator stotyAnim = DataManager.instance.storyPnl.GetComponent<Animator>();
        if (stotyAnim.GetCurrentAnimatorStateInfo(0).IsName("1"))
        {
            stotyAnim.Play("2", 0);
        }
        else if (stotyAnim.GetCurrentAnimatorStateInfo(0).IsName("2"))
        {
            stotyAnim.Play("3", 0);
        }
        else if (stotyAnim.GetCurrentAnimatorStateInfo(0).IsName("3"))
        {
            stotyAnim.Play("4", 0);
        }
        else if (stotyAnim.GetCurrentAnimatorStateInfo(0).IsName("4"))
        {
            stotyAnim.Play("5", 0);
            btnTxt.text = "FINISH";
        }
        else if (stotyAnim.GetCurrentAnimatorStateInfo(0).IsName("5"))
        {
            stotyAnim.gameObject.SetActive(false);
        }
    }
    public void WatchVideoToDoubleCoin()
    {
        SfxManager.instance.PlayButtonClick();
      // AdsManager_Mediation.Instance.ShowRewardAd(RewardVideoType.DoubleIt);
    }
    public void MakeDoubleTheCoin()
    {
        hedrCoinTxt.text = "" + DataManager.instance.AddCoin(coinsCltd);
        int fromvalue = coinsCltd;
        int tovalue = coinsCltd * 2;
        //coinOnGameOver.text = "" + coinsCltd;
        StartCoroutine(doubleItTextUpdate(fromvalue,tovalue,coinOnGameOver));
    }

    void UpdateTextValues()
    {
        scoreOnGameOver.text = "" + TrackManager._instance.playerScript.m_Score;
        hedrCoinTxt.text = "" + DataManager.instance.AddCoin(coinsCltd);
        hedrKeyTxt.text = "" + DataManager.instance.Keys;
        coinOnGameOver.text = "" + coinsCltd;
        deathCount = 0;
    }
    public void SaveMe(bool videoCmpltd = false)
    {
        if(TrackManager._instance._gameState == GameState.PREGAMEOVER)
        {
            SfxManager.instance.PlayButtonClick();
            gameoverMenu.SetActive(false);
            header.SetActive(false);
            int needKeys = 1;//(int)Mathf.Pow(2, deathCount);
            if (DataManager.instance.Keys >= needKeys || videoCmpltd)
            {
                if (lastRoutine != null)
                    StopCoroutine(lastRoutine);
                TrackManager._instance._SaveMe();

                preGameOveMenu.SetActive(false);
                if (!videoCmpltd)
                    DataManager.instance.AddKey(-needKeys);
                hedrKeyTxt.text = "" + DataManager.instance.Keys;
                uimanager.instance.hedrkeyobj.Play();

                deathCount += 1;
                PowerupOnOff(powerupArray[3], 3, false);
                StopChase();
            }
            else
            {
                OpenStorePanel(false);
            }
          //FirebaseEvents.instance.LogFirebaseEvent("Screen3", "ThirdScreen", "Revive-Gameplay");
        }
    }
    public void WatchVideoToContinue()
    {
        SfxManager.instance.PlayButtonClick();
        //AdsManager_Mediation.Instance.ShowRewardAd(RewardVideoType.SaveMe);
    }
    public void Pause(bool pause)
    {
        if (!TrackManager._instance.tutorialPld || camCntrl.introScene.activeInHierarchy)
        {
            return;
        }
        if (pause && TrackManager._instance._gameState != GameState.PLAYING)
            return;
        SfxManager.instance.PlayButtonClick();       
        header.SetActive(pause);
        enemyAnim.enabled = pause;
        pauseMenu.SetActive(pause);
        if (pause)
        {
            TrackManager._instance._gameState = GameState.PAUSE;          
            MissionManager.instance.OnPausePnlAppear();
        }
        else
        {
            resumeMenu.SetActive(true);
        }

    }
    public bool _isPaused = false;
    void OnApplicationPause(bool pausestate)
    {
        _isPaused = pausestate;
        if (_isPaused&& TrackManager._instance._gameState == GameState.PLAYING)
        {
            Pause(true);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        _isPaused = !focus;

    }

    public void DailyChallenge(ScrollRect rect)
    {
        SfxManager.instance.PlayButtonClick();
        rect.verticalNormalizedPosition = 1.0f;
        TrackManager._instance.dailyBonus.OnDailyChallengeMenuAppear();
       //FirebaseEvents.instance.LogFirebaseEvent("Screen2", "SecondaryScreens", "InDailyChallengePanel");
    }


    public void PauseToHome()
    {
        SfxManager.instance.PlayButtonClick();
        pauseMenu.SetActive(false);
        GameOver(true);
        TrackManager._instance.playerScript.anim.enabled = true;
        TrackManager._instance.playerScript.anim.Play("Idle1");
        for(int i=0;i < powerupArray.Length; i++)
        {
            PowerupOnOff(powerupArray[i], i, false);
        }
        TrackManager._instance.playerScript.StopFlying();
    }

    IEnumerator doubleItTextUpdate(int fromvalue,int tovalue,TextMeshProUGUI txt)
    {
        if(fromvalue > 10)
        {
            int temp = fromvalue / 20;
            for(int i=0; i < 20; i++)
            {
                fromvalue += temp;
                txt.text = fromvalue.ToString();
                yield return new WaitForSeconds(0.02f);
            }
        }
        txt.text = tovalue.ToString();
        doubleItCoinIcon.clip = doubleItCoinIcon.GetClip("gameovercoinsdoubled");
        doubleItCoinIcon.GetComponent<Animation>().Play();
    }
    public void Home()
    {
        SfxManager.instance.PlayButtonClick();
        gameOverMesh.transform.parent.gameObject.SetActive(false);
        doubleItCoinIcon.clip = doubleItCoinIcon.GetClip("gameovercoinsidle");
        doubleItCoinIcon.GetComponent<Animation>().Play();
        gameoverMenu.SetActive(false);
        mainMenu.SetActive(true);
        coinsCltd = 0;
        inGameCoinTxt.text = "" + 0;
        TrackManager._instance._gameState = GameState.HOME;
     // FirebaseEvents.instance.LogFirebaseEvent("Screen1", "MainMenu", "OnHome(Gameplay)");
        ratecounter += 1;
        openratepanel();
    }

    public void UpdateCoin()
    {
        SfxManager.instance.PlayCoinSound();
        coinsCltd += 1;
        if (powerupArray[1].isActivated)
        {
            coinsCltd += 1;
        }
        inGameCoinTxt.text = "" + coinsCltd;
        MissionManager.instance.CoinTrigger();
    }
    public void ShowNotEnoughCoin(bool coin = true)
    {
        lessCoinMenu.SetActive(true);
        if (!coin)
        {

        }
    }
    public void TapHeadBoost(GameObject self)
    {
        self.SetActive(false);
        StopChase();
        TrackManager._instance._gameState = GameState.FLASH;
        DataManager.instance.GetHeadBoost -= 1;
    }

    void OnApplicationQuit()
    {
        if (TrackManager._instance._gameState == GameState.PLAYING)
        {
            MissionManager.instance.GameOver(coinsCltd, (int)TrackManager._instance.playerScript.m_TotalWorldDistance,TrackManager._instance.playerScript.m_Score);
        }
        TimeSpan ts = DateTime.Now.Subtract(GameOpenTime);
       //FirebaseEvents.instance.LogMulFirebaseEvent("OnQuitEvent", new Firebase.Analytics.Parameter[] {
       //     new Firebase.Analytics.Parameter("SessionCount",TrackManager._instance.sesstionCount),
       //     new Firebase.Analytics.Parameter("SesstionTime",ts.TotalMinutes),
       //     new Firebase.Analytics.Parameter("InterSitialPlayed",intersitialCount),
       //     new Firebase.Analytics.Parameter("RewardVideoPlayed",rewardVideoCount),
       // });
       // FirebaseEvents.instance.SetUserProperty("ADSSHOWN", "" + intersitialCount);
       // FirebaseEvents.instance.SetUserProperty("USERSESSION", ""+ts.TotalMinutes);
       // FirebaseEvents.instance.SetUserProperty("REWARDSSHOWN", "" + rewardVideoCount);
       // FirebaseEvents.instance.SetUserProperty("GAMESPLAYED", "" + TrackManager._instance.sesstionCount);

        ///
        //local notifications
        }
    public void VaultNotification()
    {
        if (DataManager.instance.GiftBoxCount > 0)
        {

            string dString = DataManager.instance.GetBoxTime;
            if (dString != null)
            {
                DateTime oldDate = Convert.ToDateTime(dString);
                DateTime currentTime = DateTime.Now;
                TimeSpan deff = currentTime.Subtract(oldDate);
                //if (NotificationExample.Instance != null)
                //{
                //    NotificationExample.Instance.CancelAll();
                //    NotificationExample.Instance.ScheduleNormal((int)deff.TotalSeconds, "VAULT IS READY", "vault is ready to open now");
                //}
            }
            else
            {
                DataManager.instance.GetBoxTime = Convert.ToString(DateTime.Now);
            }
        }
    }

    private void OnDestroy()
    {
        //VaultNotification();
        //NotificationExample.Instance.ScheduleNormal(300, LocalnotifTitles[UnityEngine.Random.Range(0, LocalnotifTitles.Length)], Localnotifdescs[UnityEngine.Random.Range(0, Localnotifdescs.Length)]); ///callback user on quiting notification
    }
    private string[] LocalnotifTitles = new string[] { "CAN YOU BEAT", "BICYCLE RIDE?", "JUMPER", "MAKE YOUR MARK" };
    private string[] Localnotifdescs = new string[] { "Beat Your Highscore and get more rewards", "Can you ride cycle without crashing, lets try!", "ComeOn! Swing with pogostick on roads and enjoy", "Beat scores in leaderboard and stand top in leaderboards" };
    // enemy control \\
    [Header("EnemyThings")]
    public float follwTime;
    public float follwDistance;
    public CameraController camCntrl;
    public Transform EnemyTransform;
    public Animator enemyAnim;
    IEnumerator EnemyChase()
    {
        inChase = true;
        float fTime = follwTime;
        float dis = follwDistance+3.0f;
        Transform pTransform = TrackManager._instance.playerScript.transform;
        EnemyTransform.gameObject.SetActive(true);
        enemyAnim.enabled = true;
        enemyAnim.SetBool("catch", false);
        enemyAnim.transform.parent.gameObject.SetActive(true);
        while (fTime > 0)
        {
            if (TrackManager._instance._gameState == GameState.PAUSE)
                yield return new WaitUntil(() => TrackManager._instance._gameState == GameState.PLAYING);
            fTime -= Time.deltaTime;
           
            if(fTime > 8f)
            {
                camCntrl.offsetPosition.z = Mathf.Lerp(camCntrl.offsetPosition.z, -8f, Time.deltaTime);
                camCntrl.offsetPosition.y = Mathf.Lerp(camCntrl.offsetPosition.y, 5.5f, Time.deltaTime);
                dis = Mathf.Lerp(dis, follwDistance, Time.deltaTime);
            }
            if(fTime < 2f)
            {
                camCntrl.offsetPosition.z = Mathf.Lerp(camCntrl.offsetPosition.z, -5.5f, Time.deltaTime);
                camCntrl.offsetPosition.y = Mathf.Lerp(camCntrl.offsetPosition.y, 4.5f, Time.deltaTime);
                dis += Time.deltaTime * 0.5f;
                if (inChase)
                {
                    inChase = false;
                }
            }
            EnemyTransform.position = pTransform.TransformPoint(new Vector3(0, 0, -dis));
            EnemyTransform.rotation = pTransform.rotation;
            yield return null;
        }
     
        EnemyTransform.gameObject.SetActive(false);
        camCntrl.offsetPosition.z = -5.5f;
        camCntrl.offsetPosition.y = 4.5f;
        enemyAnim.transform.parent.gameObject.SetActive(false);

    }
    IEnumerator EnterDie()
    {
        Transform player_ = TrackManager._instance.playerScript.transform;
        Vector3 initialPos = Vector3.zero;
        if (chaseScene != null)
        {
            StopCoroutine(chaseScene);
            initialPos = EnemyTransform.position;
        }
        else
        {
            initialPos = player_.TransformPoint(new Vector3(-1f, 0, -5.5f));
        }
        Vector3 wantedPos = getEmptyPos(player_);//player_.TransformPoint(new Vector3(-1f,0,-0.5f));
        wantedPos.y = 0;
        EnemyTransform.gameObject.SetActive(true);
   
        float progress = 0.0f;
        while (progress < 1)
        {
            progress += Time.deltaTime;
            EnemyTransform.position = Vector3.Lerp(initialPos, wantedPos, progress);
            camCntrl.tCamera.localPosition -= Vector3.forward*progress*0.07f;
            yield return null;
        }
        enemyAnim.SetBool("catch", true);
        EnemyTransform.LookAt(player_);
        EnemyTransform.eulerAngles = new Vector3(0, EnemyTransform.eulerAngles.y, 0);
    }

    Vector3 getEmptyPos(Transform player_)
    {
        Vector3 freeSpace = player_.TransformPoint(new Vector3(-1f, 0, -0.5f));
        freeSpace.y = 1f;
        Collider[] hitColliders = Physics.OverlapSphere(freeSpace, 1.5f);

        if (hitColliders.Length > 0)
        {
            freeSpace = player_.TransformPoint(new Vector3(1f, 0, -0.5f));
            freeSpace.y = 1f;
            hitColliders = Physics.OverlapSphere(freeSpace, 0.5f);
            if (hitColliders.Length > 0)
            {
                freeSpace = player_.TransformPoint(new Vector3(1f, 0, 0.5f));
            }
        }
        freeSpace.y = 0.0f;
        return freeSpace;
    }

    // Native Share --------------------------
    // while publishing the app for ios replace ios app id
    private string appUrl = "";
    public void NativeShareLink(bool share)
    {
        string subjectString = "All New Exciting Endless Runner Game";
#if UNITY_ANDROID
        appUrl = "https://play.google.com/store/apps/details?id=" + Application.identifier;
#elif UNITY_IOS
                        appUrl = "itms-apps://itunes.apple.com/app/id" + "1473255832" + "?mt=8";
#endif

        //if (share)
        //{
        //    new NativeShare().SetTitle(Application.productName).SetText(subjectString + "\n" + appUrl).Share();
        //  FirebaseEvents.instance.LogFirebaseEvent("NativeShare", "Success", "");
        //}
        //else
        //{
        //        Application.OpenURL(appUrl);
        //      FirebaseEvents.instance.LogFirebaseEvent("RateUS", "Clicked", "");
        //        PlayerPrefs.SetInt("RatingDone", 1);
        //}
    }

    private int ratecounter = 0;
    public void openratepanel()
    {
        if (ratecounter == 3)
        {
            if (PlayerPrefs.GetInt("RatingDone") == 0)
            {
                ratepanel.SetActive(true);
            }
            else
            {
                ratecounter = 5;
            }
        }
        else if( ratecounter >= 6)
        {
            //if (FirebaseRemoteConfiguration.instance.Remoteiapactive == "ON")
            //{
            //   offeriapbtn1.SetActive(false);
            //   RemotePack.SetActive(true);
            //   offeriapbtn1_0.SetActive(true);
            //   offeriapbtn2.SetActive(false);
            //   OfferIappanel.GetComponent<Animation>().Play();
            //}else if (FirebaseRemoteConfiguration.instance.specialiapactive == "ON")
            //{
            //    offeriapbtn1.SetActive(true);
            //    offeriapbtn1_0.SetActive(true);
            //    offeriapbtn2.SetActive(false);
            //    OfferIappanel.GetComponent<Animation>().Play();
            //}
            ratecounter = 0;
        }
    }

    public void RateNothanks()
    {
        PlayerPrefs.SetInt("RatingDone", 1);
        ratepanel.SetActive(false);
    }
    //  -------------------------------------
}
[Serializable]
public class PowerUp
{
    public String name;
    public GameObject UIRepresentObj;
    public GameObject PlrRepObj;
    public Image fillImg;
    public float time;
    public int UpgradeNo;
    public float upgradeMul;
    public bool isActivated = false;
    [Header("shop")]
    public Image upgradeFillImg;
    public TextMeshProUGUI upgradePrice;
    public TextMeshProUGUI UpgradedVlue;
}
public enum PlayerAge
{
    Beiginer,
    Intermediate,
    Advanced
}

