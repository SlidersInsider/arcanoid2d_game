using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = Camera.main.GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
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
        var bonus = Instantiate(Bonuses[Random.Range(0, Bonuses.Length)], transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
