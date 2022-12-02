using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class BonusBlockScript : MonoBehaviour
{
    public GameObject textObject;
    public Text text;
    
    AudioSource audioSrc;
    public AudioClip hitSound;

    public GameDataScript gameData;

    public GameObject[] Bonuses; 

    Rigidbody2D rb;

    private Dictionary<string, int> randToBonus;
    private int sum; 

    private System.Random r; 

    // Start is called before the first frame update
    void Start()
    {
        r = new System.Random(); 
        audioSrc = Camera.main.GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        randToBonus = new Dictionary<string, int>(); 
        int x = 0; 
        foreach (var bonus in gameData.BonusDistribution)
        {
            x += bonus.Value; 
            randToBonus.Add(bonus.Key, x); 
        }
        sum = x; 
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameData.sound)
        {
            audioSrc.PlayOneShot(hitSound, 5);
        }
        Destroy(gameObject);
        double d = r.NextDouble() * sum; 
        string s = ""; 
        foreach (var item in randToBonus)
        {
            if (item.Value >= d)
            {
                s = item.Key;
                break; 
            }
        }

        var bonus = Instantiate(Array.Find(Bonuses, x => x.name == s), transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
