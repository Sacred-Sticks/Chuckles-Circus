using System;
using System.Security.Authentication.ExtendedProtection;
using Kickstarter.Inputs;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Locomotion : MonoBehaviour, IInputReceiver, ILocomotion
{
    [SerializeField] private Vector2Input movementInput;
    [field: SerializeField] public float Speed { get; set; }
    [SerializeField] private Bounds bounds;
    
    private Rigidbody body;
    private Vector3 rawInput;
    
    #region UnityEvents
    private void Awake()
    {
        transform.root.TryGetComponent(out body);
        body.useGravity = false;
    }

    private void Update()
    {
        KeepWithinBorder();
    }

    private void FixedUpdate()
    {
        MoveCamera();
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

    private void MoveCamera()
    {
        body.velocity = rawInput * Speed;
    }

    private void KeepWithinBorder()
    {
        var rootPosition = transform.root.position;
        if (rootPosition.x < bounds.center.x - bounds.extents.x)
            rootPosition.x = bounds.center.x - bounds.extents.x;
        if (rootPosition.x > bounds.center.x + bounds.extents.x)
            rootPosition.x = bounds.center.x + bounds.extents.x;
        if (rootPosition.z < bounds.center.z - bounds.extents.z)
            rootPosition.z = bounds.center.z - bounds.extents.z;
        if (rootPosition.z > bounds.center.z + bounds.extents.z)
            rootPosition.z = bounds.center.z + bounds.extents.z;
        transform.root.position = rootPosition;
    }
}

public interface ILocomotion
{
    public float Speed { get; set; }
}