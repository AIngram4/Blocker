using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dot : MonoBehaviour {

    private int _value;
    private int _speed = 0;
    private Snake _snake;

    private Rigidbody2D _rig;

    public Canvas dotTextCanvasPrefab;
    private Canvas tempCanvas;

    public float test = 0;

	// Use this for initialization
	void Start () {
        _snake = GameObject.Find("Snake").GetComponent<Snake>();
        _rig = GetComponent<Rigidbody2D>();
        _value = Random.Range(1, 6);
        _rig.velocity = new Vector2(0, _speed);
        tempCanvas = Instantiate(dotTextCanvasPrefab, new Vector2(transform.position.x, transform.position.y-0.2f), Quaternion.identity);
        tempCanvas.GetComponentInChildren<Text>().text = "" + _value;


    }
	
	// Update is called once per frame
	void Update () {
        Move();

        if (GameManager.Instance.IsGameOver() == false && transform.position.y < GameManager.Instance.headNode.position.y - 6)
        {
            Destroy(tempCanvas.gameObject);
            Destroy(this.gameObject);
        }

        if (GameManager.Instance.ResetObjects())
        {
            Destroy(tempCanvas.gameObject);
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Snake")
        {
            _snake.AddNodes(_value);
            Destroy(this.gameObject);
            if (tempCanvas != null)
            {
                Destroy(tempCanvas.gameObject);
            }
            
        }
    }

    private void SlowToStop()
    {
        _rig.velocity = Vector2.zero;
    }

    private void Move()
    {
        if (GameManager.Instance.GetBlocksTouched() == 0 && GameManager.Instance.IsGameOver() == false)
        {
            transform.Translate(Vector3.down * Time.deltaTime * GameManager.Instance.GetBlockSpeed());
            tempCanvas.transform.position = transform.position;
        }

    }
}
