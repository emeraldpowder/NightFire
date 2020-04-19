using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text CaptionText;

    private void OnEnable()
    {
        MainMenu.IsGameStarted = false;
        Cursor.visible = true;

        float time = Firepit.TimePassed;
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        
        int best = PlayerPrefs.GetInt("BestTime", 0);
        if (time > best)
        {
            PlayerPrefs.SetInt("BestTime", (int) time);
        }

        string message;
        if (minutes <= 2) message = "Could've been better";
        else if (minutes <= 10) message = "Not bad at all";
        else message = "That's impressive!";
        
        CaptionText.text =
            $"Fire has faded out. Game over.\n\nYou held up for <color=#E7834F>{minutes}</color> minutes <color=#E7834F>{seconds}</color> seconds.\n{message}";
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) Restart();
    }

    public void Restart()
    {
        MainMenu.IsGameStarted = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
