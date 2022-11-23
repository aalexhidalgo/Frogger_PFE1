using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_PlayerController : MonoBehaviour
{
    private float spaceLimits = 4f;
    private float distance = 1f; //Distance between steps
    private Vector3 nextPos;

    private bool isJumping;

    private Animator playerAnimator;
    private Rigidbody2D playerRigidbody;

    public bool active;

    private int lifeCounter = 3;

    private int stepScore = 5;

    private AH_GameManager GameManagerScript;

    void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        GameManagerScript = FindObjectOfType<AH_GameManager>();
    }

    void Update()
    {
        //Player Movement

        if (isJumping == false) //As a cooldown, we cannot press a key if we didn't arive to the new position
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                StartCoroutine(Movement());
                nextPos.y = transform.position.y + distance;
                transform.position = nextPos;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                GameManagerScript.UpdateScore(stepScore);
            }

            if (Input.GetAxisRaw("Vertical") < 0 && transform.position.y != -spaceLimits)
            {
                StartCoroutine(Movement());
                nextPos.y = transform.position.y - distance;
                transform.position = nextPos;
                transform.rotation = Quaternion.Euler(0, 0, -180);
            }

            if (Input.GetAxisRaw("Horizontal") > 0 && transform.position.x != spaceLimits)
            {
                StartCoroutine(Movement());
                nextPos.x = transform.position.x + distance;
                transform.position = nextPos;
                transform.rotation = Quaternion.Euler(0, 0, -90);
            }

            if (Input.GetAxisRaw("Horizontal") < 0 && transform.position.x != -spaceLimits)
            {
                StartCoroutine(Movement());
                nextPos.x = transform.position.x - distance;
                transform.position = nextPos;
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }

    }

    private void LateUpdate()
    {
        playerAnimator.SetBool("IsJumping", isJumping);
    }

    //Animation transition betweens steps
    private IEnumerator Movement()
    {
        active = true;
        isJumping = true;
        yield return new WaitForSeconds(0.25f);
        isJumping = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {           
            lifeCounter--;
            UpdateLife();

            //Blink effect

            if (lifeCounter <= 0)
            {
                lifeCounter = 0;
                GameManagerScript.GameOver();
            }
        }
    }

    private void UpdateLife()
    {
        GameManagerScript.lifeImage.sprite = GameManagerScript.lifeSpriteArray[lifeCounter];
    }
}
