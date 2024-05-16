using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeButton : MonoBehaviour
{
    public GameObject on, off;
    public string buttonType;

    private void OnEnable()
    {
        if (buttonType == "Sound")
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                on.SetActive(false);
                off.SetActive(true);
            }
            else
            {
                on.SetActive(true);
                off.SetActive(false);
            }
        }
        else
        {
            if (Vibration.VibrationActive == 0)
            {
                on.SetActive(false);
                off.SetActive(true);
            }
            else
            {
                on.SetActive(true);
                off.SetActive(false);
            }
        }
    }

    public void Change()
    {
        if (on.activeSelf)
        {
            on.SetActive(false);
            off.SetActive(true);
            if (buttonType == "Sound")
            {
                AudioListener.volume = 0;
                SkillzCrossPlatform.setSkillzMusicVolume(0);
                PlayerPrefs.SetInt("Sound", 0);
            }
            else if (buttonType == "Vibration")
            {
                Vibration.VibrationActive = 0;
            }

        }
        else
        {
            on.SetActive(true);
            off.SetActive(false);
            if (buttonType == "Sound")
            {
                AudioListener.volume = 1;
                SkillzCrossPlatform.setSkillzMusicVolume(1);
                PlayerPrefs.SetInt("Sound", 1);

            }
            else if (buttonType == "Vibration")
            {
                Vibration.VibrationActive = 1;
            }
        }
    }
}
