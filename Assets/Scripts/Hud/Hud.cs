
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [Header("Main info")]
    [SerializeField]
    private Button menuBtn;

    [Space]
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI levelTxt;
    [Header("Panels")]
    [SerializeField]
    private LoadingPanel loadingPanel;

    public PopUp popUpPanel;

    public Action OnPauseGame;

    private void Start()
    {
        menuBtn.onClick.AddListener(ShowPopUp);
    }
    public void Init(string scoreValue, string levelValue)
    {
        SetTxtValue(scoreTxt, scoreValue);
        SetTxtValue(levelTxt, levelValue);
    }
    public void SetTxtValue(TextMeshProUGUI txt, string value)
    {
        txt.text = value;
    }
    public void ShowLoading(EndStatements statement)
    {
        loadingPanel.gameObject.SetActive(true);
        if (statement == EndStatements.Victory)
        {
            loadingPanel.ShowStatement(loadingPanel.victoryPanel);
        }
        else if (statement == EndStatements.GameOver)
        {
            loadingPanel.ShowStatement(loadingPanel.gameOverPanel);
        }
        else
        {
        }
    }
    public void HideLoadng(bool gameState)
    {
        if (gameState)
            loadingPanel.gameObject.SetActive(false);

        loadingPanel.HideStatement(gameState);
    }

    private void ShowPopUp()
    {
        OnPauseGame?.Invoke();
        popUpPanel.gameObject.SetActive(true);
        popUpPanel.OpenMenu();
    }
    private void OnDestroy()
    {
        menuBtn.onClick.RemoveListener(ShowPopUp);
    }
}
