using System;
using UnityEngine;
using TMPro;
public class VaultTimer : MonoBehaviour
{
    TextMeshProUGUI txt;
    void Start()
    {
        txt = GetComponentInChildren<TextMeshProUGUI>();
    }

    void FixedUpdate()
    {
        int boxCount = DataManager.instance.GiftBoxCount;
        if(boxCount > 0)
        {
            string dString = DataManager.instance.GetBoxTime;
            if (dString != null)
            {
                DateTime oldDate = Convert.ToDateTime(dString);
                DateTime currentTime = DateTime.Now;
                TimeSpan deff = currentTime.Subtract(oldDate);
                if ((int)deff.TotalHours >= 3)
                {
                    if (txt.gameObject.activeInHierarchy)
                        txt.gameObject.SetActive(false);
                }
                else
                {
                    TimeSpan rem = new TimeSpan(3, 0, 0).Subtract(new TimeSpan(deff.Hours, deff.Minutes, deff.Seconds));
                    txt.text = rem.Hours.ToString("00") + ":" + rem.Minutes.ToString("00") + ":" + rem.Seconds.ToString("00");
                    if (!txt.gameObject.activeInHierarchy)
                        txt.gameObject.SetActive(true);
                }
            }
            else
            {
                if (txt.gameObject.activeInHierarchy)
                    txt.gameObject.SetActive(false);
            }
        }
        else
        {
            if (txt.gameObject.activeInHierarchy)
                txt.gameObject.SetActive(false);
        }
    }
}
