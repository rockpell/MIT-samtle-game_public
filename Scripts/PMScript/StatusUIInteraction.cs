using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUIInteraction : MonoBehaviour {

    private Slider _statusBar;
    private Text _statusValue;
    private Text _uiName;

	// Use this for initialization
	void Start () {
        _statusBar = transform.Find("StatusBar").GetComponent<Slider>();
        _statusValue = transform.Find("StatusValue").GetComponent<Text>();
        _uiName = transform.Find("StatusName").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        _statusValue.text = _statusBar.value.ToString();
    }

    public void ChangeName(string name) {
        _uiName.text = name;
    }
}
