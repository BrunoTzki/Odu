using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Image _healthFill;
    [SerializeField] private Image _healthEfx;
    [SerializeField] private EnemyScript _enemy;

    private Transform _cam;

    private void Start() {
        _cam = Camera.main.transform;
        _enemy.OnDamaged += SetHealth;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _cam.forward);
    }

    public void SetHealth(int _currentHealth, int _maxHealth)
    {
        Vector3 araFillScale = _healthFill.rectTransform.localScale;
        araFillScale.x = (float)_currentHealth / (float)_maxHealth;
        _healthFill.rectTransform.localScale = araFillScale;
        StartCoroutine(DecresingAraEfx(araFillScale));
    }

    IEnumerator DecresingAraEfx(Vector3 newScale)
    {
        yield return new WaitForSeconds(0.25f);
        Vector3 araEfxScale = _healthEfx.rectTransform.localScale;

        while (_healthEfx.transform.localScale.x > newScale.x)
        {
            araEfxScale.x -= Time.deltaTime * 0.4f;
            _healthEfx.rectTransform.localScale = araEfxScale;

            yield return null;
        }
    }
}
