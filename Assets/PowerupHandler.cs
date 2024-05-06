using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerupHandler : MonoBehaviour
{
    public static PowerupHandler instance;
    public float totalCoins = 100f; // Total number of coins you want to collect
    private int collectedCoins = 0;

    public List<GameObject> allPowerups;
    public List<GameObject> selectedObjects = new List<GameObject>();

    public List<Button> powerupButtons;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        SetButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetButtons()
    {
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, allPowerups.Count);
            GameObject selectedObject = allPowerups[randomIndex];

            // Check if the object is already selected to avoid duplicates
            if (!selectedObjects.Contains(selectedObject))
            {
                selectedObjects.Add(selectedObject);
                Debug.Log("Selected Object: " + selectedObject.name);
            }
            else
            {
                // If the object is already selected, decrement the loop counter to select another one
                i--;
            }

        }

        for (int i = 0; i < powerupButtons.Count; i++)
        {
            powerupButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = selectedObjects[i].name;
        }
    }

    float fillPercent;
    public Image fillBar;
    bool activatePowerup;

    public void FillSectionBonusBar()
    {
        fillPercent = collectedCoins / 21f;
        //sectionBonusFillImg.fillAmount = fillPercent;
        StartCoroutine(FillImageCoroutine());
        //if (fillPercent < 1.1f)

    }
    IEnumerator FillImageCoroutine()
    {
        collectedCoins++;
        iTween.ValueTo(fillBar.gameObject, iTween.Hash(
            "from", fillBar.fillAmount,
            "to", fillPercent,
            "time", 0.5f,
            "onupdate", "UpdateFillAmount"
        ));
        fillPercent = collectedCoins / totalCoins;
        yield return new WaitForSeconds(0.15f);
        /*if (fillPercent * 2 > 2.2f)
        {
            fillPercent = 1.1f;
        }*/

        fillBar.fillAmount = fillPercent;
        if (fillBar.fillAmount >= 1)
        {
            fillBar.fillAmount = 0;
            collectedCoins = 0;
            Debug.Log("----");
            activatePowerup = true;
            SelectDisabledButton();

        }
    }
    void UpdateFillAmount(float newFillAmount)
    {
        /* if (newFillAmount * 2 > 2.2f)
         {
             newFillAmount = 1.1f;
         }*/
        fillBar.fillAmount = fillPercent;
        
    }

    // Function to add coins to the progress bar
    /*  public void AddCoins(int amount)
      {
          collectedCoins += amount;
          UpdateProgressBar();
      }*/
    void SelectDisabledButton()
    {
        List<Button> disabledButtons = new List<Button>();

        // Iterate through the buttons to find disabled ones
        foreach (Button button in powerupButtons)
        {
            if (!button.interactable)
            {
                disabledButtons.Add(button);
            }
        }

        // If there are disabled buttons, select one randomly
        if (disabledButtons.Count > 0 && activatePowerup)
        {
            activatePowerup = false;
            int randomIndex = SkillzCrossPlatform.Random.Range(0, disabledButtons.Count);
            Button selectedButton = disabledButtons[randomIndex];

            // Do something with the selected disabled button
            selectedButton.interactable = true;
            Debug.Log("Selected disabled button: " + selectedButton.name);
        }
        else
        {
            // If all buttons are enabled
            Debug.Log("All buttons are enabled");
        }
        
    }
    public void UsePowerup(int count)
    {
        powerupButtons[count].interactable = false;
        int c = (int)selectedObjects[count].GetComponent<powerupCntrl>().powerUpType;

        uimanager.instance.PowerUpCollected(c);
    }
}
