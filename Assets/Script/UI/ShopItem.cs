using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Text title;
    public Text buttonText;

    [NonSerialized]
    public Power relatedPower;

    public void Init(Power power)
    {
        relatedPower = power;

        UpdateText();
    }

    public void Button_OnClick()
    {
        relatedPower.purchased = !relatedPower.purchased;

        UpdateText();
    }

    private void UpdateText()
    {
        title.text = relatedPower.powerName;
        buttonText.text = relatedPower.purchased ? "Enabled" : "Disabled";
    }
}