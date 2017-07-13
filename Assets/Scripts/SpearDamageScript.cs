using UnityEngine;
using System.Collections;

public class SpearDamageScript : MonoBehaviour {

    public float stabDistance;
    public PlayerHealth playerHpScript;
    public CharacterMovement charMvmtScript;

    private Animator anim;

    //to add charged bubble, set the bubble as a child, arrange position on character animation, enable/disable depending on charge

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        if (transform.eulerAngles.z > -1 && transform.eulerAngles.z < 1)
        {
            anim.SetFloat("SpearVerticalDirection", 1);
            anim.SetFloat("SpearHorizontalDirection", 0);
        }
        else if (transform.eulerAngles.z > 179 && transform.eulerAngles.z < 181)
        {
            anim.SetFloat("SpearVerticalDirection", -1);
            anim.SetFloat("SpearHorizontalDirection", 0);
        }
        else
        {
            anim.SetFloat("SpearHorizontalDirection",1);
            anim.SetFloat("SpearVerticalDirection", 0);
        }

        if (Input.GetButton("Fire1") || charMvmtScript.spearAttack)
            anim.SetFloat("Stab", 1);
        else
        {
            anim.SetFloat("Stab", 0);
            if(!Input.GetButtonUp("Fire1")) //attempt to fix problem of speed remaining = 3 while charging
                charMvmtScript.speed = 3;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyGiant") || other.CompareTag("EnemyFast"))
        {
            playerHpScript.couldBeTouchingPlayer = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyGiant") || other.CompareTag("EnemyFast"))
        {
            playerHpScript.couldBeTouchingPlayer = true;
        }
    }


}
