using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetViewPoint : MonoBehaviour {

    public int wayPointId = 0;

    private void Start()
    {
        wayPointId = UnityEngine.Random.Range(1, 10000);
    }

}
