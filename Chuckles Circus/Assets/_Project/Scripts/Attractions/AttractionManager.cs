using UnityEngine;
using UnityEngine.InputSystem;

public class AttractionManager : MonoBehaviour, IAttractionManager, ICycle<AttractionObject>
{
    [SerializeField] private AttractionObject[] attractions;
    [SerializeField] private LayerMask attractionLayer;
    [SerializeField] private Shader blueHologramShader;
    [SerializeField] private Shader redHologramShader;

    private int selectedAttractionIndex;

    private GameObject attractionGO;
    private IWallet wallet;
    
    #region UnityEvents
    private void Awake()
    {
        TryGetComponent(out wallet);
    }
    #endregion
    
    #region TentCycle
    public AttractionObject Increment()
    {
        selectedAttractionIndex = selectedAttractionIndex < attractions.Length - 1 ? selectedAttractionIndex + 1 : 0;
        return attractions[selectedAttractionIndex];
    }

    public AttractionObject Decrement()
    {
        selectedAttractionIndex = selectedAttractionIndex > 0 ? selectedAttractionIndex - 1 : attractions.Length - 1;
        return attractions[selectedAttractionIndex];
    }

    public AttractionObject Retrieve()
    {
        return attractions[selectedAttractionIndex];
    }
    #endregion
    
    #region AttractionManagement
    public bool HologramActive { get; private set; }

    public void CreateHologram()
    {
        attractionGO = Instantiate(attractions[selectedAttractionIndex].AttractionPrefab);
        attractionGO.TryGetComponent(out IAttraction attraction);
        var shader = blueHologramShader;
        if (wallet.Currency < attractions[selectedAttractionIndex].CostToBuild)
            shader = redHologramShader;
        attraction.SetToHologram(shader);
        attraction.IgnoreLayers = attractionLayer;
        HologramActive = true;
    }

    public void CancelHologram()
    {
        Destroy(attractionGO);
        HologramActive = false;
    }

    public void BuildAttraction()
    {
        if (wallet.Currency < attractions[selectedAttractionIndex].CostToBuild)
            return;
        attractionGO.TryGetComponent(out IAttraction attraction);
        attraction.LockedPosition = true;
        attraction.BuildAttraction();
        HologramActive = false;
        wallet.Spend(attractions[selectedAttractionIndex].CostToBuild);
    }

    public void DestroyAttraction()
    {
        var target = GetCursorHit();
        if (target != null)
            Destroy(target.transform.root.gameObject);
    }
    #endregion
    
    private GameObject GetCursorHit()
    {
        var mousePosition = Mouse.current.position.ReadValue();
        var ray = Camera.main.ScreenPointToRay(mousePosition);

        return Physics.Raycast(ray, out var hit, float.MaxValue, attractionLayer) ? hit.transform.gameObject : null;
    }
}

public interface IAttractionManager
{
    public bool HologramActive { get; }
    
    public void CreateHologram();
    public void CancelHologram();
    public void BuildAttraction();
    public void DestroyAttraction();
}
