using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AH_PlayerController : MonoBehaviour
{
    private float spaceLimits = 4f;
    private float distance = 1f; //Distance between steps
    private Vector2 nextPos, respawnPos, maxPos;
    private Vector2 InitialPos = new Vector2(0f, -5.5f);

    private bool isJumping, isInmortal;

    private Animator playerAnimator;
    private Rigidbody2D playerRigidbody;
    private SpriteRenderer playerRenderer;

    public bool active;
    private bool canMove = true;

    public int lifeCounter = 3;

    private int stepScore = 5;

    private bool isOnPlatform, isOnWater;
    private bool cooldown, attack;

    public GameObject bombDeathPrefab, particlePrefab, waterParticlePrefab; //ParticleSystem
    public GameObject mysteryBoxparticle;
    public Volume damage_PostProcess;
    private Vignette vg;

    public GameObject canvasNumber;

    private AH_GameManager GameManagerScript;
    private AH_TurtleAnim TurtleAnimScript;

    private int randIndx;

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
        maxPos = transform.position;
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

                if (maxPos.y < nextPos.y)
                {
                    GameManagerScript.UpdateScore(stepScore);
                }
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

        if (transform.position.x > spaceLimits || transform.position.x < -spaceLimits) //In case we have been on a tree and we touch a limit
        {
            GameManagerScript.GameOver();
        }
    }

    private void LateUpdate()
    {
        playerAnimator.SetBool("IsJumping", isJumping);
        playerAnimator.SetBool("IsInmortal", isInmortal);
    }

    //Animation transition betweens steps
    private IEnumerator Movement()
    {
        active = true;
        isJumping = true;
        yield return new WaitForSeconds(0.25f);
        isJumping = false;
    }

    #region Trigger System
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            maxPos = transform.position;

            if(isInmortal == false)
            {
                StartCoroutine(Death(particlePrefab));             
            }
        }

        if (other.gameObject.CompareTag("Water")) //GAMEOVER instantly
        {
            isOnWater = true;
            cooldown = false;
        }

        if (other.gameObject.CompareTag("Platform")) //In Tree Platform
        {
            isOnPlatform = true;
            transform.parent = other.transform;
        }

        if (other.gameObject.CompareTag("Turtle")) //Funciona :D (Hacer padre de las tortugitas)
        {
            attack = true;

            if (TurtleAnimScript.underWater == true)
            {
                Debug.Log("Hola");
                cooldown = false;
                StartCoroutine(Death(particlePrefab));

                /*if (lifeCounter <= 0)
                {
                    lifeCounter = 0;
                    StartCoroutine(GameOver_Death()); //GAMEOVER instantly
                }*/
            }
        }

        if (other.gameObject.CompareTag("RespawnPoint"))
        {
            respawnPos = transform.position;
        }

        if (other.gameObject.CompareTag("MysteryBox"))
        {
            randIndx = Random.Range(0, 3);
            Destroy(other.gameObject);
            Instantiate(mysteryBoxparticle, transform.position, transform.rotation);

            if (randIndx == 0)
            {
                lifeCounter++;
                UpdateLife();

                if (lifeCounter >= 3)
                {
                    lifeCounter = 3;
                    GameObject canvas = Instantiate(canvasNumber, transform.position, Quaternion.identity);
                    canvas.GetComponent<AH_UINumber>().score = 100;
                    GameManagerScript.UpdateScore(100);
                }
            }
            else if (randIndx == 1)
            {
                StartCoroutine(Temporal_Inmortal());
            }
            else if (randIndx == 2)
            {
                GameObject canvas = Instantiate(canvasNumber, transform.position, Quaternion.identity);
                canvas.GetComponent<AH_UINumber>().score = 250;
                GameManagerScript.UpdateScore(250);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Turtle"))
        {
            if (TurtleAnimScript.underWater == true)
            {
                if (cooldown == false)
                {
                    StartCoroutine(Cooldown_Turtle());
                }
            }
        }

        if (other.gameObject.CompareTag("Water")) //GAMEOVER instantly
        {
            if (isOnPlatform == false)
            {
                if (cooldown == false)
                {
                    StartCoroutine(Cooldown_Water());
                    StartCoroutine(Death(waterParticlePrefab));
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
        if (other.gameObject.CompareTag("Platform")) //Outside platform
        {
            isOnPlatform = false;
            transform.parent = null;
        }
    }
    #endregion

    #region Cooldown System
    private IEnumerator Cooldown_Turtle()
    {
        cooldown = true;
        yield return new WaitForSeconds(2f);//En este tiempo la tortuga no ataca
        cooldown = false;
    }

    private IEnumerator Cooldown_Water()
    {
        cooldown = true;
        yield return new WaitForSeconds(0.01f);
    }
    #endregion

    #region Life System
    private void UpdateLife()
    {
        GameManagerScript.lifeImage.sprite = GameManagerScript.lifeSpriteArray[lifeCounter];
    }

    private IEnumerator Death(GameObject Particle)
    {
        if(isInmortal == false)
        {
            lifeCounter--;
            UpdateLife();

            if (lifeCounter > 0)
            {
                Instantiate(Particle, transform.position, transform.rotation);

                canMove = false;
                Color color = playerRenderer.color;
                color = new Color(color.r, color.g, color.b, 0);
                playerRenderer.color = color;
                yield return new WaitForSeconds(1f);

                transform.position = respawnPos;
                color.a = 1f;
                playerRenderer.color = color;
                canMove = true;
            }
            else
            {
                damage_PostProcess.profile.TryGet(out vg);
                vg.intensity.value = 0f;

                for (int i = 0; i < 3; i++)
                {
                    vg.intensity.value += 0.1f;
                    yield return new WaitForSeconds(0.05f);
                }

                StartCoroutine(GameOver_Death());
            }
        }
    }

    private IEnumerator GameOver_Death()
    {
        canMove = false;
        GameManagerScript.gameOver = true;
        Color color = playerRenderer.color;
        color = new Color(color.r, color.g, color.b, 0);
        playerRenderer.color = color;
        Instantiate(bombDeathPrefab, transform.position, bombDeathPrefab.transform.rotation);
        isOnWater = false;
        attack = false;
        yield return new WaitForSeconds(1.5f);
        GameManagerScript.GameOver();
    }

    private IEnumerator Temporal_Inmortal()
    {
        isInmortal = true;
        yield return new WaitForSeconds(10f); //Invencibilidad de 10 segundos
        isInmortal = false;
    }

    #endregion

    //Falta sonido
    //Ranking de score
}
