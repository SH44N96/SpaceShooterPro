using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private int powerupID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if(transform.position.y <= -6)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();

            if(player != null)
            {
                switch(powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    default:
                        Debug.Log("Unrecognized Powerup");
                        break;
                }

                Vector3 audioLocation = new Vector3(transform.position.x, transform.position.y, -10);
                AudioSource.PlayClipAtPoint(clip, audioLocation);
            }

            Destroy(this.gameObject);
        }
    }
}
