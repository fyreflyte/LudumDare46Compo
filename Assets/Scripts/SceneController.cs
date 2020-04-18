using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public void LoadNewScene(int sceneNum)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNum);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
