using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public int playSceneIndex = 1;

    public void StartGame()
    {
        SceneManager.LoadScene(playSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
