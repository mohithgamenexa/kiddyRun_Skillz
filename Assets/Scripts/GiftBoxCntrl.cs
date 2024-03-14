
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
public class GiftBoxCntrl : MonoBehaviour
{
    int boxCount = 0;
    bool canClaim = false;    
    public GameObject[] GiftBoxes;
    public TextMeshProUGUI timerTxt, vaultitemlabel;
    public GameObject openNowBtn;
    public GameObject claimBtn;
    public GameObject BoxMenu;
    public ScrollRect myRect;
    public GameObject Panelobjects, noVaultLabel;
    public Sprite bagavail, nobag;
    //public TextMeshProUGUI boxcounter;

    public void doclaimavail()
    {
        boxCount = DataManager.instance.GiftBoxCount;
        if (boxCount > 0)
        {
            string dString = DataManager.instance.GetBoxTime;
            if (dString != null)
            {
                DateTime oldDate = Convert.ToDateTime(dString);
                DateTime currentTime = DateTime.Now;
                TimeSpan deff = currentTime.Subtract(oldDate);
                if ((int)deff.TotalHours >= 3)
                {
                    uimanager.instance.claimnotif.Play();
                }
            }
        }
    }

    public void OpenMenu()
    {
        SfxManager.instance.PlayButtonClick();
        myRect.verticalNormalizedPosition = 1.0f;
        BoxMenu.SetActive(false);
        canClaim = false;
        RefreshChestSlot();
        gameObject.SetActive(true);
        if (boxCount > 0)
        {
            //boxcounter.text = (boxCount - 1).ToString();
            Panelobjects.SetActive(true);
            noVaultLabel.SetActive(false);
        }
        else
        {
            Panelobjects.SetActive(false);
            noVaultLabel.SetActive(true);
        }
    }
    void Update()
    {
        claimavail();
    }

    public void HandlerDecvaulttime()
    {
        string dString = DataManager.instance.GetBoxTime;
        if (dString != null )
        {
            DateTime oldDate = Convert.ToDateTime(dString);
            DateTime rem = oldDate.Subtract(new TimeSpan(1, 0, 0));
            DataManager.instance.GetBoxTime = rem.ToString();
        }
    }

    public void reducevaultTime()
    {
//       AdsManager_Mediation.Instance.ShowRewardAd(RewardVideoType.decVaultTime);
    }

    public void claimavail()
    {
        if (boxCount > 0 && !canClaim)
        {
            string dString = DataManager.instance.GetBoxTime;
            if (dString != null)
            {
                DateTime oldDate = Convert.ToDateTime(dString);
                DateTime currentTime = DateTime.Now;
                TimeSpan deff = currentTime.Subtract(oldDate);
                if ((int)deff.TotalHours >= 3)
                {
                    canClaim = true;
                    timerTxt.text = "00:00:00";
                    claimBtn.SetActive(true);
                    openNowBtn.SetActive(false);
                    uimanager.instance.claimnotif.Play();
                }
                else
                {
                    TimeSpan rem = new TimeSpan(3, 0, 0).Subtract(new TimeSpan(deff.Hours, deff.Minutes, deff.Seconds));
                    timerTxt.text = rem.Hours.ToString("00") + ":" + rem.Minutes.ToString("00") + ":" + rem.Seconds.ToString("00");
                    claimBtn.SetActive(false);
                    openNowBtn.SetActive(true);
                }
            }
            else
            {
                DataManager.instance.GetBoxTime = Convert.ToString(DateTime.Now);
            }
        }
    }


    public void testclaim()
    {
        string dString = DataManager.instance.GetBoxTime;
        if (dString != null)
        {
            DataManager.instance.GetBoxTime = DateTime.Now.Subtract(new TimeSpan(3, 0, 0)).ToString();
        }
    }
    void RefreshChestSlot()
    {
        boxCount = DataManager.instance.GiftBoxCount;
        for (int i = 0; i < GiftBoxes.Length; i++)
        {
            if (i < boxCount)
            {
                GiftBoxes[i].SetActive(true);
            }
            else
            {
                GiftBoxes[i].SetActive(false);
            }
        }

        ////
        /// 
        for (int i = 1; i < GiftBoxes.Length; i++)
        {
            if (i < boxCount)
            {
                GiftBoxes[i].transform.parent.gameObject.GetComponent<Image>().sprite = bagavail;
            }
            else
            {
                GiftBoxes[i].transform.parent.gameObject.GetComponent<Image>().sprite = nobag;
            }
        }

        if (boxCount <= 0)
        {
            timerTxt.text = "00:00:00";
            openNowBtn.gameObject.SetActive(false);
            claimBtn.SetActive(false);
        }
    }



    public void WatchVideoToBox()
    {
        SfxManager.instance.PlayButtonClick();
        if (boxCount >= 4)
        {
            StartCoroutine(uimanager.instance.ShowInfo("NO SLOTS AVAILBLE", 0.0f));
        }
        else
        {
           //AdsManager_Mediation.Instance.ShowRewardAd(RewardVideoType.BuyVault);
        }     
    }
    public void GetBox()
    {
        DataManager.instance.GiftBoxCount += 1;
        OpenMenu();
    }
    public void ClaimWithKeys()
    {
        SfxManager.instance.PlayButtonClick();
        if (DataManager.instance.Keys >= 36)
        {
            DataManager.instance.Keys -= 36;
            uimanager.instance.hedrKeyTxt.text = "" + DataManager.instance.Keys;
            uimanager.instance.hedrkeyobj.Play();

            BoxMenu.SetActive(true);
        }
        else
        {
            uimanager.instance.OpenStorePanel(false);
        }
    }
    int rewardCount;
    int reward1, reward2;
    string s1, s2;
  

 
    public void TapToOpenBox()
    {
            uimanager.instance.claimnotif.Stop();
            BoxMenu.SetActive(false);
            canClaim = false;
            openNowBtn.gameObject.SetActive(true);
            claimBtn.SetActive(false);
            //DataManager.instance.GiftBoxCount -= 1;
            DataManager.instance.GetBoxTime = Convert.ToString(DateTime.Now);
            OpenMenu();
            DataManager.instance.bagcount += 1;
            
            if (DataManager.instance.bagcount == 1)
            {
               //PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQCw");
            }
            else if(DataManager.instance.bagcount == 5)
            {
              //PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQEA");
            }
            else if (DataManager.instance.bagcount == 10)
            {
               //PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQEQ");
            }
            else if (DataManager.instance.bagcount == 20)
            {
               //PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQEg");
            }
            else if (DataManager.instance.bagcount == 50)
            {
               //PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQEw");
            }
            else if (DataManager.instance.bagcount == 100)
            {
              //PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQFA");
            }
            else if (DataManager.instance.bagcount == 200)
            {
               //PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQFQ");
            }

            ///
            if (boxCount > 0)
            {
                uimanager.instance.VaultNotification();
            }
        uimanager.instance.hedrKeyTxt.text = "" + DataManager.instance.Keys;
        uimanager.instance.hedrCoinTxt.text = "" + DataManager.instance.Coins; 

    }

    public void OpenBox()
    {
       if(boxCount > 0 && canClaim)
        {
            DataManager.instance.GetBoxTime = Convert.ToString(DateTime.Now);
            canClaim = false;
            //DataManager.instance.GiftBoxCount -= 1;
            RefreshChestSlot();
        }
    }
}
