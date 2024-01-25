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
    public void CreateHologram()
    {
        hologram = Instantiate(attractions[selectedAttractionIndex].attractionPrefab);
    }

    public void CancelHologram()
    {
        Destroy(hologram);
    }

    public void BuildAttraction()
    {
        // TODO_IMPLEMENT_ME();
    }
    #endregion
}

public interface IAttractionManager
{
    public void CreateHologram();
    public void CancelHologram();
    public void BuildAttraction();
}
