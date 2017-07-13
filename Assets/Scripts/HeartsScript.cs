using UnityEngine;
using System.Collections;

public class HeartsScript : MonoBehaviour {

    public PlayerHealth playerHPScript;
    public GameObject[] hearts;

    void Awake()
    {
        foreach (GameObject n in hearts)
            n.SetActive(true);
    }

	void Update()
    {
        if(playerHPScript.playerHP <= 90)
        {
            hearts[9].SetActive(false);
            if (playerHPScript.playerHP <= 80)
            {
                hearts[8].SetActive(false);
                if (playerHPScript.playerHP <= 70)
                {
                    hearts[7].SetActive(false);
                    if (playerHPScript.playerHP <= 60)
                    {
                        hearts[6].SetActive(false);
                        if (playerHPScript.playerHP <= 50)
                        {
                            hearts[5].SetActive(false);
                            if (playerHPScript.playerHP <= 40)
                            {
                                hearts[4].SetActive(false);
                                if (playerHPScript.playerHP <= 30)
                                {
                                    hearts[3].SetActive(false);
                                    if (playerHPScript.playerHP <= 20)
                                    {
                                        hearts[2].SetActive(false);
                                        if (playerHPScript.playerHP <= 10)
                                        {
                                            hearts[1].SetActive(false);
                                            if (playerHPScript.playerHP <= 0)
                                            {
                                                hearts[0].SetActive(false);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
