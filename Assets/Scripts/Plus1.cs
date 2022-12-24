using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Plus1 : BonusBase
{
    
    AudioSource audioSrc;

    Rigidbody2D rb;

    // добавляем +1 шар
    public override void BonusActivate()
    {
        gameData.balls++;
    }

    void Start()
    {
        audioSrc = Camera.main.GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
        rb.AddForce(new Vector2(0, -50));
    }


    // активируем бонус
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
