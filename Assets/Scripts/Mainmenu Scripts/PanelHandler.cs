using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHandler : MonoBehaviour
{
    public string myPanelName;
    public bool PauseTheGame;

    private void LateUpdate()
    {
        HandlePanelVisibility();
        HandleGamePause();

    }

    private void HandlePanelVisibility()
    {
        if(myPanelName == MainmenuHandler.Instance.panelName)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            if(MainmenuHandler.Instance.panelName != "loading")
            {
                this.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void HandleGamePause()
    {
        if (PauseTheGame)
        {
            if(this.transform.GetChild(0).gameObject.activeSelf)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
}
