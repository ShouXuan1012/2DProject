using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class ShootController : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;    
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate;

    private Animator animator;
    private float nextFireTime;
    
    void Start()
    {   
        animator = GetComponent<Animator>();
        Managers.Pool.CreatePool(bulletPrefab, 20, 40);
    }

    void Update()
    {
        if (Managers.Input.AttackPressed && Time.time >= nextFireTime)
        {
            animator.SetTrigger("IsShooted");
            nextFireTime = Time.time + fireRate;
            Fire();
        }      
    }

    private void Fire()
    {
        Bullet bullet = Managers.Pool.GetFromPool(bulletPrefab);
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.identity;
        float dir = transform.localScale.x > 0 ? 1f : -1f;
        bullet.SetDirection(dir);
    }
}
