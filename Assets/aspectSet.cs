using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aspectSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        float aspect = Camera.main.aspect;
        if (aspect < 0.6f)
        {
            GetComponent<RectTransform>().localScale = new Vector2(0.9f, 0.9f);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
        }
        else
        {
            GetComponent<RectTransform>().localScale = new Vector2(1.09f, 1.09f);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 250.0f);
        }
    }


}
