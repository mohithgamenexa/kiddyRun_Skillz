using System.Collections;
using UnityEngine;

public class WordHandler : MonoBehaviour
{
    public GameObject[] aToz;
    char wantedChar = 'h';
    char[] cArray;
    void Start()
    {
        if (TrackManager._instance.canGiveWrd)
        {
            string word = TrackManager._instance.dayWord;
            cArray = word.ToCharArray();
            wantedChar = cArray[TrackManager._instance.dayWordIndx];
            int index = char.ToUpper(wantedChar) - 64;//index == 1
            for (int i = 0; i < aToz.Length; i++)
            {
                aToz[i].SetActive(false);
            }
            aToz[index - 1].SetActive(true);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        
    }
    void FixedUpdate()
    {
        if(!TrackManager._instance.canGiveWrd)
        {
            DestroyImmediate(gameObject);
        }
        else if(wantedChar != cArray[TrackManager._instance.dayWordIndx])
        {
            Start();
        }
    }
}
