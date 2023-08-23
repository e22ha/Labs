using UnityEngine;

public class Looking : MonoBehaviour
{
    private static bool CursorLook = true;

    public Transform player;
    public Transform cam;

    [Range(50f, 100f)] public float xSens = 70f;
    [Range(50f, 100f)] public float ySens = 70f;

    private Quaternion _center;

    private void Start()
    {
        _center = cam.localRotation;
    }

    private void Update()
    {
        var mouseY = Input.GetAxis("Mouse Y") * ySens * Time.deltaTime;
        var yRot = cam.localRotation * Quaternion.AngleAxis(mouseY, -Vector3.right);

        if (Quaternion.Angle(_center, yRot) < 90f)
            cam.localRotation = yRot;
        var mouseX = Input.GetAxis("Mouse X") * xSens * Time.deltaTime;
        var xRot = player.localRotation * Quaternion.AngleAxis(mouseX, Vector3.up);

        player.localRotation = xRot;

        if (CursorLook)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (Input.GetKeyDown(KeyCode.Escape)) CursorLook = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (Input.GetKeyDown(KeyCode.Escape)) CursorLook = true;
        }
    }
}