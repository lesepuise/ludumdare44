using CleverCode;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    private const string LEVEL_01_NAME = "Level_BasicMountain";
    private const string LEVEL_02_NAME = "Level_Volcano";

    [SerializeField] private ShopItem _passiveItemTemplate;
    [SerializeField] private ChoiceItem _launcherItemTemplate;

    [SerializeField] private GameObject[] _launchers;
    private List<ChoiceItem> _launcherItems = new List<ChoiceItem>();

    [SerializeField] private Button[] _tabButtons;
    [SerializeField] private GameObject[] _tabs;
    private int _selectedTab = -1;
    private Color _selectedColor;

    [SerializeField] private Text _currencyField;

    private GameManager _gameManager;

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            GameStarter.StartGameCorrectly();
            return;
        }

        _gameManager = GameManager.Instance;
        _selectedColor = _tabButtons[0].colors.selectedColor;

        _passiveItemTemplate.gameObject.SetActive(false);
        InitPassivePowers();
        _launcherItemTemplate.gameObject.SetActive(false);
        InitLaunchers();

        UpdateShop();
        MusicManager.Instance.SetScene(1);
    }

    public void Button_Start()
    {
        SceneManager.LoadScene(LEVEL_01_NAME);
    }

    public void Button_Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void SwitchTab(int tabIndex)
    {
        if (tabIndex == _selectedTab)
            return;

        if (_selectedTab != -1)
        {
            _tabs[_selectedTab].SetActive(false);
            ChangeButtonColor(_selectedTab, Color.white);
        }

        _selectedTab = tabIndex;
         _tabs[_selectedTab].SetActive(true);
        ChangeButtonColor(_selectedTab, _selectedColor);
    }

    private void ChangeButtonColor(int index, Color color)
    {
        var colors = _tabButtons[index].colors;
        colors.normalColor = color;
        _tabButtons[index].colors = colors;
    }
    
    private void InitPassivePowers()
    {
        foreach (Power power in PowerupManager.Instance.PassivePowers.GetPowers())
        {
            SpawnShopItem(power, _passiveItemTemplate);
        }
    }

    private void SpawnShopItem(Power power, ShopItem template)
    {
        ShopItem newShopItem = Instantiate(template, template.transform.parent);

        newShopItem.Init(power, UpdateShop);
        newShopItem.gameObject.SetActive(true);
    }

    private void InitLaunchers()
    {
        foreach (GameObject launcher in _launchers)
        {
            SpawnLauncherItem(launcher.GetComponent<Launcher>(), _launcherItemTemplate);
        }
    }

    private void SpawnLauncherItem(Launcher launcher, ChoiceItem template)
    {
        if (launcher.cost == 0)
            GameManager.Instance.PurchaseLauncher(launcher.LauncherName, launcher.cost);
        
        ChoiceItem newChoiceItem = Instantiate(template, template.transform.parent);

        newChoiceItem.InitLauncher(launcher, UpdateShop);
        newChoiceItem.gameObject.SetActive(true);

        _launcherItems.Add(newChoiceItem);
    }

    private void UpdateShop()
    {
        _currencyField.text = string.Format("{0} $now", _gameManager.metaSnow);

        var launcherIndex = GameManager.Instance.chosenLauncherIndex;
        foreach (var launcher in _launcherItems)
            launcher.ChangeButtonColor(launcherIndex, _selectedColor);
    }
}