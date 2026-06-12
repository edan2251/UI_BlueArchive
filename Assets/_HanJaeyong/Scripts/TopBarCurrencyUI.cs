using UnityEngine;
using TMPro;
using DG.Tweening;

public class TopBarCurrencyUI : MonoBehaviour
{
    [Header("[텍스트 UI]")]
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI diaText;

    [Header("[숫자 올라가는 시간]")]
    public float countDuration = 0.5f;

    private void Start()
    {
        PlayerResourceManager.Instance.OnEnergyChanged += AnimateEnergy;
        PlayerResourceManager.Instance.OnMoneyChanged += AnimateMoney;
        PlayerResourceManager.Instance.OnDiaChanged += AnimateDia;

        UpdateEnergyText(PlayerResourceManager.Instance.Energy);
        UpdateMoneyText(PlayerResourceManager.Instance.Money);
        UpdateDiaText(PlayerResourceManager.Instance.Dia);
    }

    private void OnDestroy()
    {
        if (PlayerResourceManager.Instance != null)
        {
            PlayerResourceManager.Instance.OnEnergyChanged -= AnimateEnergy;
            PlayerResourceManager.Instance.OnMoneyChanged -= AnimateMoney;
            PlayerResourceManager.Instance.OnDiaChanged -= AnimateDia;
        }
    }

    private void AnimateEnergy(int oldVal, int newVal)
    {
        DOVirtual.Int(oldVal, newVal, countDuration, (value) =>
        {
            UpdateEnergyText(value);
        }).SetEase(Ease.OutCubic);
    }

    private void AnimateMoney(int oldVal, int newVal)
    {
        DOVirtual.Int(oldVal, newVal, countDuration, (value) =>
        {
            UpdateMoneyText(value);
        }).SetEase(Ease.OutCubic);
    }

    private void AnimateDia(int oldVal, int newVal)
    {
        DOVirtual.Int(oldVal, newVal, countDuration, (value) =>
        {
            UpdateDiaText(value);
        }).SetEase(Ease.OutCubic);
    }

    private void UpdateEnergyText(int value)
    {
        energyText.text = $"{value}/999";
    }

    private void UpdateMoneyText(int value)
    {
        moneyText.text = value.ToString("N0");
    }

    private void UpdateDiaText(int value)
    {
        diaText.text = value.ToString("N0");
    }

    public void OnClickAddEnergyButton() => PlayerResourceManager.Instance.AddRandomEnergy();
    public void OnClickAddMoneyButton() => PlayerResourceManager.Instance.AddRandomMoney();
    public void OnClickAddDiaButton() => PlayerResourceManager.Instance.AddRandomDia();
}