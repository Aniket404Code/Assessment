using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainmenuHandler : MonoBehaviour
{
    public static MainmenuHandler Instance;
    public Sprite[] shapeSprites, lineSprites;
    public Transform itemsContent;
    public GameObject ItemPreafab_TextBased, ItemPrefab_SpriteBased;
    [HideInInspector] public string panelName;
    public string SelectedLetter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Optional if you want to keep across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }



    public void ShowTracingItems(string category)
    {
        foreach (Transform child in itemsContent)
        {
            Destroy(child.gameObject);
        }
        switch (category)
        {
            case "alphabet":
                for (int i = 0; i < 26; i++)
                {
                    int num = i + 65; //ASCII Code A-Z
                    SetItemText(i, num);
                }
                break;

            case "number":
                for (int i = 0; i <= 9; i++)
                {
                    int num = i + 48;  // ASCII code 0-9
                    SetItemText(i, num);
                }
                break;
            case "shape":
                for (int i = 0; i < shapeSprites.Length; i++)
                {              
                    SetItemImage(i, shapeSprites);
                }
                break;
            case "line":
                for (int i = 0; i < lineSprites.Length; i++)
                {
                    SetItemImage(i, lineSprites);
                }
                break;
               
        }

    }

    private void SetItemText(int i, int num)
    {
        GameObject _item = Instantiate(ItemPreafab_TextBased, itemsContent);
        _item.GetComponentInChildren<TextMeshProUGUI>().text = Convert.ToChar(num).ToString();

    }

    private void SetItemImage(int i, Sprite[] spritesItem)
    {
        GameObject _item = Instantiate(ItemPrefab_SpriteBased, itemsContent);
        _item.transform.GetChild(0).GetComponent<Image>().sprite = spritesItem[i]; 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}