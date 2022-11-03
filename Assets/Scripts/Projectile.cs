using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    void Start() {
        GetComponent<Rigidbody>().velocity = (transform.up * 10);
    }

    void OnCollisionEnter(Collision collision) {
        Debug.Log("Explosion");
        Destroy(gameObject);
    }
}
