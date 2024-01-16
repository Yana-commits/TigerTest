using System;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField]
    private Button closeBtn;

    [Header("Panels")]
    public MenuPanel menuPanel;
    public OptionPanel optionPanel;

    public Action OnStartGame;

    private void OnEnable()
    {
        closeBtn.onClick.AddListener(Close);
        menuPanel.OnShowOptions += OpenOptions;
        menuPanel.OnClose += Close;
        optionPanel.OnClose += Close;
    }
    private void Close()
    {
        optionPanel.CloseWin();
        menuPanel.CloseWin();
        this.gameObject.SetActive(false);
        OnStartGame?.Invoke();
    }
    private void OpenOptions()
    {
        menuPanel.CloseWin();
        optionPanel.Open();
        optionPanel.UpdateOptions(true);   
    }
    public void OpenMenu()
    {
        optionPanel.CloseWin();
        menuPanel.Open();
    }
    private void OnDisable()
    {
        closeBtn.onClick.RemoveListener(Close);
        menuPanel.OnShowOptions -= OpenOptions;
        menuPanel.OnClose -= Close;
        optionPanel.OnClose -= Close;
    }
}
