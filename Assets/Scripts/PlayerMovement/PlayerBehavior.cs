using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{

    public Image damageScreen;
    public Image youDiedScreen;
    private Color alphaColor;
    private float spawnTime;
    public GameObject spawnPoint;
    public GameObject teleportLocation;

    public int health = 50;
    private int startHealth;

    // Start is called before the first frame update
    void Start()
    {
        alphaColor = damageScreen.color;
       startHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            alphaColor.a = 1;
            youDiedScreen.color = alphaColor;
        }

        if (Time.realtimeSinceStartup >= spawnTime + 0.35)
        {
            spawnTime = 0;
            alphaColor.a = 0;
            damageScreen.color = alphaColor;
        }
    }

    public void hit(int damage) 
    {
        Debug.Log("Hit!");
        health -= damage;
        alphaColor.a = 1;
        damageScreen.color = alphaColor;
        spawnTime = Time.realtimeSinceStartup;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            Debug.Log("Key!");
            transform.position = teleportLocation.transform.position;
        }
    }
}
