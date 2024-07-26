
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class GameData
{
    public string gameName;
    public string gameImageLink;
    public string gameStoreLink;
}

[System.Serializable]
public class RootObject
{
    public string GamenexaSkillzGames;
    public int totalGames;
    public List<GameData> games;
}

public class MoreGamesManager : MonoBehaviour
{
    public string serverUrl = "https://cdn.appsupstudios.com/yatzy/yatzyskillz.json";
    public List<GameData> GamesList = new List<GameData>();
    public int NumberOfGames;
    public List<MoreGamesBtn> moreGamesBtns;
    public Transform content;
    public GameObject buttonPrefab;
    public RectTransform contentRectTransform;
    private void Start()
    {
        StartCoroutine(LoadJSONFromServer());
    }

    IEnumerator LoadJSONFromServer()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(serverUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error loading JSON from server: " + www.error);
            }
            else
            {
                string jsonText = www.downloadHandler.text;
                var rootObject = new { GamenexaSkillzGames = new { AdsActive = false, games = new List<GameData>() } };//NumberOfGames = 0, 
                rootObject = JsonConvert.DeserializeAnonymousType(jsonText, rootObject);


                if (rootObject.GamenexaSkillzGames != null)
                {
                    // NumberOfGames = rootObject.GamenexaSkillzGames.NumberOfGames; // Retrieve NumberOfGames value
                    UnityAdsManager.instance.adsActive = rootObject.GamenexaSkillzGames.AdsActive;
                    GamesList = rootObject.GamenexaSkillzGames.games;

                   // UnityAdsManager.instance.adsActive = false;
                    Debug.Log("Show Ads::" + UnityAdsManager.instance.adsActive);
                    //foreach (var game in GamesList)
                    //{
                    //    Debug.Log("Game Name: " + game.gameName);
                    //    Debug.Log("Image Link: " + game.gameImageLink);
                    //    Debug.Log("Store Link: " + game.gameStoreLink);
                    //}
                }
                else
                {
                    Debug.LogError("Invalid JSON structure. Unable to parse.");
                }
            }
        }
        InitializeScrollView(GamesList);

        //AssignDataToButtons();
    }

    // Function to initialize the ScrollView with buttons and data
    public void InitializeScrollView(List<GameData> games)
    {
        GamesList = games;

        // Clear existing buttons
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        float totalWidth = 0f;

        // Create buttons dynamically
        foreach (GameData game in GamesList)
        {
            GameObject buttonGO = Instantiate(buttonPrefab, content);
            MoreGamesBtn mgBtn = buttonGO.GetComponent<MoreGamesBtn>();
            mgBtn.gameName = game.gameName;
            mgBtn.gameImgLink = game.gameImageLink;
            mgBtn.gameStoreLink = game.gameStoreLink;
            mgBtn.AssignValues();

            // Set button label
            Text buttonText = buttonGO.GetComponentInChildren<Text>();
            buttonText.text = game.gameName;

            // Set button position with custom spacing
            RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();
            buttonRect.anchoredPosition = new Vector2(totalWidth, 0f);

            // Update total width
            totalWidth += buttonGO.GetComponent<RectTransform>().rect.width;
        }

        // Set content width to accommodate all buttons
        contentRectTransform.sizeDelta = new Vector2(totalWidth/2, contentRectTransform.sizeDelta.y);
    }

    // Function to handle button click event
    private void OnButtonClick(GameData game)
    {
        Debug.Log("Button clicked: " + game.gameName);
        // You can perform actions related to the clicked game here
    }


    void AssignDataToButtons()
    {
        for (int i = 0; i < moreGamesBtns.Count; i++)
        {
            moreGamesBtns[i].gameName = GamesList[i].gameName;
            moreGamesBtns[i].gameImgLink = GamesList[i].gameImageLink;
            moreGamesBtns[i].gameStoreLink = GamesList[i].gameStoreLink;

            moreGamesBtns[i].AssignValues();
        }
    }
}
