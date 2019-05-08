using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CutScene
{
    public List<Sprite> _imageList;

    public bool _isAnimation = false;
    public int _frameRate = 10;

    private int _index = 0;
    private int _frame = 0;

    public void ViewImage()
    {
        if ((!_isAnimation) && _index >= _imageList.Count)
        {
            _index = 0;
            return;
        }
        _frame++;
        if (_frame < _frameRate)
            return;
        CutSceneSwap.SetImage(_imageList[_index]);
        _frame = 0;
        if(_isAnimation)
            _index++;

        if (_index >= _imageList.Count)
            _index = 0;
    }
}

[System.Serializable]
public class CutSceneSwap : MonoBehaviour
{
    public List<CutScene> _cutSceneList;
    public int _cutSceneIndex = 0;
    public bool _isActive = false;

    private static Image _image;
    private static float _imageWidth;
    private static float _imageHeight;
    // Use this for initialization
    void Start ()
    {
        _image = GetComponent<Image>();
        _imageWidth = _image.rectTransform.rect.width;
        _imageHeight = _image.rectTransform.rect.height;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_isActive)
        {
            _image.color = new Color(255,255,255,255);
            _cutSceneList[_cutSceneIndex].ViewImage();
        }
        else
        {
            _image.color = new Color(255, 255, 255, 0);
        }
	}

    public void SwapCutScene(string eventName)
    {
        StatusManager statusManager = StatusManager.Instance; 
        switch (eventName)
        {
            case "수업":
                _cutSceneIndex = 0;
                statusManager.AddValueStatus("코딩력", 10);
                statusManager.AddValueStatus("대인관계", 5);
                statusManager.AddValueStatus("스트레스", 10);
                UIManager.Instance.ChangeDiaglogTextSchedule("코딩수업을 듣습니다.");
                break;
            case "공부":
                _cutSceneIndex = 1;
                statusManager.AddValueStatus("학점", 10);
                statusManager.AddValueStatus("스트레스", 30);
                UIManager.Instance.ChangeDiaglogTextSchedule("학과공부를 합니다.");
                break;
            case "휴식":
                _cutSceneIndex = 2;
                statusManager.AddValueStatus("신앙심", 10);
                statusManager.AddValueStatus("스트레스", -20);
                UIManager.Instance.ChangeDiaglogTextSchedule("휴식~");
                break;
            case "술자리":
                _cutSceneIndex = 3;
                statusManager.AddValueStatus("알콜수치", 15);
                statusManager.AddValueStatus("대인관계", 10);
                statusManager.AddValueStatus("스트레스", -25);
                UIManager.Instance.ChangeDiaglogTextSchedule("술을 마십니다.");
                break;
            case "여행":
                _cutSceneIndex = 4;
                statusManager.AddValueStatus("감수성", 10);
                statusManager.AddValueStatus("대인관계", 10);
                statusManager.AddValueStatus("스트레스", -40);
                statusManager.AddMoney(-20);
                UIManager.Instance.ChangeDiaglogTextSchedule("여행을 떠나요~");
                break;
            case "알바":
                _cutSceneIndex = 5;
                statusManager.AddValueStatus("스트레스", 25);
                statusManager.AddMoney(50);
                UIManager.Instance.ChangeDiaglogTextSchedule("돈을 벌자!");
                break;
            case "봄소풍":
                _cutSceneIndex = 6;
                UIManager.Instance.ChangeDiaglogTextSchedule("봄소풍");
                break;
            case "MT":
                _cutSceneIndex = 7;
                UIManager.Instance.ChangeDiaglogTextSchedule("MT");
                break;
            case "체전":
                _cutSceneIndex = 8;
                UIManager.Instance.ChangeDiaglogTextSchedule("체전");
                break;
            case "자유행동":
                SceneActive(false);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Gold");
                break;
            case "수업2":
                _cutSceneIndex = 6;
                statusManager.AddValueStatus("코딩력", 10);
                statusManager.AddValueStatus("대인관계", 5);
                statusManager.AddValueStatus("스트레스", 10);
                UIManager.Instance.ChangeDiaglogTextSchedule("코딩수업을 듣습니다.");
                break;
            default:
                Debug.Log("정의되지 않은 일정이 넘겨졌습니다.");
                break;
        }
    }

    public void SceneActive(bool isActive)
    {
        _isActive = isActive;
    }

    public static void SetImage(Sprite image)
    {
        _image.rectTransform.sizeDelta = new Vector2(image.bounds.size.x * _imageWidth / image.bounds.size.x, 
                image.bounds.size.y * _imageWidth / image.bounds.size.x);
        _image.sprite = image;
    }
}
