using System.Collections.Generic;
using UnityEngine;

public class RampSpot : MonoBehaviour
{
    public List<GameObject> ramps;
    public GameObject ramp;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(4.0f, 4.0f, 6.0f));
    }

    private void Start()
    {
        int doRender = Random.Range(0, 5);
        if (doRender < 3)
        {
            int rampToRender = Random.Range(0, ramps.Count);
            ramp = Instantiate(ramps[rampToRender], transform);
            TruncateName(ramp);
        }
    }

    private void TruncateName(GameObject go)
    {
        go.name = go.name.Remove(go.name.Length - 7); //Remove the "(Clone)" from the name
    }

}