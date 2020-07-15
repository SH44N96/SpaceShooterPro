using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float rotateSpeed = 20f;
    private SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if(spawnManager == null)
        {
            Debug.LogError("Asteroid: SpawnManager is NULL");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser")
        {
            Destroy(collision.gameObject);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.GetComponent<SpriteRenderer>(), 0.75f);

            spawnManager.StartSpawning();
            
            Destroy(this.gameObject, 2.75f);
        }
    }
}
