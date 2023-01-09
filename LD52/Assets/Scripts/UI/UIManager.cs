using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager main;

    [SerializeField]
    private UIGameTimer uiGameTimer;
    [SerializeField]
    private UIDifficultyDisplay uiDifficultyDisplay;

    [SerializeField]
    private UIMenuScreen uiScreenPrefab;
    [SerializeField]
    private Transform menuScreenContainer;
    private UIMenuScreen uiPauseScreen;

    private UIMenuScreen uiGameOverScreen;

    [SerializeField]
    private Transform poppingTextContainer;
    [SerializeField]
    private PoppingText poppingTextPrefab;

    private UIMenuScreen uiTheEndScreen;
    private UIMenuScreen uiSpellUnlockScreen;
    [SerializeField]
    private MenuScreen pauseScreen;
    [SerializeField]
    private MenuScreen gameOverScreen;
    [SerializeField]
    private MenuScreen theEndScreen;
    [SerializeField]
    private MenuScreen spellUnlockScreen;

    [SerializeField]
    private UIEffect bloodEffect;

    [SerializeField]
    private UIBerryBar berryBar;

    private bool aMenuIsOpen = false;

    private void Awake()
    {
        main = this;
    }

    public void ShowPoppingText(Vector3 position, string message, Color color, int fontSize = 8)
    {
        PoppingText pop = Instantiate(poppingTextPrefab, position, Quaternion.identity, poppingTextContainer);
        pop.Show(position, message, color, fontSize);
    }

    public void Initialize(GameTimer timer, Difficulty difficulty, int maxBerries)
    {
        uiGameTimer.Initialize(timer);
        uiDifficultyDisplay.Initialize(difficulty);
        uiPauseScreen = Instantiate(uiScreenPrefab, Vector3.zero, Quaternion.identity, menuScreenContainer);
        uiPauseScreen.Initialize(pauseScreen);
        uiGameOverScreen = Instantiate(uiScreenPrefab, Vector3.zero, Quaternion.identity, menuScreenContainer);
        uiGameOverScreen.Initialize(gameOverScreen);
        uiTheEndScreen = Instantiate(uiScreenPrefab, Vector3.zero, Quaternion.identity, menuScreenContainer);
        uiTheEndScreen.Initialize(theEndScreen);
        berryBar.Initialize(maxBerries);
    }

    public void ShowSpellUnlock(CastableSpell spell)
    {
        uiSpellUnlockScreen = Instantiate(uiScreenPrefab, Vector3.zero, Quaternion.identity, menuScreenContainer);
        uiSpellUnlockScreen.Initialize(spellUnlockScreen);
        uiSpellUnlockScreen.Open(spell.UnlockTitle, spell.UnlockMessage, spell.Icon, false);
    }

    [SerializeField]
    private UIHealth uiPlayerHealth;

    public void AddBerry(int count)
    {
        berryBar.AddBerry(count);
    }

    public void ShowBloodEffect()
    {
        bloodEffect.Play();
    }

    public void RegisterPlayerHealth(EntityHealth playerHealth)
    {
        uiPlayerHealth.Initialize(playerHealth);
    }

    public void MenuWasClosed()
    {
        aMenuIsOpen = false;
    }

    public void OpenGameOverMenu()
    {
        uiGameOverScreen.Open();
    }

    public void OpenTheEndMenu()
    {
        uiTheEndScreen.Open();
    }

    private void CheckMenus()
    {
        if (aMenuIsOpen)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            uiPauseScreen.Open();
        }
    }

    void Update()
    {
        CheckMenus();
    }

}
