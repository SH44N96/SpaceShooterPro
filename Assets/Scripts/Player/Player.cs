using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject tripleShotPrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject shieldVisualizer;
    [SerializeField] private GameObject rightEngine, leftEngine;
    [SerializeField] private AudioClip laserSoundClip;
    [SerializeField] private float speed = 5;
    [SerializeField] private float fireRate = 0.25f;
    [SerializeField] private int lives = 3;
    [SerializeField] private bool isPlayerOne;
    [SerializeField] private bool isPlayerTwo;
    [SerializeField] private bool isTripleShotActive;
    [SerializeField] private bool isShieldActive;
    private GameManager gameManager;
    private UIManager uiManager;
    private float canFire;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(gameManager == null)
        {
            Debug.LogError("Player: GameManage is NULL");
        }

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(uiManager == null)
        {
            Debug.LogError("Player: UIManager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(isPlayerOne)
        {
            if(Input.GetKeyDown(KeyCode.Space) && (Time.time >= canFire))
            {
                FireLaser();
            }
        }
        else if(isPlayerTwo)
        {
            if(Input.GetKeyDown(KeyCode.Return) && (Time.time >= canFire))
            {
                FireLaser();
            }
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = 0;
        float verticalInput = 0;

        if(gameManager.GetCoOpMode())
        {
            if(isPlayerOne)
            {
                horizontalInput = Input.GetAxis("Horizontal1");
                verticalInput = Input.GetAxis("Vertical1");
            }
            else if(isPlayerTwo)
            {
                horizontalInput = Input.GetAxis("Horizontal2");
                verticalInput = Input.GetAxis("Vertical2");
            }
        }
        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * speed * Time.deltaTime);
        
        if(transform.position.x >= 11.25)
        {
            transform.position = new Vector3(-11.25f, transform.position.y, 0);
        }
        else if(transform.position.x <= -11.25)
        {
            transform.position = new Vector3(11.25f, transform.position.y, 0);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4, 0), 0);
    }

    void FireLaser()
    {
        if(!isTripleShotActive)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, 1.05f, 0);
            Instantiate(laserPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Instantiate(tripleShotPrefab, transform.position, Quaternion.identity);
        }

        canFire = Time.time + fireRate;
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);

        isTripleShotActive = false;
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);

        speed /= 2;
    }

    public void Damage()
    {
        if(isShieldActive)
        {
            isShieldActive = false;
            shieldVisualizer.SetActive(false);

            return;
        }

        lives--;

        if(isPlayerOne)
        {
            uiManager.UpdateLives(lives, 1);

        }
        else if(isPlayerTwo)
        {
            uiManager.UpdateLives(lives, 2);
        }

        if(lives == 2)
        {
            rightEngine.SetActive(true);
        }
        else if(lives == 1)
        {
            leftEngine.SetActive(true);
        }
        else if(lives <= 0)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        isTripleShotActive = true;

        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedBoostActive()
    {
        speed *= 2;

        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public void ShieldActive()
    {
        isShieldActive = true;

        shieldVisualizer.SetActive(true);
    }

    public bool IsPlayerOne()
    {
        return isPlayerOne;
    }
}
