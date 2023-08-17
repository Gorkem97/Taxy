using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public int extraJump;
    public bool myTurn = false;
    public float frictionAmount;
    public bool isActive = false;
    public bool isGround;
    public bool FallHarden;
    float CayoteeWait =0;
    public float CayoteeFall;
    public float GravityNormal = 9.81f;
    public float GravityTransition = 25;
    public float GravityFall = 15f;
    public float jumpForce;
    Rigidbody2D rb;
    public float moveSpeed = 9;
    public float acceleration = 13;
    public float decceleration = 16;
    public float velPower = 0.96f;
    private Vector2 _moveInput;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isActive)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            FallHarden = false;
            CayoteeWait = 0.4f;
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            FallHarden = true;
        }

        CayoteeFall -= Time.deltaTime;
        CayoteeWait -= Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (!isActive)
        {
            if (isGround)
            {
                Friction();
            }
            return;
        }
        _moveInput.x = Input.GetAxisRaw("Horizontal");

        if (isGround && Mathf.Abs(_moveInput.x) < 0.01f)
        {
            Friction();
        }
        if (CayoteeFall> 0 && CayoteeWait > 0)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            CayoteeWait = 0;
            CayoteeFall = 0;
        }
        if (!isGround && Input.GetKeyDown(KeyCode.Space) && extraJump>0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (rb.velocity.y >= 0 && !FallHarden)
        {
            Physics2D.gravity = new Vector3(0, -GravityNormal, 0);
        }
        if (rb.velocity.y >= 0 && FallHarden)
        {
            Physics2D.gravity = new Vector3(0, -GravityTransition, 0);
        }
        if (rb.velocity.y < 0)
        {
            Physics2D.gravity = new Vector3(0, -GravityFall, 0);
        }
        float targetSpeed = _moveInput.x * moveSpeed;
        float SpeedDif = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(SpeedDif) * accelRate, velPower)*Mathf.Sign(SpeedDif);
        rb.AddForce(movement * Vector2.right);
    }
    void Friction()
    {
        
        float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
        amount *= Mathf.Sign(rb.velocity.x);
        rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        
    }
}
