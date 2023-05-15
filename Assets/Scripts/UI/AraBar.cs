using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AraBar : MonoBehaviour
{
    public Slider slider;
    
    public void SetMaxAra(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetAra(int health)
    {
        slider.value = health;
    }

}
