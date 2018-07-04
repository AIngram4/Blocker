using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("There is no instance of UIManager.");
            }
            return _instance;
        }
    }
    private Animator _menuAnim, _tapAnim, _endMenuAnim;
    private GameObject _scorePanel;
    private bool _scoreActive = false;
    public Canvas snakeTextCanvas;
    public GameObject button;

    public Text _nodesText;
    public Text _scoreText;
    public Text _endHighScoreText;
    public Text _endScoreText;
    public Text _endCountdownText;
    public Text _bestScoreText;

    private void Awake()
    {
        _instance = this;
        _menuAnim = GameObject.Find("Menu").GetComponent<Animator>();
        _endMenuAnim = GameObject.Find("EndMenu").GetComponent<Animator>();
        _scorePanel = GameObject.Find("Score");
        _scoreText.text = "";
        SetBestScoreText();
        

    }

    private void Update()
    {
        if (GameManager.Instance.headNode != null)
        {
            snakeTextCanvas.transform.position = new Vector2(GameManager.Instance.headNode.position.x, snakeTextCanvas.transform.position.y);
        }
    }


    public void UpdateNumberOfNodesText(int nodes)
    {
        if (nodes > -1)
        {
            _nodesText.text = "" + nodes;
        } else
        {
            _nodesText.text = "" + 0;
        }
        
    }

    public void UpdateScoreText(int score)
    {
        if (_scoreActive == false)
        {
            _scorePanel.SetActive(true);
        }
        _scoreText.text = "" + score;
    }

    public void FadeMenu()
    {
        _menuAnim.SetTrigger("Exit");
    }

    public void EndMenuEnter()
    {
        _endMenuAnim.SetTrigger("GameOver");
        _endHighScoreText.text = "" + GameManager.Instance.GetHighScore();
        _endScoreText.text = "" + GameManager.Instance.GetScore();
        if (GameManager.Instance.EligibleForAd())
        {
            _endCountdownText.text = "" + 5;
            StartCoroutine(Countdown());
        } else
        {
            _endMenuAnim.SetTrigger("EndMenu");
            GameManager.Instance.SetOnEndMenu();
        }
        
        
    }
    
    private IEnumerator Countdown()
    {
        for (int i = 4; i >= 0; i--)
        {
            if (GameManager.Instance.IsGameOver() == false)
            {
                break;
            }
            yield return new WaitForSeconds(1);
            _endCountdownText.text = "" + i;

        }
        if (GameManager.Instance.IsGameOver())
        {
            _endMenuAnim.SetTrigger("EndMenu");
            GameManager.Instance.SetOnEndMenu();
        }
        


    }

    private void EndMenuExit()
    {
        _endMenuAnim.SetTrigger("Disapear");
    }

    private void StartMenuEnter()
    {
        _menuAnim.SetTrigger("Start");
    }

    public void ResetUI()
    {
        EndMenuExit();
        StartMenuEnter();
        _scoreText.text = "";
        _nodesText.text = "4";
        SetBestScoreText();
    }

    public void SetBestScoreText()
    {
        _bestScoreText.text = "" + GameManager.Instance.GetHighScore();
    }

    public void TryAgainUI()
    {
        button.SetActive(false);
        _endMenuAnim.SetTrigger("TryAgain");
    }
}
