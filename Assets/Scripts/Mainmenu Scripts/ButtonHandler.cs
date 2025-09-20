using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public void ActivatePanel(string _panelName)
    {
        MainmenuHandler.Instance.panelName = _panelName;
    }

    public void ActivateLetterTracing(string letter)
    {
        MainmenuHandler.Instance.SelectedLetter = letter;
        MainmenuHandler.Instance.panelName = "ItemTrace";  
    }
}
