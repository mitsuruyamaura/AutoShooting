using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {

    Camera mapCamera;

	// Use this for initialization
	void Start () {
        mapCamera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.K))
        {
            mapCamera.enabled = !mapCamera.enabled;
        }
	}
}
