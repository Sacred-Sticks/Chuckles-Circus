using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attraction : MonoBehaviour, IAttraction
{
    public LayerMask IgnoreLayers { private get; set; }
    
    public bool LockedPosition { private get; set; }

    private MeshRenderer[] renderers;
    private readonly Dictionary<MeshRenderer, List<Shader>> rendererMaterials = new Dictionary<MeshRenderer, List<Shader>>();

    #region UnityEvents
    private void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
    }

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

    #region Attractions
    public void BuildAttraction()
    {
        ResetShaders();
    }

    public void SetToHologram(Shader hologramShader)
    {
        foreach (var meshRenderer in renderers)
        {
            rendererMaterials.Add(meshRenderer, new List<Shader>());
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                rendererMaterials[meshRenderer].Add(meshRenderer.materials[i].shader);
                meshRenderer.materials[i].shader = hologramShader;
            }
        }
    }

    public void ResetShaders()
    {
        foreach (var meshRenderer in renderers)
        {
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                meshRenderer.materials[i].shader = rendererMaterials[meshRenderer][i];
            }
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

        return Physics.Raycast(ray, out var hit, float.MaxValue, ~IgnoreLayers) ? hit.point : Vector3.zero;
    }
}

public interface IAttraction
{
    public bool LockedPosition { set; }
    public LayerMask IgnoreLayers { set; }

    public void BuildAttraction();
    public void SetToHologram(Shader hologramShader);
    public void ResetShaders();
}
