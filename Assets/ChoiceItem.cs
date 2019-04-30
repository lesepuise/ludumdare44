using System;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceItem : MonoBehaviour
{
    [SerializeField] private Button _selectButton;
    [SerializeField] private Button _buyButton;

    [SerializeField] private GameObject _priceBanner;
    [SerializeField] private Text _priceText;
    [SerializeField] private Text _name;
    [SerializeField] private Image _image;
    private int _itemIndex;
    private ItemState _state = ItemState.Unavailable;
    public int Price = 50000;
    private string _launcherName;
    private Launcher _launcher;

    private Action _updateShop;

    private void Start()
    {
        SetPrice(Price);
    }

    public void InitItem(string name, Sprite image, int price)
    {
        _name.text = name;
        _image.sprite = image;
        SetPrice(price);
    }

    public void InitLauncher(Launcher launcher, Action callback)
    {
        _updateShop = callback;
        _launcher = launcher;
        _launcherName = _launcher.LauncherName;
        _name.text = _launcherName;
        _image.sprite = _launcher.ShopImage;
        SetPrice(_launcher.cost);

        if(GameManager.Instance.IsLauncherPurchased(_launcherName))
        UpdateButtons();
        ChangeButtonColor(GameManager.Instance.chosenLauncherIndex, _selectButton.colors.highlightedColor);
    }

    private void SetPrice(int price)
    {
        Price = price;
        _priceText.text = string.Format("{0} $now", Price);
        _priceBanner.SetActive(_state == ItemState.Unavailable);
    }

    public void Buy_OnClick()
    {
        if (!GameManager.Instance.PurchaseLauncher(_launcherName, Price))
            return;

        _updateShop();
        UpdateButtons();
    }

    public void Select_OnClick()
    {
        if (!GameManager.Instance.IsLauncherPurchased(_launcherName))
            return;

        GameManager.Instance.chosenLauncherIndex = _itemIndex;

        _updateShop();
    }

    private void UpdateButtons()
    {
        _selectButton.gameObject.SetActive(true);
        _buyButton.gameObject.SetActive(false);
    }

    public void ChangeButtonColor(int indexSelected, Color selectedColor)
    {
        var colors = _selectButton.colors;
        colors.normalColor = indexSelected == _itemIndex ? selectedColor : Color.white;
        _selectButton.colors = colors;
    }
}
