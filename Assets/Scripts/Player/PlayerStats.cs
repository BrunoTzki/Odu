using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Image araFill;
    public Image araEfx;


    public int maxAra;
    public int currentAra;

    public AraBar araBar;

    private bool isInvencible = false;

    // Start is called before the first frame update
    void Start()
    {
        currentAra = maxAra;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentAra -= damage;
        int amount = currentAra - damage;
        SetHealth(amount);
        araBar.SetAra(currentAra);
    }

    public void SetHealth(int amount)
    {
        if (amount< 0)
        {
            if (isInvencible)
                return;
        }

        currentAra = Mathf.Clamp(currentAra + amount, 0, maxAra);
        
        Vector3 araFillScale = araFill.rectTransform.localScale;
        araFillScale.x = (float)currentAra / (float)maxAra;
        araFill.rectTransform.localScale = araFillScale;
        
    }
}
