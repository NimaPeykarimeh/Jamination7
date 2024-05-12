using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartButton() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    public void ExitButton() => Application.Quit();
    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}