using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attraction : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;

    public bool LockedPosition { private get; set; }
    
    #region UnityEvents
    private IEnumerator Start()
    {
        var endOfFrame = new WaitForEndOfFrame();
        while (!LockedPosition)
        {
            MoveToCursor();
            yield return endOfFrame;
        }
    }
    #endregion
    
    private void MoveToCursor()
    {
        transform.position = GetCursorPosition();
    }
    
    private Vector3 GetCursorPosition()
    {
        var mousePosition = Mouse.current.position.ReadValue();
        var ray = Camera.main.ScreenPointToRay(mousePosition);

        return Physics.Raycast(ray, out var hit, float.MaxValue, targetLayers) ? hit.point : Vector3.zero;
    }
}
