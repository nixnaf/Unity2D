using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;
    
    
    [Header("FacingDirection")]
    [SerializeField] protected int facingDir = 1;
    [SerializeField] protected int standFacingDir = 1;

    [Header("GroundCheck")]
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    protected bool isGrounded;
    
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        FacingChecks();
        CollisionChecks();
    }
    
    protected virtual void FacingChecks()
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
    protected virtual void Flip()
    {
        facingDir *= -1;
        standFacingDir *= -1;
        transform.Rotate(0,180,0);
    }
    
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position,new Vector3(transform.position.x,transform.position.y-groundCheckDistance));
    }
    protected virtual void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position,Vector2.down,groundCheckDistance, whatIsGround);
    }
    
    
}
