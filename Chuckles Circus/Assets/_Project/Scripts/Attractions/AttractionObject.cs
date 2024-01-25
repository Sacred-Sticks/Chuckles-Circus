using UnityEngine;

[CreateAssetMenu(menuName = "Chuckles Circus/Attracion", fileName = "Attraction", order = 0)]
public class AttractionObject : ScriptableObject
{
    [field: SerializeField] public GameObject attractionPrefab { get; private set; }
    [field: SerializeField] public Shader attractionShader { get; private set; }
    [field: SerializeField] public Shader hologramShader { get; private set; }
    [field: SerializeField] public float costToBuild { get; private set; }
}