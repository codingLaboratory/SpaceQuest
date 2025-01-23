using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
    }

    public void NewGame(){
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame(){
        Application.Quit();
    }

}
