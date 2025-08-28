using System;
using System.Collections;
using System.Collections.Generic;
using Terresquall;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public VirtualJoystick moveJoystick;
    public Transform aimPoint;
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;
    public float fireInterval = 3f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float targetingRange = 30f;

    private Rigidbody rb;
    private float fireTimer;
    public static Action<float> OnFuelChange;
    public static Action OnInitialize;
    public ParticleSystem muzzleFlash;
    public AudioSource playerSource;
    public AudioClip[] shootSounds;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        OnInitialize?.Invoke();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    void Update()
    {
        AutoAimAndShoot();
    }

    void HandleMovement()
    {
        Vector2 moveInput = moveJoystick.GetAxis();
        float throttle = moveInput.y;
        float steering = moveInput.x;

        Vector3 move = transform.forward * throttle * moveSpeed;
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);

        Quaternion turn = Quaternion.Euler(0f, steering * rotationSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turn);
    }

    void AutoAimAndShoot()
    {
        GameObject target = FindClosestEnemy();
        if (target == null) return;

        Vector3 directionToTarget = (target.transform.position - aimPoint.position).normalized;
        directionToTarget.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        aimPoint.rotation = Quaternion.RotateTowards(aimPoint.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireInterval;
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance && distance <= targetingRange)
            {
                minDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }

    void Shoot()
    {
        muzzleFlash.Play();
        AudioManager.Instance.PlaySound(playerSource, shootSounds[UnityEngine.Random.Range(0, shootSounds.Length)], true);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().SetTarget(FindClosestEnemy().transform);
    }
}