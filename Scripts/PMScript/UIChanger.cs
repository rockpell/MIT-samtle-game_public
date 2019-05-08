using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChanger : MonoBehaviour {
    private UIManager _uiManager;
    private SoundManager _soundManager;
    private string _uiName;

	// Use this for initialization
	void Start () {
        _uiName = transform.Find("Text").GetComponent<Text>().text;
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onUIclick() {
        _uiManager.WorkUI(_uiName);
        if(_uiName == "뒤로가기" || _uiName == "취소" || _uiName == "보류" || _uiName =="안간다") {
            _soundManager.MenuClickNegativeSound();
        } else {
            _soundManager.MenuClickSound();
        }
        
    }

    public void ChangName(string name) {
        _uiName = name;
        transform.Find("Text").GetComponent<Text>().text = name;
    }
}
