using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionPower = 100f;
    Rigidbody rb;
    bool exploded = false;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.velocity = (transform.up * 10);
        StartCoroutine(DestroyProjectile());
    }

    void OnCollisionEnter(Collision collision) {
        if (!exploded) {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
            foreach (Collider hit in colliders) {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null) {
                    rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius);
                }
            }
            exploded = true;
        }
    }

    // Coroutine to destroy the projectile after 30 seconds
    IEnumerator DestroyProjectile() {
        yield return new WaitForSeconds(30);
        Destroy(gameObject);
    }
}
