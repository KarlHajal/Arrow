using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public GameObject player;
    private Vector3 offset;
    private GameObject Water;
    private WaterTrigger WaterScript;


	void Start ()
    {
        offset = transform.position - player.transform.position;

        Water = GameObject.Find("WaterTrigger");
        WaterScript = Water.GetComponent<WaterTrigger>();
	}
	
	
	void Update ()
    {
        if (WaterScript.Drowned == false)
            transform.position = player.transform.position + offset;
	}
}
