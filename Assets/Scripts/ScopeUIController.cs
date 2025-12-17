using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeUIController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject scopeUI;

    void Start()
    {
        scopeUI.SetActive(false);
    }

    void Update()
    {
        if (playerInput.currentAimState == AimState.SCope)
        {
            scopeUI.SetActive(true);
        }
        else
        {
            scopeUI.SetActive(false);
        }
    }
}
