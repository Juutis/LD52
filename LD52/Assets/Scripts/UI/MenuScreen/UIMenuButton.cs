using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMenuButton : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI txtTitle;

    [SerializeField]
    private Button uiButton;

    private MenuButton button;
    private UIMenuScreen screen;
    public void Initialize(MenuButton newButton, UIMenuScreen newScreen)
    {
        screen = newScreen;
        button = newButton;
        uiButton.onClick.AddListener(OnClick);
        txtTitle.text = newButton.Title;
    }

    public void OnClick()
    {
        screen.OnClick(button.Action);

    }
}
