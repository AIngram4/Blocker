using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    private Animator _anim;
    private bool _playing = false;

	// Use this for initialization
	void Start ()
    {
        _anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (GameManager.Instance.GetPoweredUp() && _playing == false)
        {
            _anim.SetTrigger("Special");
            _playing = true;
        }
        else if (GameManager.Instance.GetPoweredUp() == false && _playing == true)
        {
            _anim.SetTrigger("Done");
            _playing = false;
        }
	}
}
