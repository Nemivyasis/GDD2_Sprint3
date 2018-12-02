/** Jonathan So, jds7523@rit.edu
 * Handles the UI button's scene changes.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour {

	public void ChangeScene(string sceneName) {
		PlayerPrefs.SetInt("lives", 5); // So, every time we load up a new scene, set the player's number of lives to 5.
		LifeCounter.instance.UpdateText();
		SceneManager.LoadScene(sceneName);
	}
}
