using UnityEngine;

public class Wallet : MonoBehaviour, IWallet
{
    [SerializeField] private int InitialCurrency;

    public int Currency { get; private set; }

    #region UnityEvents
    private void Start()
    {
        Currency = InitialCurrency;
    }
    #endregion

    public void Spend(int price)
    {
        Currency -= price;
    }

    public void Collect(int income)
    {
        Currency += income;
    }
}

public interface IWallet
{
    public int Currency { get; }
    public void Spend(int price);
    public void Collect(int income);
}
