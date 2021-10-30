using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTest : MonoBehaviour
{
    float speed = 0;
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            this.speed = 0.2f;
            Debug.Log(this.speed);
        }
        transform.Translate(this.speed, 0, 0);
        this.speed *= 0.98f;
    }
}
