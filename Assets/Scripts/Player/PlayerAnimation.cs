using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Player player;
    private GameManager gameManager;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        if(player == null)
        {
            Debug.LogError("PlayerAnimation: Player is NULL");
        }

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(gameManager == null)
        {
            Debug.LogError("PlayerAnimation: GameManager is NULL");
        }

        animator = GetComponent<Animator>();
        if(animator == null)
        {
            Debug.LogError("PlayerAnimation: Animator is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameManager.GetCoOpMode())
        {
            PlayerOneAnimation();
            PlayerTwoAnimation();
        }
        else
        {
            if(player.IsPlayerOne())
            {
                PlayerOneAnimation();
            }
            else
            {
                PlayerTwoAnimation();
            }
        }
    }

    void PlayerOneAnimation()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            animator.SetBool("TurnLeft", true);
        }
        else if(Input.GetKeyUp(KeyCode.A))
        {
            animator.SetBool("TurnLeft", false);
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            animator.SetBool("TurnRight", true);
        }
        else if(Input.GetKeyUp(KeyCode.D))
        {
            animator.SetBool("TurnRight", false);
        }
    }

    void PlayerTwoAnimation()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            animator.SetBool("TurnLeft", true);
        }
        else if(Input.GetKeyUp(KeyCode.LeftArrow))
        {
            animator.SetBool("TurnLeft", false);
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            animator.SetBool("TurnRight", true);
        }
        else if(Input.GetKeyUp(KeyCode.RightArrow))
        {
            animator.SetBool("TurnRight", false);
        }
    }
}
