using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionPower = 100f;
    public GameObject explosion;
    bool exploded = false;
    public Material explodedMaterial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.relativeVelocity.magnitude);
        if (collision.relativeVelocity.magnitude > 10)
        {
            Explode();
        }
    }

    public void OnNearExplosion() {
        StartCoroutine(ExplodeAfterDelay());
    }

    IEnumerator ExplodeAfterDelay() {
        yield return new WaitForSeconds(.5f);
        Explode();
    }

    void Explode() {
        if (exploded) return;
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
        GetComponent<MeshRenderer>().material = explodedMaterial;
    }
}
