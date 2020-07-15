using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip clip;
    [SerializeField] private float speed = 7.5f;
    private bool isEnemyLaser;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 audioLocation = new Vector3(transform.position.x, transform.position.y, -10);
        AudioSource.PlayClipAtPoint(clip, audioLocation);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isEnemyLaser)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && isEnemyLaser)
        {
            Player player = collision.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
            
            Vector3 explosionLocation = transform.position + new Vector3(0, 1.5f, 0);
            Instantiate(explosionPrefab, explosionLocation, Quaternion.identity);

            Destroy(this.gameObject);
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if(transform.position.y >= 7)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if(transform.position.y <= -5.5)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public bool IsEnemyLaser()
    {
        return isEnemyLaser;
    }
    
    public void AssignEnemyLaser()
    {
        isEnemyLaser = true;
    }
}
