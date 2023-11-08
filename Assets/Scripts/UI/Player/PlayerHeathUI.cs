using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeathUI : MonoBehaviour
{
    [SerializeField] private Image araFill;
    [SerializeField] private Image araEfx;


    [SerializeField] private int maxAra;
    [SerializeField] private int currentAra;

    private void Start() {
        currentAra = maxAra;

        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        playerStats.OnDamageTaken += PlayerStats_OnDamageTaken;
    }

    private void PlayerStats_OnDamageTaken(int amount){
        SetHealth(amount);
    }
    
    public void SetHealth(int amount)
    {
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
