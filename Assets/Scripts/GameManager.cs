using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float worldSpeed;

    void Awake(){
        if (Instance != null){
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Fire3")){
            Pause();
        }
    }

    public void Pause(){
        if (UIController.Instance.pausePanel.activeSelf == false){
            UIController.Instance.pausePanel.SetActive(true);
            Time.timeScale = 0f;
            AudioManager.Instance.PlaySound(AudioManager.Instance.pause);
        } else {
            UIController.Instance.pausePanel.SetActive(false);
            Time.timeScale = 1f;
            PlayerController.Instance.ExitBoost();
            AudioManager.Instance.PlaySound(AudioManager.Instance.unpause);
        }
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void GoToMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver(){
        StartCoroutine(ShowGameOverScreen());
    }

    IEnumerator ShowGameOverScreen(){
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("GameOver");
    }
}
