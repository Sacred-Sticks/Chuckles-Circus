using Kickstarter.Inputs;
using Kickstarter.Observer;
using UnityEngine;

public class TentController : Observable, IInputReceiver
{
    [SerializeField] private FloatInput placeTentInput;
    [SerializeField] private FloatInput destroyTentInput;
    [SerializeField] private FloatInput cycleTentInput;
    
    #region InputHandler
    public void RegisterInputs(Player.PlayerIdentifier playerIdentifier)
    {
        placeTentInput.RegisterInput(OnPlaceTentInputChange, playerIdentifier);
        destroyTentInput.RegisterInput(OnDestroyTentInputChange, playerIdentifier);
        cycleTentInput.RegisterInput(OnCycleTentInputChange, playerIdentifier);
    }

    public void DeregisterInputs(Player.PlayerIdentifier playerIdentifier)
    {
        placeTentInput.DeregisterInput(OnPlaceTentInputChange, playerIdentifier);
        destroyTentInput.DeregisterInput(OnDestroyTentInputChange, playerIdentifier);
        cycleTentInput.DeregisterInput(OnCycleTentInputChange, playerIdentifier);
    }

    private void OnPlaceTentInputChange(float input)
    {
        if (input == 0)
            return;
        Debug.Log("Place Tent");
    }

    private void OnDestroyTentInputChange(float input)
    {
        if (input == 0)
            return;
        Debug.Log("Destroy Tent");
    }

    private void OnCycleTentInputChange(float input)
    {
        if (input == 0)
            return;
        Debug.Log("Cycle Active Tent");
    }
    #endregion
}
