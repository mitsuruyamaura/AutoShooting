using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour {
    
    Canvas canvas;

	// Use this for initialization
	void Start () {
        canvas = GetComponent<Canvas>();
	}

    // Update is called once per frame
    void Update()
    {
        //  表示していれば非表示。逆なら表示
        if (Input.GetKeyDown("p"))
        {
            canvas.enabled = !canvas.enabled;
        }
    }
}
