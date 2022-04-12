using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables de movimiento
    public float jumpForce = 6f;
    public float runningSpeed = 4f;

    private Rigidbody2D rigidBody;
    Animator animator;
    Vector3 startPosition;
    private const string STATE_ALIVE = "isAlive";
    private const string STATE_ON_THE_GROUND = "isOnTheGround";

    [SerializeField] private int healthPoints, manaPoints;
    public const int INITIAL_HEALTH = 100, INITIAL_MANA =15, MAX_HEALTH=200, MAX_MANA=30, MIN_HEALTH=10, MIN_MANA=0;
    public const int SUPERJUMP_COST=5;
    public const float SUPERJUMP_FORCE = 1.5f;
    public LayerMask groundMask;
    // Start is called before the first frame update
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;
        startPosition = this.transform.position;
    }

    public void StartGame()
    {
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, true);

        Invoke("RestartPosition", 0.15f);
    }

    void RestartPosition()
    {
        this.transform.position = startPosition;
        this.rigidBody.velocity = Vector2.zero;
        GameObject maincamera = GameObject.Find("Main Camera");
        maincamera.GetComponent<CameraFollow>().ResetCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump(false);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Jump(true);
        }
        if (Input.GetButton("Horizontal") && runningSpeed<=10)
        {
            runningSpeed += Input.GetAxisRaw("Horizontal") * 0.3f;
            if (runningSpeed <= 0)
            {
                runningSpeed = 0;
            }
            else if (runningSpeed >= 10)
            {
                runningSpeed = 9;
            }
        }
        animator.SetBool(STATE_ON_THE_GROUND, isTouchingTheGround());

        //Debug.DrawRay(this.transform.position, Vector2.down * 1.5f, Color.red);
        //Debug.DrawRay(this.transform.position, new Vector3(1.5f,-1.5f), Color.red);
    }

    private void FixedUpdate()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (rigidBody.velocity.x < runningSpeed)
            {
                rigidBody.velocity = new Vector2(runningSpeed, rigidBody.velocity.y);
            }
        }
        else
        {
            rigidBody.velocity = new Vector2(0,rigidBody.velocity.y);
        }
    }

    void Jump(bool superjump)
    {
        float jumpForceFactor = jumpForce;
        if (superjump && manaPoints>=SUPERJUMP_COST)
        {
            manaPoints -= SUPERJUMP_COST;
            jumpForceFactor *= SUPERJUMP_FORCE;
        }
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (isTouchingTheGround())
            {
                GetComponent<AudioSource>().Play();
                rigidBody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);
            }
        }
    }

    //Metodo que nos indica si el personaje toca el suelo
    bool isTouchingTheGround()
    {
        if(Physics2D.Raycast(this.transform.position,Vector2.down,1.5f,groundMask))
        {
            //animator.enabled = true;
            return true;
        }
        else if (Physics2D.Raycast(this.transform.position,new Vector2(1,-1).normalized, 2.5f, groundMask))
        {
            //animator.enabled = true;
            return true;
        }
        else
        {
            //animator.enabled = false;
            return false;
        }
    }

    public void Die()
    {
        float travelledDistance = GetTravelledDistance();
        float previousMaxDistance = PlayerPrefs.GetFloat("maxscore",0f);
        if (travelledDistance > previousMaxDistance)
        {
            PlayerPrefs.SetFloat("maxscore", travelledDistance);
        }

        this.animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
        this.healthPoints = INITIAL_HEALTH;
    }

    public void CollectHealth(int points)
    {
        Debug.Log(healthPoints);
        this.healthPoints += points;
        if (this.healthPoints >= MAX_HEALTH)
        {
            this.healthPoints = MAX_HEALTH;
        }
        if (this.healthPoints <= 0)
        {
            Die();
        }
    }
    public void CollectMana(int points)
    {
        this.manaPoints += points;
        if (this.manaPoints >= MAX_MANA)
        {
            this.manaPoints = MAX_MANA;
        }
    }

    public int GetHealth()
    {
        return healthPoints;
    }

    public int GetMana()
    {
        return manaPoints;
    }

    public float GetTravelledDistance()
    {
        return this.transform.position.x - startPosition.x;
    }
}
