
using UnityEngine;

public class WndowPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject title = null;
    public void Open()
    { 
        
        this.gameObject.SetActive(true);
        title.SetActive(true);
    }

    public virtual void CloseWin()
    { 
        title.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
