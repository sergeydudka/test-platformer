using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroBehaviour : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    Camera HeroCamera;

    bool grounded = false;
    Vector2 lastPoint;

    const float walkSpeed = 6f;
    const float jumpSpeed = 5f;

    const float maxFallDistance = 10f;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        HeroCamera = Camera.main;

        updateLastPoint();
    }

    // Update is called once per frame
    void Update()
    {
        // Hero jump
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            jump();
        }
    }

    private void FixedUpdate()
    {
        // Move hero
        Rigidbody.velocity = new Vector2(Input.GetAxis("Horizontal") * walkSpeed, Rigidbody.velocity.y);

        if (isFalling() && !isSomethingBelow()) {
            Invoke("reloadLevel", 2);
        }
    }

    private void jump()
    {
        if (grounded) {
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
