using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisenCloudScript : MonoBehaviour
{
    public float spawnDuration = 10;
    private float spawnTime;
    public float damageRate = 1;
    public int damage = 5;
    private float lastHit = 0;
    private PlayerBehavior playerBehavior;
    private bool colliding = false;

    // Start is called before the first frame update
    void Start()
    {
        playerBehavior = GameObject.FindWithTag("Player").GetComponent<PlayerBehavior>();
        spawnTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup >= spawnDuration + spawnTime)
        {
            Destroy(this);
        }

        if (colliding && (lastHit == 0 || Time.realtimeSinceStartup >= lastHit + damageRate))
        {
            lastHit = Time.realtimeSinceStartup;
            playerBehavior.hit(damage);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            colliding = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            colliding = false;
        }
    }
}
