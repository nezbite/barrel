using UnityEngine;

public class ExplosionPhysics
{
    public static void Explosion(Vector3 explosionPos, float explosionRadius, float explosionPower) {
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        foreach (Collider hit in colliders) {
            Destructable d = hit.GetComponent<Destructable>();
            if (d != null) {
                d.Destruction();
            }
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius);
            }
            Barrel b = hit.GetComponent<Barrel>();
            if (b != null) {
                b.OnNearExplosion();
            }
        }
    }
}
