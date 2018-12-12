using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    public float moveSpeed = 10f;
    public float movement = 0f;
    public bool jump = false;
    public float jumpHeight = 1f;
    public bool onGrapple = false;
    private Rigidbody2D rBody;
    public Animator animator;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        movement = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(movement));
        if(Input.GetAxisRaw("Vertical") > 0)
        {
            jump = true;
            animator.SetBool("IsJump", true);
        }
	}

    public void OnLand()
    {
        animator.SetBool("IsJump", false);
    }

    void FixedUpdate()
    {
        //rBody.AddForce(new Vector2((movement * Time.fixedDeltaTime * moveSpeed - rBody.velocity.x) * 2f, 0));
        //rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y);
        transform.Translate(Vector3.right * movement * Time.fixedDeltaTime * moveSpeed);
        if (jump)
        {
            //rBody.velocity = new Vector2(rBody.velocity.x, jumpHeight);
            transform.Translate(Vector3.up * jumpHeight * Time.fixedDeltaTime);
            jump = false;
        }
    }
}
