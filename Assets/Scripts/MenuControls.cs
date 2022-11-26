using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MenuControls : MonoBehaviour
{
    public InputField inputNameField;
    [SerializeField] public Text inputNameText;
    public GameDataScript gameData;
    public Text player1;
    public Text player2;
    public Text player3;
    public Text player4;
    public Text player5;
    public Behaviour exitButton;
    public Behaviour exitText;
    public Behaviour congratsText;
    public Text congrats;

    public void PlayPressed()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ExitPressed()
    {
        gameData.playerName = "no_name";
        gameData.firstOpen = true;
        gameData.isNewBestResult = false;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void SaveName() 
    {
        gameData.playerName = inputNameText.text;
    }

    public void Start()
    {
        if (gameData.firstOpen)
        {
            exitButton.enabled = false;
            exitText.enabled = false;
            gameData.firstOpen = false;
        }
        else
        {
            exitButton.enabled = true;
            exitText.enabled = true;
        }
        if (!gameData.isNewBestResult)
        {
            congratsText.enabled = false;
        }
        else
        {
            congratsText.enabled = true;
            congrats.text = gameData.gratsText;
        }
        gameData.LoadPlayers();
        Debug.Log(gameData.topPlayers.Count);
        if (gameData.topPlayers.Count == 0) 
        {
            player5.text = "1. ";
            player4.text = "2. ";
            player3.text = "3. ";
            player2.text = "4. ";
            player1.text = "5. ";
        }
        if (gameData.topPlayers.Count == 1)
        {
            player5.text = "5. ";
            player4.text = "4. ";
            player3.text = "3. ";
            player2.text = "2. ";
            player1.text = "1. " + gameData.topPlayers[0].Key + " " + gameData.topPlayers[0].Value;
        }
        if (gameData.topPlayers.Count == 2)
        {
            player5.text = "5. ";
            player4.text = "4. ";
            player3.text = "3. ";
            player2.text = "2. " + gameData.topPlayers[1].Key + " " + gameData.topPlayers[1].Value;
            player1.text = "1. " + gameData.topPlayers[0].Key + " " + gameData.topPlayers[0].Value;
        }
        if (gameData.topPlayers.Count == 3)
        {
            player5.text = "5. ";
            player4.text = "4. ";
            player3.text = "3. " + gameData.topPlayers[2].Key + " " + gameData.topPlayers[2].Value;
            player2.text = "2. " + gameData.topPlayers[1].Key + " " + gameData.topPlayers[1].Value;
            player1.text = "1. " + gameData.topPlayers[0].Key + " " + gameData.topPlayers[0].Value;
        }
        if (gameData.topPlayers.Count == 4)
        {
            player5.text = "5. ";
            player4.text = "4. " + gameData.topPlayers[3].Key + " " + gameData.topPlayers[3].Value;
            player3.text = "3. " + gameData.topPlayers[2].Key + " " + gameData.topPlayers[2].Value;
            player2.text = "2. " + gameData.topPlayers[1].Key + " " + gameData.topPlayers[1].Value;
            player1.text = "1. " + gameData.topPlayers[0].Key + " " + gameData.topPlayers[0].Value;
        }
        if (gameData.topPlayers.Count == 5)
        {
            player5.text = "5. " + gameData.topPlayers[4].Key + " " + gameData.topPlayers[4].Value;
            player4.text = "4. " + gameData.topPlayers[3].Key + " " + gameData.topPlayers[3].Value;
            player3.text = "3. " + gameData.topPlayers[2].Key + " " + gameData.topPlayers[2].Value;
            player2.text = "2. " + gameData.topPlayers[1].Key + " " + gameData.topPlayers[1].Value;
            player1.text = "1. " + gameData.topPlayers[0].Key + " " + gameData.topPlayers[0].Value;
        }
    }
}
