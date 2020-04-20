using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlienHealthController : MonoBehaviour
{
    public static AlienHealthController Instance;
    public List<AlienType> alienLevels = new List<AlienType>();
    public List<HealthBar> healthBarUIs;
    public List<GameObject> aliensInTankImages;
    private Dictionary<string, HealthBar> healthBarUIDict;
    public InputObject alienTankObject;
   
    public Dictionary<string, float> currentAlienNeeds;
    public Dictionary<string, float> currentAlienHealth;
    float healthLossRate = 1f;
    float healthAddIncrement = 25;

    public bool playerControlsLocked = true;
    public GameObject gameOverWindow;
    public GameObject winGameWindow;
    public TextMeshProUGUI aliensSavedText;
    public int countdownTimer;
    public TextMeshProUGUI countdownText;

    public GameObject storyPanel;
    public GameObject goodWorkText;
    public TextMeshProUGUI storyDialogText;
    public int currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;

        AlienType newAlien;
        Dictionary<string, float> rNeeds;

        // Need 1 of each
        rNeeds = new Dictionary<string, float>() { { "Raw Meat", 1.8f }, { "Sodium", 1.8f }, { "Sulphur", 1.8f }, { "Iron", 1.8f } };
        newAlien = new AlienType(0, rNeeds, 60);
        alienLevels.Add(newAlien);

        // Need 2 meat, 3 Na, 1 Cl, 3 Su
        rNeeds = new Dictionary<string, float>() { { "Raw Meat", 1.2f }, { "Sodium", 1.3f }, { "Chlorine", 1f }, { "Sulphur", 1.3f } };
        newAlien = new AlienType(1, rNeeds, 90);
        alienLevels.Add(newAlien);

        // Need 2 meats, 3 Na, 2 NaCl, 3 Cl
        rNeeds = new Dictionary<string, float>() { { "Cooked Meat", 0.9f }, { "Sodium", 1 }, { "Salt", 0.9f }, { "Chlorine", 1 } };
        newAlien = new AlienType(2, rNeeds, 120);
        alienLevels.Add(newAlien);

        // Need 3 raw meat, 3 cooked meat, 6 Na, 2 fire
        rNeeds = new Dictionary<string, float>() { { "Raw Meat", 1 }, { "Cooked Meat", 1 }, { "Sodium", 1.5f }, { "Fire", 1.1f } };
        newAlien = new AlienType(3, rNeeds, 120);
        alienLevels.Add(newAlien);

        // Need 6 NaCl, 4 Cl, 4 Su, 10 Fire
        rNeeds = new Dictionary<string, float>() { { "Salt", 0.9f }, { "Chlorine", 0.75f }, { "Sulphur", 0.75f }, { "Fire", 1.3f } };
        newAlien = new AlienType(4, rNeeds, 180);
        alienLevels.Add(newAlien);

        currentLevel = 0;
        CompleteLevel();
    }

    public void DEBUG_SetShortLevelTimes()
    {
        foreach (AlienType alien in alienLevels)
        {
            alien.saveTime = 3;
        }
        BeginLevel(1);
    }

    /// <summary>
    /// Set up alien health values and begin the new level
    /// </summary>
    /// <param name="levelNum">Integer number of the level. Currently 1-5</param>
    public void BeginLevel(int levelNum)
    {
        Dictionary<string, float> resourceNeeds = new Dictionary<string, float>(alienLevels[levelNum - 1].resourceNeeds);
        // Set up UI
        healthBarUIDict = new Dictionary<string, HealthBar>();
        int i = 0;
        foreach (string resource in resourceNeeds.Keys)
        {
            healthBarUIs[i].SetName(resource);
            healthBarUIs[i].SetHealthBar(1);
            healthBarUIDict[resource] = healthBarUIs[i];
            i++;
        }
        EnableAlienImage(alienLevels[levelNum - 1].aType);

        // Data setup
        currentAlienNeeds = resourceNeeds;
        currentAlienHealth = new Dictionary<string, float>();
        alienTankObject.validReceivableObjects = new List<string>();
        foreach (string resource in currentAlienNeeds.Keys)
        {
            alienTankObject.validReceivableObjects.Add(resource);
            currentAlienHealth[resource] = 100;
        }
        CancelInvoke("UpdateAlienHealth");
        Invoke("UpdateAlienHealth", 3);
        StartCountdown(alienLevels[levelNum - 1].saveTime);
        playerControlsLocked = false;
    }

    /// <summary>
    /// Enables one alien image while disabling the rest
    /// </summary>
    /// <param name="num">The index of the enabled image</param>
    public void EnableAlienImage(int num)
    {
        for (int i = 0; i < aliensInTankImages.Count; i++)
        {
            aliensInTankImages[i].SetActive(i == num);
        }
    }

    public void UpdateAlienHealth()
    {
        Debug.Log("Update " + Time.time);
        foreach (string resource in currentAlienNeeds.Keys)
        {
            currentAlienHealth[resource] -= currentAlienNeeds[resource] * healthLossRate;
            healthBarUIDict[resource].SetHealthBar(currentAlienHealth[resource]/100f);

            if (currentAlienHealth[resource] < 0)
                GameOver();
        }
        CancelInvoke("UpdateAlienHealth");
        Invoke("UpdateAlienHealth", 1);
    }

    /// <summary>
    /// Called when player inserts a needed recource into a
    /// valid InputObject
    /// </summary>
    /// <param name="resourceName">Name of the resource added</param>
    public void AddHealthObject(string resourceName)
    {
        if (!currentAlienNeeds.ContainsKey(resourceName))
            return;

        currentAlienHealth[resourceName] += healthAddIncrement;
        if (currentAlienHealth[resourceName] > 100)
            currentAlienHealth[resourceName] = 100;
        UpdateAlienHealth();
    }

    /// <summary>
    /// This is not an efficient way to set up a timer - if you're reading this
    /// you should simply store the current time and then check against that
    /// as needed. However, in the interest of my sanity, I'm taking the easy
    /// way for this one.
    /// </summary>
    /// <param name="timeInSeconds">Time in seconds... duh</param>
    public void StartCountdown(int timeInSeconds)
    {
        countdownText.text = FormatCountdownTimer(timeInSeconds);
        countdownTimer = timeInSeconds;
        CancelInvoke("CountTimeDown");
        Invoke("CountTimeDown", 1);
    }

    public void CountTimeDown()
    {
        countdownTimer--;
        countdownText.text = FormatCountdownTimer(countdownTimer);
        if (countdownTimer < 1)
        {
            // Go next level
            CompleteLevel();
            return;
        }
        else
            Invoke("CountTimeDown", 1);
    }

    private string FormatCountdownTimer(int time)
    {
        int minutes = time / 60;
        int seconds = time % 60;

        return "0" + minutes + ":" + (seconds > 9 ? seconds.ToString() : "0" + seconds);
    }


    public void CompleteLevel()
    {
        CancelInvoke("UpdateAlienHealth");
        goodWorkText.SetActive(currentLevel != 0);
        aliensSavedText.text = currentLevel.ToString();
        storyPanel.SetActive(true);
        storyDialogText.text = QueryStoryDialog(currentLevel);
        playerControlsLocked = true;
        Invoke("BeginNextLevel", 5);
    }

    public void BeginNextLevel()
    {
        storyPanel.SetActive(false);
        currentLevel++;
        if (currentLevel > 5)
        {
            WinGame();
            return;
        }

        BeginLevel(currentLevel);
    }

    public string QueryStoryDialog(int level)
    {
        string nl = System.Environment.NewLine;
        // this alien will surely perish if its nourishment requirements are not met
        switch (level)
        {
            case 0:
                return "the human race is doomed..." + nl + "or is it..." + nl + "working quickly, you must keep your alien subjects alive long enough to manufacture antibodies";
            case 1:
                return "you kept the alien nourshed long enough to produce some useful antibodies" + nl + nl +"on to the next...";
            case 2:
                return "the alien survived, and you were able to use it to manufacture another set of antibodies" + nl + nl + "only three more to go";
            case 3:
                return "you are becoming more adept at your work, but the remaining aliens are fading fast" + nl + nl + "the pressure is mounting...";
            case 4:
                return "you're getting close. just one more set of antibodies and the human race is saved" + nl + nl + "but your next subject requires... fire";
            case 5:
                return "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

            default:
                return "";
        }

    }

    public void GameOver()
    {
        CancelInvoke("UpdateAlienHealth");
        playerControlsLocked = true;
        // TODO: Alien dies animation, fade in window, change music
        gameOverWindow.SetActive(true);
    }

    public void WinGame()
    {
        CancelInvoke("UpdateAlienHealth");
        playerControlsLocked = true;
        winGameWindow.SetActive(true);
    }
}

public class AlienType
{
    // Monster type
    public int aType;
    // Dict of resources needed and how fast they decay
    public Dictionary<string, float> resourceNeeds;
    public int saveTime;

    public AlienType(int typeNum, Dictionary<string, float> resources, int timeInSeconds)
    {
        aType = typeNum;
        resourceNeeds = resources;
        saveTime = timeInSeconds;
    }
}
