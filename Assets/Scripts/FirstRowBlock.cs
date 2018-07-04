using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstRowBlock : MonoBehaviour
{

    private Snake _snake;
    private SpriteRenderer _blockSprite, _numberSprite;
    private Rigidbody2D _rig;
    private Animator _anim;

    [SerializeField]
    private bool _canCollide = true;
    private bool _beingTouched = false;
    private int _blockSize;


    void Start()
    {
        _snake = GameObject.Find("Snake").GetComponent<Snake>();
        _blockSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _numberSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
        _rig = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _blockSize = CurveWeightedRandom();
        UpdateColor();
    }


    void Update()
    {
        if (GameManager.Instance.GetBlocksTouched() == 0 && GameManager.Instance.IsGameOver() == false)
        {
            Move();
        }


        if (GameManager.Instance.IsGameOver() == false && transform.position.y < GameManager.Instance.headNode.position.y - 6)
        {
            Destroy(this.gameObject);
        }

        if (GameManager.Instance.ResetObjects())
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.gameObject.tag == "Snake" && _canCollide == true)
        {

            GameManager.Instance.IncrementBlocksTouched();
            _rig.velocity = Vector3.zero;
            _beingTouched = true;

        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other != null && other.gameObject.tag == "Snake" && _canCollide == true)
        {
            GameManager.Instance.DecrementBlocksTouched();
            _beingTouched = false;

        }


    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_beingTouched && other.gameObject.tag == "Snake")
        {
            _anim.SetTrigger("Hit");
            _blockSize--;
            _snake.RemoveNode();
            GameManager.Instance.IncrementScore(1);

            if (_blockSize <= 0)
            {
                GameManager.Instance.DecrementBlocksTouched();
                Destroy(this.gameObject);
            }
            else
            {
                UpdateColor();
            }
            StartCoroutine(CollisionCooldown());
        }


    }

    private void UpdateColor()
    {
        Color newCol;
        ColorUtility.TryParseHtmlString("" + GameManager.Instance.colors[_blockSize - 1], out newCol);
        _blockSprite.color = newCol;
        _numberSprite.sprite = GameManager.Instance.numbers[_blockSize - 1];
    }

    public int CurveWeightedRandom()
    {
        return Random.Range(1, 3);
    }

    private void SlowToStop()
    {
        _rig.velocity = Vector2.zero;
    }

    private void Move()
    {
        transform.Translate(Vector3.down * Time.deltaTime * GameManager.Instance.GetBlockSpeed());

    }

    private IEnumerator CollisionCooldown()
    {
        _beingTouched = false;
        yield return new WaitForSeconds(0.05f);
        _beingTouched = true;
    }
}
