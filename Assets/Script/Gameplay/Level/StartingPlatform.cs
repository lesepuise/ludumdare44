using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPlatform : MonoBehaviour
{
    public Transform ballStartPos; //for cam positioning
    public List<Launcher> launchers;

    public void Awake()
    {
        launchers.ForEach(launcher => launcher.gameObject.SetActive(launcher.Active));
    }

    public void SpawnPlayer()
    {
        Player player = Instantiate(GameManager.Instance.playerPrefab);

        player.transform.position = ballStartPos.position;
        player.transform.rotation = ballStartPos.rotation;

        player.InitCamera();

        launchers[GameManager.Instance.chosenLauncherIndex].Init(player);
    }
}