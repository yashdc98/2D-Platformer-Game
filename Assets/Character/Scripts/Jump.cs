using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;


    public int totalJumps;
    int availableJumps;
    bool multipleJump;

    bool isGrounded;

    public Transform groundCheckCollider;
    public LayerMask groundLayer;
    const float groundCheckRadius = 0.2f;

    [SerializeField] float jumpPower = 500;
    bool coyoteJump;



    void Awake()
    {
        availableJumps = totalJumps;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            Jump1();
        
           
    }

    void FixedUpdate()
    {
        GroundCheck();

    }
    void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        //Check if the GroundCheckObject is colliding with other
        //2D Colliders that are in the "Ground" Layer
        //If yes (isGrounded true) else (isGrounded false)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
            if (!wasGrounded)
            {
                availableJumps = totalJumps;
                multipleJump = false;

                //AudioManager.instance.PlaySFX("landing");
            }         
        }
        else
        {
            //Un-parent the transform
            transform.parent = null;

            if (wasGrounded)
                StartCoroutine(CoyoteJumpDelay());
        }

        //As long as we are grounded the "Jump" bool
        //in the animator is disabled
        animator.SetBool("Jump", !isGrounded);
    }

    IEnumerator CoyoteJumpDelay()
    {
        coyoteJump = true;
        yield return new WaitForSeconds(0.2f);
        coyoteJump = false;
    }

    void Jump1()
    {
        if (isGrounded)
        {
            multipleJump = true;
            availableJumps--;

            rb.velocity = Vector2.up * jumpPower;
            animator.SetBool("Jump", true);
        }
        else
        {
            if (coyoteJump)
            {
                multipleJump = true;
                availableJumps--;

                rb.velocity = Vector2.up * jumpPower;
                animator.SetBool("Jump", true);
            }

            if (multipleJump && availableJumps > 0)
            {
                availableJumps--;

                rb.velocity = Vector2.up * jumpPower;
                animator.SetBool("Jump", true);
            }
        }
    }

}