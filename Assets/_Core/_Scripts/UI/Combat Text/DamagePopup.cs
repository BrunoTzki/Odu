using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private float _timer = 1f;
    public static Transform _popUpObj;

    private void Awake() {
        Destroy(this.gameObject,_timer);
    }
    
    public void Setup(int damage){
        _textMesh.SetText(damage.ToString());
    }

}
