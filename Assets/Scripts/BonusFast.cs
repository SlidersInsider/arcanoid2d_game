using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class BonusFast : BonusBase
{
    
    AudioSource audioSrc;

    Rigidbody2D rb;

    // создаем нужный нам бонус быстрого шарика
    public override void BonusActivate()
    {
        if (gameData.sound)
        {
            audioSrc.PlayOneShot(pointSound, 5);
        }
        var balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (var ball in balls)
        {
            var rb = ball.GetComponent<Rigidbody2D>(); 
            rb.velocity *= 1.1f;
        }
    }

    void Start()
    {
        audioSrc = Camera.main.GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
        rb.AddForce(new Vector2(0, -50));
    }

    // активируем наш бонус быстрого шарика
    private void OnCollisionEnter2D(Collision2D collision)
    {
        BonusActivate();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameData.sound)
        {
            audioSrc.PlayOneShot(loseSound, 5);
        }
        Destroy(gameObject);
    }
}
