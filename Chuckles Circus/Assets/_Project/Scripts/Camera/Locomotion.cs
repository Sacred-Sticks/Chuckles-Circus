using Kickstarter.Inputs;
using UnityEngine;

public class Locomotion : MonoBehaviour, IInputReceiver, ILocomotion
{
    [SerializeField] private Vector2Input movementInput;
    [field: SerializeField] public float Speed { get; set; }
    
    private Rigidbody body;
    private Vector3 rawInput;
    
    #region UnityEvents
    private void Awake()
    {
        transform.root.TryGetComponent(out body);
        body.useGravity = false;
    }

    private void FixedUpdate()
    {
        body.velocity = rawInput * Speed;
    }
    #endregion
    
    #region InputHandler
    public void RegisterInputs(Player.PlayerIdentifier playerIdentifier)
    {
        movementInput.RegisterInput(OnMovementInputChange, playerIdentifier);
    }

    public void DeregisterInputs(Player.PlayerIdentifier playerIdentifier)
    {
        movementInput.DeregisterInput(OnMovementInputChange, playerIdentifier);
    }

    private void OnMovementInputChange(Vector2 input)
    {
        rawInput = new Vector3(input.x, 0, input.y);
    }
    #endregion
}

public interface ILocomotion
{
    public float Speed { get; set; }
}