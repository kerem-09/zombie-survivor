using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // OYUNU BAÞLAT
    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    // OYUNDAN ÇIK
    public void QuitGame()
    {
        Application.Quit();
    }
}