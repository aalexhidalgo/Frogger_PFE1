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
    private SpriteRenderer playerRenderer;

    public bool active;
    private bool canMove = true;

    private int lifeCounter = 3;

    private int stepScore = 5;

    public bool isOnPlatform, isOnWater;

    public GameObject bombDeathPrefab;

    private AH_GameManager GameManagerScript;
    private AH_TurtleAnim TurtleAnimScript;

    void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
        GameManagerScript = FindObjectOfType<AH_GameManager>();
        TurtleAnimScript = FindObjectOfType<AH_TurtleAnim>();
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

    //Trigger: Platforms
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            lifeCounter--;
            UpdateLife();

            //Blink effect o otro (partículas)
            //Meter corrutina menos a la hora de valer 0

            if (lifeCounter <= 0)
            {
                lifeCounter = 0;
                StartCoroutine(Death_1());
            }
        }

        if (other.gameObject.CompareTag("Water")) //GAMEOVER instantly
        {
            isOnWater = true;
            if (isOnWater == true && isOnPlatform == false) // (NOT) Arreglo temporal muerte instantáneas
            {
                lifeCounter = 0;
                UpdateLife();
            }
        }
        if (other.gameObject.CompareTag("Platform")) //In Tree Platform
        {
            isOnPlatform = true;
            transform.parent = other.transform;
        }
        if(other.gameObject.CompareTag("Turtle")) //Funciona :D
        {
            if (TurtleAnimScript.underWater == true)
            {
                Debug.Log("Hola");
                lifeCounter--;
                UpdateLife();

                if (lifeCounter <= 0)
                {
                    lifeCounter = 0;
                    StartCoroutine(Death_1()); //GAMEOVER instantly
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            isOnWater = false;
        }
        if (other.gameObject.CompareTag("Platform")) //Outside Tree Platform
        {
            isOnPlatform = false;
            transform.parent = null;
        }
    }

    private void UpdateLife()
    {
        GameManagerScript.lifeImage.sprite = GameManagerScript.lifeSpriteArray[lifeCounter];
    }

    private IEnumerator Death_1()
    {
        canMove = false;
        Color color = playerRenderer.color;
        color = new Color(color.r, color.g, color.b, 0);
        playerRenderer.color = color;
        Instantiate(bombDeathPrefab, transform.position, bombDeathPrefab.transform.rotation);
        yield return new WaitForSeconds(1.5f);
        GameManagerScript.GameOver();
    }

}
