using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroBehaviour : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    Animator Anim;

    bool grounded = false;
    bool isDead = false;

    Vector2 lastPoint;

    const float walkSpeed = 6f;
    const float jumpSpeed = 5f;
    const float bounceJumpSpeed = 3f;

    const float maxFallDistance = 10f;

    const int IDLE_ANIM_ID = 0;
    const int RUN_ANIM_ID = 1;
    const int HIT_ANIM_ID = 2;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();

        updateLastPoint();
    }

    // Update is called once per frame
    void Update()
    {
        // Hero jump
        if (!isDead)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                jump();
            }
        }

    }

    private void FixedUpdate()
    {
        // Move hero
        if (!isDead) {
            if (Input.GetAxis("Horizontal") != 0)
            {
                Rigidbody.velocity = new Vector2(Input.GetAxis("Horizontal") * walkSpeed, Rigidbody.velocity.y);
                Anim.SetInteger("AnimationID", RUN_ANIM_ID);
            }
            else
            {
                Rigidbody.velocity = new Vector2(0, Rigidbody.velocity.y);
                Anim.SetInteger("AnimationID", IDLE_ANIM_ID);
            }

            if (Input.GetKeyDown(KeyCode.RightControl)) {
                Anim.SetInteger("AnimationID", HIT_ANIM_ID);
            }
        }



        if ((isFalling() && !isSomethingBelow()) || isDead) {
            Invoke("reloadLevel", 2);
        }
    }

    private void jump()
    {
        if (grounded) {
            Rigidbody.velocity = new Vector2(0, 0);
            Rigidbody.AddForce(transform.up * jumpSpeed, ForceMode2D.Impulse);
           
            grounded = false;
        }
        
    }

    private void reloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private bool isSomethingBelow()
    {
        Ray2D ray = new Ray2D(transform.position, Vector2.down);
        return Physics2D.Raycast(ray.origin, ray.direction, maxFallDistance);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        

        if (collision.contacts.Length > 0)
        {
            ContactPoint2D contact = collision.contacts[0];

            if (collision.gameObject.GetComponent<MobsBehaviour>())
            {
                Rigidbody2D ColisionRB = collision.gameObject.GetComponentInParent<Rigidbody2D>();
                float angle = Mathf.Atan2(Rigidbody.position.y - ColisionRB.position.y, Rigidbody.position.x - ColisionRB.position.x) * Mathf.Rad2Deg;

                if (angle > 70 && angle < 110)
                {
                    Rigidbody.AddForce(transform.up * bounceJumpSpeed, ForceMode2D.Impulse);
                }
                else 
                {
                    isDead = true;
                }
            }

            if (Vector2.Dot(contact.normal, Vector2.up) > 0.5) 
            {
                grounded = true;
                updateLastPoint();
            }
        }
    }

    private void updateLastPoint()
    {
        lastPoint = transform.position;
    }

    private bool isFalling() 
    {
        return lastPoint.y - transform.position.y > maxFallDistance;
    }
}
