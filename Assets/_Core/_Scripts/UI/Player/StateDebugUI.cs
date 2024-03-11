using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateDebugUI : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine _psm;
    [SerializeField] private TextMeshProUGUI _text;

    private void Update() {
        _text.text = _psm.CurrentState.GetActiveStates();
    }
}
