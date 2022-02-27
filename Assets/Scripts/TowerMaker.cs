using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerMaker : MonoBehaviour {

    public static TowerMaker instance { get; private set; }

    [SerializeField]
    private int moneyForVictory;

    private int towerPrice = 1;
    private int towerPriveAtStart;

    private int money = 3;
    private int moneyAtStart;
    [SerializeField]
    private GameObject balistPrefab;
    private TextMeshProUGUI moneyText;
    private TextMeshProUGUI towerPriceText;

    private AudioSource meowAudioSource;
    private AudioSource angryMeowAudioSource;
    private GameObject goWinPanel;
    private GameObject goGamePanel;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start() {
        moneyAtStart = money;
        towerPriveAtStart = towerPrice;
        moneyText = GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>();
        towerPriceText = GameObject.Find("TowerCostText").GetComponent<TextMeshProUGUI>();
        updateText();
        meowAudioSource = GameObject.Find("CatMeow").GetComponent<AudioSource>();
        angryMeowAudioSource = GameObject.Find("AngryCatMeow").GetComponent<AudioSource>();
        goWinPanel = GameObject.Find("WinPanel");
        goGamePanel = GameObject.Find("GamePanel");
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            PlaceTower(Input.mousePosition);
		}
    }

    public void AddMoney() {
        money += 1;
        updateText();
        if (money >= moneyForVictory) {
            GameStateManager.gamePaused = true;
            goWinPanel.SetActive(true);
            goGamePanel.SetActive(false);
		}
	}

    private void PlaceTower(Vector3 towerPosition) {
        if (money >= towerPrice && !GameStateManager.gamePaused) {
            Cellule cell = Grille.instance.getCellAtPosition(towerPosition);
            if (cell != null && !cell.hasTower) {
                Instantiate(balistPrefab, cell.positionXYZ, Quaternion.identity, transform);
                cell.hasTower = true;
                money -= towerPrice;
                towerPrice += 1;
                updateText();
                meowAudioSource.Play();

            }
        } else if (!GameStateManager.gamePaused)
        {
            angryMeowAudioSource.Play();
            Cellule cell = Grille.instance.getCellAtPosition(towerPosition);
            if (cell != null) {
                TweenManager.instance.TweenScale(cell.go.transform, TweenManager.instance.tweenScaleDefault, null);
			}
        }
	}

    private void updateText() {
        moneyText.text = "$ " + (money.ToString());
        towerPriceText.text = "COST $" + towerPrice.ToString();
    }

    public void RestardTowerMaker() {
        money = moneyAtStart;
        towerPrice = towerPriveAtStart;
        updateText();
        for (int i = transform.childCount - 1; i >= 0; i--) {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
