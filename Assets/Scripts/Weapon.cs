using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectile;
    public GameObject muzzleFlash;
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
        StartCoroutine(Flash());
    }

    IEnumerator Flash() {
        GameObject flash = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation*Quaternion.Euler(-90, 0, 0));
        flash.transform.parent = firePoint;
        yield return new WaitForSeconds(0.25f);
        Destroy(flash);
    }
}
