using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float WALKSPEED = 3f;
    public float RUNSPEED = 6f;
    public float mouseSensitivity = .5f;

    float speed = 0f;

    Input inp;
    Camera cam;

    void Awake() {
        inp = new Input();
        inp.Player.Enable();
        cam = transform.GetComponentInChildren<Camera>();
    }

    void Update()
    {
        // Movement
        speed = inp.Player.Sprint.IsPressed() ? RUNSPEED : WALKSPEED;
        Vector2 movement = inp.Player.Move.ReadValue<Vector2>();
        if (movement != Vector2.zero) {
            transform.position += transform.forward * speed * movement.y * Time.deltaTime;
            transform.position += transform.right * speed * movement.x * Time.deltaTime;
        }

        // Camera
        Vector2 look = inp.Player.Look.ReadValue<Vector2>();
        if (look != Vector2.zero) {
            // Rotate camera with limits
            float lookY = (look.y > 20 ? 20 : look.y < -20 ? -20 : look.y) * mouseSensitivity;
            float lookX = look.x/2 * mouseSensitivity;
            Quaternion newCameraRotation = cam.transform.localRotation * Quaternion.Euler(-lookY, 0, 0);
            if (newCameraRotation.eulerAngles.x < 80 || newCameraRotation.eulerAngles.x > 280) {
                // smooth turn to target
                cam.transform.localRotation = newCameraRotation;
            }
            // Rotate Player
            transform.Rotate(0, lookX, 0);
        }
    }
}
