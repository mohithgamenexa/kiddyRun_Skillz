using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    public CharacterType character;    
    public GameObject[] parts;   
    public Material[] matArray;   
    public SkinnedMeshRenderer mainBody;
    public Material invisible;

    public CharacterType characterType
    {
        get
        {
            return character;
        }
        set
        {
            character = value;
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].SetActive(false);
            }
            int CH_no = (int)character;
            parts[CH_no].SetActive(true);
            mainBody.material = matArray[CH_no];
            invisible.mainTexture = mainBody.material.mainTexture;
        }
    }

    public void _invisble(bool t)
    {
        int CH_no = (int)characterType;
        Transform p = parts[CH_no].transform;
       // parts[5].SetActive(t);
        if (t)
        {
            mainBody.material = invisible;
            for (int i = 0; i < p.childCount; i++)
            {
                p.GetChild(i).GetComponent<SkinnedMeshRenderer>().material = invisible;
            }
        }
        else
        {
            mainBody.material = matArray[CH_no];
            for(int i = 0; i<p.childCount; i++)
            {
                p.GetChild(i).GetComponent<SkinnedMeshRenderer>().material = matArray[CH_no];
            }
        }
    }
    public bool shopItem;
    void Start()
    {
        
        if (shopItem)
        {
            characterType = character;
        }
        else
        {
            int chNo = DataManager.instance.GetCharacter;
            characterType = (CharacterType)chNo;
        }
    }
}
