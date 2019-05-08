using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
	private static GoldManager _instance;

	[SerializeField] private UnityEngine.UI.Text _step;

	private int _maxStep;
	private int _remainedStep;

	private void Awake ()
	{
		_instance = this;
	}

	private void Start ()
	{
		_maxStep = 100;

        if(GameManager.Instance.RemainedStep > 0)
        {
            GameObject.Find("Player").transform.position = GameManager.Instance.PlayerPosition;
            RemainedStep = GameManager.Instance.RemainedStep;
        } else
        {
            RemainedStep = _maxStep;
        }

        RefreshUI(RemainedStep);
    }

	public static GoldManager Instance
	{
		get { return _instance; }
		private set { _instance = value; }
	}

	public int RemainedStep
	{
		get { return _remainedStep; }

		set
		{
			_remainedStep = value;
            GameManager.Instance.RemainedStep = value;
            GameManager.Instance.PlayerPosition = GameObject.Find("Player").transform.position;
            if (_remainedStep == 0)
            {
                // Scene 나가기
                UnityEngine.SceneManagement.SceneManager.LoadScene("PrincessMaker");
            }

            else RefreshUI(value);

        }
	}

    private void RefreshUI(int value)
    {
        _step.GetComponent<UnityEngine.UI.Text>().text = value + "";
    }

}