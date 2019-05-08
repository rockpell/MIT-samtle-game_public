using UnityEngine;
using UnityEngine.UI;

public class ItemUIInteraction : MonoBehaviour {
    private UIManager _uiManager;
    private SoundManager _soundManager;
    private string _uiName;

    // Use this for initialization
    void Start () {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _uiName = gameObject.transform.Find("Item_name").GetComponent<Text>().text;
        _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnUIClick() {
        _uiManager.SelectShoppingItem(_uiName);
        _soundManager.MenuClickSound();
    }
}
