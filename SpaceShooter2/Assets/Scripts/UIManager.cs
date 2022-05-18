using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField]
    private Text _gameScore;
    [SerializeField]
    private Sprite[] _playerLivesSprites;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Image _pausePanel;
    [SerializeField]
    private Button _resumeButton;
    [SerializeField]
    private Button _backToMainButton;
    [SerializeField]
    private Text _bestScoreUI;
    public int bestScore;

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        _bestScoreUI.text = "Best score: " + bestScore;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _pausePanel.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !_gameManager.isGameOver)
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        if (_gameManager.isGamePaused)
        {
            _pausePanel.gameObject.SetActive(false);
            _gameManager.ResumeGame();
        }
    }

    public void BackToMain()
    {
        if (_gameManager.isGamePaused)
        {
            _pausePanel.gameObject.SetActive(false);
            _gameManager.ResumeGame();
            SceneManager.LoadSceneAsync(0);
        }
    }

    private void PauseGame()
    {
        _pausePanel.gameObject.SetActive(true);
        _gameManager.PauseGame();
    }

    public void UpdatePlayerScore(int score)
    {
        _gameScore.text = "Score: " + score.ToString();
    }

    public void CheckBestScore(int score)
    {
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            _bestScoreUI.text = "Best score: " + score.ToString();
        }
    }
    

    public void UpdatePlayerLivesImage(int currentLives)
    {
        _livesImage.sprite = _playerLivesSprites[currentLives];

        if(currentLives == 0)
        {
            DisplayGameOverSequence();
            _gameManager.GameOver();
        }
    }

    void DisplayGameOverSequence()
    {
        StartCoroutine(GameOverCounter());
    }

    IEnumerator GameOverCounter()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _restartText.gameObject.SetActive(true);
        }
    }
 }
