using CleverCode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopMenu : MonoBehaviour
{
    private const string LEVEL_01_NAME = "Level_BasicMountain";
    private const string LEVEL_02_NAME = "Level_Volcano";

    [SerializeField] private ShopItem _passiveItemTemplate;

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            GameStarter.StartGameCorrectly();
            return;
        }

        _passiveItemTemplate.gameObject.SetActive(false);
        InitPassivePowers();

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

        newShopItem.Init(power);
        newShopItem.gameObject.SetActive(true);
    }
}