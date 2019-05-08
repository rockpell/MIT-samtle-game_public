using UnityEngine;
using UnityEngine.UI;

public class ScheduleUIInteration : MonoBehaviour {
    private UIManager _uiManager;
    private SoundManager _soundManager;

    public bool _isSelected;
    private bool _isSelectable;
    private string _uiName;

	// Use this for initialization
	void Start () {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _uiName = transform.Find("Text").GetComponent<Text>().text;
        _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        _isSelectable = true;
    }
	
	// Update is called once per frame
	void Update () {
        if(_uiName == "공부" || _uiName == "알바") {
            if(StatusManager.Instance.IsStress(_uiName)) {// stress가 많으면 true 반환
                _isSelectable = false;
                GetComponent<Image>().color = Color.red;
                // 색깔변화
            } else {
                _isSelectable = true;
                GetComponent<Image>().color = Color.white;
                // 색깔 복구
            }
        }

        if (_uiName == "여행")
        {
            if(StatusManager.Instance.Money < 20) // 여행 비용
            {
                _isSelectable = false;
                GetComponent<Image>().color = Color.red;
            } else
            {
                _isSelectable = true;
                GetComponent<Image>().color = Color.white;
            }
        }
	}

    public void onUIClick() {
        if(!_isSelectable) {
            return;
        }

        if(!_isSelected) {
            _uiManager.selectSchedule(this.gameObject);
            _soundManager.MenuClickSound();
        } else {
            SelfDestroy();
            _uiManager.RemoveInBottleSchedule(this.gameObject);
            _soundManager.MenuClickNegativeSound();
        }
        _uiManager.DecideAcceptButton();
    }

    public void SetIsSelected() {
        _isSelected = true;
    }

    public void SelfDestroy() {
        Destroy(this.gameObject);
    }
}
