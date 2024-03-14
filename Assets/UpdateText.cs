
using UnityEngine;
using TMPro;

public class UpdateText : MonoBehaviour
{
    private TextMeshProUGUI _txt;
    public bool iscoins;
    private void Start()
    {
        _txt = GetComponent<TextMeshProUGUI>();

    }

    private void FixedUpdate()
    {
        if (iscoins)
        {
            _txt.text = "" + DataManager.instance.Coins;
        }
        else
        {
            _txt.text = "" + DataManager.instance.Keys;
        }
    }
}
