using CleverCode;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    public int chosenVariant = 0;

    public Player playerPrefab;

    public float maxMovementForDeath = 0.4f;

    public int chosenLauncherIndex = 0; //0 = None, 1 = Cannon
    public List<string> boughtLaunchers;

    public int metaSnow;

    public bool IsLauncherPurchased(string name)
    {
        return boughtLaunchers.Contains(name); 
    }

    public bool PurchaseLauncher(string name, int price)
    {
        if (IsLauncherPurchased(name) || metaSnow < price)
            return false;

        metaSnow -= price;
        boughtLaunchers.Add(name);
        return true;
    }
}