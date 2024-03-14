using UnityEngine;
using TMPro;
namespace IndigoBunting.Lang
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UILangText : MonoBehaviour
    {
        [SerializeField]
        private string key;
        
        private TextMeshProUGUI text;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
            UpdateText();
        }

        private void Start()
        {
            Localizer.OnChangedLanguage += Localizer_OnChangedLanguage;
        }

        private void OnDestroy()
        {
            Localizer.OnChangedLanguage -= Localizer_OnChangedLanguage;
        }

        private void Localizer_OnChangedLanguage(object sender, LanguageEventArgs e)
        {
            UpdateText();
        }

        public void UpdateText()
        {
            if (!string.IsNullOrEmpty(key))
            {
                text.text = Localizer.GetText(key);
            }
            else
            {
                Debug.LogWarning("Key in the UILangText component on " + gameObject.name + " is null or empty");
            }
        }
    }
}


