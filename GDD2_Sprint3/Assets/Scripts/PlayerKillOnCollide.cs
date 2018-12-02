using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerKillOnCollide : MonoBehaviour {



	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag == "Player" && !GameObject.FindObjectOfType<ResetDeathManager>().isDead)
		{
			PlayerPrefs.SetInt("lives", PlayerPrefs.GetInt("lives") - 1); // Reduce number of lives.
			LifeCounter.instance.UpdateText();
			Jukebox.instance.DamagedSFX();
			Jukebox.instance.AddSpeaker(Jukebox.instance.audioSrcs.Count - PlayerPrefs.GetInt("lives")); // Mix up another audio channel in order to up the tension.
			if (PlayerPrefs.GetInt("lives") <= 0) {// If player has lost all lives...
				Jukebox.instance.KOSFX(); // Play KO noise
				StartCoroutine("GameOver"); // Reset to start.
			}
			// Jukebox.instance.Victory(); //Uncomment if you want Guile's theme to play upon death... and afterwards.
			GameObject.FindGameObjectWithTag("GameManager").GetComponent<ResetDeathManager>().KillPlayer(other.gameObject);
		}
	}

	// Wait 4 seconds, then return to the title screen.
	private IEnumerator GameOver() {
		yield return new WaitForSeconds(4.0f);
		LifeCounter.instance.DestroyLifeCounter();
		SceneManager.LoadScene("Title Screen");
	}
}
