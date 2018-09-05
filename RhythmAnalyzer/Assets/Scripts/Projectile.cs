using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int speed = 25;
    public float jumpHeight = 1f;
    public float gravity = 5f;

    private void Update()
    {
        Vector3 jumpPos = transform.position;

        //Apply gravity
        jumpPos.y += Mathf.Lerp(0, jumpHeight, Time.deltaTime);
        jumpHeight -= Time.deltaTime * gravity;

        //Move forward
        jumpPos.x -= Time.deltaTime * speed;
        transform.position = jumpPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //Deal damage to player
            Player.Instance.InstaKill();
        }
    }
}
