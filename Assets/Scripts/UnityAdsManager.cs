using UnityEngine;
using UnityEngine.Advertisements;
using SkillzSDK;
public class UnityAdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    string _gameId;
    [SerializeField] bool _testMode = true;

    public static UnityAdsManager instance;

    public bool adsActive;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        InitializeAds();
    }
    public void ShowAd()
    {
        Match match = SkillzCrossPlatform.GetMatchInfo();
        if (match != null)
        {
            bool isCashTournament= System.Convert.ToBoolean(match.IsCash);

            if (!isCashTournament)
            {
                Debug.Log("This is virtual currency tournament, show AD now");
                if (Advertisement.isInitialized)
                {
                    Debug.Log("Advertisement is Initialized,show AD Now");
                    SkillzManager.gameOverCount++;
                    Debug.Log("Gameover Count::" + SkillzManager.gameOverCount);
                    if (SkillzManager.gameOverCount % 2 != 0)
                    {
                        if (adsActive)
                            LoadInerstitialAd();
                    }
                }
                else
                {
                    InitializeAds();
                }
            }
            else
            {
                Debug.Log("This is cash tournament. Do not show AD");
            }
        }
        

        
    }
    public void InitializeAds()
    {
        //  _gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? _iOSGameId : _androidGameId;
#if UNITY_IPHONE
        _gameId = _iOSGameId;
#else
_gameId = _androidGameId;
#endif
        Advertisement.Initialize(_gameId, _testMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        //LoadInerstitialAd();
       // LoadBannerAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void LoadInerstitialAd()
    {
#if UNITY_ANDROID
    Advertisement.Load("Interstitial_Android", this);
#elif UNITY_IOS
        Advertisement.Load("Interstitial_iOS", this);
#endif

    }

    public void LoadRewardedAd()
    {
        //Advertisement.Load("Rewarded_Android", this);
#if UNITY_ANDROID
    Advertisement.Load("Rewarded_Android", this);
#elif UNITY_IOS
        Advertisement.Load("Rewarded_iOS", this);
#endif
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("OnUnityAdsAdLoaded");
        Advertisement.Show(placementId, this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("OnUnityAdsShowFailure");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("OnUnityAdsShowStart");
        Time.timeScale = 0;
        Advertisement.Banner.Hide();
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("OnUnityAdsShowClick");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("OnUnityAdsShowComplete " + showCompletionState);
        if (placementId.Equals("Rewarded_Android") && UnityAdsShowCompletionState.COMPLETED.Equals(showCompletionState))
        {
            Debug.Log("rewared Player");
        }
        Time.timeScale = 1;
        Advertisement.Banner.Show("Banner_Android");
    }



    public void LoadBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load("Banner_Android",
            new BannerLoadOptions
            {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError
            }
            );
    }

    void OnBannerLoaded()
    {
        Advertisement.Banner.Show("Banner_Android");
    }

    void OnBannerError(string message)
    {

    }

}