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
        int amount = -damage;
        SetHealth(amount);
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
        StartCoroutine(DecresingAraEfx(araFillScale));
    }

    IEnumerator DecresingAraEfx(Vector3 newScale)
    {
        yield return new WaitForSeconds(0.25f);
        Vector3 araEfxScale = araEfx.rectTransform.localScale;

        while (araEfx.transform.localScale.x > newScale.x)
        {
            araEfxScale.x -= Time.deltaTime * 0.4f;
            araEfx.rectTransform.localScale = araEfxScale;

            yield return null;
        }
    }
}
