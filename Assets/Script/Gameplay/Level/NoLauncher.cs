using UnityEngine;

public class NoLauncher : Launcher
{
    public override void Init(Player player)
    {
        base.Init(player);

        player.transform.position += Vector3.up * PlayerData.Instance.GetStartSize() / 2f;
        player.Unpause();
        player.UnpausePhysic();
    }
}