using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    public PlayerHealth playerHP;
    //public float playerRespawnTime;
    public float enemySpawnTime;
    public int maxEnemyNb;
    public int EnemyCount;
    public GameObject playerCharacter;
    public GameObject HeldArrow;
    public GameObject Poof;
    public GameObject[] enemies;
    public GameObject[] enemySpawnPoints;
    public WaterTrigger waterTriggerScript;
    public bool playerKilled;
    public float score;
    public float normalEnemyScore;
    public float fastEnemyScore;
    public float giantEnemyScore;
    public Text scoreText;

    //private Vector2 SpawnPosition = new Vector2(0, 4);
    private Vector2 playerPosition;
    private Quaternion poofRotation = new Quaternion(0f, 0f, 0f, 0f);
    //private float nextRespawn;


    private float nextEnemySpawn;

    void Start()
    {
        score = 0;
        nextEnemySpawn = Time.time + enemySpawnTime;
    }

    void Update ()
    {
        scoreText.text = "Score: " + score;
	    if(playerHP.dead && !playerKilled)
        {
            KillPlayer();
        }
        
        /*
        if (playerKilled && Time.time >= nextRespawn)
        {
            RespawnPlayer();
        }
        */

        if(Time.time >= nextEnemySpawn && EnemyCount<maxEnemyNb)
        {
            nextEnemySpawn = Time.time + enemySpawnTime;
            SpawnEnemy();
        }
        else if(EnemyCount == maxEnemyNb)
        {
            nextEnemySpawn = Time.time + enemySpawnTime;
        }

        if(waterTriggerScript.Drowned)
        {
            nextEnemySpawn = Time.time + waterTriggerScript.RespawnTime;
        }

	}

    void KillPlayer()
    {
        Instantiate(Poof, new Vector2(playerCharacter.transform.position.x, playerCharacter.transform.position.y - 0.7f), poofRotation); //poof
        HeldArrow.SetActive(false);        
        playerCharacter.SetActive(false);
        //nextRespawn = Time.time + playerRespawnTime;
        playerKilled = true;
    }

    /*
    void RespawnPlayer()
    {
        playerCharacter.transform.position = SpawnPosition;
        playerCharacter.SetActive(true);
        //playerHP.playerHP = 100f;
        playerKilled = false;
    }
    */

    void SpawnEnemy()
    {
        EnemyCount++;
        Instantiate(enemies[UnityEngine.Random.Range(0,enemies.Length)], enemySpawnPoints[UnityEngine.Random.Range(0, enemySpawnPoints.Length)].transform.position,poofRotation);
    }
}
