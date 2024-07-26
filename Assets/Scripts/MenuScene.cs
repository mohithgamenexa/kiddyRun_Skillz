using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScene : MonoBehaviour
{
   
    public void PlayBtnClicked()
    {
        SkillzCrossPlatform.LaunchSkillz();
    }

#if UNITY_ANDROID
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
#endif

}
