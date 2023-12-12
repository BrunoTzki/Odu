using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeathUI : MonoBehaviour
{
    [SerializeField] private Image _araFill;
    [SerializeField] private Image _araEfx;


    [SerializeField] private int _maxAra;
    private int _currentAra;

    private PlayerStats _playerStats;

    private void Start() {
        _currentAra = _maxAra;

        _playerStats = FindObjectOfType<PlayerStats>();

        _playerStats.OnDamageTaken += PlayerStats_OnDamageTaken;
    }

    private void PlayerStats_OnDamageTaken(int amount){
        SetHealth(amount);
    }

    public void SetHealth(int amount)
    {
        Vector3 araFillScale = _araFill.rectTransform.localScale;

        araFillScale.x = (float)_playerStats.CurrentHealth / (float)_playerStats.MaxHealth;
        araFillScale.x = Mathf.Clamp(araFillScale.x,0,_playerStats.MaxHealth);

        _araFill.rectTransform.localScale = araFillScale;
        StartCoroutine(DecresingAraEfx(araFillScale));
    }

    IEnumerator DecresingAraEfx(Vector3 newScale)
    {
        yield return new WaitForSeconds(0.25f);
        Vector3 araEfxScale = _araEfx.rectTransform.localScale;

        while (_araEfx.transform.localScale.x > newScale.x)
        {
            araEfxScale.x -= Time.deltaTime * 0.4f;
            _araEfx.rectTransform.localScale = araEfxScale;

            yield return null;
        }
    }
}
