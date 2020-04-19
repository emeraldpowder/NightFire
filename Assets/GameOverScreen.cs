using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text CaptionText;

    private void OnEnable()
    {
        MainMenu.IsGameStarted = false;
        
        int minutes = (int)Time.timeSinceLevelLoad / 60;
        int seconds = (int)Time.timeSinceLevelLoad % 60;

        string message;
        if (minutes <= 2) message = "Could've been better";
        else if (minutes <= 10) message = "Not bad at all";
        else message = "That's impressive!";
        
        CaptionText.text =
            $"Fire is faded out. Game over.\n\nYou held up for <color=#E7834F>{minutes}</color> minutes <color=#E7834F>{seconds}</color> seconds.\n{message}";
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
