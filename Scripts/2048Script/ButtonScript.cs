using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void restartGame() {
        SceneManager.LoadScene("2048(256)");
    }

    public void loadMainMenu() {
    }

    public void loadGame() {
    }

    public void exitGame() {
        //Application.Quit();
    }
}
