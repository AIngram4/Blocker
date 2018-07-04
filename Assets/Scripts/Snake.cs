using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{

    public List<Transform> Nodes = new List<Transform>();

    private float _moveSpeed = 10;
    private int _beginSize = 4;
    private int _layerLevel = -1;
    
    private Transform _curNode;
    private Transform _nextNode;

    private Vector2 mousePreviousPos, mouseCurrentPos;

    public GameObject headNodePrefab;
    public GameObject nodePrefab;
    private bool _snakeStarted = false;


    private Vector3 _circleCenter;

    private void Start()
    {
        AddNodes(1);
        GameManager.Instance.headNode = Nodes[0];
    }

    private void InstantiateSnake()
    {
        for (int i = 0; i < _beginSize; i++)
        {
            AddNodes(1);
        }
        GameManager.Instance.headNode = Nodes[0];
        UIManager.Instance.UpdateNumberOfNodesText(Nodes.Count-1);
        GameManager.Instance.GameStart();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver() == false && _snakeStarted == false)
        {
            _snakeStarted = true;
            InstantiateSnake();
        }

        if (GameManager.Instance.IsGameOver() == false)
        {
            Move();
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddNodes(4);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            RemoveNode();
        }
    }

    public void Move()
    {
        if (Input.GetMouseButton(0) && GameManager.Instance.IsGameOver() == false && Nodes.Count > 0)
        {
            Vector3 curPos = Input.mousePosition;
            curPos.z = -1;
            curPos = Camera.main.ScreenToWorldPoint(curPos);
            Nodes[0].GetComponent<Rigidbody2D>().velocity = new Vector2(curPos.x - Nodes[0].position.x, 0) * 20;


        }
        else
        {
            Nodes[0].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }

        for (int i = 1; i < Nodes.Count; i++)
        { 
            Vector2 newPos = new Vector2(Nodes[i-1].position.x, Nodes[i-1].position.y-0.35f);
            if (GameManager.Instance.GetPoweredUp())
            {
                Nodes[i].position = Vector2.Lerp(Nodes[i].position, newPos, Time.deltaTime * 14);
            }
            else
            {
                Nodes[i].position = Vector2.Lerp(Nodes[i].position, newPos, Time.deltaTime * 10);
            }
            
        }
    }

    public void AddNodes(int nodes)
    {
        for (int i = 0; i < nodes; i++)
        {
            Transform newNode;

            if (Nodes.Count > 0)
            {
                newNode = (Instantiate(nodePrefab, new Vector2(Nodes[Nodes.Count - 1].position.x, Nodes[Nodes.Count - 1].position.y - 0.35f), Quaternion.identity) as GameObject).transform;
            }
            else
            {
                if (GameManager.Instance.secondTry)
                {
                    newNode = (Instantiate(headNodePrefab, new Vector2(GameManager.Instance.lastBlockTouched.transform.position.x, 0), Quaternion.identity) as GameObject).transform;
                } else
                {
                    newNode = (Instantiate(headNodePrefab, Vector2.zero, Quaternion.identity) as GameObject).transform;
                    
                }
                GameManager.Instance.headNode = newNode;

            }

            newNode.SetParent(transform);
            SpriteRenderer tempSprite = newNode.GetComponent<SpriteRenderer>();
            tempSprite.sortingOrder = _layerLevel;
            _layerLevel--;

            Nodes.Add(newNode);
        }
        if (GameManager.Instance.IsGameOver() == false)
        {
            UIManager.Instance.UpdateNumberOfNodesText(Nodes.Count-1);
        }


    }

    public void RemoveNode()
    {
        if (Nodes.Count > 0)
        {
            GameObject temp = Instantiate(GameManager.Instance.explosionPrefab, Nodes[0].position, Quaternion.identity);
            Destroy(temp, 1.0f);
            Destroy(Nodes[Nodes.Count - 1].gameObject);
            Nodes.RemoveAt(Nodes.Count - 1);
            if (Nodes.Count == 0)
            {
                GameManager.Instance.SetGameOver();
            }
            _layerLevel++;

        }
        UIManager.Instance.UpdateNumberOfNodesText(Nodes.Count-1);

    }
}

