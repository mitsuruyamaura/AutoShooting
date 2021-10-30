using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public Rigidbody rb;

    public int attackPower;

    private const string PLAYER = "Player";

    private float bulletDuration = 1.0f;   // 弾の生存時間

    public void InitFireProjectile(int power, float duration) {
        attackPower = power;
        bulletDuration = duration;
    }

    private void OnCollisionEnter(Collision col) {
        if (col.gameObject.CompareTag(PLAYER)) {
            return;
        }
        Destroy(gameObject);
    }

    void Update() {
        bulletDuration -= Time.deltaTime;

        if (bulletDuration <= 0) {
            Destroy(gameObject);
        }
    }
}
