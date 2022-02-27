using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBolts : MonoBehaviour
{
    public float delayBetweenShots;
    public float delayUntilTheBallistaCompleteItsShot;
    public GameObject boltPrefab;
    public float range;

    private float timeOfLastShot;
    private Animator animator;
    private bool isReloading;

    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        timeOfLastShot = Time.time;
        isReloading = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameStateManager.gamePaused)
        {
            float currentTime = Time.time;
            float timeSinceLastShot = currentTime - timeOfLastShot;
            if (timeSinceLastShot >= delayBetweenShots && CheckIfEnemyInRange())
            {
                RotateTowardTarget();
                ArmingBallista();
            }
            else if (timeSinceLastShot >= delayUntilTheBallistaCompleteItsShot && !isReloading && CheckIfEnemyInRange())
            {
                Shoot();
            }
        }
    }

    private void ArmingBallista()
    {
        animator.SetTrigger("Firing");
        timeOfLastShot = Time.time;
        isReloading = false;
    }

    private void Shoot()
    {
        Instantiate(boltPrefab, transform);
        isReloading = true;
    }

    //position souris - position balliste
    private void RotateTowardTarget()
    {
        Vector3 offset = new Vector3 (0, 0, 90);
        if (CheckIfEnemyInRange())
        {
            target = FindClosestEnemy();
        }
        transform.rotation = Quaternion.LookRotation(Vector3.forward, target.transform.position - transform.position + offset);
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    public bool CheckIfEnemyInRange()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;

        if(gos.Length > 0)
        {
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            foreach (GameObject go in gos)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }

            float realDistance = (float) System.Math.Sqrt(distance);

            if (realDistance <= range)
            {
                return true;
            }
        } 

        return false;
    }
}
