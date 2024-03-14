
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class CharacterSelection : MonoBehaviour
{
    int chNo;
   // public AudioClip[] characterClips;
    public TextMeshProUGUI priceTxt,nameTxt;
    public string[] CharacterNames;
    ////
    public Sprite coin, tick;
    public Image Coinimage;
    ////
    public int[] PriceArray;
    public Transform pnl;
    bool moving;
    IEnumerator OnSwipe()
    {
        moving = true;
        Vector3 targetPos = Vector3.right*chNo * -2f;
        Vector3 startPos = pnl.transform.localPosition;
        float progress = 0.0f;
        while(progress < 1)
        {
            progress += Time.deltaTime * 3;
            pnl.localPosition = Vector3.Lerp(startPos, targetPos, progress);
            yield return null;
        }
        pnl.localPosition = targetPos;
        moving = false;
       // SfxManager.instance.PlayCharacterSelectionSound(characterClips[chNo]);
    }

    public void OnSelectCharacter()
    {
        SfxManager.instance.PlayButtonClick();
        if (chNo != DataManager.instance.GetCharacter)
        {
            if(priceTxt.text != "Selected" && priceTxt.text != "Select")
            {
                if (DataManager.instance.haveEnoughCoins(PriceArray[chNo]))
                {
                    DataManager.instance.AddCoin(-PriceArray[chNo]);
                    uimanager.instance.hedrCoinTxt.text = "" + DataManager.instance.Coins;
                    uimanager.instance.hedrCoinobj.Play();

                    PlayerPrefs.SetInt("ch" + chNo, 1);
                    bool achivmentCp = true;
                    for(int i =0; i< System.Enum.GetValues(typeof(CharacterType)).Length; i++)
                    {
                        if(PlayerPrefs.GetInt("ch" +i) == 0)
                        {
                            achivmentCp = false;
                        }
                    }
                    if (achivmentCp)
                    {
                       // Debug.Log("Achievment Completed --> all characters unlocked");
                       //PGGC_Manager._instance.callAchievement("CgkIjr6Osv0VEAIQAg");
                    }
                    #if UNITY_ANDROID
                        //  PGGC_Manager._instance.SaveToCloud();
                    #endif
                }
                else
                {
                    // uimanager.instance.StartCoroutine(uimanager.instance.ShowInfo("Insufficient Coins", 0.0f));
                    uimanager.instance.OpenStorePanel(false);
                    return;
                }
            }
            DataManager.instance.GetCharacter = chNo;
            priceTxt.text = "Selected";
            uimanager.instance.chMesh.characterType = (CharacterType)chNo;
            uimanager.instance.flashMesh.characterType = (CharacterType)chNo;
            uimanager.instance.gameOverMesh.characterType = (CharacterType)chNo;
          //  FirebaseEvents.instance.LogFirebaseEvent("CharacterSelected","Name" , uimanager.instance.chMesh.characterType.ToString());
           // FirebaseEvents.instance.SetUserProperty("CHARACTER", "" + uimanager.instance.chMesh.characterType.ToString());
        }
        UpdatePriceTxt();     
    }
    public void PrevOrNext(int mul)
    {
        if (!moving)
        {
            SfxManager.instance.PlayButtonClick();
            chNo += mul;
            chNo = Mathf.Clamp(chNo, 0, PriceArray.Length-1);
            StartCoroutine(OnSwipe());
            UpdatePriceTxt();
        }       
    }
    void UpdatePriceTxt()
    {
        if (DataManager.instance.GetCharacter == chNo)
        {
            priceTxt.text = "Selected";

            Coinimage.gameObject.SetActive(true);
            Coinimage.sprite = tick;
        }else if(chNo == 0 || PlayerPrefs.GetInt("ch"+chNo) == 1)
        {
            priceTxt.text = "Select";
            Coinimage.gameObject.SetActive(false);
        }
        else
        {
            priceTxt.text = ""+PriceArray[chNo];
            Coinimage.gameObject.SetActive(true);
            Coinimage.sprite = coin;
        }
        nameTxt.text = CharacterNames[chNo];
    }
    public void OnAppear()
    {
        chNo = DataManager.instance.GetCharacter;
        Vector3 targetPos = Vector3.right * chNo * -2f;
        pnl.localPosition = targetPos;
        StartCoroutine(DelayPrice());
    }
    IEnumerator DelayPrice()
    {
        UpdatePriceTxt();
        priceTxt.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        priceTxt.gameObject.SetActive(true);
    }



    void Update()
    {
        CheckInput();
    }
    protected Vector2 m_StartingTouch;
    protected bool m_IsSwiping = false;
    void CheckInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PrevOrNext(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PrevOrNext(1);
        }
        #else
        // Use touch input on mobile
        if (Input.touchCount == 1)
        {
            if (m_IsSwiping)
            {
                Vector2 diff = Input.GetTouch(0).position - m_StartingTouch;
                diff = new Vector2(diff.x / Screen.width, diff.y / Screen.width);
                if (diff.magnitude > 0.015f)
                {
                    if (diff.x < 0)
                    {
                        PrevOrNext(1);
                    }
                    else
                    {
                        PrevOrNext(-1);
                    }

                    m_IsSwiping = false;
                }
            }
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                m_StartingTouch = Input.GetTouch(0).position;
                m_IsSwiping = true;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                m_IsSwiping = false;
            }
        }

#endif
    }
}
public enum CharacterType
{
    Boy,
    Cowboy,
    FootballPlayer,
    Girl,
    Jocker,  
    King,
    Boxer,
    Bull,
    Ninja,
    Pirate,
    Princess,
    Robo,
    TomnRider,
    Witch,
    Zombie,
}
