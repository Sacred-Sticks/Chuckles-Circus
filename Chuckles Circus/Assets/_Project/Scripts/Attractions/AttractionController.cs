using Kickstarter.Inputs;
using Kickstarter.Observer;
using UnityEngine;

public class AttractionController : Observable, IInputReceiver
{
    [SerializeField] private FloatInput placeAttractionInput;
    [SerializeField] private FloatInput destroyAttractionInput;
    [SerializeField] private FloatInput cycleAttractionInput;

    private ICycle<AttractionObject> attrationCycle;
    
    #region UnityEvents
    private void Awake()
    {
        TryGetComponent(out attrationCycle);
    }
    #endregion
    
    #region InputHandler
    public void RegisterInputs(Player.PlayerIdentifier playerIdentifier)
    {
        placeAttractionInput.RegisterInput(OnPlaceAttractionInputChange, playerIdentifier);
        destroyAttractionInput.RegisterInput(OnDestroyAttractionInputChange, playerIdentifier);
        cycleAttractionInput.RegisterInput(OnCycleAttractionInputChange, playerIdentifier);
    }

    public void DeregisterInputs(Player.PlayerIdentifier playerIdentifier)
    {
        placeAttractionInput.DeregisterInput(OnPlaceAttractionInputChange, playerIdentifier);
        destroyAttractionInput.DeregisterInput(OnDestroyAttractionInputChange, playerIdentifier);
        cycleAttractionInput.DeregisterInput(OnCycleAttractionInputChange, playerIdentifier);
    }

    private void OnPlaceAttractionInputChange(float input)
    {
        if (input == 0)
            return;
        Debug.Log("Place Tent");
    }

    private void OnDestroyAttractionInputChange(float input)
    {
        if (input == 0)
            return;
        Debug.Log("Destroy Tent");
    }

    private void OnCycleAttractionInputChange(float input)
    {
        if (input == 0)
            return;
        var attraction = input switch
        {
            > 0 => attrationCycle.Increment(),
            < 0 => attrationCycle.Decrement(),
            _ => attrationCycle.Retrieve(),
        };
    }
    #endregion
}
