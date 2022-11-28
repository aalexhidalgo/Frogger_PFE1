using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_PlayerController : MonoBehaviour
{
    private float spaceLimits = 4f;
    private float distance = 1f; //Distance between steps
    private Vector2 nextPos;
    private Vector2 InitialPos = new Vector2(0f, -4.5f);

    private bool isJumping;

    private Animator playerAnimator;
    private Rigidbody2D playerRigidbody;

    public bool active;

    private int lifeCounter = 3;

    private int stepScore = 5;

    private bool canMove = true;

    public bool isOnTree, isOnWater;

    private AH_GameManager GameManagerScript;

    void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        GameManagerScript = FindObjectOfType<AH_GameManager>();
    }

    void Start()
    {
        transform.position = InitialPos;
    }

    void Update()
    {
        //Player Movement

        if (isJumping == false && canMove) //As a cooldown, we cannot press a key if we didn't arive to the new position
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                StartCoroutine(Movement());
                nextPos = new Vector2(transform.position.x, transform.position.y + distance);
                transform.position = nextPos;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                GameManagerScript.UpdateScore(stepScore);
            }

            if (Input.GetAxisRaw("Vertical") < 0 && transform.position.y != -spaceLimits) //Temporal, no ha de ir hacia atrás
            {
                StartCoroutine(Movement());
                nextPos = new Vector2(transform.position.x, transform.position.y - distance);
                transform.position = nextPos;
                transform.rotation = Quaternion.Euler(0, 0, -180);
            }

            if (Input.GetAxisRaw("Horizontal") > 0 && transform.position.x != spaceLimits)
            {
                StartCoroutine(Movement());
                nextPos = new Vector2(transform.position.x + distance, transform.position.y);
                transform.position = nextPos;
                transform.rotation = Quaternion.Euler(0, 0, -90);
            }

            if (Input.GetAxisRaw("Horizontal") < 0 && transform.position.x != -spaceLimits)
            {
                StartCoroutine(Movement());
                nextPos = new Vector2(transform.position.x - distance, transform.position.y);
                transform.position = nextPos;
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }

        if (isOnWater == true && isOnTree == false) // Arreglo temporal muerte instantánea
        {
            lifeCounter = 0;
            UpdateLife();
            GameManagerScript.GameOver();
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

    //Colision y trigger
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {           
            lifeCounter--;
            UpdateLife();

            //Blink effect
            //Meter corrutina menos a la hora de valer 0

            if (lifeCounter <= 0)
            {
                lifeCounter = 0;
                GameManagerScript.GameOver();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water")) //GAMEOVER instantly
        {
            isOnWater = true;
        }
        if (other.gameObject.CompareTag("Tree")) //In Tree Platform
        {
            isOnTree = true;
            transform.parent = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            isOnWater = false;
        }
        if (other.gameObject.CompareTag("Tree")) //Outside Tree Platform
        {
            isOnTree = false;
            transform.parent = null;
        }
    }

    private void UpdateLife()
    {
        GameManagerScript.lifeImage.sprite = GameManagerScript.lifeSpriteArray[lifeCounter];
    }

    /*private IEnumerator()
    {
        //canMove = false;
        //hacer true la animación del alpha cuando se acabe la animación can Move vuelve a valer true
    }*/
}
