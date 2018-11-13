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
}
