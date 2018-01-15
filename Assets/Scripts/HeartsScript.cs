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
		for (int j = (int)playerHPScript.playerHP; j < 100; j += 10)
			hearts [j / 10].SetActive (false);

    }
}
