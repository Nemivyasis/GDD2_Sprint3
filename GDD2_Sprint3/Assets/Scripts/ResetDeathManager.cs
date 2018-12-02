using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDeathManager : MonoBehaviour {

    public bool isDead = false;
    private Joycon joy;

    private Vector3 startPosition;

    public GameObject[] displayOnDeath;
    // Use this for initialization
    void Start () {
        joy = JoyconManager.Instance.j;
        startPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        Checkpoint();

        if (isDead)
        {
            if (joy != null)
            {
                if (joy.GetButton(Joycon.Button.SR) || joy.GetButton(Joycon.Button.SL))
                {
                    Reset();
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reset();
            }
        }
	}

    public void KillPlayer(GameObject player)
    {
        isDead = true;
        player.GetComponent<Rigidbody2D>().isKinematic = true;

        foreach (var item in displayOnDeath)
        {
            item.SetActive(true);
        }
    }

    private void Reset()
    {
        isDead = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().isKinematic = false;
        GameObject.FindGameObjectWithTag("Player").transform.position = startPosition;
        foreach (var item in displayOnDeath)
        {
            item.SetActive(false);
        }
    }

    private void Checkpoint()
    {
        if (GameObject.FindGameObjectWithTag("Player").transform.position.x <= GameObject.FindGameObjectWithTag("Checkpoint").transform.position.x &&
            GameObject.FindGameObjectWithTag("Player").transform.position.x >= GameObject.FindGameObjectWithTag("Checkpoint").transform.position.x - 1)
        {
            if (GameObject.FindGameObjectWithTag("Player").transform.position.y <= GameObject.FindGameObjectWithTag("Checkpoint").transform.position.y &&
            GameObject.FindGameObjectWithTag("Player").transform.position.y >= GameObject.FindGameObjectWithTag("Checkpoint").transform.position.y - 1)
            {
                GameObject.FindGameObjectWithTag("Checkpoint").GetComponent<SpriteRenderer>().color = new Color(155f, 255f, 155f);
                startPosition = GameObject.FindGameObjectWithTag("Checkpoint").transform.position;
            }
        }
    }
}
