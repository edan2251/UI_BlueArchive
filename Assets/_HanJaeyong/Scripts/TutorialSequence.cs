using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TutorialSequence : MonoBehaviour
{
    [Header("[튜토리얼 정보]")]
    public string sceneName = "학생 리스트 화면";
    public string authorName = "한재용";
    [TextArea(3, 5)]
    public string description = "화면에대한설명";

    [Header("[UI 컴포넌트 연결]")]
    [Tooltip("회색배경넣기")]
    public CanvasGroup darkOverlay;

    [Tooltip("팝업UI")]
    public CanvasGroup popupUI;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;

    [Header("[클릭 감지 및 패널]")]
    public Button screenClickButton;

    [Tooltip("튜토리얼 패널들")]
    public GameObject[] tutorialPanels;

    private int currentPanelIndex = -1;
    private bool hasPlayedOnce = false;

    private void Start()
    {
        PlayTutorialIfNeeded();
    }

    public void PlayTutorialIfNeeded()
    {
        if (hasPlayedOnce) return;
        hasPlayedOnce = true;

        if (titleText != null) titleText.text = $"{sceneName} / 작업자: {authorName}";
        if (descText != null) descText.text = $"[작업 내용]\n{description}";

        darkOverlay.gameObject.SetActive(true);
        darkOverlay.alpha = 1f;

        popupUI.gameObject.SetActive(true);
        popupUI.alpha = 0f;

        foreach (var p in tutorialPanels)
        {
            if (p != null) p.SetActive(false);
        }

        screenClickButton.gameObject.SetActive(false);
        screenClickButton.onClick.RemoveAllListeners();
        screenClickButton.onClick.AddListener(OnClickScreen);

        Sequence seq = DOTween.Sequence();
        seq.Append(popupUI.DOFade(1f, 0.5f));

        seq.AppendInterval(3f);

        seq.Append(popupUI.DOFade(0f, 0.5f));
        seq.AppendCallback(() =>
        {
            popupUI.gameObject.SetActive(false);

            currentPanelIndex = 0;
            ShowCurrentPanel();
            screenClickButton.gameObject.SetActive(true);
        });
    }

    private void ShowCurrentPanel()
    {
        for (int i = 0; i < tutorialPanels.Length; i++)
        {
            if (tutorialPanels[i] != null)
            {
                tutorialPanels[i].SetActive(i == currentPanelIndex);
            }
        }
    }

    private void OnClickScreen()
    {
        currentPanelIndex++;

        if (currentPanelIndex >= tutorialPanels.Length)
        {
            screenClickButton.gameObject.SetActive(false);

            darkOverlay.DOFade(0f, 0.5f).OnComplete(() =>
            {
                darkOverlay.gameObject.SetActive(false);
            });
        }
        else
        {
            ShowCurrentPanel();
        }
    }
}