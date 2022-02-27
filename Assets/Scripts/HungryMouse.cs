using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungryMouse : MonoBehaviour {

    private GameObject healthBar;
    private Image healthBarFill;
    private float minRangeToWayPoint;

    [SerializeField]
    private float offset;

    [SerializeField]
    private GameObject healthBarPrefab;

    [SerializeField]
    private float hitpoints;

    private float maxHitpoints;

    [SerializeField]
    private float speed;
    private SingleWaypoint nextWayPoint;

    // Start is called before the first frame update
    void Start() {
        minRangeToWayPoint = Random.Range(0.001f, 0.1f);
        maxHitpoints = hitpoints;
        CreateHealthBars(GameObject.Find("GamePanel").transform);
        RotateTowardTarget();
    }

    // Update is called once per frame
    void Update() {
        MoveToTarget();
        UpdateHealthBars();
    }

    public void SetInitialPosition(SingleWaypoint wayPoint) {
        nextWayPoint = wayPoint.nextWaypoint;
        transform.position = wayPoint.positionXYZ;
    }

    public void InitializeHitPoints(int hp) {
        hitpoints = hp;
        maxHitpoints = hp;
    }

    private void MoveToTarget() {
        if (!GameStateManager.gamePaused) {
            if (nextWayPoint != null) {
                if (Vector3.Distance(transform.position, nextWayPoint.positionXYZ) < minRangeToWayPoint) {
                    nextWayPoint = nextWayPoint.nextWaypoint;
                    RotateTowardTarget();
                } else {
                    transform.position = Vector3.MoveTowards(transform.position, nextWayPoint.positionXYZ, speed * Time.deltaTime);
                }
            } else {
                Cheese.instance.takeDamage(1);
                Die(false);
            }
        }
    }

    private void RotateTowardTarget() {
        if (nextWayPoint != null) {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, nextWayPoint.positionXYZ - transform.position);    
        }
	}

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Projectile")) {
            TakeHit();
		}
    }

    private void TakeHit() {
        hitpoints -= 1;
        float amount = hitpoints / maxHitpoints;

        healthBarFill.fillAmount = amount;

        if (hitpoints <= 0) {
            Die(true);
		}
	}

    public void Die(bool killedByPlayer) {
        if (killedByPlayer && TowerMaker.instance) {
            TowerMaker.instance.AddMoney();
        }
        DestroyHealthBars();
        Destroy(gameObject);
    }

    public void CreateHealthBars(Transform gamePanel)
    {
        healthBar = Instantiate(healthBarPrefab, gamePanel);
        healthBarFill = healthBar.transform.Find("HealthBar").Find("HealthBarGreen").GetComponent<Image>();
    }
    private void UpdateHealthBars()
    {
        healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * offset;
    }
    private void DestroyHealthBars()
    {
        Destroy(healthBar);
    }

}
