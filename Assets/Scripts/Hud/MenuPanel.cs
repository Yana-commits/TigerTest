using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : WndowPanel
{

    [SerializeField]
    private Button optionBtn;
    [SerializeField]
    private Button continueBtn;
    [SerializeField]
    private Button resetBtn;
    [SerializeField]
    private Button exitBtn;

    public Action OnClose;
    public Action OnReset;
    public Action OnShowOptions;
    private void OnEnable()
    {
        optionBtn.onClick.AddListener(Options);
        resetBtn.onClick.AddListener(ResetLvl);
        continueBtn.onClick.AddListener(CloseWindow);
        exitBtn.onClick.AddListener(Exit);
    }
    private void CloseWindow()
    {
        OnClose?.Invoke();
    }
    private void Options()
    {
        OnShowOptions?.Invoke();
    }
    private void ResetLvl()
    {
        SaveSystem.instance.Resest();
        OnReset?.Invoke();
        OnClose?.Invoke();
    }
    private void Exit()
    {
        Application.Quit();
    }

    private void OnDisable()
    {

        optionBtn.onClick.RemoveListener(Options);
        resetBtn.onClick.RemoveListener(ResetLvl);
        continueBtn.onClick.RemoveListener(CloseWindow);
        exitBtn.onClick.RemoveListener(Exit);
    }
}
