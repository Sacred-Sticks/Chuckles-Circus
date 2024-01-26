using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GameplayInterface : MonoBehaviour, IGameplayInterface
{
    [SerializeField] private string currencyTarget;
    [SerializeField] private string patronsTarget;
    
    private Label currency;
    private Label patrons;
    
    
    #region UnityEvents
    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        currency = root.Q<Label>(currencyTarget);
        patrons = root.Q<Label>(patronsTarget);
    }
    #endregion
    
    #region GameplayInterface
    public void UpdateCurrency(int currency)
    {
        this.currency.text = $"Currency: {currency.ToString()}";
    }

    public void UpdatePatrons(int patrons)
    {
        this.patrons.text = patrons.ToString();
    }
    #endregion
}

public interface IGameplayInterface
{
    public void UpdateCurrency(int currency);

    public void UpdatePatrons(int patrons);
}
