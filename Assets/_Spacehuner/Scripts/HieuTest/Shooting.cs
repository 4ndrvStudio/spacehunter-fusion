using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("SpaceShip")]
    //[SerializeField] private GameObject spaceShip;
    [SerializeField] private GameObject gun;
    [Header("Bullet")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject fireShoot;
    [SerializeField] private float fireRate = 0.5f;
    private bool canShoot = false;
    public float throwForce;
    public float throwUpwardForce;

    //[Header("Skill Rocket")]
    //[SerializeField] private GameObject rocket;
    //[SerializeField] private GameObject fireShootSkill;
    //[SerializeField] private float cDRocket = 10f;

    private float tempTime = 0f;
    private float orginalTimeSkill = 0f;
    
    private bool canUseSkill = false;

    public void ShootPointerDown(bool shoot = true)
    {
        if (Time.time >= tempTime)
        {
            Instantiate(fireShoot, gun.transform.position, gun.transform.rotation);
            GameObject projectitle = Instantiate(bullet, gun.transform.position, gun.transform.rotation);
            Rigidbody projectileRb = projectitle.GetComponent<Rigidbody>();
            Vector3 forceToAdd = gun.transform.forward * throwForce + transform.up * throwUpwardForce;
            projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
            tempTime = 1 / fireRate + Time.time;
        }
    }
}
