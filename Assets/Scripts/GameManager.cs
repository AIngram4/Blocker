using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform headNode;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("There is no instance of GameManager.");
            }
            return _instance;
        }
    }
    
    private int _blocksTouched = 0;
    private int _score = 0;
    public AnimationCurve curve;
    public GameObject explosionPrefab;
    public GameObject snakePrefab;
    private bool _isGameOver = true;
    private bool _isFirstTime = true;
    public string[] colors;
    public Sprite[] numbers;
    private bool _eligibleForAd = true;
    private int _highScore;
    private bool _onEndScreen = false;
    private bool _onBeginScreen = true;
    private bool _resetObjects = true;
    public bool secondTry = false;
    private GameObject _snake;
    private BlockSpawnManager _bsm;
    public GameObject lastBlockTouched;
    private int _startBlockSize = 25;
    private bool _isPoweredUp;
    private int _blockSpeed = 4;
    private float _screenPixelWidth;
    public GameObject leftWall, rightWall;
    public GameObject block;
    public int screenRatio;
    public float[] xPositions = new float[5];


    private void Awake()
    {
        _instance = this;
        _snake = Instantiate(snakePrefab, Vector3.zero, Quaternion.identity);
        _snake.name = "Snake";
        _highScore = PlayerPrefs.GetInt("highScore", 0);
        _bsm = GameObject.Find("GameManager").GetComponent<BlockSpawnManager>();
        _screenPixelWidth = Screen.width / 70.0f * 2;
    }

    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space) && _onBeginScreen == true || Input.GetMouseButtonDown(0) && _onBeginScreen == true)
        {
            _resetObjects = false;
            UIManager.Instance.FadeMenu();
            _isGameOver = false;
            _bsm.Restart();

        } else if (Input.GetKeyDown(KeyCode.Space) && _onEndScreen == true || Input.GetMouseButtonDown(0) && _onEndScreen == true)
        {
            Restart();
        }

        if (_blocksTouched < 0)
        {
            _blocksTouched = 0;
        }
    }

    public void SetGameOver()
    {
        _isGameOver = true;
        if (_score > _highScore)
        {
            SetHighScore(_score);
        }
        UIManager.Instance.EndMenuEnter();

        

    }

    public int GetBlocksTouched()
    { 
        return _blocksTouched;
    }

    public void IncrementBlocksTouched()
    {
        _blocksTouched++;
    }

    public void DecrementBlocksTouched()
    {
        _blocksTouched--;
    }
    public void IncrementScore(int score)
    {
        _score+=score;
        UIManager.Instance.UpdateScoreText(_score);
    }
    public int GetScore()
    {
        return _score; 
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }

    public bool IsFirstTime()
    {
        return _isFirstTime;
    }

    public void SetFirstTime(bool value)
    {
        _isFirstTime = value;
    }

    public void GameStart()
    {
        _isGameOver = false;
        _onBeginScreen = false;
    }

    private void SetHighScore(int score)
    {
        _highScore = score;
        PlayerPrefs.SetInt("highScore", _highScore);
    }

    public int GetHighScore()
    {
        return _highScore;
    }

    private void Restart()
    {
        UIManager.Instance.ResetUI();
        _resetObjects = true;
        _score = 0;
        Destroy(_snake.gameObject);
        _snake = Instantiate(snakePrefab, Vector2.zero, Quaternion.identity);
        _snake.name = "Snake";
        _highScore = PlayerPrefs.GetInt("highScore", 0);
        _onEndScreen = false;
        _onBeginScreen = true;
        _isFirstTime = true;
        _blocksTouched = 0;
        _eligibleForAd = true;
        _startBlockSize = 25;
    }

    public bool ResetObjects()
    {
        return _resetObjects;
    }

    public void SetOnEndMenu()
    {
        _onEndScreen = true;
    }

    public void OneMoreTry()
    {
        secondTry = true;
        _isGameOver = false;
        _snake.GetComponent<Snake>().AddNodes(5);
        UIManager.Instance.TryAgainUI();
        _eligibleForAd = false;
        Destroy(lastBlockTouched.gameObject);
        _blocksTouched = 0;      
        
    }

    public bool EligibleForAd()
    {
        return _eligibleForAd;
    }

    public int GetStarBlockSize()
    {
        return _startBlockSize;
    }

    public void IncrementStarBlockSize()
    {
        if (_startBlockSize < 50)
        {
            _startBlockSize += 5;
        }
        
    }


    public void DestroyedSpecialBlock()
    {
        _isPoweredUp = true;
        _blockSpeed = 6;
        StartCoroutine(PowerUpCooldown());
    }

    public bool GetPoweredUp()
    {
        return _isPoweredUp;
    }

    private IEnumerator PowerUpCooldown()
    {
        yield return new WaitForSeconds(10.0f);
        _blockSpeed = 4;
        _isPoweredUp = false;
    }

    public int GetBlockSpeed()
    {
        return _blockSpeed;
    }

}

