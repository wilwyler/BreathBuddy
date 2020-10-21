using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class HappinessBar : MonoBehaviour
{

    public Slider slider; 

    public Gradient gradient; 

    public Image fill; 
    // Start is called before the first frame update
    public void initBar(int health)
    {
        slider.maxValue = 100; 
        slider.value = health; 
        fill.color = gradient.Evaluate(slider.normalizedValue); 
    }

    public void setHealth(int health)
    {
        slider.value = health; 
        fill.color = gradient.Evaluate(slider.normalizedValue); 
    }
}
