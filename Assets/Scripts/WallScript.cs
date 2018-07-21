using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            Brain clone = collision.gameObject.GetComponent<PlayerScript>().GetBrain();
            GameManagerScript.deadPlayers.Add(clone);
            Destroy(collision.gameObject);
        }
    }
}
