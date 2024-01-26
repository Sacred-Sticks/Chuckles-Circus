using UnityEngine;

public class Wallet : MonoBehaviour, IWallet
{
    [SerializeField] private int InitialCurrency;

    private int currency;
    public int Currency
    {
        get => currency;
        private set
        {
            currency = value;
            gameplayUI.UpdateCurrency(currency);
        }
    }

    private IGameplayInterface gameplayUI;
    
    #region UnityEvents
    private void Awake()
    {
        gameplayUI = FindObjectOfType<GameplayInterface>();
    }

    private void Start()
    {
        Currency = InitialCurrency;
    }
    #endregion

    #region Wallet
    public void Spend(int price)
    {
        Currency -= price;
    }

    public void Collect(int income)
    {
        Currency += income;
    }
    #endregion
}

public interface IWallet
{
    public int Currency { get; }
    public void Spend(int price);
    public void Collect(int income);
}
