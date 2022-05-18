using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isGameOver { get; set; }
    public bool isGamePaused { get; set; }

    private Animator _pauseAnimator;
    [SerializeField]
    public bool isCoopMode;

    private void Start()
    {
        isGameOver = false;
        isGamePaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        _pauseAnimator = GameObject.Find("PauseMenu").GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        _pauseAnimator.SetBool("isGamePaused", false);
    }

    private void Update()
    {
        if (isGameOver == true)
        {
            if (Input.GetKeyDown(KeyCode.R) && isCoopMode)
            {
                // Reload multiplayer game scene.
                SceneManager.LoadSceneAsync(3);
            }

            else if (Input.GetKeyDown(KeyCode.R) && !isCoopMode)
            {
                // Reload single player game scene.
                SceneManager.LoadSceneAsync(2);
            }

            else if (Input.GetKeyDown(KeyCode.N))
            {
                // Reload the menu scene.
                SceneManager.LoadSceneAsync(1);
            }
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    public void GameOver()
    {
        isGameOver = true;

    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        isGamePaused = false;
        _pauseAnimator.SetBool("isGamePaused", false);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        isGamePaused = true;
        _pauseAnimator.SetBool("isGamePaused", true);
    }
}