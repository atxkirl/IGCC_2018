using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int speed;
    public int damage;

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Deal Damage
        //Check if colliding object has HealthBase script attached
        if (other.gameObject.GetComponent<Health>())
        {
            other.gameObject.GetComponent<Health>().ModifyCurrentHealth(-damage);
        }
    }
}
