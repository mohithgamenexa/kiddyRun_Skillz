
using UnityEngine;
using TMPro;
public class Counter : MonoBehaviour
{
    
    public TextMeshProUGUI txt;
    int CountDown;
    void AnimationStart()
    {
        CountDown = 3;
        OnAnimation();
    }
    
    void OnAnimation()
    {
        if (CountDown < 0)
        {
            
        }
        else if (CountDown == 0)
        {
            txt.text = "GO!";
        }
        else
        {
            txt.text = "" + CountDown;
        }
        CountDown -= 1;
    }
}
