using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cheese : MonoBehaviour {

    public static Cheese instance { get; private set; }

    public float currentLife;
    public float maxLife;
    private bool firstThirdReached = false;
    private bool secondThirdReached = false;
    private GameObject goGameOverPanel;
    private TextMeshProUGUI lifeRemainingText;

    [SerializeField]
    private Sprite cheeseFirstThird;
    [SerializeField]
    private Sprite cheeseSecondThird;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    void Start() {
        goGameOverPanel = GameObject.Find("GameOverPanel");
        lifeRemainingText = GameObject.Find("LifeRemainingText").GetComponent<TextMeshProUGUI>();
        lifeRemainingText.text = currentLife.ToString();
    }

    void Update() {

    }

    public void takeDamage(float damage) {
        currentLife -= damage;
        if (currentLife < (maxLife*0.66f) && !firstThirdReached) {
            firstThirdReached = true;
            GetComponent<SpriteRenderer>().sprite = cheeseFirstThird;

        } 
        if (currentLife < (maxLife*0.33f) && !secondThirdReached) {
            secondThirdReached = true;
            GetComponent<SpriteRenderer>().sprite = cheeseSecondThird;
        }
        if (currentLife <= 0) {
            currentLife = 0;
            GameStateManager.gamePaused = true;
            goGameOverPanel.SetActive(true);
        }
        
        lifeRemainingText.text = currentLife.ToString();
    }

    public void RestartCheese() {
        currentLife = maxLife;
        firstThirdReached = false;
        secondThirdReached = false;
        lifeRemainingText.text = currentLife.ToString();
    }
}
