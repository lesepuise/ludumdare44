using UnityEngine;

public abstract class Launcher : MonoBehaviour
{
    public Transform PlayerPosition;
    public bool Active;
    public ItemState State = ItemState.Unavailable;

    public virtual void Init(Player player)
    {
        gameObject.SetActive(true);

        Active = true;
        player.Pause();
        player.PausePhysic();

        player.transform.position = PlayerPosition.position;
        player.transform.rotation = PlayerPosition.rotation;
    }

    public void LaunchPlayer(Vector3 launchingVector, float strength)
    {
        Player player = PlayerData.Instance.CurrentPlayer;
        player.Unpause();
        player.UnpausePhysic();
        player.RigidBody.AddForce(launchingVector * strength, ForceMode.VelocityChange);
        Active = false;
    }

    public bool UpKey => Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
    public bool LeftKey => Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
    public bool DownKey => Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
    public bool RightKey => Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

    public bool ActionKey => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return);
}