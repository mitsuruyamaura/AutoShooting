using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float coefficient; 
    public float speed;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            return;
        }
        GameObject obj = col.gameObject;
        var velocity = (obj.transform.position - transform.position).normalized * speed;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.AddForce(coefficient * velocity);
    }
}
