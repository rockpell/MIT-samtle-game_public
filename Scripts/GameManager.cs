using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance;

	private List<string> _phoneNumbers;
	private Vector3 _playerPosition;
	private Sprite _playerSprite;

    private bool _isStartPrologue;

    private int _remainedStep;

    private void Awake ()
	{
        if (_instance == null)
        {
            _instance = this;
        }
        _isStartPrologue = false;
        _phoneNumbers = new List<string>();

        _remainedStep = 0;
        //_playerPosition = new Vector3(-0.5f, -50.5f, 0);

        DontDestroyOnLoad ( gameObject );
	}

	public static GameManager Instance
	{
		get { return _instance; }
		private set { _instance = value; }
	}

	public List<string> PhoneNumbers
	{
		get { return _phoneNumbers; }
		set { _phoneNumbers = value; }
	}

	public Vector3 PlayerPosition
	{
		get { return _playerPosition; }
		set { _playerPosition = value;}
	}

	public Sprite PlayerSprite
	{
		get { return _playerSprite; }
		set { _playerSprite = value; }
	}

    public bool IsStartPrologue {
        get { return _isStartPrologue; }
        set { _isStartPrologue = value; }
    }

    public void GainPhoneNumber(string text) {
        _phoneNumbers.Add(text);
    }

    public void RemoveTempNPCS() {
        GameObject target = GameObject.Find("TempNPCS");
        if (target != null) {
            Destroy(target);
        }
    }

    public bool CheckEndDay()
    {
        if (Calendar.GetInstance().GetMonth() == 4 && Calendar.GetInstance().GetDay() == 15)
        {
            return true;
        }
        return false;
    }

    public int RemainedStep {
        get { return _remainedStep; }
        set { _remainedStep = value; }
    }
}
