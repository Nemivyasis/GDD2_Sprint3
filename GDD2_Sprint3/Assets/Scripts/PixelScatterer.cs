/** Jonathan So, jds7523@rit.edu
 * As per Weez's suggestions, scatters green and brown pixels all over the screen.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelScatterer : MonoBehaviour {

	public Color[] colors; // The green and brown colors for the pixels.
	public GameObject pixel; // Pixel prefab gameobject.

	private void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}

	// Generate an amount of colored pixels in the scene.
	private void Start() {
		for (int i = 0; i < 25 + Random.Range(0, 25); i++) {
			GameObject obj = (GameObject) Instantiate(pixel, new Vector3(Random.Range(-12f, 12f), Random.Range(-8f, 8f), 0f), Quaternion.identity);
			obj.GetComponent<SpriteRenderer>().color = colors[Random.Range(0, colors.Length)];
		}
	}
}
