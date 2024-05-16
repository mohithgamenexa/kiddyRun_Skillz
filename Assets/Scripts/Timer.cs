using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Timer : MonoBehaviour
{
    public float totalTime;

    public bool startTimer;

    [SerializeField]
    TextMeshProUGUI timerText;

    [SerializeField]
    Image timerImg;

    [SerializeField]
    List<Sprite> timerSpr;

    public Image timeLeftObj;

    public Sprite timeLeftSpr1, timeLeftSpr2, timeLeftSpr3;
    public GameObject counterObj;
    public static Timer instance;
    public GameObject timeUpObj;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        /* if (SkillzManager.skillzGameTimer > 0)
             totalTime = SkillzManager.skillzGameTimer;
         else
             totalTime = 150;*/

        SetTime();
    }
    public void SetTime()
    {
        float minutes = Mathf.FloorToInt(totalTime / 60);
        float seconds = Mathf.FloorToInt(totalTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator ShowTimeLeft(Sprite spr)
    {
        timeLeftObj.gameObject.SetActive(true);
        timerImg.sprite = spr;
        //UIFeedback.Instance.PlayHapticMedium();

        yield return new WaitForSeconds(1.5f);
        timeLeftObj.gameObject.SetActive(false);

    }

    bool a, b, c;
    void Update()
    {

        if (startTimer)
        {
            totalTime -= Time.deltaTime;


            SetTime();


            if (totalTime <= 120 && !a)
            {
                a = true;
                //StartCoroutine(EnableTimeRemainingObj(0f, 0));
                StartCoroutine(ShowTimeLeft(timeLeftSpr1));
            }
            else if (totalTime <= 60 && !b)
            {
                b = true;
                //StartCoroutine(EnableTimeRemainingObj(0f, 1));
                StartCoroutine(ShowTimeLeft(timeLeftSpr2));


            }
            else if (totalTime <= 30 && !c)
            {
                c = true;
                // StartCoroutine(EnableTimeRemainingObj(0f, 2));
                StartCoroutine(ShowTimeLeft(timeLeftSpr3));


            }
            else if (totalTime <= 5)
            {
                Debug.Log("time is less than 5");
                c = true;
                // StartCoroutine(EnableTimeRemainingObj(0f, 2));
                //timerText.gameObject.GetComponent<Animator>().enabled = true;
                //counterObj.SetActive(true);
            }
            if (totalTime <= 0)
            {
                startTimer = false;
                //counterObj.SetActive(false);

                timerText.text = "00:00";
                //GameController.inst.shuffleCardsBtn.GetComponentInChildren<TextMeshProUGUI>().color = GameController.inst.noOfShufflesTextColor;
                StartCoroutine(ShowTimeUpObj(0));
                //timerText.gameObject.GetComponent<Animator>().enabled = false;
                //GamePlayUI.Instance.TryRescueGame(GameOverReason.TIME_OVER);
            }
        }

    }


    IEnumerator ShowTimeUpObj(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        timeUpObj.SetActive(true);
        yield return new WaitForSeconds(1f);
        timeUpObj.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        // MainGameController.instance.GameOver();
    }
    public void ResetTimer()
    {
        startTimer = false;
        totalTime = 180;
        timerText.text = "03:00";
    }
}



