using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float WALKSPEED = 3f;
    public float RUNSPEED = 6f;
    public float MAXSPEED = 5f;
    public float mouseSensitivity = .5f;
    public BoxCollider wallDetection;

    float speed = 0f;
    bool wallRunning = false;

    Input inp;
    Camera cam;

    void Awake() {
        inp = new Input();
        inp.Player.Enable();
        cam = transform.GetComponentInChildren<Camera>();
    }


    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        // Movement with rigidbody
        speed = inp.Player.Sprint.IsPressed() ? RUNSPEED : WALKSPEED;
        Vector2 movement = inp.Player.Move.ReadValue<Vector2>();
        if (movement != Vector2.zero) {
            Vector3 movementDirection = transform.forward*movement.y + transform.right*movement.x;
            Debug.Log(movementDirection.normalized * speed);
            rb.AddForce(movementDirection * speed);
            // Limit Max Speed
            if (rb.velocity.magnitude > MAXSPEED) {
                rb.velocity = rb.velocity.normalized * MAXSPEED;
            }
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
        Debug.Log("Hello?");
        Debug.Log(wallRunning ? "Wall Running" : "Not Wall Running");
    }
}
