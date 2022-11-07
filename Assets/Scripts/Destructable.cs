using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public bool keepRigidBody = false;
    bool destroyed = false;
    bool delay = false;
    Rigidbody rb;

    public void Destruction() {
        if (destroyed) return;
        destroyed = true;
        rb = gameObject.AddComponent<Rigidbody>();
        rb.AddForce(new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)), ForceMode.Impulse);
        StartCoroutine(StartDelay());
    }

    void OnCollisionEnter(Collision collisionInfo) {
        if (collisionInfo.gameObject.tag == "Player") return;
        if (collisionInfo.relativeVelocity.magnitude > 10) {
            Destruction();
        }
    }

    void Update() {
        if (keepRigidBody) return;
        if (rb != null) {
            if (delay && rb.velocity == Vector3.zero) {
                Destroy(rb);
                rb = null;
                destroyed = false;
                delay = false;
            }
        }
    }
    
    IEnumerator StartDelay() {
        yield return new WaitForSeconds(1);
        delay = true;
    }
}
