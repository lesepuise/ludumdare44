using System.Collections;
using System.Collections.Generic;
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
    private ItemState _state = ItemState.Unavailable;
    public int Price = 50000;

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

    private void SetPrice(int price)
    {
        Price = price;
        _priceText.text = string.Format("{0} $now", Price);
        _priceBanner.SetActive(_state == ItemState.Unavailable);
    }
}
