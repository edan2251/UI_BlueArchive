using UnityEngine;
using System;

public class PlayerResourceManager : MonoBehaviour
{
    public static PlayerResourceManager Instance;

    public int Energy { get; private set; } = 0;
    public int Money { get; private set; } = 0;
    public int Dia { get; private set; } = 0;

    private const int MAX_ENERGY = 999;
    private const int MAX_MONEY = 9999999;
    private const int MAX_DIA = 99999;

    public event Action<int, int> OnEnergyChanged;
    public event Action<int, int> OnMoneyChanged;
    public event Action<int, int> OnDiaChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddRandomEnergy()
    {
        int oldAmount = Energy;
        int addAmount = UnityEngine.Random.Range(50, 101);

        Energy = Mathf.Min(Energy + addAmount, MAX_ENERGY);

        if (oldAmount != Energy)
        {
            OnEnergyChanged?.Invoke(oldAmount, Energy);
        }
    }

    public void AddRandomMoney()
    {
        int oldAmount = Money;
        int addAmount = UnityEngine.Random.Range(500, 1501);

        Money = Mathf.Min(Money + addAmount, MAX_MONEY);

        if (oldAmount != Money)
        {
            OnMoneyChanged?.Invoke(oldAmount, Money);
        }
    }

    public void AddRandomDia()
    {
        int oldAmount = Dia;
        int addAmount = UnityEngine.Random.Range(50, 101);

        Dia = Mathf.Min(Dia + addAmount, MAX_DIA);

        if (oldAmount != Dia)
        {
            OnDiaChanged?.Invoke(oldAmount, Dia);
        }
    }
}