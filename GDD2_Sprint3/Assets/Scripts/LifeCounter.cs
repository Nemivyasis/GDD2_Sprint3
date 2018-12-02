using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCounter : MonoBehaviour {

	public static LifeCounter instance;
	private Text text;

	private void Awake() {
		if (instance == null) {
			instance = this;
		}
		text = GetComponent<Text>();
	}

	public void Start() {
		UpdateText();
	}

	public void UpdateText() {
		text.text = "LIVES - " + PlayerPrefs.GetInt("lives").ToString();
	}

	public void DestroyLifeCounter() {
		Destroy(this.gameObject);
	}
}
