
using UnityEngine;
using System;
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public GameObject storyPnl;
    public Animation _btnbag;
    void Awake()
    {
     //PlayerPrefs.DeleteAll();
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            DestroyImmediate(gameObject);
        }
        if (PlayerPrefs.GetInt("init") == 0)
        {
            PlayerPrefs.SetInt("init", 1);
            AddCoin(1000);
            GetMusic = 1;
            GetSFX = 1;
            GetNotification = 1;
            AddKey(1);
            GetAdsON = 1;
            removeAdsIap = 0;
            fbloginstatus = 0;
            HighSpeed = 45;
            lastPlayedDate = DateTime.Now.ToString();
            daysplayed += 1;
            storyPnl.SetActive(true);
            GiftBoxCount = 1;
            GetBoxTime = DateTime.Now.Subtract(new TimeSpan(3, 0, 0)).ToString();
            _btnbag.Play();
        }
        else
        {
            storyPnl.SetActive(false);
        }
        Adsfrequency = 2;
        //Coins = 1500;
    }

    public int Keys
    {
        get
        {
            return PlayerPrefs.GetInt("key");
        }
        set
        {
            PlayerPrefs.SetInt("key", value);
        }
    }
    public int AddKey(int count)
    {
        int _keys = PlayerPrefs.GetInt("key");
        _keys += count;
        PlayerPrefs.SetInt("key", _keys);
        return _keys;
    }
    public int AddCoin(int count)
    {
        int _coins = PlayerPrefs.GetInt("coin");
        _coins += count;
        PlayerPrefs.SetInt("coin", _coins);
        return _coins;
    }
    public int GetPowUpNo(int no)
    {
        return PlayerPrefs.GetInt("powUp" + no);
    }
    public void UpdagradePowrup(int no)
    {
        int n = GetPowUpNo(no);
        PlayerPrefs.SetInt("powUp" + no, n + 1);
    }
    public int HighScore
    {
        get
        {
           return PlayerPrefs.GetInt("hghscr");
        }
        set
        {
            PlayerPrefs.SetInt("hghscr", value);
        }
    }

    public int HighSpeed
    {
        get
        {
            return PlayerPrefs.GetInt("hghspd");
        }
        set
        {
            PlayerPrefs.SetInt("hghspd", value);
        }
    }
    public int Coins
    {
        get
        {
            return PlayerPrefs.GetInt("coin");
        }
        set
        {
            PlayerPrefs.SetInt("coin", value);
        }       
    }
    public int Boards
    {
        get
        {
            return PlayerPrefs.GetInt("board");
        }
        set
        {
            PlayerPrefs.SetInt("board", value);
        }
    }
    public int ScoreMul
    {
        get
        {
            return PlayerPrefs.GetInt("+5Mul");
        }
        set
        {
            PlayerPrefs.SetInt("+5Mul", value);
        }
    }
    public int GiftBoxCount
    {
        get
        {
            return PlayerPrefs.GetInt("GiftBox");
        }
        set
        {
            if(value <= 4)
            {
                if(PlayerPrefs.GetInt("GiftBox") <= 0)
                {
                    GetBoxTime = Convert.ToString(DateTime.Now);
                }
                PlayerPrefs.SetInt("GiftBox", value);
            }            
        }
    }
    public String GetBoxTime
    {
        get
        {
            return PlayerPrefs.GetString("gTime");
        }
        set
        {
            PlayerPrefs.SetString("gTime", value);
        }
    }
    public int GetCharacter
    {
        get
        {
          return PlayerPrefs.GetInt("chSlctd");
        }
        set
        {
            PlayerPrefs.SetInt("chSlctd",value);
        }
    }
    public bool haveEnoughCoins(int cost)
    {
        return cost <= PlayerPrefs.GetInt("coin");
    }
    public int tutorialPlyd
    {
        get
        {
            return PlayerPrefs.GetInt("TTplyd");
        }
        set
        {
            PlayerPrefs.SetInt("TTplyd",1);
        }
    }
    public int GetHeadBoost
    {
        get
        {
            return PlayerPrefs.GetInt("Hbst");
        }
        set
        {
            PlayerPrefs.SetInt("Hbst", value);
        }
    }
    public int GetMusic
    {
        get
        {
           return PlayerPrefs.GetInt("Music");
        }
        set
        {
            PlayerPrefs.SetInt("Music", value);
        }
    }
    public int GetSFX
    {
        get
        {
            return PlayerPrefs.GetInt("SFX");
        }
        set
        {
            PlayerPrefs.SetInt("SFX", value);
        }
    }
    public int GetNotification
    {
        get
        {
            return PlayerPrefs.GetInt("notfi");
        }
        set
        {
            PlayerPrefs.SetInt("notfi", value);
        }
    }

    public int GetAdsON
    {
        get
        {
            return PlayerPrefs.GetInt("InterstitialAds");
        }
        set
        {
            PlayerPrefs.SetInt("InterstitialAds", value);
        }
    }

    public int Adsfrequency
    {
        get
        {
            return PlayerPrefs.GetInt("Adsfrequency");
        }
        set
        {
            PlayerPrefs.SetInt("Adsfrequency", value);
        }
    }
    public int IAPSuccessCount
    {
        get
        {
            return PlayerPrefs.GetInt("IAPSUC");
        }
        set
        {
            PlayerPrefs.SetInt("IAPSUC",value);
        }
    }
    public int bagcount
    {
        get
        {
            return PlayerPrefs.GetInt("bagCnt");
        }
        set
        {
            PlayerPrefs.SetInt("bagCnt", value);
        }
    }

    public int removeAdsIap
    {
        get
        {
            return PlayerPrefs.GetInt("removeAdsIap");
        }
        set
        {
            PlayerPrefs.SetInt("removeAdsIap", value);
        }
    }

    public int fbloginstatus
    {
        get
        {
            return PlayerPrefs.GetInt("fbloginstatus");
        }
        set
        {
            PlayerPrefs.SetInt("fbloginstatus", value);
        }
    }

    public string lastPlayedDate
    {
        get
        {
            return PlayerPrefs.GetString("installeddate");
        }
        set
        {
            PlayerPrefs.SetString("installeddate", value);
        }
    }

    public int daysplayed
    {
        get
        {
            return PlayerPrefs.GetInt("daysplayed");
        }
        set
        {
            PlayerPrefs.SetInt("daysplayed", value);
        }
    }


    public int logininit
    {
        get
        {
            return PlayerPrefs.GetInt("logininit");
        }
        set
        {
            PlayerPrefs.SetInt("logininit", value);
        }
    }
    public int GamesPlayedOL
    {
        get
        {
            return PlayerPrefs.GetInt("GplydOL");
        }
        set
        {
            PlayerPrefs.SetInt("GplydOL", value);
        }
    }
}
