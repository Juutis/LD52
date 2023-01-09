using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMenuScreen : MonoBehaviour
{
    [SerializeField]
    private List<Image> imgBackground;

    [SerializeField]
    private Image difficultyImage;
    [SerializeField]
    private Image spellIcon;
    [SerializeField]
    private TextMeshProUGUI txtTitle;
    [SerializeField]
    private TextMeshProUGUI txtDescription;
    [SerializeField]
    private TextMeshProUGUI txtExtraTitle;
    [SerializeField]
    private TextMeshProUGUI txtExtraDescription;
    [SerializeField]
    private TextMeshProUGUI txtDifficulty;
    [SerializeField]
    private TextMeshProUGUI txtTimer;
    [SerializeField]
    private Transform buttonContainer;
    [SerializeField]
    private UIMenuButton buttonPrefab;

    private MenuScreen menu;

    [SerializeField]
    private GameObject difficultyDisplay;

    private bool initialized = false;

    [SerializeField]
    private Animator animator;

    public void Initialize(MenuScreen newMenu)
    {
        if (initialized)
        {
            return;
        }
        menu = newMenu;
        imgBackground.ForEach(img => img.sprite = menu.Background);
        txtTitle.text = menu.Title;
        txtDescription.text = menu.Description;
        foreach (MenuButton button in menu.Buttons)
        {
            UIMenuButton menuButton = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, buttonContainer);
            menuButton.Initialize(button, this);
        }
        initialized = true;
        transform.localPosition = Vector2.zero;
    }

    public void OnClick(MenuButtonAction action)
    {
        if (action == MenuButtonAction.MainMenu)
        {
            GameManager.main.OpenMainMenu();
        }
        if (action == MenuButtonAction.Restart)
        {
            GameManager.main.RestartGame();
        }
        if (action == MenuButtonAction.Continue)
        {
            Close();
        }
    }

    public void Open(string title = "", string description = "", Sprite icon = null, bool showDifficulty = true)
    {
        txtExtraTitle.text = title;
        txtExtraDescription.text = description;
        if (icon != null)
        {
            spellIcon.sprite = icon;
            spellIcon.enabled = true;
        }
        GameManager.main.PauseGame();
        animator.Play("uiMenuScreenOpen");
        txtDifficulty.text = $"Difficulty: {GameManager.main.Difficulty.Type}";
        txtTimer.text = GameManager.main.GameTimer.GetString();
        difficultyImage.color = GameManager.main.Difficulty.Color;

        if (!showDifficulty)
        {
            difficultyDisplay.SetActive(false);
        }
    }
    public void CloseFinished()
    {
        UIManager.main.MenuWasClosed();
        GameManager.main.ResumeGame();
    }
    private void Close()
    {
        animator.Play("uiMenuScreenClose");
    }
}

public enum MenuButtonAction
{
    Restart,
    MainMenu,
    Continue
}

[System.Serializable]
public class MenuButton
{
    [SerializeField]
    private string title;
    public string Title { get { return title; } }
    [SerializeField]
    private MenuButtonAction action;

    public MenuButtonAction Action { get { return action; } }
}

[System.Serializable]
public class MenuScreen
{

    [SerializeField]
    private string title;
    public string Title { get { return title; } }

    [SerializeField, TextArea]
    private string description;
    public string Description { get { return description; } }

    [SerializeField]
    private Sprite background;
    public Sprite Background { get { return background; } }

    [SerializeField]
    private List<MenuButton> buttons;
    public List<MenuButton> Buttons { get { return buttons; } }
}