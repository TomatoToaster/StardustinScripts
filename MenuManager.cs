using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public int playSceneIndex = 1;
    public int tutorialSceneIndex = 2;

    void Start()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene(playSceneIndex);
    }
    public void StartTutorial()
    {
        SceneManager.LoadScene(tutorialSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
