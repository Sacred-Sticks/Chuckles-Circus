using UnityEngine;

[CreateAssetMenu(menuName = "Chuckles Circus/Attracion", fileName = "Attraction", order = 0)]
public class Attraction : ScriptableObject
{
    [SerializeField] private GameObject attractionPrefab;
    [SerializeField] private Shader attractionShader;
    [SerializeField] private Shader hologramShader;
    [SerializeField] private float costToBuild;
}
