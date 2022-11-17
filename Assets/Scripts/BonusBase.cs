using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class BonusBase : MonoBehaviour
{
    public GameObject textObject;
    public Text text;
    public Color bonusColor; 
    public Color textColor; 
    
    AudioSource audioSrc;
    public AudioClip hitSound;
    public AudioClip loseSound;
    public AudioClip pointSound;
    public GameDataScript gameData;

    Rigidbody2D rb;

    public virtual void BonusActivate()
    {
        gameData.points += 100;
        if (gameData.sound)
        {
            audioSrc.PlayOneShot(pointSound, 5);
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
