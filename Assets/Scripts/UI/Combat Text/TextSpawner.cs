using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSpawner : MonoBehaviour
{
    public static TextSpawner Instance;

    [SerializeField] private GameObject _textPopUp;
    private Camera _cam;

    private Quaternion camRotation;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        _cam = Camera.main;
        camRotation = Camera.main.transform.rotation;
    }

    public void SpawnPopupDamage(int damage, Vector3 startPosition){
        DamagePopup popup;
        Vector3 screenPos = _cam.WorldToScreenPoint(startPosition);
        popup=Instantiate(_textPopUp,screenPos,Quaternion.Euler(0,0,0),transform).GetComponent<DamagePopup>();
        //popup = Instantiate(_textPopUp,startPosition,camRotation).GetComponent<DamagePopup>();
        popup.Setup(damage);
    }
}
