using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienHealthController : MonoBehaviour
{
    public static AlienHealthController Instance;
    public List<AlienType> alienLevels = new List<AlienType>();
    public List<HealthBar> healthBarUIs;
    private Dictionary<string, HealthBar> healthBarUIDict;
    public InputObject alienTankObject;

    public Dictionary<string, float> currentAlienNeeds;
    public Dictionary<string, float> currentAlienHealth;
    [Range(0.1f, 2)]
    public float healthLossRate = 1;
    public float healthAddIncrement = 10;

    public bool playerControlsLocked = true;
    public GameObject gameOverWindow;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;

        AlienType newAlien;
        Dictionary<string, float> rNeeds;

        rNeeds = new Dictionary<string, float>() { { "Raw Meat", 1 }, { "Sodium", 1 }, { "Sulphur", 1 }, { "Iron", 1 } };
        newAlien = new AlienType("level_01", rNeeds);
        alienLevels.Add(newAlien);

        rNeeds = new Dictionary<string, float>() { { "Raw Meat", 1 }, { "Sodium", 2 }, { "Chlorine", 0.5f }, { "Sulphur", 3 } };
        newAlien = new AlienType("level_02", rNeeds);
        alienLevels.Add(newAlien);

        rNeeds = new Dictionary<string, float>() { { "Cooked Meat", 0.75f }, { "Sodium", 1 }, { "Salt", 0.75f }, { "Chlorine", 1 } };
        newAlien = new AlienType("level_03", rNeeds);
        alienLevels.Add(newAlien);

        rNeeds = new Dictionary<string, float>() { { "Raw Meat", 1 }, { "Cooked Meat", 1 }, { "Sodium", 3 }, { "Fire", 0.5f } };
        newAlien = new AlienType("level_04", rNeeds);
        alienLevels.Add(newAlien);

        rNeeds = new Dictionary<string, float>() { { "Salt", 1 }, { "Chlorine", 0.75f }, { "Sulphur", 0.75f }, { "Fire", 1.3f } };
        newAlien = new AlienType("level_05", rNeeds);
        alienLevels.Add(newAlien);
    }

    // Update is called once per frame
    void Update()
    {
        
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

        playerControlsLocked = false;
    }

    public void UpdateAlienHealth()
    {
        foreach (string resource in currentAlienNeeds.Keys)
        {
            currentAlienHealth[resource] -= currentAlienNeeds[resource];
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

    public void GameOver()
    {
        playerControlsLocked = true;
        // TODO: Alien dies animation, fade in window, change music
        gameOverWindow.SetActive(true);
    }
}

public class AlienType
{
    // Monster type - not displayed
    string aType;
    // Dict of resources needed and how fast they decay
    public Dictionary<string, float> resourceNeeds;
    public AlienType(string typeName, Dictionary<string, float> resources)
    {
        aType = typeName;
        resourceNeeds = resources;
    }
}
