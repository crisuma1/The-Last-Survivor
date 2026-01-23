using UnityEngine;

public class ScopeUIController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerHandStateController stateController;
    [SerializeField] private GameObject scopeUI;

    void Start()
    {
        scopeUI.SetActive(false);
    }

    void Update()
    {
        bool isScopeInput = playerInput.currentAimState == AimState.SCope;
        bool isShooterState = stateController.CurrentState.StateType == HandStateType.Shooter;

        scopeUI.SetActive(isScopeInput && isShooterState);


    }
}
