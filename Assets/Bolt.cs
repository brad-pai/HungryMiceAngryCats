using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    private GameObject boltShooter;
    private GameObject target;
    public AudioSource mouseSlash;

    public float speed = 10f;
    public float launchHeight = 1;
    public float launchLength = 1;

    public Vector3 movePosition;

    private float boltX;
    private float boltY;
    private float nextX;
    private float nextY;
    private float baseX;
    private float baseY;
    private float targetX;
    private float targetY;
    private float height;
    private float length;

    private float distX;
    private float distY;

    private Vector3 targetPos;


    // Start is called before the first frame update
    void Start()
    {
        boltShooter = this.gameObject.transform.parent.gameObject;
        mouseSlash = GameObject.Find("MouseSlash").GetComponent<AudioSource>();
        target = FindClosestEnemy();

        boltX = boltShooter.transform.position.x;
        boltY = boltShooter.transform.position.y;

        targetX = target.transform.position.x;
        targetY = target.transform.position.y;

        targetPos = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameStateManager.gamePaused)
        {
            distX = targetX - boltX;
            distY = targetY - boltY;

            if (Math.Abs(distX) > Math.Abs(distY))
            {
                shootHorizontally();
            }
            else
            {
                shootVertically();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        mouseSlash.Play();
        Destroy(gameObject);
    }


    private void shootVertically()
    {
        nextY = Mathf.MoveTowards(transform.position.y, targetY, speed * Time.deltaTime);
        baseX = Mathf.Lerp(boltShooter.transform.position.x, targetX, (nextY - boltY) / distY);
        length = launchLength * (nextY - boltY) * (nextY - targetY) / (-0.25f * distY * distY);

        movePosition = new Vector3(baseX + length, nextY, transform.position.z);

        transform.rotation = LookAtTarget(movePosition - transform.position);
        transform.position = movePosition;

        if (movePosition == targetPos)
        {
            Destroy(gameObject);
        }
    }

    private void shootHorizontally()
    {
        nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        baseY = Mathf.Lerp(boltShooter.transform.position.y, targetY, (nextX - boltX) / distX);
        height = launchHeight * (nextX - boltX) * (nextX - targetX) / (-0.25f * distX * distX);

        movePosition = new Vector3(nextX, baseY + height, transform.position.z);

        transform.rotation = LookAtTarget(movePosition - transform.position);
        transform.position = movePosition;

        if (movePosition == targetPos)
        {
            Destroy(gameObject);
        }
    }

    public static Quaternion LookAtTarget(Vector2 r)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
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
}
