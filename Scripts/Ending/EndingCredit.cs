using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingCredit : MonoBehaviour
{
	[SerializeField] private GameObject _leftAlign;
	[SerializeField] private GameObject _rightAlign;

	[SerializeField] private Credit[] _credits;
	[SerializeField] private float _space;

    [SerializeField] private Sprite[] _mainEndginScenes;
    [SerializeField] private Sprite[] _subEndginScenes;

    private List<GameObject> _creditList;

    private PriorityQueue<Node> _mainEndingList;
    private PriorityQueue<Node> _subEndingList;

    private string _mainEnding;
    private string _subEnding;

	private void Awake ()
	{
		_creditList = new List<GameObject> ();

        _mainEndingList = new PriorityQueue<Node>();
        _subEndingList = new PriorityQueue<Node>();

        SelectEnding();

        SetCredits ();
		AlignCredit ();
	}

	private void SetCredits ()
	{
		for ( int i = 0 ; i < _credits.Length ; ++i )
		{
			GameObject align = null;

			if ( _credits[i]._direction == CreditAlign.Left ) align = Instantiate ( _leftAlign ) as GameObject;
			else if ( _credits[i]._direction == CreditAlign.Right ) align = Instantiate ( _rightAlign ) as GameObject;

			if ( align != null )
			{
				align.transform.parent = transform;
				align.transform.localScale = new Vector3 ( 1, 1, 1 );

				Bounds bounds = _credits[i]._image.bounds;
				align.transform.Find ( "Image" ).GetComponent<Image> ().sprite = _credits[i]._image;
				align.transform.Find ( "Image" ).transform.localScale = new Vector3 ( bounds.size.x * 0.5f, bounds.size.y * 0.5f, 1 );

				align.transform.Find ( "Text" ).GetComponent<Text> ().text = "";

				for ( int line = 0 ; line < _credits[i]._logs.Length ; ++line )
				{
					align.transform.Find ( "Text" ).GetComponent<Text> ().text += _credits[i]._logs[line];

					if ( line < _credits[i]._logs.Length - 1 )
						align.transform.Find ( "Text" ).GetComponent<Text> ().text += "\n";
				}

				_creditList.Add ( align );
			}
		}
	}

	private void AlignCredit ()
	{
		for ( int cur = 1 ; cur < _creditList.Count ; ++cur )
		{
			int prev = cur - 1;
			float height = _creditList[prev].transform.Find ( "Image" ).GetComponent<Image> ().sprite.rect.height * 0.5f;

			_creditList[cur].transform.localPosition = 
				new Vector3 ( _creditList[prev].transform.localPosition.x, _creditList[prev].transform.localPosition.y - height - _space, 0 );
		}
	}

	public GameObject GetLastCredit()
	{
		return _creditList[_creditList.Count - 1];
	}

    private void SelectEnding() {
        if(GameManager.Instance == null) {
            Debug.Log("GameManager not found");
            return;
        } else if(StatusManager.Instance == null) {
            Debug.Log("StatusManager not found");
            return;
        }

        StatusManager statusManager = GameManager.Instance.gameObject.GetComponent<StatusManager>();

        if (statusManager.GetStatus("코딩력") >= 100)
        {
            _mainEndingList.Add(new Node("정회원", 1));
        }
        if (statusManager.GetStatus("학점") >= 100)
        {
            _mainEndingList.Add(new Node("연구실", 2));
        }
        if (statusManager.GetStatus("학점") >= 80 && statusManager.GetStatus("감수성") >= 80)
        {
            _mainEndingList.Add(new Node("전과", 3));
        }
        if (statusManager.GetStatus("감수성") >= 100)
        {
            _mainEndingList.Add(new Node("휴학", 4));
        }
        if (statusManager.GetStatus("전화번호") >= 20 && statusManager.GetStatus("알바횟수") >= 10)
        {
            _mainEndingList.Add(new Node("창업", 5));
            Debug.Log("창업!");
        }
        if (_mainEndingList.Count == 0)
        {
            _mainEndingList.Add(new Node("휴학", 4));
        }
        ////
        ////
        if (statusManager.GetHouse())
        {
            _subEndingList.Add(new Node("건물주", 1));
        }
        if (statusManager.GetStatus("학점") >= 80)
        {
            _subEndingList.Add(new Node("성적확인", 2));
        }
        if (statusManager.GetStatus("알콜수치") >= 80)
        {
            _subEndingList.Add(new Node("술고래", 3));
        }
        if (statusManager.GetStatus("신앙심") >= 80)
        {
            _subEndingList.Add(new Node("팬미팅", 4));
        }
        if (statusManager.GetStatus("대인관계") >= 80)
        {
            _subEndingList.Add(new Node("단체사진", 5));
        }
        if (statusManager.GetStatus("스팀게임") >= 80)
        {
            _subEndingList.Add(new Node("스트리머", 6));
        }
        if (_subEndingList.Count == 0)
        {
            _subEndingList.Add(new Node("술고래", 3));
        }

        _mainEnding = _mainEndingList.Pop().GetName();
        _subEnding = _subEndingList.Pop().GetName();

        SelectEndigScenes();
    }

    private void SelectEndigScenes() {
        switch(_mainEnding) {
            case "정회원":
                _credits[1]._image = _mainEndginScenes[0];
                break;
            case "연구실":
                _credits[1]._image = _mainEndginScenes[1];
                break;
            case "전과":
                _credits[1]._image = _mainEndginScenes[3];
                break;
            case "휴학":
                _credits[1]._image = _mainEndginScenes[4];
                break;
            case "창업":
                _credits[1]._image = _mainEndginScenes[2];
                break;
        }

        switch(_subEnding) {
            case "건물주":
                _credits[0]._image = _subEndginScenes[0];
                break;
            case "성적확인":
                _credits[0]._image = _subEndginScenes[1];
                break;
            case "술고래":
                _credits[0]._image = _subEndginScenes[2];
                break;
            case "팬미팅":
                _credits[0]._image = _subEndginScenes[3];
                break;
            case "단체사진":
                break;
            case "스트리머":
                _credits[0]._image = _subEndginScenes[4];
                break;
        }
    }
}

[System.Serializable]
class Credit
{
	public Sprite _image;
	public string[] _logs;
	public CreditAlign _direction;
}

public enum CreditAlign
{
	None = 0,
	Left = 1,
	Right = 2
}

class Node : IComparable {
    private int _weight;
    private string _name;

    public Node(string text, int weight) {
        _name = text;
        _weight = weight;
    }

    public int CompareTo(object other) {
        if((other is Node) == false) return 0;
        return _weight.CompareTo((other as Node)._weight);
    }

    public string GetName() {
        return _name;
    }
}