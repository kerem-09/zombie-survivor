using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public AudioSource menuMusic; // Menüde sürekli çalan fon müziði
    public AudioSource startSound; // image_69ab8c.png dosyasýndaki 'start' sesi

    public void PlayGame()
    {
        // Direkt sahne yüklemek yerine sýrayý (Coroutine) baþlatýyoruz
        StartCoroutine(PlayStartSequence());
    }

    IEnumerator PlayStartSequence()
    {
        // 1. Arkaplan müziðini sustur
        if (menuMusic != null) menuMusic.Stop();

        // 2. 'Start' sesini çal
        if (startSound != null) startSound.Play();

        // 3. 2 saniye bekle (Sesin duyulmasý için)
        // 'Realtime' kullanýyoruz ki Time.timeScale'den etkilenmesin
        yield return new WaitForSecondsRealtime(2f);

        // 4. Oyunu baþlat
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}