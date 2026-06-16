using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    public int coins = 500000;
    public int diamonds = 1000;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI diamondText;

    private void Awake()
    {
        if (instance == null) instance = this;
        UpdateCurrencyUI();
    }

    public bool SpendCoin(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            UpdateCurrencyUI();
            return true;
        }
        return false;
    }

    public void UpdateCurrencyUI()
    {
        if (coinText != null) coinText.text = coins.ToString("N0");
        if (diamondText != null) diamondText.text = diamonds.ToString("N0");
    }
}