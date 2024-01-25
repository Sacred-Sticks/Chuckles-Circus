using UnityEngine;

public class AttractionManager : MonoBehaviour, IAttractionManager, ICycle<AttractionObject>
{
    [SerializeField] private AttractionObject[] attractions;

    private int selectedAttractionIndex;

    private GameObject hologram;
    
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
        hologram = Instantiate(attractions[selectedAttractionIndex].attractionPrefab);
        HologramActive = true;
    }

    public void CancelHologram()
    {
        Destroy(hologram);
        HologramActive = false;
    }

    public void BuildAttraction()
    {
        hologram.TryGetComponent(out IAttraction attraction);
        attraction.LockedPosition = true;
        attraction.BuildAttraction();
        HologramActive = false;
    }
    #endregion
}

public interface IAttractionManager
{
    public bool HologramActive { get; }
    
    public void CreateHologram();
    public void CancelHologram();
    public void BuildAttraction();
}
