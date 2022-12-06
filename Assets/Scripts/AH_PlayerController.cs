using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AH_PlayerController : MonoBehaviour
{
    //Vector and Screen Space
    private float spaceLimits = 4f;
    private float distance = 1f; //Distance between steps
    private Vector2 nextPos, respawnPos, maxPos;
    private Vector2 InitialPos = new Vector2(0f, -5.5f);

    //Boolean conditions for animations
    private bool isJumping, isInmortal;

    private Animator playerAnimator;
    private Rigidbody2D playerRigidbody;
    private SpriteRenderer playerRenderer;

    //Boolean conditions for movement
    public bool active;
    private bool canMove = true;

    public int lifeCounter = 3;

    private int stepScore = 5;

    //Boolean conditions for trigger system
    private bool isOnPlatform, isOnWater, isOnTurtle;
    private bool cooldown, attack;

    //ParticleSystem and Post-Processing
    public GameObject bombDeathPrefab, particlePrefab, waterParticlePrefab; 
    public GameObject mysteryBoxparticle;
    public Volume damage_PostProcess;
    private Vignette vg;

    //UI
    public GameObject canvasNumber, canvasHeart; 

    //Audio
    public AudioClip[] soundEffects;
    private AudioSource gameManagerAudioSource;

    //Scripts
    private AH_GameManager GameManagerScript;
    private AH_TurtleAnim TurtleAnimScript;

    void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
        GameManagerScript = FindObjectOfType<AH_GameManager>();
    }

    void Start()
    {
        gameManagerAudioSource = GameManagerScript.GetComponent<AudioSource>();
        transform.position = InitialPos;
        maxPos = transform.position;
    }
    #region Player Movement
    void Update()
    {
        //Directions: Up, left and right
        if (isJumping == false && canMove) //As a cooldown, we cannot press a key again if we didn't arive to the new position
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                StartCoroutine(Movement());
                nextPos = new Vector2(transform.position.x, transform.position.y + distance); //Each step will be +0.5 (*)
                transform.position = nextPos;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                gameManagerAudioSource.PlayOneShot(soundEffects[0]); //Jump effect (**)

                if (nextPos.y > maxPos.y) //The score will increase 5 points each step as long as we reach the prevoius maxPos.y 
                {
                    GameManagerScript.UpdateScore(stepScore);
                }
            }

            if (Input.GetAxisRaw("Horizontal") > 0 && transform.position.x != spaceLimits)
            {
                StartCoroutine(Movement());
                nextPos = new Vector2(transform.position.x + distance, transform.position.y); //(*)
                transform.position = nextPos;
                transform.rotation = Quaternion.Euler(0, 0, -90);
                gameManagerAudioSource.PlayOneShot(soundEffects[0]); //(**)
            }

            if (Input.GetAxisRaw("Horizontal") < 0 && transform.position.x != -spaceLimits)
            {
                StartCoroutine(Movement());
                nextPos = new Vector2(transform.position.x - distance, transform.position.y); //(*)
                transform.position = nextPos;
                transform.rotation = Quaternion.Euler(0, 0, 90);
                gameManagerAudioSource.PlayOneShot(soundEffects[0]);//(**)
            }
        }

        if (transform.position.x > spaceLimits || transform.position.x < -spaceLimits) //In case we have been on a platform (tree, turtle or cocodrile) and we touch a limit
        {
            GameManagerScript.GameOver();
        }
    }

    //Animations
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

    #endregion

    #region Trigger System
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            maxPos = transform.position; //It saves the last max position
            StartCoroutine(Death(particlePrefab, 2));             
        }

        if (other.gameObject.CompareTag("Water"))
        {
            maxPos = transform.position; //It saves the last max position
            isOnWater = true;
            cooldown = false;
        }

        if (other.gameObject.CompareTag("Platform")) //For tree and crocodile
        {
            isOnPlatform = true;
            transform.parent = other.transform;
        }

        if (other.gameObject.CompareTag("Turtle")) //Exclusive tag for the turtle because it took me so long to make it work that I didn't want more problems :s
        {
            maxPos = transform.position;
            TurtleAnimScript = FindObjectOfType<AH_TurtleAnim>();
            isOnTurtle = true;
            transform.parent = other.transform;
            attack = true;

            if (TurtleAnimScript.underWater == true) //ONLY if it's underwater the turtle attacks (one single hit)
            {
                cooldown = false;
            }
        }

        if (other.gameObject.CompareTag("RespawnPoint"))
        {
            respawnPos = transform.position;
        }

        if (other.gameObject.CompareTag("MysteryBox")) //A little bonus to make the game more enjoyable
        {
            int randIndx = Random.Range(0, 3);
            Destroy(other.gameObject);
            Instantiate(mysteryBoxparticle, transform.position, transform.rotation);

            if (randIndx == 0) //The first option allow us to regain a heart
            {
                gameManagerAudioSource.PlayOneShot(soundEffects[1]);
                lifeCounter++;
                UpdateLife();
                GameObject canvasLife = Instantiate(canvasHeart, transform.position, Quaternion.identity); //UI feedback to make it more dynamic (***)

                if (lifeCounter > 3) //If this option appears but we already have 3 heart, 100 points will be add to the score counter
                {
                    lifeCounter = 3;
                    GameObject canvas = Instantiate(canvasNumber, transform.position, Quaternion.identity); //(***)
                    canvas.GetComponent<AH_UINumber>().score = 100;
                    GameManagerScript.UpdateScore(100);
                }
            }
            else if (randIndx == 1)//The second option allow us to be inmortal through 10 seconds
            {
                gameManagerAudioSource.Stop();
                gameManagerAudioSource.PlayOneShot(soundEffects[5]);
                gameManagerAudioSource.Play();
                StartCoroutine(Temporal_Inmortal());
            }
            else if (randIndx == 2)//The third and last option adds 250 points to the score counter
            {
                gameManagerAudioSource.PlayOneShot(soundEffects[1]);
                GameObject canvas = Instantiate(canvasNumber, transform.position, Quaternion.identity); //(***)
                canvas.GetComponent<AH_UINumber>().score = 250;
                GameManagerScript.UpdateScore(250);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) //Allow the turtle and water to make us damage
    {
        if (other.gameObject.CompareTag("Turtle"))
        {
            if (TurtleAnimScript.underWater == true && isOnTurtle == true)
            {
                if (cooldown == false)
                {
                    transform.parent = null;
                    StartCoroutine(Cooldown_Turtle());
                    StartCoroutine(Death(particlePrefab, 2));
                }
            }
        }

        if (other.gameObject.CompareTag("Water"))
        {
            if (isOnPlatform == false && isOnTurtle == false)
            {
                if (cooldown == false)
                {
                    StartCoroutine(Cooldown_Water());
                    StartCoroutine(Death(waterParticlePrefab, 4));
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

        if (other.gameObject.CompareTag("Turtle")) //Outside turtle
        {
            isOnTurtle = false;
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
    private void UpdateLife() //To update the UI life image with the actual life in the counter
    {
        GameManagerScript.lifeImage.sprite = GameManagerScript.lifeSpriteArray[lifeCounter];
    }

    private IEnumerator Death(GameObject Particle, int Sound) //Two types of death
    {
        if(isInmortal == false)
        {
            lifeCounter--;
            UpdateLife();

            if (lifeCounter > 0) //In the first one, we make appear a hit effect and the player is send back to the closest respawn position
            {
                gameManagerAudioSource.PlayOneShot(soundEffects[Sound]);
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
            else //In the seconds one, when we have the last heart, we make appear a red vignette effect (see the rest below)
            {
                gameManagerAudioSource.PlayOneShot(soundEffects[3]);
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

    private IEnumerator GameOver_Death() //A bomb appears, the player disappears and after, we travel to the gameOver scene
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

    private IEnumerator Temporal_Inmortal() //The Player is inmortal through 10 seconds
    {
        isInmortal = true;
        yield return new WaitForSeconds(10f);
        isInmortal = false;
    }

    #endregion
}
