using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRotScript : MonoBehaviour {

    public float rotationSpeed = 10;

    private GameObject player;

    private bool inRot = false;
    private float turnLerp;
    private int prevAngle;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Rotate(int currGrav)
    {
        if(currGrav == prevAngle)
        {
            if (!inRot)
            {
                return;
            }
        }
        else
        {
            inRot = true;
            prevAngle = currGrav;
            turnLerp = 0;
        }

        turnLerp += (Time.deltaTime) * rotationSpeed;

        if(turnLerp >= 1)
        {
            inRot = false;
            turnLerp = 1;
        }

        Debug.Log(turnLerp);
        Vector3 newRot = Vector3.zero;


        if (currGrav == 0)
        {
            newRot = Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(0.0f, 0.0f, 180.0f), turnLerp);
        }
        else if (currGrav == 1 && (Vector3.Distance(transform.eulerAngles, Vector3.zero) > 0.01f))
        {
            newRot = Vector3.Lerp(transform.rotation.eulerAngles, Vector3.zero, turnLerp);
        }
        else if (currGrav == 2)
        {
            newRot = Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(0.0f, 0.0f, 90.0f), turnLerp);
        }
        else if (currGrav == 3 )
        {
            newRot = Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(0.0f, 0.0f, -90.0f), turnLerp);
        }

        Vector3 rotChange = newRot - transform.rotation.eulerAngles;

        player.transform.position = ZRot(player.transform.position, -1 * rotChange.z);

        transform.eulerAngles = newRot;
    }

    private Vector3 ZRot(Vector3 startingPos, float angChange)
    {
        angChange = Mathf.Deg2Rad * angChange;

        Vector3 updatedPos = startingPos;

        updatedPos.x = startingPos.x * Mathf.Cos(angChange) + startingPos.y * Mathf.Sin(angChange);
        updatedPos.y = -1 * startingPos.x * Mathf.Sin(angChange) + startingPos.y * Mathf.Cos(angChange);

        return updatedPos;
    }
}
