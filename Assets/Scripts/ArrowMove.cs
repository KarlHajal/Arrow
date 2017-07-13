using UnityEngine;
using System.Collections;

public class ArrowMove : MonoBehaviour {

    public float speed;
    public Rigidbody2D rb2D;

	void Start ()
    {
        rb2D.velocity = speed * transform.forward;
     }
	
	

	}

	
