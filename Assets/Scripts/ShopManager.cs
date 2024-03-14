
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopManager : MonoBehaviour
{
    public Image[] btnBG;
    public GameObject[] menuS;
    public TextMeshProUGUI cycleHolder;
    public TextMeshProUGUI headBoostHolder;
    public GameObject purchasedBtn;
    public GameObject giftBoxMenu, boxMenu1;
    public CharacterSelection chCam;
    public ScrollRect myScroll;

    public void OnTopButtonPress(int no)
    {
        for (int i = 0; i < btnBG.Length; i++){
            btnBG[i].enabled = false;
            menuS[i].SetActive(false);
        }
        if(no == 1)
        {
            chCam.gameObject.SetActive(true);
            chCam.OnAppear();
            if(TrackManager._instance._gameState == GameState.GAMEOVER)
            {
                uimanager.instance.gameOverMesh.transform.parent.gameObject.SetActive(false);
            }
        }
        else
        {
            chCam.gameObject.SetActive(false);
        }
       
        btnBG[no].enabled = true;
        menuS[no].SetActive(true);
    }
    public void OnShopButtonPress()
    {
        cycleHolder.text = "you have " + DataManager.instance.Boards;
        headBoostHolder.text = "You have " + DataManager.instance.GetHeadBoost;
        myScroll.verticalNormalizedPosition = 1.0f;
        if (DataManager.instance.ScoreMul == 5)
        {
            purchasedBtn.SetActive(true);
        }
        else
        {
            purchasedBtn.SetActive(false);
        }
    }
    public void CloseShop()
    {
        SfxManager.instance.PlayButtonClick();
        chCam.gameObject.SetActive(false);
        if (TrackManager._instance._gameState == GameState.GAMEOVER)
        {
            uimanager.instance.gameOverMesh.transform.parent.gameObject.SetActive(true);
        }
        gameObject.SetActive(false);
    }

    public void OnBoardBuy(int cost)
    {
        SfxManager.instance.PlayButtonClick();
        if (DataManager.instance.haveEnoughCoins(cost))
        {
            uimanager.instance.PlayCoinParticle();
            DataManager.instance.AddCoin(-cost);
            DataManager.instance.Boards = DataManager.instance.Boards + 1;
            cycleHolder.text = "you have "+DataManager.instance.Boards;
            uimanager.instance.UpdateHeader();
//            FirebaseEvents.instance.LogFirebaseEvent("SuitPower", "Purchased", "Sucess");
        }
        else
        {
            uimanager.instance.ShowNotEnoughCoin();
        }
    }
    public void OnHeadBoostBuy(int cost)
    {
        SfxManager.instance.PlayButtonClick();
        if (DataManager.instance.haveEnoughCoins(cost))
        {
            uimanager.instance.PlayCoinParticle();
            DataManager.instance.AddCoin(-cost);
            uimanager.instance.UpdateHeader();
            DataManager.instance.GetHeadBoost += 1;
            headBoostHolder.text = "You have " + DataManager.instance.GetHeadBoost;
          //  FirebaseEvents.instance.LogFirebaseEvent("HeadBoost", "Purchased", "SingleUsage");
        }
        else
        {
            uimanager.instance.ShowNotEnoughCoin();
        }
    }
    public void ScoreMulBuy(int cost)
    {
        SfxManager.instance.PlayButtonClick();
        if (DataManager.instance.haveEnoughCoins(cost))
        {
            uimanager.instance.PlayCoinParticle();
            DataManager.instance.AddCoin(-cost);
            DataManager.instance.ScoreMul = 5;
            uimanager.instance.UpdateHeader();
            purchasedBtn.SetActive(true);
          // FirebaseEvents.instance.LogFirebaseEvent("+5Multiplier", "Purchased", "Sucess");
        }
        else
        {
            uimanager.instance.ShowNotEnoughCoin();
        }
    }
    // -------------------
}
