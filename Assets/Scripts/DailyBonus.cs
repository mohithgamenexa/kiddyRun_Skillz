using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class DailyBonus : MonoBehaviour
{
    public bool isPauseMenu;
    public TextMeshProUGUI discriptonTxt;
    public TextMeshProUGUI letterTxt;
    public TextMeshProUGUI timerTxt;
    public TextMeshProUGUI dayCount;
    public List<string> dailyWords;
    public Image[] rewardImgs;
    public Sprite claimed, onTheday, futureDay;
    public GameObject[] claimedObjs;
    void FixedUpdate()
    {
        dayCheck();
    }
    bool isRwrdReady;

    public void dayCheck()
    {
        isRwrdReady = false;
        string stringDate = PlayerPrefs.GetString("PlayDate");
        if (stringDate != "")
        {
            DateTime oldDate = Convert.ToDateTime(stringDate);
            DateTime newDate = System.DateTime.Now;
            TimeSpan difference = newDate.Subtract(oldDate);
            if (difference.TotalDays >= 1)
            {
                if (difference.TotalDays >= 2)
                {
                    GetSetRwrdNo = 0;
                }                            
                DayChanched();
            }
            else
            {
                TimeSpan tS = new TimeSpan(24, 0, 0).Subtract(difference);
                if (PlayerPrefs.GetInt("Dplyd") != 0)
                {
                    if (!isPauseMenu)
                    {
                        discriptonTxt.text = "Daily Challenge Completed";
                        letterTxt.text = "Time For Next Battle";
                        timerTxt.text = tS.Hours + " : " + tS.Minutes + " : " + tS.Seconds;
                    }
                    else
                    {
                        discriptonTxt.text = "Challenge Completed Wait For \n" + tS.Hours.ToString("00") + ":" + tS.Minutes.ToString("00") + ":" + tS.Seconds.ToString("00");
                    }
                }
                else
                {
                    if (!isPauseMenu)
                    {
                        discriptonTxt.text = "COLLECT THE LETTERS";
                        letterTxt.text = "" + dailyWords[PlayerPrefs.GetInt("wrd")];
                        timerTxt.text = tS.Hours + " : " + tS.Minutes + " : " + tS.Seconds;
                    }
                    else
                    {
                        discriptonTxt.text = "<color=white>" + "COLLECT THE LETTERS : " + " </color>" + "<size=+18>" + "<color=green>" + dailyWords[PlayerPrefs.GetInt("wrd")]+ "\n" + "</color>" + "</size>"   + "<color=red>" + tS.Hours + " : " + tS.Minutes + " : " + tS.Seconds + "</color>";
                    }                    
                    isRwrdReady = true;
                }    
            }          
        }
        else
        {
            DayChanched();
        }
    }
    public void DayChanched()
    {
        isRwrdReady = true;
        PlayerPrefs.SetInt("Dplyd", 0);
        string s = Convert.ToString(DateTime.Now);
        PlayerPrefs.SetString("PlayDate", s);
        TrackManager._instance.dayWordIndx = 0;
        TrackManager._instance.canGiveWrd = true;
    }
    public void OnDailyChallengeMenuAppear()
    {
        int rwrdNo = GetSetRwrdNo;
        if(rwrdNo > 4)
        {
            dayCount.gameObject.SetActive(true);
            dayCount.text = (30 - rwrdNo) + " days to go!";
        }
        else
        {
            dayCount.gameObject.SetActive(false);
            for (int i = 0; i<4; i++)
            {
                if( i == rwrdNo)
                {
                    rewardImgs[i].sprite = onTheday;
                    claimedObjs[i].SetActive(false);
                }
                else if(i < rwrdNo)
                {
                    rewardImgs[i].sprite = claimed;
                    claimedObjs[i].SetActive(true);
                }
                else
                {
                    rewardImgs[i].sprite = futureDay;
                    claimedObjs[i].SetActive(false);
                }
            }
        }
        gameObject.SetActive(true);
    }
    public bool CanDoDailyBattle()
    {
        dayCheck();
        /*int plyd = PlayerPrefs.GetInt("Dplyd");
        if(plyd == 1)
        {
            return false;
        }*/
        return isRwrdReady;
    }
    public void WordCompleted()
    {
        SetDayPlayCmpltd();
        int wrdNo = GetSetWrdNo;
        
        wrdNo += 1;
        if(wrdNo >= dailyWords.Count)
        {
            wrdNo = 0;
           //PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQDw");
        }
        GetSetWrdNo = wrdNo;
        int rwrdNo = GetSetRwrdNo;
        rwrdNo += 1;
        if (rwrdNo < 1)
            rwrdNo = 1;
        if (rwrdNo == 1)
        {
            // day 1 reward
            DataManager.instance.AddCoin(500);
            string infoTxt = "Word Completed" + "\n you got " + 500 + " Coins.";
            uimanager.instance.DelayOnWordCmpltd(infoTxt);
        }
        else if (rwrdNo == 2)
        {
            // day 2 reward
            DataManager.instance.Boards += 2;
            string infoTxt = "Word Completed" + "\n you got " + 2 + " BiCycles.";
            uimanager.instance.DelayOnWordCmpltd(infoTxt);
        }
        else if (rwrdNo == 3)
        {
            // day 3 reward
            DataManager.instance.AddKey(10);
            string infoTxt = "Word Completed" + "\n you got " + 10 + " MedicalKits.";
            uimanager.instance.DelayOnWordCmpltd(infoTxt);
        }
        else if (rwrdNo == 4)
        {
            // day 4 reward
            DataManager.instance.GetHeadBoost += 2;
            string infoTxt = "Word Completed" + "\n you got " + 2 + " HeadBoosts.";
            uimanager.instance.DelayOnWordCmpltd(infoTxt);
           // PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQAw");
        }
        else if (rwrdNo >= 30)
        {
            // day 30 reward
            rwrdNo = 0;
            string infoTxt = "Word Completed" + "\n MegaReward Claimed";
            uimanager.instance.DelayOnWordCmpltd(infoTxt);
          /// PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQDA");
        }

        GetSetRwrdNo = rwrdNo;
        if(wrdNo == 1)
        {
           //PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQDQ");
        }
        if (wrdNo == 5)
        {
          // PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQDg");

        }
        if (wrdNo == 20)
        {
          //PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQBw");
        }
    }
    public void ClaimDailyBonus(int rwrdNo)
    {
        
    }
    int GetSetWrdNo
    {
        get
        {
            return PlayerPrefs.GetInt("wrd");
        }
        set
        {
            PlayerPrefs.SetInt("wrd",value);
        }
    }
    int GetSetRwrdNo
    {
        get
        {
            return PlayerPrefs.GetInt("rwrdDay");
        }
        set
        {
            PlayerPrefs.SetInt("rwrdDay",value);
        }
    }
    void SetDayPlayCmpltd()
    {
        PlayerPrefs.SetInt("Dplyd", 1);
    }
}
