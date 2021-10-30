using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazzleShot : MonoBehaviour
{
    public FireProjectile fireProjectilePrefab;

    public void CreateFireProjectile(int power, float speed, float duration) {
        FireProjectile fireProjectile = Instantiate(fireProjectilePrefab, transform.position, Quaternion.identity);
        fireProjectile.InitFireProjectile(power, duration);
        fireProjectile.rb.AddForce(transform.forward * speed);

        // TODO SE

    }
}
