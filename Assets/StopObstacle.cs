using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CleverCode;

public class StopObstacle : MonoBehaviour
{
    [SerializeField] private bool _shakeOnHit;
    public List<GameObject> obstacles;
    public GameObject obstacle;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(4.0f, 6.0f, 4.0f));
    }

    private void Start()
    {
        int obstacleToRender = Random.Range(0, obstacles.Count);
        obstacle = Instantiate(obstacles[obstacleToRender], transform);
        TruncateName(obstacle);
    }

    private void TruncateName(GameObject go)
    {
        go.name = go.name.Remove(go.name.Length - 7); //Remove the "(Clone)" from the name
    }


    #region Collision Management

    private void OnCollisionEnter(Collision col)
    {
        Collider other = col.collider;
        if (other.gameObject.layer == Layer.Ball)
        {
            Player player = other.transform.parent.gameObject.GetComponent<Player>();
            player.Hit();
            Shake();
        }
    }

    private void Shake()
    {
        if (_shakeOnHit)
        {
            //Shake it baby!
        }
    }

    #endregion
}
