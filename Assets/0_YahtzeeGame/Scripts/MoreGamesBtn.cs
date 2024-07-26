using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MoreGamesBtn : MonoBehaviour
{
    public string gameName;
    public string gameImgLink;
    public string gameStoreLink;


    public Text gameNameText;
    public RawImage gameImg;
    public Button button;


    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    public void AssignValues()
    {
        gameNameText.text = gameName;
        LoadImageFromURL(gameImgLink);
    }

    void OnButtonClick()
    {
        //Debug.Log("on button clicked" + gameStoreLink);
        Application.OpenURL(gameStoreLink);
    }


    public void LoadImageFromURL(string imageUrl)
    {
        StartCoroutine(LoadImageCoroutine(imageUrl));
    }

    private IEnumerator LoadImageCoroutine(string imageUrl)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error loading image from URL: " + www.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);

                // Assign the downloaded texture to the RawImage component.
                if (gameImg != null)
                {
                    gameImg.texture = texture;
                }
                else
                {
                    Debug.LogError("RawImage component not assigned.");
                }
            }
        }
    }
}
