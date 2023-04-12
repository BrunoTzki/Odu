using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public int maxAra;
    public int currentAra;

    public AraBar araBar;

    // Start is called before the first frame update
    void Start()
    {
        currentAra = maxAra;

        araBar.SetMaxAra(maxAra);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentAra -= damage;

        araBar.SetAra(currentAra);
    }
}
