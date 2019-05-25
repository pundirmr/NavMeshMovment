using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Manager : MonoBehaviour {

	public GameObject[] Roads;

	void Start ()
	{
		for (int i = 0; i < Roads.Length; i++)
		{
			Roads [i].AddComponent<NavMeshSurface> ();
		}
		for (int i = 0; i < Roads.Length; i++) 
		{
			Roads [i].GetComponent<NavMeshSurface>().BuildNavMesh ();    
		} 
	}		
}
