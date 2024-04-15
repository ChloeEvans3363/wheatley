using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageScenes : MonoBehaviour
{
    private static ManageScenes _instance;

    public static ManageScenes Instance { get { return _instance; } }

    [SerializeField] GameObject SearchFailedText;
    [SerializeField] GameObject SearchSucceededText;
    public bool needsToRestart = false;
    [SerializeField] GameObject pushBlockCounter;
    [SerializeField] GameObject immovableBlockCounter;

    int pushBlockCount = 0;
    int immovableBlockCount = 0;

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

    public void SetNumPushBlocks(int num)
    {
        pushBlockCount = num;
        if(num == 1)
        {
            pushBlockCounter.GetComponent<TextMeshProUGUI>().text = num.ToString() + " Block";
            return;
        }
        pushBlockCounter.GetComponent<TextMeshProUGUI>().text = num.ToString() + " Blocks";
    }

    public void SetNumImmovableBlocks(int num)
    {
        immovableBlockCount = num;
        if (num == 1)
        {
            immovableBlockCounter.GetComponent<TextMeshProUGUI>().text = num.ToString() + " Block";
            return;
        }
        immovableBlockCounter.GetComponent<TextMeshProUGUI>().text = num.ToString() + " Blocks";
    }

    public void UpdateNumPushBlocks(int offset)
    {
        pushBlockCount += offset;

        if (pushBlockCount == 1)
        {
            pushBlockCounter.GetComponent<TextMeshProUGUI>().text = pushBlockCount.ToString() + " Block";
            return;
        }
        pushBlockCounter.GetComponent<TextMeshProUGUI>().text = pushBlockCount.ToString() + " Blocks";
    }

    public void UpdateNumImmovableBlocks(int offset)
    {
        immovableBlockCount += offset;

        if (immovableBlockCount == 1)
        {
            immovableBlockCounter.GetComponent<TextMeshProUGUI>().text = immovableBlockCount.ToString() + " Block";
            return;
        }
        immovableBlockCounter.GetComponent<TextMeshProUGUI>().text = immovableBlockCount.ToString() + " Blocks";
    }
}
