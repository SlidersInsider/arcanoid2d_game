using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XOScript : MonoBehaviour
{
    public GameObject textObject;
    public Text textComponent;
    public int points;
    PlayerScript playerScript;

    void Start()
    {
        if (textObject != null)
        {
            textComponent = textObject.GetComponent<Text>();
            textComponent.text = "O";
        }
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    // обрабатываем касание блока XO
    private void OnCollisionEnter2D(Collision2D collision)
    {
        textComponent = textObject.GetComponent<Text>();
        textComponent.text = textComponent.text == "X" ? "O" : "X";

        playerScript.CheckXO(points); 
    }
}
