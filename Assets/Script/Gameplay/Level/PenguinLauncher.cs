using System.Collections;
using UnityEngine;

public class PenguinLauncher : Launcher
{
    public Transform Rotater;
    public AnimationCurve Curve;
    public float rotationTime = 2f;
    public float rotationFactor = 1080f;

    public Transform ShootingPoint;
    public float ShootingStrength;

    [SerializeField] private AudioSource _cannonSound;

    private float StartTime;
    private bool shooting;

    public override void Init(Player player)
    {
        base.Init(player);

        StartTime = Time.time;
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

        if (shooting)
        {
            return;
        }

        if (ActionKey || Time.time - StartTime > 10f)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        shooting = true;
        StartCoroutine(ShootCo());
    }

    private IEnumerator ShootCo()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / rotationTime;

            float rotation = Curve.Evaluate(t);

            Rotater.transform.localEulerAngles = new Vector3(0f, rotation * rotationFactor, 0f);

            yield return null;
        }

        _cannonSound.Play();
        LaunchPlayer(ShootingPoint.forward, ShootingStrength);
    }
}