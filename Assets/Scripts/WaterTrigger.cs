using UnityEngine;
using System.Collections;

public class WaterTrigger : MonoBehaviour {

    private Vector2 SpawnPosition = new Vector2(0, 4);
    private Vector2 CollisionPosition;
    private Quaternion SpawnRotation = new Quaternion(0f, 0f, 0f, 0f);
    private float TimetoRespawn;


    public bool Drowned = false;
    public float RespawnTime;
    public GameObject PlayerCharacter;
    public PlayerHealth playerHealthScript;
    public GameObject HeldArrow;
    public GameObject Splash;
    public float waterDamage;

	void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            Drowned = true;
            playerHealthScript.playerHP -= waterDamage;

            if(playerHealthScript.playerHP <= 0)
                playerHealthScript.damageImage.color = playerHealthScript.redFlashColor;

            CollisionPosition = PlayerCharacter.transform.position;
            CollisionPosition.y -= 0.7f;            
            Instantiate(Splash, CollisionPosition, SpawnRotation);
            if (playerHealthScript.dead == false)
            {
                HeldArrow.SetActive(false);
                PlayerCharacter.SetActive(false);
                TimetoRespawn = Time.time + RespawnTime;
            }      
        }
        
    }

    void Update()
    {
        if (Drowned && Time.time >= TimetoRespawn && playerHealthScript.dead == false && playerHealthScript.playerHP > 0)
        {
                PlayerCharacter.transform.position = SpawnPosition;
                PlayerCharacter.SetActive(true);
                Drowned = false;
                
        }

    }
}
