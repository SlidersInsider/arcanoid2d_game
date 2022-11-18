using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Plus2 : BonusBase
{
    
    AudioSource audioSrc;

    Rigidbody2D rb;

    static ContactFilter2D contactFilter = new ContactFilter2D();

    public override void BonusActivate()
    {
        gameData.balls += 2;
        
        var player = GameObject.FindGameObjectWithTag("Player");
        var script = player.GetComponent<PlayerScript>(); 
        var ball = script.ballPrefab;

        for (int i = 0; i < 2; i++)
        {
            for (int k = 0; k < 20; k++) 
            {
                var obj = Instantiate(ball, new Vector3((Random.value * 2 - 1) * script.GetXMax(), Random.value * script.GetYMax(), 0), Quaternion.identity);
                if (obj.GetComponent<Collider2D>().OverlapCollider(contactFilter.NoFilter(), script.GetColliders()) == 0) 
                {
                    var brb = obj.GetComponent<Rigidbody2D>();
                    brb.isKinematic = false;
                    brb.AddForce(new Vector2(Random.Range(-300, 300), Random.Range(-300, 300)));
                    break;
                }
                Destroy(obj);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = Camera.main.GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
        rb.AddForce(new Vector2(0, -50));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
