using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private int points = 10;
    private GameManager gameManager;
    private UIManager uiManager;
    private Animator anim;
    private AudioSource audioSource;
    private float fireRate = 2.5f;
    private float canFire;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(gameManager == null)
        {
            Debug.LogError("Enemy: GameManager is NULL");
        }

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(uiManager == null)
        {
            Debug.LogError("Enemy: UIManager is NULL");
        }

        anim = GetComponent<Animator>();
        if(anim == null)
        {
            Debug.LogError("Enemy: Animator is NULL");
        }

        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            Debug.LogError("Enemy: AudioSource is NULL");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if(!gameManager.IsGameOver())
        {
            CalculateMovement();

            if(Time.time > canFire)
            {
                GameObject enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
                
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
                foreach(var laser in lasers)
                {
                    laser.AssignEnemyLaser();
                }

                fireRate = UnityEngine.Random.Range(5f, 7.5f);
                canFire = Time.time + fireRate;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Player temp = collision.GetComponent<Player>();
            if(temp != null)
            {
                temp.Damage();
            }

            anim.SetTrigger("OnEnemyDeath");
            Destroy(this.GetComponent<BoxCollider2D>());
            audioSource.Play();
            speed = 0;
            
            Destroy(this.gameObject, 2.75f);
        }
        else if(collision.tag == "Laser")
        {
            Laser laser = collision.GetComponent<Laser>();
            if(laser != null)
            {
                if(!laser.IsEnemyLaser())
                {
                    if(uiManager != null)
                    {
                        uiManager.UpdateScore(points);
                    }

                    anim.SetTrigger("OnEnemyDeath");
                    Destroy(this.GetComponent<BoxCollider2D>());
                    audioSource.Play();
                    speed = 0;

                    Destroy(collision.gameObject);
                    Destroy(this.gameObject, 2.75f);
                }
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if(transform.position.y <= -5.5f)
        {
            transform.position = new Vector3(UnityEngine.Random.Range(-9.5f, 9.5f), 7, 0);
        }
    }
}
