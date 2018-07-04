using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCollider : MonoBehaviour {

    private Transform _blockTransform;
    private BoxCollider2D _collider;
    private Rigidbody2D _rig;

    private float _entryX;

	// Use this for initialization
	void Start () {
		_blockTransform = GetComponentInParent<Transform>();
        _collider = GetComponent<BoxCollider2D>();
        _rig = GetComponent<Rigidbody2D>();
        _rig.velocity = new Vector2(0,-4);
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.GetBlocksTouched() > 0)
        {
            SlowToStop();
        }
        else
        {
            _rig.velocity = new Vector2(0, -5);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Snake")
        {
            Debug.Log("Trigger Entered");
            _entryX = collision.gameObject.transform.position.x;
            Transform tempNode = collision.transform;
            tempNode.position = new Vector2(_entryX,collision.transform.position.y);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Snake")
        {
            Debug.Log("Trigger Staying");
            Transform tempNode = collision.transform;
            tempNode.position = new Vector2(_entryX, collision.transform.position.y);
        }
    }

    private void SlowToStop()
    {
        //_rig.velocity = Vector2.Lerp(_rig.velocity, new Vector2(0, 0), Time.deltaTime * 10);
        _rig.velocity = Vector2.zero;
    }
}
