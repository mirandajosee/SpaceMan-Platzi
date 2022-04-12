using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float runningSpeed = 1.5f;
    public int enemyDamage = 10;
    Rigidbody2D rigidBody;
    public bool facingRight = false;
    private Vector3 startPosition;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        //startPosition = this.transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        //this.transform.position = startPosition;
    }

    private void FixedUpdate()
    {
        float currentRunningSpeed = runningSpeed;
        if (facingRight)
        {
            currentRunningSpeed = runningSpeed;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            currentRunningSpeed = -runningSpeed;
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            rigidBody.velocity = new Vector2(currentRunningSpeed, rigidBody.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().CollectHealth(-1*enemyDamage);
            return;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            facingRight = !facingRight;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
