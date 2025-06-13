using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    
    [Header("Movement")]
    [SerializeField] float xInput;
    [SerializeField] private float moveForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private int facingDir = 1;
    [SerializeField] private int standFacingDir = 1;
   

    [Header("Dash")] 
    [SerializeField] private float dashDuration; 
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCoolDown;
     private float dashTimer;
     private float dashCoolDownTimer;
    


    [Header("Attack")] 
    [SerializeField] private float comboCounter;
    [SerializeField] private float comboCooldown;
    [SerializeField] private float attackTimer;
    
    [Header("GroundCheck")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    
    
    private bool isMoving;
    private bool isDashing;
    private bool isAttacking;
    private bool isGrounded;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    
    
    
    // Update is called once per frame
    void Update() 
    {
        CheckInput(); 
        Movement();
        AnimatorControllers(); 
        CollisionChecks();
        FacingChecks();
        GlobalTimer(); //需要被每帧调用持续计时，作为全局计时器使用，需要放在Update内。
        
        
        
    }

    private void GlobalTimer()
    {
        dashCoolDownTimer -= Time.deltaTime;
    }


    private void Dash()
    {
        if (dashCoolDownTimer <= 0)
        {
            dashTimer = dashDuration;
            dashCoolDownTimer = dashCoolDown;
        }
    }
    
    
    
    
    
    private void FacingChecks()
    {
        if (rb.velocity.x < 0)
        {
            facingDir = -1;
        }
        else if (rb.velocity.x > 0)
        {
            facingDir = 1;
        }

        if (facingDir != standFacingDir)
        {
            Flip();
        }
    }

    
    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position,Vector2.down,groundCheckDistance, whatIsGround);
    }


    private void AnimatorControllers()
    {
        anim.SetBool("isMoving",  xInput != 0);
        anim.SetBool("isDashing", dashTimer > 0);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetFloat("yVelocity",rb.velocity.y);
        anim.SetBool("isGrounded",isGrounded);
        
        
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.J))
        {
            isAttacking = true;
        }
        
        
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }
        
        
    }
    
    private void Movement()
    {
        dashTimer -= Time.deltaTime;
        //dashTimer不需要被每帧调用
        
        if (dashTimer >= 0)
        {
            
            rb.velocity = new Vector2(facingDir * dashForce, rb.velocity.y);  
            
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveForce, rb.velocity.y);   
        }
        
        
    }
    
    
    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x,jumpForce);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position,new Vector3(transform.position.x,transform.position.y-groundCheckDistance));
    }

    private void Flip()
    {
        facingDir *= -1;
        standFacingDir *= -1;
        transform.Rotate(0,180,0);
    }
    
}

    