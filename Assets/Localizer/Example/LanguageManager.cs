using System.Collections;
using System.Collections.Generic;
using IndigoBunting.Lang;
using UnityEngine;
using TMPro;

//[ExecuteInEditMode]
public class LanguageManager : MonoBehaviour
{

    public List<TextClass> arrText;
    public List<FontAssets> _font;

    /*public void Start()
    {
        TextMeshProUGUI[] _textassets = GameObject.FindObjectsOfType<TextMeshProUGUI>();
        for (int i = 0; i < _textassets.Length; i++)
        {
            TextClass T = new TextClass();
            T._text = _textassets[i];
            T.key = "";
            arrText.Add(T);
        }
    }*/


    public void OnLanguageChanged(int num){
        Localizer.SetLang((Language)num);
        for (int i = 0; i < arrText.Count; i++)
        {
            arrText[i]._text.font = _font[num]._fontasset;
            arrText[i]._text.fontSharedMaterial = _font[num]._fontmaterial;
            arrText[i]._text.text = Localizer.GetText(arrText[i].key);
        }
    }

}
[System.Serializable]
public class TextClass
{
    public TextMeshProUGUI _text;
    public string key;
}

[System.Serializable]
public class FontAssets
{
    public TMPro.TMP_FontAsset _fontasset;
    public Material _fontmaterial;
}