using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionPower = 100f;
    public GameObject explosion;
    Rigidbody rb;
    bool exploded = false;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.velocity = (transform.up * 100);
        gameObject.layer = 6;
        StartCoroutine(DestroyProjectile());
    }

    void OnCollisionEnter(Collision collision) {
        if (!exploded) {
            if (collision.gameObject.tag == "Player") return;
            GetComponent<ParticleSystem>().Stop();
            GetComponent<ParticleSystem>().Clear();
            gameObject.layer = 0;
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
            foreach (Collider hit in colliders) {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null) {
                    rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius);
                }
                Barrel b = hit.GetComponent<Barrel>();
                if (b != null) {
                    b.OnNearExplosion();
                }
            }
            Instantiate(explosion, transform.position, Quaternion.identity);
            exploded = true;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (!exploded) {
            if (other.tag == "Player") return;
            GetComponent<ParticleSystem>().Stop();
            GetComponent<ParticleSystem>().Clear();
            gameObject.layer = 0;
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
            foreach (Collider hit in colliders) {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null) {
                    rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius);
                }
                Barrel b = hit.GetComponent<Barrel>();
                if (b != null) {
                    b.OnNearExplosion();
                }
            }
            Instantiate(explosion, transform.position, Quaternion.identity);
            exploded = true;
        }
    }

    // Coroutine to destroy the projectile after 30 seconds
    IEnumerator DestroyProjectile() {
        yield return new WaitForSeconds(30);
        Destroy(gameObject);
    }
}
