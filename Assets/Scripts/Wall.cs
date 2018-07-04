using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Move();

        if (GameManager.Instance.IsGameOver() == false && transform.position.y < GameManager.Instance.headNode.position.y - 6)
        {
            Destroy(this.gameObject);
        }

        if (GameManager.Instance.ResetObjects())
        {
            Destroy(this.gameObject);
        }
    }

    private void Move()
    {
        if (GameManager.Instance.GetBlocksTouched() == 0 && GameManager.Instance.IsGameOver() == false)
        {
            transform.Translate(Vector3.down * Time.deltaTime * GameManager.Instance.GetBlockSpeed());
        }
    }
}
