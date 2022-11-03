using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectile;
    Input inp;
    // Start is called before the first frame update
    void Start()
    {
        inp = new Input();
        inp.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (inp.Player.Fire.WasPerformedThisFrame()) {
            Shoot();
        }
    }

    void Shoot() {
        Instantiate(projectile, firePoint.position, firePoint.rotation);
    }
}
