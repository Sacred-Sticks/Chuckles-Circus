using Cinemachine;
using Kickstarter.Inputs;
using UnityEngine;

public class CameraZoom : MonoBehaviour, IInputReceiver
{
    [SerializeField] private FloatInput cameraZoomInput;
    [SerializeField] private Vector2Int maxZoomRange;
    
    private CinemachineVirtualCamera virtualCamera;
    private ILocomotion locomotion;
    private int zoomLevel;
    
    #region UnityEvents
    private void Awake()
    {
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        TryGetComponent(out locomotion);
    }
    #endregion
    
    #region InputHandler
    public void RegisterInputs(Player.PlayerIdentifier playerIdentifier)
    {
        cameraZoomInput.RegisterInput(OnCameraZoomInputChange, playerIdentifier);
    }

    public void DeregisterInputs(Player.PlayerIdentifier playerIdentifier)
    {
        cameraZoomInput.DeregisterInput(OnCameraZoomInputChange, playerIdentifier);
    }

    private void OnCameraZoomInputChange(float input)
    {
        float multiplier = input switch
        {
            < 0 => 0.5f,
            > 0 => 2f,
            _ => 1,
        };
        AdjustZoom(multiplier);
    }
    #endregion

    private void AdjustZoom(float multiplier)
    {
        int adjustment = multiplier switch
        {
            > 1 => 1,
            < 1 => -1,
            _ => 0,
        };
        if (zoomLevel + adjustment < maxZoomRange.x || zoomLevel + adjustment > maxZoomRange.y)
            return;
        zoomLevel += adjustment;
        locomotion.Speed *= multiplier;
        transform.localPosition *= multiplier;
    }
}
