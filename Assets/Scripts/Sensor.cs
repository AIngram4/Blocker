using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour {

    private Rigidbody2D _rig;
    
	void Start () {
        _rig = GetComponent<Rigidbody2D>();
        _rig.velocity = new Vector2(0,0);
	}

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (GameManager.Instance.GetBlocksTouched() == 0 && GameManager.Instance.IsGameOver() == false)
        {
            transform.Translate(Vector3.down * Time.deltaTime * GameManager.Instance.GetBlockSpeed());
        }

    }
}
