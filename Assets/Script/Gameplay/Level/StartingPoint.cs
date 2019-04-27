using UnityEngine;

public class StartingPoint : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void SpawnPlayer()
    {
        GameObject player = Instantiate(GameManager.Instance.playerPrefab).gameObject;

        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
    }
}