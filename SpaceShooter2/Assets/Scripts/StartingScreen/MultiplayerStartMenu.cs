using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerStartMenu : MonoBehaviour
{
    public void LoadMultiPlayerScene()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void LoadSinglePlayerScene()
    {
        SceneManager.LoadSceneAsync(2);
    }
}