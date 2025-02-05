using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed;
    public float zoomSpeed;

    public float minZoom;
    public float maxZoom;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    void Update ()
    {
        Move();
        Zoom();
    }

    void Move ()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 dir = transform.forward * z + transform.right * x;

        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    void Zoom ()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float dist = Vector3.Distance(transform.position, cam.transform.position);

        if(dist < minZoom && scrollInput > 0.0f)
            return;
        else if(dist > maxZoom && scrollInput < 0.0f)
            return;

        cam.transform.position += cam.transform.forward * scrollInput * zoomSpeed;
    }
}