using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GIftBoxOPen : MonoBehaviour
{
    public GameObject[] rewardIcons;
    public GameObject rewardMenu;
    public  static int rewardCount;
    public TextMeshProUGUI vaultitemlabel,_vaultcount;
    public GiftBoxCntrl _Cntrl;

    private void OnEnable()
    {
        rewardMenu.transform.GetChild(0).gameObject.SetActive(true);
        for (int i = 0; i < rewardIcons.Length; i++)
        {
            rewardIcons[i].SetActive(false);
        }
        rewardMenu.SetActive(true);
        vaultitemlabel.text = "Tap to Open";
        if (rewardCount <= 0)
        {
            rewardCount = 1;
        }
        _vaultcount.text = rewardCount + "";
    }

    public void TapToOpenBox(Animation VIpop)
    {
        for (int i = 0; i < rewardIcons.Length; i++)
        {
            rewardIcons[i].SetActive(false);
        }
        rewardMenu.transform.GetChild(0).gameObject.SetActive(false);
        VIpop.Play();
        if (rewardCount > 0)
        {
            int shuffleNo = UnityEngine.Random.Range(0, 5);
            rewardIcons[shuffleNo].SetActive(true);

            if (shuffleNo == 0)
            {
                int coins = UnityEngine.Random.Range(10, 26) * 10;
                vaultitemlabel.text = "+" + coins + " Coins";
                DataManager.instance.AddCoin(coins);
                uimanager.instance.hedrCoinTxt.text = "" + DataManager.instance.Coins;
                uimanager.instance.hedrCoinobj.Play();

            }
            else if (shuffleNo == 1)
            {
                vaultitemlabel.text = "Got 1 Cycle";
                DataManager.instance.Boards += 1;
            }
            else if (shuffleNo == 2)
            {
                vaultitemlabel.text = "Got 1 Key";
                DataManager.instance.Keys += 1;
                uimanager.instance.hedrKeyTxt.text = "" + DataManager.instance.Keys;
                uimanager.instance.hedrkeyobj.Play();
            }
            else if (shuffleNo == 3)
            {
                vaultitemlabel.text = "Got 1 HeadBoost";
                DataManager.instance.GetHeadBoost += 1;
            }
            else if (shuffleNo == 4)
            {
                vaultitemlabel.text = "Got +5Multiplier";
                //DataManager.instance.ScoreMul = 5;
            }
            rewardCount -= 1;
        }
        else
        {
            if (TrackManager._instance._gameState != GameState.GAMEOVER)
            {
                _Cntrl.TapToOpenBox();
            }
            else
            {
                uimanager.instance.gameoverMenu.SetActive(true);
            }
            rewardMenu.SetActive(false);
            gameObject.SetActive(false);
        }
        _vaultcount.text = rewardCount + "";

    }
}
