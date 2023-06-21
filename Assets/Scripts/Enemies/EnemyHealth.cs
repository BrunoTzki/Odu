using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Vida do Inimigo")]

    [SerializeField] protected Collider _healthCollider;
    [SerializeField] int _maxHealth;

    [SerializeField] public Image healthFill;
    [SerializeField] public Image healthEfx;

    public int _currentHealth;
    public bool isInvencible = false;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        int amount = -damage;
        SetHealth(amount);
    }

    public void SetHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvencible)
                return;
        }
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);

        Vector3 araFillScale = healthFill.rectTransform.localScale;
        araFillScale.x = (float)_currentHealth / (float)_maxHealth;
        healthFill.rectTransform.localScale = araFillScale;
        StartCoroutine(DecresingAraEfx(araFillScale));
    }

    IEnumerator DecresingAraEfx(Vector3 newScale)
    {
        yield return new WaitForSeconds(0.25f);
        Vector3 araEfxScale = healthEfx.rectTransform.localScale;

        while (healthEfx.transform.localScale.x > newScale.x)
        {
            araEfxScale.x -= Time.deltaTime * 0.4f;
            healthEfx.rectTransform.localScale = araEfxScale;

            yield return null;
        }
    }
}
