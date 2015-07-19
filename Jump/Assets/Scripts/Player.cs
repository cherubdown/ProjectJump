using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float GravityScale;
    public float xSpeed;
    public float jumpFactor;

    private Animator animator;
    private Rigidbody2D rigidBody;
    private float triggerDownTime;
    private float jumpStrength;
    private PlayerStatus Status;

    private float maxHeight;
    public float MaxHeight
    {
        get
        {
            return maxHeight;
        }
    }

	void Start () {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = GravityScale;
	}
	
	void Update () {
        UserInputUpdates();
        UpdateMaxHeight();
	}

    private void UserInputUpdates()
    {
        // user input
        if (Status == PlayerStatus.Jumping || Status == PlayerStatus.Charging && rigidBody.velocity.x != 0)
        {
            // keyboard input
            if (Input.GetKey(KeyCode.A))
            {
                rigidBody.AddForce(new Vector2(-30f, 0f), ForceMode2D.Force);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rigidBody.AddForce(new Vector2(30f, 0f), ForceMode2D.Force);
            }

            // accelerometer input
            Vector3 dir = Vector3.zero;
            dir.x = Input.acceleration.x;
            if (dir.sqrMagnitude > 1)
                dir.Normalize();
            dir *= Time.deltaTime;
            rigidBody.transform.Translate(dir * xSpeed);
        }
    }

    private void UpdateMaxHeight()
    {
        if (rigidBody.transform.position.y > maxHeight)
        {
            maxHeight = rigidBody.transform.position.y;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Player.OnCollisionEnter2D");
        Land(other);

        if (other.tag == "StickyPlatform")
        {
            rigidBody.AddForce(Vector2.up * 10.0f, ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Land(other);
    }

    public void ChargeJump()
    {
        Debug.Log("ChargeJump");
        SetStatus(PlayerStatus.Charging);
        triggerDownTime = Time.time;
    }

    public void Jump()
    {
        if (Status == PlayerStatus.Charging && rigidBody.velocity == new Vector2(0, 0))
        {
            Debug.Log("Jump");
            rigidBody.gravityScale = GravityScale;
            jumpStrength = Mathf.Clamp(40f * (Time.time - triggerDownTime), 0f, 40f) * jumpFactor;
            Debug.Log("JumpStrength: " + jumpStrength);
            rigidBody.velocity = new Vector2(0, jumpStrength);
            triggerDownTime = 0f;
            SetStatus(PlayerStatus.Jumping);
        }
        else
        {
            SetStatus(PlayerStatus.Stopped);
        }
    }

    public void Land(Collider2D platform)
    {
        if (rigidBody.velocity.y < 0)
        {
            if (platform.tag == "BasicPlatform")
            {
                Debug.Log("Land");
                rigidBody.gravityScale = 0f;
                rigidBody.velocity = new Vector2(0, 0);
                if (Status == PlayerStatus.Jumping)
                {
                    SetStatus(PlayerStatus.Stopped);
                }
            }
        }
    }

    public void SetStatus(PlayerStatus status)
    {
        switch (status)
        {
            case PlayerStatus.Stopped:
                animator.SetInteger("PlayerState", 0);
                break;
            case PlayerStatus.Charging:
                animator.SetInteger("PlayerState", 1);
                break;
            case PlayerStatus.Jumping:
                animator.SetInteger("PlayerState", 2);
                break;
        }
        Status = status;
    }
}

public enum PlayerStatus 
{
    Stopped = 0,
    Charging = 1,
    Jumping = 2,
}