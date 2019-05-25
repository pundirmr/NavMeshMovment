using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DirectedAgent : MonoBehaviour {

	private NavMeshAgent agent;
	float speedH = 1.0f;
	float yaw = 0.0f;
	float pitch = 0.0f;

	void Awake () 
	{
		gameObject.AddComponent<NavMeshAgent> ();
		agent = GetComponent<NavMeshAgent> ();  
		agent.speed = 300;
		agent.autoRepath = false;
		agent.autoTraverseOffMeshLink = false;
		agent.autoBraking = true;
		agent.baseOffset = 2f;
	}

	public void StopAgent()
	{
		agent.isStopped = true;
	}

	void Update () 
	{
		ControlCamera ();
		ControlMovement ();
	}

	void ControlMovement ()
	{
		Vector3 p_Velocity = new Vector3();

		if (Input.GetKey(KeyCode.UpArrow))
		{
			Vector3 movement = agent.GetComponent<Transform> ().transform.forward * Time.deltaTime * 50f;
			agent.Move (movement);
		}

		if (Input.GetKey(KeyCode.DownArrow))
		{
			Vector3 movement = (agent.GetComponent<Transform> ().transform.forward * -1f) * (Time.deltaTime * 50f);
			agent.Move (movement);
		}

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			Vector3 movement = (agent.GetComponent<Transform> ().transform.right * -1f) * (Time.deltaTime * 50f);
			agent.Move (movement);
		}

		if (Input.GetKey(KeyCode.RightArrow))
		{
			Vector3 movement = agent.GetComponent<Transform> ().transform.right * Time.deltaTime * 50f;
			agent.Move (movement);
		}

		if (Input.GetKeyUp (KeyCode.UpArrow) || Input.GetKeyUp (KeyCode.DownArrow) || Input.GetKeyUp (KeyCode.RightArrow)
			|| Input.GetKeyUp (KeyCode.LeftArrow)) {
			StopAgent ();
		}
	}

	private void ControlCamera()
	{
		pitch -= speedH * Input.GetAxis("Mouse Y");
		yaw += speedH * Input.GetAxis("Mouse X");

		pitch = Mathf.Clamp(pitch, -90f, 90f);

		while (yaw < 0f)
		{
			yaw += 360f;
		}
		while (yaw >= 360f)
		{
			yaw -= 360f;
		}

		transform.eulerAngles = new Vector3(pitch, yaw, 0f);
	}
}

