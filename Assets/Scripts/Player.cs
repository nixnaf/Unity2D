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

public class Player : Entity
{
   
    
    [Header("Movement")]
    [SerializeField] float xInput;
    [SerializeField] private float moveForce;
    [SerializeField] private float jumpForce;
    private bool isMoving;
    
   

    [Header("Dash")] 
    [SerializeField] private float dashDuration; 
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCoolDown;
    private float dashTimer;
    private float dashCoolDownTimer;
    private bool isDashing;


    [Header("Attack")] 
    [SerializeField] private int comboCounter;
    [SerializeField] private float comboCoolDown;
    [SerializeField] private float comboTimer;
    private bool isAttacking;
    
    
    
 
   
    
    
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }


    // Update is called once per frame
    protected override void Update() 
    {
        base.Update();
        CheckInput(); 
        Movement();
        AnimatorControllers(); 
        GlobalTimer(); //需要被每帧调用持续计时，作为全局计时器使用，需要放在Update内。
        
      
        
    }

    public void AttackOver()
    {
        isAttacking = false;
        
        comboCounter++;
        if (comboCounter > 2)
        {
            comboCounter = 0;
        }
        
    }
    
    
    
    private void GlobalTimer()
    {
        dashCoolDownTimer -= Time.deltaTime;
        comboTimer -= Time.deltaTime;
        
    }


    private void Dash()
    {
        if (dashCoolDownTimer <= 0)
        {
            dashTimer = dashDuration;
            dashCoolDownTimer = dashCoolDown;
        }
    }
    
  


    private void AnimatorControllers()
    {
        anim.SetBool("isMoving",  xInput != 0);
        anim.SetBool("isDashing", dashTimer > 0);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("isGrounded",isGrounded);
        anim.SetFloat("yVelocity",rb.velocity.y);
        anim.SetInteger("comboCounter", comboCounter);
        
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
            AttackStart();
        }
        
        
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }
        
        
    }

    private void AttackStart()
    {
        if (comboTimer < 0)
        {
            comboCounter = 0;
        }
            
        isAttacking = true;
        comboTimer = comboCoolDown;
    }

    private void Movement()
    {
        dashTimer -= Time.deltaTime;
        //dashTimer不需要被每帧调用

        if (isAttacking)
        {
            rb.velocity = new Vector2(0, 0);
        }
        //攻击时禁止移动
        else if (dashTimer >= 0)
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


    
    
}
