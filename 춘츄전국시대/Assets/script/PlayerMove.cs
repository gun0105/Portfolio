using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public GameManager gameManager;
    public float maxSpeed;
    public float jumpPower;
    public bool itladder;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public Animator anim;
    CapsuleCollider2D capsuleCollider;
    bool isdie = false;
    public bool stageend = false;
    bool isdamage = false;
    bool isjump = false;


    void Awake () {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping") && !isjump)
        {
            isjump = true;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }
        //stop
        if (Input.GetButtonUp("Horizontal")) {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }
        //turn
         if (Input.GetAxisRaw("Horizontal") == -1)
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == 1;
        }
        else if (Input.GetAxisRaw("Horizontal") == 1)
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == 1;
        }
        //ani
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
        //ladder
        if (Ladder.instance.islad == true)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                anim.SetBool("isladdering", true);
                anim.enabled = true;
            }
            else
            {
                anim.enabled = false;
            }
        }
        else {
            anim.SetBool("isladdering", false);
            anim.enabled = true;
        }
    }

    void FixedUpdate () {
        //move
        if (isdie == false)
        {
            float h = Input.GetAxisRaw("Horizontal");

            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            if (rigid.velocity.x > maxSpeed)
                rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);//Right maxSpeed
            else if (rigid.velocity.x < maxSpeed * (-1))
                rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);//Left maxSpeed

            //Landing Platform
            if (rigid.velocity.y < 0)
            {
                Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
                RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform", "box"));
                if (rayHit.collider != null)
                {
                    if (rayHit.distance < 0.6f)
                    {
                        anim.SetBool("isJumping", false);
                        isjump = false;
                    }
                }
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !isdamage) {
            isdamage = true;
            OnDamaged(collision.transform.position);
        }    
    }

    void OnDamaged(Vector2 targetPos)
    {
        isjump = true;
        //health down
        gameManager.HealthDown() ;
        //change layer
        gameObject.layer = 11;
        //view damaged
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        int damaged = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(damaged, 1)*2, ForceMode2D.Impulse);
        //invincibility time 1sec
        Invoke("OffDamaged", 1);
    }
    void OffDamaged() {
        isdamage = false;
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item") {
            //point
            bool isfish = collision.gameObject.name.Contains("fish");
            bool iscat_snack = collision.gameObject.name.Contains("cat_Snack");
            if (isfish)
            {
                gameManager.stagePoint += 100;
            }
            else if (iscat_snack) {
                gameManager.stagePoint += 300;
                gameManager.helthup();
            } 
            //Disapper Item
            collision.gameObject.SetActive(false);
        } else if (collision.gameObject.tag == "Finish" && stageend == false) {
            //Next Stage
            stageend = true;
            gameManager.NextStage();
        }
    }

    public void OnDie() {
        isdie = true;
        spriteRenderer.color = new Color(1, 0, 0, 0.4f);
        anim.SetTrigger("isdie");
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }
    public void VelocityZero() {
        rigid.velocity = Vector2.zero;
    }
}