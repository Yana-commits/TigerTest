using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    public GameObject victoryPanel = null;
    public GameObject gameOverPanel = null;
    public GameObject previuePanel = null;


    public void ShowStatement(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
    public void HideStatement(bool gameSate)
    {
        victoryPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        if (gameSate)
        {
            previuePanel.SetActive(false);
        }
        else
        {
            previuePanel.SetActive(true);
        }
         
    }
  
}
