using UnityEngine;

[CreateAssetMenu(menuName = "Chuckles Circus/Attraction", fileName = "Attraction", order = 0)]
public class AttractionObject : ScriptableObject
{
    [field: SerializeField] public GameObject AttractionPrefab { get; private set; }
    [field: SerializeField] public float CostToBuild { get; private set; }
}