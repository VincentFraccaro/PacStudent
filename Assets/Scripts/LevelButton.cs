using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public void LoadLevel1()
    {
        SceneManager.LoadScene(sceneBuildIndex: 0);
        print("Pressed");
    }

    public void QuitToStart()
    {
        SceneManager.LoadScene(sceneBuildIndex: 1);
    }
}
