using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationScene2Handler : MonoBehaviour {

    private float _time;
	// Use this for initialization
	void Start () {
        _time = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Time.time - _time > 12)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
	}
}
