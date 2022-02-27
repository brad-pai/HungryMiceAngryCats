using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyCreator : MonoBehaviour {

    public static EnemyCreator instance { get; private set; }

    public GameObject mousePrefab;
    public float spawnDelay;
    public float spawnAmount;
    public AudioSource mouseSqueak;

    [SerializeField]
    private float scaleMultiplier;

    private float elapsedTime;

    private TextMeshProUGUI waveIndexText;
    private int waveIndex = 1;
    private int waveIndexAtStart;
    private int defaultHitPoints = 3;
    private int defaultHitPointAtStart;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    void Start() {
        waveIndexText = GameObject.Find("WaveText").GetComponent<TextMeshProUGUI>();
        waveIndexAtStart = waveIndex;
        defaultHitPointAtStart = defaultHitPoints;
    }

	private void UpdateWaveIndexText() {
        waveIndexText.text = "WAVE " + waveIndex.ToString();
    }

	IEnumerator SpawnEnemies() {
        while (!GameStateManager.gamePaused) {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > spawnDelay) {
                float interval = 5f/spawnAmount;
                mouseSqueak.Play();
                for (int i = 0; i < spawnAmount; i++) {
                    GameObject go = Instantiate(mousePrefab, new Vector3(Random.Range(0f, 10.0f), Random.Range(-5.0f, 5.0f), 0), Quaternion.identity, transform);
                    go.GetComponent<HungryMouse>().SetInitialPosition(Waypoints.instance.FirstWayPoint());
                    go.GetComponent<HungryMouse>().InitializeHitPoints(defaultHitPoints + (waveIndex/3));
                    if (Random.Range(0f, 100f) < 5) {
                        go.transform.localScale = new Vector3(1.5f * scaleMultiplier, 1.5f * scaleMultiplier, 1f);
                        go.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
                    } else {
                        go.transform.localScale = new Vector3(Random.Range(0.5f, 1.5f) * scaleMultiplier, Random.Range(0.5f, 1.5f) * scaleMultiplier, 1f);                            
                    }
                    yield return new WaitForSeconds(interval);
                }
                mouseSqueak.Stop();
                elapsedTime = 5f;
                spawnAmount += 1f;
                waveIndex += 1;
                UpdateWaveIndexText();
            }
            yield return null;
        }
    }

    public void RestartEnemyCreator() {
        waveIndex = waveIndexAtStart;
        defaultHitPoints = defaultHitPointAtStart;
        UpdateWaveIndexText();
        elapsedTime = 0f;
        for (int i = transform.childCount - 1; i >= 0; i--) {
            transform.GetChild(i).GetComponent<HungryMouse>().Die(false);
		}
        StartCoroutine(SpawnEnemies());
    }
}
