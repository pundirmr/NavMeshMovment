using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCameraRotation : MonoBehaviour {

	float speedH = 1.0f;
	float yaw = 0.0f;
	float pitch = 0.0f;

	void Update()
	{
		ControlCamera ();
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
