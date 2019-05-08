using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFadeManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ScreenManager screenManager = GameObject.Find("Fade").GetComponent<ScreenManager>();
        StartCoroutine(screenManager.Fade(StartPrologue, 1));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void StartPrologue()
    {
        GameManager.Instance.IsStartPrologue = true;
    }
}
