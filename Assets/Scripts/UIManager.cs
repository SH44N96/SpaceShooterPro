using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public bool resetHighScore;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pauseGamePanel;
    [SerializeField] private Sprite[] liveSprites;
    [SerializeField] private Image livesImage;
    [SerializeField] private Image livesImage2;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text bestText;
    [SerializeField] private Text gameOverText;
    private Animator pauseAnimator;
    private GameManager gameManager;
    private int currentScore;
    private int bestScore;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(gameManager == null)
        {
            Debug.LogError("UIManager: GameManager is NULL");
        }

        pauseAnimator = pauseGamePanel.GetComponent<Animator>();
        if(pauseAnimator == null)
        {
            Debug.LogError("UIManager: PauseAnimator is NULL");
        }

        pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;

        scoreText.text = "Score: 0";

        if(resetHighScore)
        {
            PlayerPrefs.DeleteAll();
        }

        if(!gameManager.GetCoOpMode())
        {
            bestScore = PlayerPrefs.GetInt("HighScore");
        }
        bestText.text = "Best: " + bestScore;

        gameOverPanel.SetActive(false);
        pauseGamePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GameOverSequence()
    {
        gameOverPanel.SetActive(true);

        gameManager.GameOver();

        StartCoroutine(GameOverFlickerRoutine());
    }

    void CheckForBestScore()
    {
        if(currentScore >= bestScore)
        {
            bestScore = currentScore;

            PlayerPrefs.SetInt("HighScore", bestScore);

            bestText.text = "Best: " + bestScore;
        }
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void UpdateScore(int points)
    {
        currentScore += points;
        scoreText.text = "Score: " + currentScore;

        if(!gameManager.GetCoOpMode())
        {
            CheckForBestScore();
        }
    }

    public void UpdateLives(int currentLives, int player)
    {
        if(currentLives <= 0)
        {
            currentLives = 0;
        }
        
        if(player == 1)
        {
            livesImage.sprite = liveSprites[currentLives];
        }
        else if(player == 2)
        {
            livesImage2.sprite = liveSprites[currentLives];
        }

        if(!gameManager.GetCoOpMode())
        {
            if(livesImage.sprite.name == "no_lives")
            {
                GameOverSequence();
            }
        }
        else
        {
            if(livesImage.sprite.name == "no_lives" && livesImage2.sprite.name == "no_lives")
            {
                GameOverSequence();
            }
        }
    }

    public void PauseGame()
    {
        pauseGamePanel.SetActive(true);

        pauseAnimator.SetBool("IsPaused", true);
    }

    public void ResumePlay()
    {
        pauseGamePanel.SetActive(false);
        pauseAnimator.SetBool("IsPaused", false);

        Time.timeScale = 1;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        
        SceneManager.LoadScene("MainMenu");
    }
}
