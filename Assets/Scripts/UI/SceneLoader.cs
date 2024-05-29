using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    

    public void LoadSceneMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSceneTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Map");
    }
}
