using UnityEngine;

public class AttractionManager : MonoBehaviour, ICycle<Attraction>
{
    [SerializeField] private Attraction[] attractions;

    private int selectedAttractionIndex;

    #region TentCycle
    public Attraction Increment()
    {
        selectedAttractionIndex = selectedAttractionIndex < attractions.Length - 1 ? selectedAttractionIndex + 1 : 0;
        return attractions[selectedAttractionIndex];
    }

    public Attraction Decrement()
    {
        selectedAttractionIndex = selectedAttractionIndex > 0 ? selectedAttractionIndex - 1 : attractions.Length - 1;
        return attractions[selectedAttractionIndex];
    }

    public Attraction Retrieve()
    {
        return attractions[selectedAttractionIndex];
    }
    #endregion
}
