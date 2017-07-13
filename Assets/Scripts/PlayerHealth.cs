using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    public GameObject PlayerObject;
    public Color color1;
    public Color color2;
    public Color color3;
    public int delay = 100;
    public float playerHP = 100f;
    public bool dead = false;
    public float normalEnemyDamage;
    public float giantEnemyDamage;
    public float fastEnemyDamage;
    public Image damageImage;
    public Color redFlashColor = new Color(1f, 0f, 0f, 0.1f);
    public float redFlashSpeed = 0.1f;
    public bool couldBeTouchingPlayer = true;
    public bool damaged = false;
    public bool vulnerable = true;

    private int counter;
    private float invulnerabilityTime;
    private float damageAmount;  
    private bool toggleFlash = false;
       



    private float timer;
    
    
    void Awake()
    {
        PlayerObject.GetComponent<SpriteRenderer>().color = color1;
    }

    void OnEnable()
    {
        timer = Time.time;
        invulnerabilityTime = 2f;
        Invulnerability();
    }

    void Update()
    {
        if(playerHP <= 0 && !dead)
        {
            damageImage.color = redFlashColor;
            isDead();
        }

        if(damaged && vulnerable)
        {
            timer = Time.time;
            Invulnerability();            
            PlayerObject.GetComponent<SpriteRenderer>().color = color2;
            TakeDamage();
        }
        else if(!vulnerable)
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, redFlashSpeed * Time.deltaTime);
            if (Time.time - timer >= invulnerabilityTime)
                Vulnerability();
            else
                Flash();
        }
        

    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (couldBeTouchingPlayer)
        {
            if (other.CompareTag("Enemy"))
            {

                invulnerabilityTime = 1f;
                if (vulnerable)
                {
                    damageImage.color = redFlashColor;
                    damageAmount = normalEnemyDamage;
                    damaged = true;
                }
                else
                    damageAmount = 0;
            }
            if (other.CompareTag("EnemyGiant"))
            {

                invulnerabilityTime = 1f;
                if (vulnerable)
                {
                    damageImage.color = redFlashColor;
                    damageAmount = giantEnemyDamage;
                    damaged = true;
                }
                else
                    damageAmount = 0;
            }
            if (other.CompareTag("EnemyFast"))
            {

                invulnerabilityTime = 1f;
                if (vulnerable)
                {
                    damageImage.color = redFlashColor;
                    damageAmount = fastEnemyDamage;
                    damaged = true;
                }
                else
                    damageAmount = 0;
            }
        }

    } 


    void TakeDamage()
    {
        playerHP -= damageAmount;
        damaged = false;
    }


    void Invulnerability()
    {
        vulnerable = false;
    }

    void Vulnerability()
    {
        PlayerObject.GetComponent<SpriteRenderer>().color = color1;
        vulnerable = true;
    }
    
    void Flash()
    {
        if(counter >= delay)
        {
            counter = 0;
            toggleFlash = !toggleFlash;

            if(toggleFlash)
            {
                PlayerObject.GetComponent<SpriteRenderer>().color = color3;
            }
            else
            {
                PlayerObject.GetComponent<SpriteRenderer>().color = color2;
            }
        }
        else
        {
            counter++;
        }

    }

    void isDead()
    {
        dead = true;
    }
}
