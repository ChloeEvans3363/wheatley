using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageScenes : MonoBehaviour
{
    private static ManageScenes _instance;

    public static ManageScenes Instance { get { return _instance; } }

    [SerializeField] GameObject SearchFailedText;
    [SerializeField] GameObject SearchSucceededText;
    public bool needsToRestart = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameplayScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void DisplaySearchFailed()
    {
        SearchFailedText.SetActive(true);
        needsToRestart = true;
    }

    public void DisplaySuccess()
    {
        SearchSucceededText.SetActive(true);
        needsToRestart = true;
    }
}
