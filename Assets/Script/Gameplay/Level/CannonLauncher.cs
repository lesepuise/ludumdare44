using UnityEngine;

public class CannonLauncher : Launcher
{
    public Rigidbody SwivelBody;

    public Transform ShootingPoint;

    public Vector3 TorqueForce;

    public float ShootingStrength;

    public override void Init(Player player)
    {
        base.Init(player);
    }

    private void Update()
    {
        if (!Active)
        {
            return;
        }

        Player player = PlayerData.Instance.CurrentPlayer;

        player.transform.position = PlayerPosition.position;
        player.transform.rotation = PlayerPosition.rotation;

        if (UpKey) SwivelBody.AddTorque(TorqueForce);
        if (DownKey) SwivelBody.AddTorque(-TorqueForce);
        
        if (ActionKey)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        LaunchPlayer(ShootingPoint.forward, ShootingStrength);
    }
}