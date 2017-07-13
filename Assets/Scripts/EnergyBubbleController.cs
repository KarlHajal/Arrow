using UnityEngine;
using System.Collections;

public class EnergyBubbleController : MonoBehaviour {

    private GameObject Bow;
    private GameObject PlayerCharacter;
    private CharacterMovement PlayerCharacterScript;
    private Vector2 position;

	void Start ()
    {
        Destroy(gameObject, 1);
        Bow = GameObject.Find("Bow");
        PlayerCharacter = GameObject.Find("Character");
        PlayerCharacterScript = PlayerCharacter.GetComponent<CharacterMovement>();
    }
	

	void Update ()
    {
        position = Bow.transform.position;

        if (PlayerCharacterScript.ArrowDirection_Y == -1)
        {
            position.y -= 0.28f;
            if (PlayerCharacterScript.facingRight)
                position.x += 0.09f;
            else
                position.x -= 0.09f;
        }
        else if (PlayerCharacterScript.ArrowDirection_Y == 1)
            position.y += 0.3f;
        else if (PlayerCharacterScript.ArrowDirection_X == -1)
        {
            position.x -= 0.35f;
            position.y += 0.17f;
        }
        else
        {
            position.y += 0.17f;
            position.x += 0.35f;
        }

        transform.position = position;
	}
}
