using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public bool islad = false;
    public static Ladder instance { get; set; }


    public float Speed = 6;
    void Awake() {
        instance = this;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            islad = true;
            collision.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            collision.GetComponent<Rigidbody2D>().drag = 10.0f;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            islad = false;
            collision.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            collision.GetComponent<Rigidbody2D>().drag = 2.0f;
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && Input.GetKey(KeyCode.UpArrow))
        {
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, Speed);
        }
        else if (other.tag == "Player" && Input.GetKey(KeyCode.DownArrow))
        {
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -Speed);
        }
        else {
            
        }      
    }
}
