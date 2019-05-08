using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController_Interview : MonoBehaviour {

    public int word_id;
    private string word_value;
    private int property;
    private Text myText;

    [SerializeField] private InterviewMainController interMC;
    // Use this for initialization
    void Start () {
        myText = GetComponent<Text>();
        //interMC = GameObject.Find("note").GetComponent<InterviewMainController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onWordClick() {
        if(!interMC.GetIsSelectOver()) {
            interMC.affectGage(property);
            interMC.reLoadWords();
            interMC.AddSelectWordTime();
        }
    }

    public void setText(string value) {
        myText.text = value;
    }

    public void setProperty(int value) {
        property = value;
    }

    public int getProperty() {
        return property;
    }
}
