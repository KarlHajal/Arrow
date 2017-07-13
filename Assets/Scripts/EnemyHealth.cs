using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    bool damaged = false;

    public Color RedFlash;
    public GameObject Enemy;
    public float flashTime;
    public GameObject Poof;

    private GameObject gameManagerObject;
    private GameManager gameManagerScript;
    private GameObject playerCharacter;
    private PlayerHealth playerHpScript;
    private CharacterMovement characterMvmtScript;
    private float timer;
    private float damageAmount;
    public float enemyHP;
    private Quaternion poofRotation = new Quaternion(0f, 0f, 0f, 0f);

    void Start()
    {
        playerCharacter = GameObject.Find("Character");      
        characterMvmtScript = playerCharacter.GetComponent<CharacterMovement>();
        playerHpScript = playerCharacter.GetComponent<PlayerHealth>();
        gameManagerObject = GameObject.Find("GameManager");
        gameManagerScript = gameManagerObject.GetComponent<GameManager>();
    }

    void Update ()
    {

	    if(damaged)
        {
            TakeDamage();
            timer = Time.time;
        }
        else
        {
            if (Time.time - timer >= flashTime)
            {
                Restore();
            }
        }

        if (enemyHP <= 0)
        {
            //Poof.GetComponent<SpriteRenderer>().color = Color.red;
            Instantiate(Poof, new Vector2(transform.position.x,transform.position.y - 0.7f), poofRotation);            
            gameManagerScript.EnemyCount--;
            gameManagerScript.score += gameManagerScript.normalEnemyScore; //increment score
            playerHpScript.couldBeTouchingPlayer = true;
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }

	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Projectile"))
        {
            damageAmount = 50f;
            damaged = true;
            if (characterMvmtScript.chargedArrow)
                damageAmount = 100f;          
        }
        else if(other.CompareTag("Spear"))
        {
            damageAmount = 40f;
            if(characterMvmtScript.speed != 3 && characterMvmtScript.spearAttack)
                damageAmount = 200f;
            damaged = true;
        }

    }

    void TakeDamage()  //give values if other weapons are implemented
    {
        Enemy.GetComponent<SpriteRenderer>().color = RedFlash;
        enemyHP -= damageAmount;
        damaged = false;
    }

    void Restore()
    {
        Enemy.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }
}
