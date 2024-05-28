using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay : MonoBehaviour
{
    int count = 0;
    [SerializeField]
    List<GameObject> imgs;
    [SerializeField]
    List<GameObject> dots;

    [SerializeField]
    GameObject backBtn;
    [SerializeField]
    GameObject nextBtn;
    /*[SerializeField]
    GameObject rulesImg;
    [SerializeField]
    GameObject bonusImg;*/
    [SerializeField]
    GameObject closeBtn;

    [SerializeField]
    GameObject howtoplayPanel;
    private void OnEnable()
    {
        gameObject.transform.SetAsLastSibling();
        count = 0;
        imgs[count].SetActive(true);
        dots[count].SetActive(true);
        if (count == 0)
        {
            nextBtn.SetActive(true);
            backBtn.SetActive(false);
            /*rulesImg.SetActive(true);
            bonusImg.SetActive(false);*/
            closeBtn.SetActive(false);

        }
        else if (count == 1)
        {
            nextBtn.SetActive(true);
            backBtn.SetActive(true);
            //rulesImg.SetActive(true);
            closeBtn.SetActive(false);
           // bonusImg.SetActive(false);
        }
        else if (count == 2)
        {
            nextBtn.SetActive(true);
            backBtn.SetActive(true);
            //rulesImg.SetActive(false);
            closeBtn.SetActive(false);
            //bonusImg.SetActive(true);
        }
        else if (count == 3)
        {
            nextBtn.SetActive(false);
            backBtn.SetActive(true);
            //rulesImg.SetActive(false);
            closeBtn.SetActive(true);
            //bonusImg.SetActive(true);
        }
        PlayerPrefs.SetInt("FirstTime", 1);
    }


    public void CloseBtnClicked()
    {
        howtoplayPanel.SetActive(false);
        DeactivatePages();

    }
    public void NextBtn()
    {
        if (count < 3)
        {
            count++;
            DeactivatePages();
            imgs[count].SetActive(true);
            //iTween.ScaleFrom(imgs[count].gameObject, iTween.Hash("x", 0, "y", 0, "time", 0.35f, "easetype", iTween.EaseType.easeOutBack));
            dots[count].SetActive(true);
            if (count == 0)
            {
                nextBtn.SetActive(true);
                backBtn.SetActive(false);
                //rulesImg.SetActive(true);
                closeBtn.SetActive(false);
                //bonusImg.SetActive(false);

            }
            else if (count == 1)
            {
                nextBtn.SetActive(true);
                backBtn.SetActive(true);
                //rulesImg.SetActive(true);
                closeBtn.SetActive(false);
               // bonusImg.SetActive(false);
            }
            else if (count == 2)
            {
                nextBtn.SetActive(true);
                backBtn.SetActive(true);
                //rulesImg.SetActive(false);
                closeBtn.SetActive(false);
                //bonusImg.SetActive(true);
            }
            else if (count == 3)
            {
                nextBtn.SetActive(false);
                backBtn.SetActive(true);
                //rulesImg.SetActive(false);
                closeBtn.SetActive(true);
                //bonusImg.SetActive(true);
            }
        }
        else
            return;

    }
    void DeactivatePages()
    {
        foreach (var item in imgs)
        {
            item.SetActive(false);
        }
        foreach (var item in dots)
        {
            item.SetActive(false);
        }
    }

    public void BackBtn()
    {
        if (count > 0)
        {
            count--;
            DeactivatePages();
            imgs[count].SetActive(true);
            //iTween.ScaleFrom(imgs[count].gameObject, iTween.Hash("x", 0, "y", 0, "time", 0.35f, "easetype", iTween.EaseType.easeOutBack));

            dots[count].SetActive(true);
            if (count == 0)
            {
                nextBtn.SetActive(true);
                backBtn.SetActive(false);
                //rulesImg.SetActive(true);
                closeBtn.SetActive(false);
                //bonusImg.SetActive(false);

            }
            else if (count == 1)
            {
                nextBtn.SetActive(true);
                backBtn.SetActive(true);
                //rulesImg.SetActive(true);
                closeBtn.SetActive(false);
                //bonusImg.SetActive(false);
            }
            else if (count == 2)
            {
                nextBtn.SetActive(true);
                backBtn.SetActive(true);
                //rulesImg.SetActive(false);
                closeBtn.SetActive(false);
                //bonusImg.SetActive(true);
            }
            else if (count == 3)
            {
                nextBtn.SetActive(false);
                backBtn.SetActive(true);
                //rulesImg.SetActive(false);
                closeBtn.SetActive(true);
                //bonusImg.SetActive(true);
            }
        }
        else
            return;


    }
}
