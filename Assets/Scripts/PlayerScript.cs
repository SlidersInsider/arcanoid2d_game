using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerScript : MonoBehaviour
{
    const int maxLevel = 30;

    [Range(1, maxLevel)]
    public int level = 1;
    public float ballVelocityMult = 0.02f;
    public GameObject bluePrefab;
    public GameObject redPrefab;
    public GameObject greenPrefab;
    public GameObject yellowPrefab;
    public GameObject ballPrefab;
    public GameObject bonusPrefab;
    public GameObject XOPrefab; 
    static Collider2D[] colliders = new Collider2D[50];
    static ContactFilter2D contactFilter = new ContactFilter2D();
    public GameDataScript gameData;
    static bool gameStarted = false;
    AudioSource audioSrc;
    public AudioClip pointSound;
    public Behaviour pauseCanvas;


    float yMaxS;
    float xMaxS;

    int requiredPointsToBall
    {
        get 
        {
            return 400 + (level - 1) * 20;
        }
    }

    public Collider2D[] GetColliders() => colliders; 
    public float GetYMax() => yMaxS; 
    public float GetXMax() => xMaxS; 

    public void CheckXO(int points)
    {
        var xo = GameObject.FindGameObjectsWithTag("XO"); 
        if (xo.All(x => x.GetComponent<XOScript>().textComponent.text == "X"))
        {
            for (int i = 0; i < xo.Length; ++i)
                Destroy(xo[i]);
            BlockDestroyed(points);
        }
    }

    void CreateBlocks(GameObject prefab, float xMax, float yMax, int count, int maxCount)
    {
        if (count > maxCount) 
        {
            count = maxCount;
        }
        for (int i = 0; i < count; i++)
        {
            for (int k = 0; k < 20; k++) 
            {
                var obj = Instantiate(prefab, new Vector3((Random.value * 2 - 1) * xMax, Random.value * yMax, 0), Quaternion.identity);
                if (obj.GetComponent<Collider2D>().OverlapCollider(contactFilter.NoFilter(), colliders) == 0) 
                {
                    break;
                }
                Destroy(obj);
            }
        }
    }

    void CreateBalls()
    {
        int count = 2;
        if (gameData.balls == 1)
        {
            count = 1;
        }
        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(ballPrefab);
            var ball = obj.GetComponent<BallScript>();
            ball.ballInitialForce += new Vector2(10 * i, 0);
            ball.ballInitialForce *= 1 + level * ballVelocityMult;
        }
    }

    void SetBackground()
    {
        var bg = GameObject.Find("Background").GetComponent<SpriteRenderer>();
        bg.sprite = Resources.Load(level.ToString("d2"), typeof(Sprite)) as Sprite;
    }

    IEnumerator BallDestriyedCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (GameObject.FindGameObjectsWithTag("Ball").Length == 0)
        {
            if (gameData.balls > 0)
            {
                CreateBalls();
            }
            else
            {  
                gameData.topPlayers.Add(new KeyValuePair<string, int>(gameData.playerName, gameData.points));
                if (gameData.topPlayers[gameData.topPlayers.Count - 1].Key == "") 
                {
                    string baseName = "no_name";
                    int playerPoints = gameData.topPlayers[gameData.topPlayers.Count - 1].Value;
                    gameData.topPlayers.RemoveAt(gameData.topPlayers.Count - 1);
                    gameData.topPlayers.Add(new KeyValuePair<string, int>(baseName, playerPoints));
                }
                gameData.topPlayers.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
                if (gameData.topPlayers.Count > 5) 
                {
                    gameData.topPlayers.RemoveAt(gameData.topPlayers.Count - 1);
                }

                gameData.SavePlayers(gameData.topPlayers);

                if (gameData.topPlayers.Contains(new KeyValuePair<string, int>(gameData.playerName, gameData.points)))
                {
                    gameData.gratsText = "Congratulations! New best results by "+ gameData.playerName +" with " + gameData.points + " points!";
                    gameData.isNewBestResult = true;
                }
                else
                {
                    gameData.isNewBestResult = false;
                }

                gameData.Reset();
                SceneManager.LoadScene("Menu");
            }
            
        }
    }

    public void BallDestroyed()
    {
        gameData.balls--;
        StartCoroutine(BallDestriyedCoroutine());
    }

    IEnumerator BlockDestroyedCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (GameObject.FindGameObjectsWithTag("Block").Length == 0)
        {
            if (level < maxLevel)
            {
                gameData.level++;
            }
            SceneManager.LoadScene("MainScene");
        }
    }

    IEnumerator BlockDestroyedCoroutine2()
    {
        for (int i = 0; i < 10; i++) 
        {
            yield return new WaitForSeconds(0.2f);
            audioSrc.PlayOneShot(pointSound, 5);
        }
    }

    public void BlockDestroyed(int points)
    {
        gameData.points += points;
        if (gameData.sound)
        {
            audioSrc.PlayOneShot(pointSound, 5);
        }
        gameData.pointsToBall += points;
        if (gameData.pointsToBall >= requiredPointsToBall)
        {
            gameData.balls++;
            gameData.pointsToBall -= requiredPointsToBall;
            if (gameData.sound) 
            {
                StartCoroutine(BlockDestroyedCoroutine2());
            }
        }
        StartCoroutine(BlockDestroyedCoroutine());
    }

    void SetMusic()
    {
        if (gameData.music)
        {
            audioSrc.Play();
        }
        else
        {
            audioSrc.Stop();
        }
    }

    void StartLevel()
    {
        SetBackground();

        CreateBlocks(XOPrefab, xMaxS, yMaxS, System.Math.Max(2, level / 2), 4);
        CreateBlocks(bonusPrefab, xMaxS, yMaxS, level, 3);
        CreateBlocks(bluePrefab, xMaxS, yMaxS, level, 6);
        CreateBlocks(redPrefab, xMaxS, yMaxS, 1 + level, 8);
        CreateBlocks(greenPrefab, xMaxS, yMaxS, 1 + level, 8);
        CreateBlocks(yellowPrefab, xMaxS, yMaxS, 2 + level, 8);
        CreateBalls();
    }

    void Start()
    {
        yMaxS = Camera.main.orthographicSize * 0.8f;
        xMaxS = Camera.main.orthographicSize * Camera.main.aspect * 0.70f;
        audioSrc = Camera.main.GetComponent<AudioSource>();
        Cursor.visible = false;
        if (!gameStarted)
        {
            gameStarted = true;
            if (gameData.resetOnStart)
            {
                gameData.Load();
            }
        }
        level = gameData.level;
        SetMusic();
        StartLevel();
        pauseCanvas.enabled = false;
    }

    void Update()
    {
        if (Time.timeScale > 0) 
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var pos = transform.position;
            pos.x = mousePos.x;
            transform.position = pos;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            gameData.music = !gameData.music;
            SetMusic();
        }
        if (Input.GetKeyDown(KeyCode.S)) 
        {
            gameData.sound = !gameData.sound;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0;
                pauseCanvas.enabled = true;
            }
            else
            {
                Time.timeScale = 1;
                pauseCanvas.enabled = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            gameData.Reset();
            SceneManager.LoadScene("MainScene");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    private void OnApplicationQuit()
    {
        gameData.Save();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(5, 4, Screen.width - 10, 100), 
            string.Format("<color=yellow><size=30>Level <b>{0}</b> Balls <b>{1}</b> Score <b>{2}</b> Player: <b>{3}</b></size></color>", 
            gameData.level, gameData.balls, gameData.points, gameData.playerName));
        /*GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperRight;
        GUI.Label(new Rect(5, 14, Screen.width - 10, 100),
            string.Format(
                "<color=yellow><size=20><color=white>Space</color>-pause {0} " +
                "<color=white>N</color>-new " +
                "<color=white>J</color>-jump " +
                "<color=white>M</color>-music {1} " +
                "<color=white>S</color>-sound {2} " +
                "<color=white>Esc</color>-exit</size></color>",
                OnOff(Time.timeScale > 0), OnOff(!gameData.music), OnOff(!gameData.sound)), style);*/
    }

    string OnOff(bool boolVal)
    {
        return boolVal ? "off" : "on";
    }

    public void StartNewGame() 
    {
        pauseCanvas.enabled = false;
        Time.timeScale = 1;
        gameData.Reset();
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame()
    {
        pauseCanvas.enabled = false;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
