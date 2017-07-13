using UnityEngine;
using System.Collections;

public class ArrowShot : MonoBehaviour {

    public float speed;

    private Rigidbody2D rb2D;
    private Vector2 direction;
    private bool StopArrow = false;

    void Start ()
    {
        GameObject charact = GameObject.Find("Character");
        CharacterMovement character = charact.GetComponent<CharacterMovement>();
        rb2D = GetComponent<Rigidbody2D>();
        direction = new Vector2(character.ArrowDirection_X, character.ArrowDirection_Y);
        if(character.ArrowDirection_X == -1)
            transform.rotation = Quaternion.AngleAxis(270f, Vector3.forward);
        Destroy(gameObject, 10);
    }

    void FixedUpdate()
    {
        if (!StopArrow)
            rb2D.MovePosition(rb2D.position + direction * speed * Time.deltaTime);
        else
            rb2D.velocity = Vector3.zero;
    }

    void OnTriggerEnter2D(Collider2D other)
   {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyGiant") || other.CompareTag("EnemyFast"))
        {
            Destroy(gameObject,0.15f);
            StopArrow = true;
            rb2D.isKinematic = true;
            rb2D.transform.parent = other.transform;
        }
        else if (other.CompareTag("Obstacle"))
        {
            StopArrow = true;
            rb2D.isKinematic = true;
            rb2D.transform.parent = other.transform;
        }
    }
}
