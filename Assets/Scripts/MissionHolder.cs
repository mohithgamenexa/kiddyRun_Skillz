
using UnityEngine;
using TMPro;

public class MissionHolder : MonoBehaviour
{
    
    public TextMeshProUGUI tittle,DisctiptionTxt,reminderTxt;  
    public GameObject priceObj, cmpltdObj;
    public int mNo;
    public Mission myMission;

    public void Reset()
    {

        if (MissionManager.instance.CurrentSet >= MissionManager.instance.data.missionSet.Length)
            return;
        myMission = MissionManager.instance.data.missionSet[MissionManager.instance.CurrentSet]._missionSet[mNo];
        tittle.text = myMission.Tittle;
        DisctiptionTxt.text = myMission.Description;
    
        if(MissionManager.instance.isMissionCmpltd(mNo) == 0)
        {
            priceObj.SetActive(true);
            cmpltdObj.SetActive(false);
            if (TrackManager._instance == null)
                Debug.LogError("sjflakslf");
            if (myMission.inOneRun && TrackManager._instance._gameState != GameState.PAUSE)
            {
                reminderTxt.gameObject.SetActive(false);
            }
            else
            {
                int r = 0;
                if(TrackManager._instance._gameState == GameState.PAUSE)
                {
                    r = MissionManager.instance.GetGamePlayRem(myMission);
                }
                else
                {
                    r = MissionManager.instance.GetReminder(myMission);
                }
                if (r <= 0)
                {                   
                    reminderTxt.gameObject.SetActive(false);
                    MissionManager.instance.SetMissionCmpltd(mNo,false);
                    priceObj.SetActive(false);
                    cmpltdObj.SetActive(true);
                    Debug.LogError("something went wrong.........");
                }
                else
                {
                    reminderTxt.text = r + " " + myMission.Reminder;
                }
            }
        }
        else
        {
            priceObj.SetActive(false);
            cmpltdObj.SetActive(true);
            reminderTxt.gameObject.SetActive(true);
            reminderTxt.text = "Completed";
        }
    }
 
    public void SkipMission()
    {
        SfxManager.instance.PlayButtonClick();
        if (DataManager.instance.haveEnoughCoins(1000))
        {
            MissionManager.instance.SetMissionCmpltd(mNo);
            DataManager.instance.AddCoin(-1000);
            uimanager.instance.hedrCoinTxt.text = DataManager.instance.Coins.ToString();
            uimanager.instance.hedrCoinobj.Play();

        }
        else
        {
            uimanager.instance.OpenStorePanel(false);
        }        
    }
}
