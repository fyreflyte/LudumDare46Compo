using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public TextMeshProUGUI resourceNameText;
    public Image healthBar;
    
    public void SetName(string name)
    {
        resourceNameText.text = name.ToLower();
        int index = HealthBarAttributeController.Instance.resourceNames.IndexOf(name);
        healthBar.color = HealthBarAttributeController.Instance.healthbarColors[index];
    }
    public void SetHealthBar(float percent)
    {
        healthBar.fillAmount = percent;
    }
}
