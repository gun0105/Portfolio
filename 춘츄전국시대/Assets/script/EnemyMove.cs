using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRendder;
    public int nextMove;

    void Awake () {
        Debug.Log("Awake");
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRendder = GetComponent<SpriteRenderer>();
        Think();

        //Invoke("Think", 5);
    }

    void FixedUpdate()
    {
        //move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Turn();
        }
	}
    
    void Think() {
        
        //set nextmove
        nextMove = Random.Range(-1, 2);
        //Debug.Log("Think : "+nextMove);

        //animation
        anim.SetInteger("Walkspeed", nextMove);
        //flip sprite
        if (nextMove != 0)
            spriteRendder.flipX = nextMove == 1;

        //Recursive
        //float nextThinkTime = Random.Range(2f, 5f);
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn() {
        //Debug.Log("Turn");
        nextMove *= -1;
        spriteRendder.flipX = nextMove == 1;

        //CancelInvoke();
        //Invoke("Think", 2);
    }
}
