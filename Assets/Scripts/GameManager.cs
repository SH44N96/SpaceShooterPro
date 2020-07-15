using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool isCoOpMode;
    [SerializeField] private bool isGameOver;
    private SpawnManager spawnManager;
    private UIManager uIManager;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if(spawnManager == null)
        {
            Debug.LogError("GameManager: SpawnManager is NULL");
        }

        uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(uIManager == null)
        {
            Debug.LogError("GameManager: UIManager is NULL");
        }

        if(isCoOpMode)
        {
            spawnManager.StartSpawning();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameOver)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                if(!isCoOpMode)
                {
                    SceneManager.LoadScene("SinglePlayer");
                }
                else
                {
                    SceneManager.LoadScene("CoOpMode");
                }
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                Time.timeScale = 0;

                uIManager.PauseGame();
            }
        }
    }

    public bool GetCoOpMode()
    {
        return isCoOpMode;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void GameOver()
    {
        isGameOver = true;

        spawnManager.OnPlayerDeath();
    }
}
