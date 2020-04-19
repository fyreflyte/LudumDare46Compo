using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarAttributeController : MonoBehaviour
{
    public static HealthBarAttributeController Instance;
    public List<string> resourceNames;
    public List<Sprite> resourceSprites;
    public List<Color> healthbarColors;
    //public List<Color> healthTextColors;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public Color GetResourceColor(string rName)
    {
        if (!resourceNames.Contains(rName))
            return Color.white;
        int index = resourceNames.IndexOf(rName);

        return healthbarColors[index];
    }

    public Sprite GetResourceSprite(string rName)
    {
        if (!resourceNames.Contains(rName))
            return null;
        int index = resourceNames.IndexOf(rName);
        Debug.Log("Returning sprite at index " + index);
        return resourceSprites[index];
    }
}
