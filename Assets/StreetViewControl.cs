using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StreetViewControl : MonoBehaviour {

    public GameObject ArrowObject;
    Ray ray;
    RaycastHit hit;
    float speedH = 1.0f;
    float yaw = 0.0f;
    float pitch = 0.0f;

	public Transform[] AllPoints;
	Plane[] planes;
	public Material redMaterial;
	public Material whiteMaterial;
	public Material greenMaterial;
	public Material vilotMaterial;
	List<Transform> InsideCamera = new List<Transform>();
	float totalRun= 0.1f;
	float mainSpeed = 0.01f;

	void Start()
	{
		// Calculate the planes from the main camera's view frustum
	}

    void Update()
    {
		CheckArrowMovement ();
        //RayCastForStreetView();
        ControlCameraRotation();
    }

	void GoToNearestPoint (bool b)
	{
		if (b) {
			//var closest = InsideCamera.Select( n => new { n, distance = ( n.position - Camera.main.transform.position )}).OrderBy( p => p.distance ).First().n;

			/*
			var nClosest = InsideCamera.OrderBy(t=>(t.position - Camera.main.transform.position).sqrMagnitude)
				.Take(1)   //or use .FirstOrDefault();  if you need just one
				.ToArray();
			*/

			var nClosest = InsideCamera.OrderBy(t=>(t.position - Camera.main.transform.position).sqrMagnitude)
				.First();

			Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.position-nClosest.transform.position);
			Vector3 dir = nClosest.transform.position - Camera.main.transform.position;

			if (Physics.Raycast (Camera.main.transform.position, dir, out hit, 1000)) {
				Debug.Log (hit.collider.name);
				//hit.collider.GetComponent<MeshRenderer> ().material = vilotMaterial;
				Debug.Log ("closest name:" + nClosest.name);
				if (hit.collider.GetComponent<StreetViewPoint>()) 
				{
					nClosest.GetComponent<MeshRenderer> ().material = greenMaterial;

					Camera.main.transform.position = nClosest.transform.position;//Vector3.Lerp (Camera.main.transform.position, nClosest [0].transform.position,Time.deltaTime * 2f);
					Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, 29f, Camera.main.transform.position.z);
				}
			}

		}
	}

	void CheckArrowMovement ()
	{
		planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

		if (Input.GetKey (KeyCode.UpArrow)) 
		{
			Debug.Log ("I am here");
			for (int i = 0; i < AllPoints.Length; i++) {
				if (GeometryUtility.TestPlanesAABB (planes, AllPoints [i].GetComponent<BoxCollider> ().bounds)) 
				{
					if (Camera.main.transform.position.x == AllPoints [i].transform.position.x && Camera.main.transform.position.z ==
					   AllPoints [i].transform.position.z) {
						InsideCamera.Remove (AllPoints [i]);
						continue;
					}
					AllPoints [i].GetComponent<MeshRenderer> ().material = redMaterial;
					InsideCamera.Add (AllPoints [i]);
				}
				else {
					AllPoints [i].GetComponent<MeshRenderer> ().material = whiteMaterial;
					InsideCamera.Remove (AllPoints [i]);
				}

			}
			GoToNearestPoint (true);
		}

//		if (Input.GetKeyUp (KeyCode.UpArrow)) {
//			InsideCamera.Clear ();
//			for (int i = 0; i < AllPoints.Length; i++) 
//			{
//				AllPoints [i].GetComponent<MeshRenderer> ().material = whiteMaterial;
//			}
//		}
	}

    private void ControlCameraRotation()
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

    private void RayCastForStreetView()
    {
        Vector3 _mousepos = Input.mousePosition;
        ray = Camera.main.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 10000))
        {
            //if (hit.collider.gameObject.name == "Point")
            //{
                Debug.DrawRay(Camera.main.transform.position, hit.transform.position);
                ArrowObject.transform.rotation = Quaternion.Euler(new Vector3(0,
                     Camera.main.transform.rotation.eulerAngles.y, 0));
                if(hit.collider.transform.position == Camera.main.transform.position)
                {
                    return;
                }
                ArrowObject.transform.position = Vector3.Lerp(ArrowObject.transform.position, hit.collider.transform.position,
                    Time.deltaTime * 2.0f);
                ArrowObject.transform.position = new Vector3(ArrowObject.transform.position.x, 1f, ArrowObject.transform.position.z);
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(MoveToPosition(Camera.main.transform, hit.transform.position, 2f));
                }
            //}

        }
    }

    IEnumerator MoveToPosition(Transform t, Vector3 finalPos, float smooth)
    {
        float time = 0f;
        float timeToMove = smooth;
        finalPos = new Vector3(finalPos.x, 29f, finalPos.z);

        while (time < 1)
        {
            time += Time.deltaTime / timeToMove;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,
                    finalPos, Time.deltaTime * smooth);
            yield return null;

        }

    }


}
