using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillOnCollide : MonoBehaviour {



	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			Jukebox.instance.DamagedSFX();
			Jukebox.instance.KOSFX();
			// Jukebox.instance.Victory(); //Uncomment if you want Guile's theme to play upon death... and afterwards.
			GameObject.FindGameObjectWithTag("GameManager").GetComponent<ResetDeathManager>().KillPlayer(other.gameObject);
		}
	}
}
