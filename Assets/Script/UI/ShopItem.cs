using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Text title;
    public Text buttonText;
    public Text price;

    [NonSerialized]
    public Power relatedPower;

    private Action _updateShop;

    public void Init(Power power, Action callback)
    {
        relatedPower = power;
        _updateShop = callback;

        UpdateText();
    }

    public void Button_OnClick()
    {
        if (!relatedPower.purchased && GameManager.Instance.metaSnow < relatedPower.cost)
            return;

        GameManager.Instance.metaSnow += relatedPower.purchased ? relatedPower.cost : -relatedPower.cost;
        _updateShop();

        relatedPower.purchased = !relatedPower.purchased;

        UpdateText();
    }

    private void UpdateText()
    {
        title.text = relatedPower.powerName;
        buttonText.text = relatedPower.purchased ? "Enabled" : "Disabled";
        price.text = string.Format("{0} $now", relatedPower.cost);
    }
}