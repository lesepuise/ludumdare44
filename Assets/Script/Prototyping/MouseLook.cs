using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 40.0f;
    public float moveSpeed = 2f;

    private GameObject lHand = null;
    private GameObject rHand = null;
    private bool _pause;

    void Start()
    {
        //Input.GetAxis("Mouse X");
        //Input.GetAxis("Mouse Y");

        transform.localPosition += Vector3.up * 1.6f;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _pause = !_pause;
        }

        Cursor.lockState = _pause ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _pause;

        if (_pause)
        {
            return;
        }

        UpdateRotation();
        UpdatePosition();

        bool fire = Input.GetButton("Fire1");
        if (lHand != null && rHand != null)
        {
            lHand.SetActive(fire);
            rHand.SetActive(fire);
        }
    }

    private void UpdatePosition()
    {
        transform.position += transform.forward * GetFrontMove() * moveSpeed * Time.deltaTime;
        transform.position += transform.right * GetRightMove() * moveSpeed * Time.deltaTime;
        transform.position += transform.up * GetUpMove() * moveSpeed * Time.deltaTime;
        /*
        Vector3 movement = Vector3.zero;

        movement += transform.forward * GetFrontMove() * moveSpeed * Time.deltaTime;
        movement += transform.right * GetRightMove() * moveSpeed * Time.deltaTime;
        movement += transform.up * GetUpMove() * moveSpeed * Time.deltaTime;

        AperiumHighLevelController.instance.SetPosition(transform.position + movement);
        */
    }

    private float GetFrontMove()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            return 1f;
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            return -1f;
        }

        return 0f;
    }

    private float GetRightMove()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            return 1f;
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            return -1f;
        }

        return 0f;
    }

    private float GetUpMove()
    {
        if (Input.GetKey(KeyCode.E))
        {
            return 1f;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            return -1f;
        }

        return 0f;
    }

    private void UpdateRotation()
    {
        Vector3 rot = transform.localEulerAngles;

        rot.y += WrapAngle(sensitivity * Time.deltaTime * Input.GetAxis("Mouse X"));
        rot.x -= WrapAngle(sensitivity * Time.deltaTime * Input.GetAxis("Mouse Y"));
        rot.z = 0f;

        transform.localEulerAngles = rot;
    }

    public void SetHand(GameObject lHand, GameObject rHand)
    {
        Transform lHandTransform = lHand.transform;
        Transform rHandTransform = rHand.transform;
        lHandTransform.localPosition = new Vector3(0.0f, 0.0f, 0.4f);
        rHandTransform.localPosition = new Vector3(0.0f, 0.0f, 0.4f);
        this.lHand = lHand;
        this.rHand = rHand;
    }

    private float WrapAngle(float angle)
    {
        while (angle > 180f)
        {
            angle -= 360f;
        }

        while (angle < -180f)
        {
            angle += 360f;
        }

        return angle;
    }
}