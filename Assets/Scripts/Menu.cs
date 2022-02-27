using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    public Sprite soundOn;
    public Sprite soundOff;
    bool musicToggle = true;
    bool soundEffectToggle = true;
    private GameObject goStartMenuPanel;
    private GameObject goGameOverPanel;
    private GameObject goWinPanel;
    private GameObject goGamePanel;

    public void Start() {
        goStartMenuPanel= GameObject.Find("StartMenuPanel");
        goGameOverPanel = GameObject.Find("GameOverPanel");
        goWinPanel = GameObject.Find("WinPanel");
        goGamePanel = GameObject.Find("GamePanel");
    }

    public void StartGame() {
        goWinPanel.SetActive(false);
        goGameOverPanel.SetActive(false);
        goStartMenuPanel.SetActive(false);
        goGamePanel.SetActive(true);
        GameStateManager.gamePaused = false;
        TowerMaker.instance.RestardTowerMaker();
        EnemyCreator.instance.RestartEnemyCreator();
        Cheese.instance.RestartCheese();
        Grille.instance.RestartGrille();
    }

    public void LoadMenu() {
        // clear game
        goStartMenuPanel.SetActive(true);
        goGameOverPanel.SetActive(false);
        goWinPanel.SetActive(false);
        GameStateManager.gamePaused = true;
    }

    public void ExitGame() {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void ToggleMusic() {
        musicToggle = !musicToggle;
        soundEffectToggle = !soundEffectToggle;
        GameObject goMusic = GameObject.Find("BackgroundMusic");
        GameObject goToggleMusic = GameObject.Find("SoundToggleImage");

        Image musicToggleImage = goToggleMusic.GetComponent<Image>();
        AudioSource audio = goMusic.GetComponent<AudioSource>();
        if (musicToggle) {
            musicToggleImage.sprite = soundOn;
            audio.UnPause();
        } else {
            musicToggleImage.sprite = soundOff;
            audio.Pause();
        }
    }
}