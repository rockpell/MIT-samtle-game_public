using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventHandler : MonoBehaviour
{
	[SerializeField] private Event[] _forcedEvent; // 강제 발생 이벤트. 처음 한 번만 발생하고 그 이후엔 사라짐
	[SerializeField] private Event[] _interactEvent; // 상호작용 이벤트. 상호작용을 할때마다 발생함
	[SerializeField] private Event[] _autoEvent; // AI 알고리즘 작성하는 곳

	private int _autoEventIndex;
    private int _forcedEventIndex;
	private int _logIndex;
	private IEnumerator _repeatEnumerator, _autoEventEnumerator, _forcedEventEnumerator;

	void Start ()
	{
		DismentleAutoEvent ();

		_autoEventIndex = 0;
		_logIndex = 0;
        _forcedEventIndex = 0;

        _repeatEnumerator = RepeatEvent ( _autoEvent );
		StartCoroutine ( _repeatEnumerator );

        _repeatEnumerator = LoopForcedEvent();
        StartCoroutine(_repeatEnumerator);
    }

    private IEnumerator LoopForcedEvent()
    {
        if (_forcedEvent.Length > 0 && !GameManager.Instance.IsStartPrologue)
        {
            yield return new WaitForSeconds(1f);
        }
        for (int i = 0; i < _forcedEvent.Length; i++)
        {
            float temp_time = 1f;
            _forcedEventEnumerator = OccurForcedEvent(_forcedEvent, i);
            
            StartCoroutine(_forcedEventEnumerator);

            if(_forcedEvent.Length > 0 &&_forcedEvent[i]._action == EventType.Dialog)
            {
                temp_time = 3f;
            }

            yield return new WaitForSeconds(temp_time);
        }
    }

	private void DismentleAutoEvent ()
	{
		List<Event> autoList = new List<Event> ();
		for ( int i = 0 ; i < _autoEvent.Length ; ++i )
		{
			if ( _autoEvent[i]._action == EventType.Walk )
			{
				int count = _autoEvent[i]._space;

				for ( int j = 0 ; j < count ; ++j )
				{
					autoList.Add ( _autoEvent[i] );
					autoList[autoList.Count - 1]._space = 1;
				}

			}

			else autoList.Add ( _autoEvent[i] );
		}

		_autoEvent = new Event[0];
		_autoEvent = autoList.ToArray ();
	}

	private IEnumerator RepeatEvent ( Event[] trees )
	{
		while ( true )
		{
			if ( trees.Length == 0 ) break;
			if ( _autoEventIndex >= trees.Length ) _autoEventIndex = 0;

			_autoEventEnumerator = OccurEvent ( trees, _autoEventIndex );
			yield return StartCoroutine ( _autoEventEnumerator );
		}
	}

	public IEnumerator InteractEvent ()
	{
		if ( _interactEvent.Length != 0 )
		{
			if ( _autoEventEnumerator != null ) StopCoroutine ( _autoEventEnumerator );
			if ( _repeatEnumerator != null ) StopCoroutine ( _repeatEnumerator );

			yield return StartCoroutine ( OccurEvent ( _interactEvent ) );

			_repeatEnumerator = RepeatEvent ( _autoEvent );
			StartCoroutine ( _repeatEnumerator );
		}
	}

	public IEnumerator OccurEvent ( Event[] trees, int startIndex = 0 )
	{
		if ( trees == _forcedEvent ){
            PlayerControl.Instance.IsControlable = false;
        }
		if ( trees.Length == 0 ) Debug.Log ( "Event가 존재하지 않습니다." );
		if ( trees == _autoEvent ) _autoEventIndex = startIndex;

        for ( int i = startIndex ; i < trees.Length ; ++i )
		{
			if ( trees == _autoEvent ) _autoEventIndex++;

            Unit target;

            if (trees[i]._target.transform.name == "Player")
            {
                target = trees[i]._target.GetComponent<Unit>();
            } else
            {
                target = trees[i]._target.transform.root.Find("Body").GetComponent<Unit>();
            }

			switch ( trees[i]._action )
			{
				case EventType.Walk:
					Vector2 direction = DirectionToVector ( trees[i]._direction );
					int count = trees[i]._space;
					target.GetComponent<Unit> ().SetStatus ( "Walk" );
					yield return StartCoroutine ( target.Move ( direction, count ) );

					break;

				case EventType.Wait:
					float waitTime = trees[i]._waitTime;
					target.GetComponent<Unit> ().SetStatus ( "Idle" );
					yield return StartCoroutine ( target.Wait ( waitTime ) );

					break;

				case EventType.Dialog:
                    for ( int logIndex = 0 ; logIndex < trees[i]._dialogLogs[_logIndex].logs.Length ; logIndex++ )
					{
						trees[i].dialogBox.gameObject.SetActive ( true );
						yield return StartCoroutine ( target.Dialog ( trees[i].dialogBox, trees[i]._dialogLogs[_logIndex].logs[logIndex] ) );
					}

					_logIndex++;
					if ( _logIndex >= trees[i]._dialogLogs.Length ) _logIndex = 0;
					break;

				case EventType.Response:
					trees[i]._responseBox.gameObject.SetActive ( true );
					yield return StartCoroutine ( target.Response ( trees[i]._responseBox, trees[i]._responseLog ) );

					trees[i]._responseBox.gameObject.SetActive ( false );

					int selectedIndex = trees[i]._responseBox.GetComponent<DialogBox> ().SelectIndex;
					i = trees[i]._route[selectedIndex];
					i--;

					break;

				case EventType.DialogToggle:
                    if (trees[i]._isEnable == false)
                    {
                        trees[i]._dialogBox.gameObject.SetActive(false);
                    }
					break;

				case EventType.ConditionalJump:
					if ( trees[i]._condition == Condition.Index ) i = trees[i]._index;

					else if ( trees[i]._condition == Condition.IsFriend )
					{
						int index = 0;

						if ( transform.root.Find ( "Body" ).GetComponent<NpcControl> ().IsFriend == true ) index = trees[i]._isTrue;
						else index = trees[i]._isFalse;

						i = index;
					}

					i--;
					break;

                case EventType.FadeInOout:
                    IEnumerator temp = FadeInOut();
                    StartCoroutine(temp);
                    if (trees[startIndex]._isRemoveNPCS)
                    {
                        GameManager.Instance.RemoveTempNPCS();
                    }

                    if(trees[startIndex]._dialogBox != null)
                        trees[startIndex]._dialogBox.gameObject.SetActive(false);
                    break;

                case EventType.LoadScene:
                    yield return new WaitForSeconds(0.7f);
                    if (transform.parent.name == "test_temp")
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("NeonShooter");
                    } else
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("Interview");
                    }
                    
                    break;
                default:
					break;
			}
		}

		if ( trees == _forcedEvent )
		{
			_forcedEvent = new Event[0];
			PlayerControl.Instance.IsControlable = true;
		}
	}

    public IEnumerator OccurForcedEvent(Event[] trees, int startIndex = 0)
    {
        if (trees == _forcedEvent)
        {
            PlayerControl.Instance.IsControlable = false;
        }
        if (trees.Length == 0) Debug.Log("Event가 존재하지 않습니다.");
        if (trees == _autoEvent) _autoEventIndex = startIndex;
        
        if (trees == _autoEvent) _autoEventIndex++;

        Unit target;

        if (trees[startIndex]._target.transform.name == "Player")
        {
            target = trees[startIndex]._target.GetComponent<Unit>();
        }
        else
        {
            target = trees[startIndex]._target.transform.root.Find("Body").GetComponent<Unit>();
        }

        switch (trees[startIndex]._action)
        {
            case EventType.Walk:
                Vector2 direction = DirectionToVector(trees[startIndex]._direction);
                int count = trees[startIndex]._space;
                target.GetComponent<Unit>().SetStatus("Walk");
                yield return StartCoroutine(target.Move(direction, count));
                break;

            case EventType.Wait:
                float waitTime = trees[startIndex]._waitTime;
                target.GetComponent<Unit>().SetStatus("Idle");
                yield return StartCoroutine(target.Wait(waitTime));
                break;

            case EventType.Dialog:
                if (trees[startIndex]._dialogClapSound)
                {
                    GameObject.Find("SoundManger").GetComponent<SoundManager>().ClapSound();
                }
                for (int logIndex = 0; logIndex < trees[startIndex]._dialogLogs[_logIndex].logs.Length; logIndex++)
                {
                    trees[startIndex].dialogBox.gameObject.SetActive(true);
                    yield return StartCoroutine(target.Dialog(trees[startIndex].dialogBox, trees[startIndex]._dialogLogs[_logIndex].logs[logIndex]));
                }

                _logIndex++;
                if (_logIndex >= trees[startIndex]._dialogLogs.Length) _logIndex = 0;
                break;

            case EventType.DialogToggle:
                if (trees[startIndex]._isEnable == false)
                {
                    trees[startIndex]._dialogBox.gameObject.SetActive(false);
                }
                break;

            case EventType.FadeInOout:
                IEnumerator temp = FadeInOut();
                StartCoroutine(temp);
                if (trees[startIndex]._isRemoveNPCS)
                {
                    GameManager.Instance.RemoveTempNPCS();
                }
                trees[startIndex]._dialogBox.gameObject.SetActive(false);
                break;
            default:
                break;
        }
        

        if (trees == _forcedEvent)
        {
            if (trees.Length - 1 == startIndex)
            {
                _forcedEvent = new Event[0];
                PlayerControl.Instance.IsControlable = true;
            }
        }
    }

    private IEnumerator FadeInOut()
    {
        GameObject panel = GameObject.Find("Main Camera").transform.Find("Fade").gameObject;
        panel.SetActive(true);
        StartCoroutine(panel.GetComponent<ScreenManager>().Fade(-1));

        yield return new WaitForSeconds(0.8f);

        yield return StartCoroutine(panel.GetComponent<ScreenManager>().Fade(1));
    }

	private Vector2 DirectionToVector ( Direction direction )
	{
		switch ( direction )
		{
			case Direction.Up:
				return new Vector2 ( 0, 1 );

			case Direction.Down:
				return new Vector2 ( 0, -1 );

			case Direction.Left:
				return new Vector2 ( -1, 0 );

			case Direction.Right:
				return new Vector2 ( 1, 0 );

			default:
				return Vector2.zero;
		}
	}
}

[System.Flags]
public enum EventType
{
	None = 0,
	Walk = 1,
	Wait = 2,
	ConditionalJump = 4,
	Dialog = 8,
	DialogToggle = 16,
	Response = 32,
    FadeInOout = 64,
    LoadScene = 128
}

public enum Direction
{
	None = 4,
	Up = 0,
	Down = 1,
	Left = 2,
	Right = 3
}

public enum Condition
{
	None = 0,
	Index = 1,
	IsFriend = 2
}

[System.Serializable]
public class Event
{
	public GameObject _target; // 움직일 오브젝트

	public EventType _action; // 이벤트 타입

	[Hide ( "_action", EventType.Walk )] public Direction _direction; // 회전할 방향
	[Hide ( "_action", EventType.Walk )] public int _space; // 이동할 횟수

	[Hide ( "_action", EventType.Wait )] public float _waitTime; // 기다릴 시간

	[Hide ( "_action", EventType.Dialog )] public Image dialogBox; // 대화창 오브젝트. 중복선언시 변수명이 inspector에서 같게 보이게 하기위해 언더바를 뺌
	[Hide ( "_action", EventType.Dialog )] public Log[] _dialogLogs; // 대사
    [Hide("_action", EventType.Dialog)] public bool _dialogClapSound; // 박수소리

    [Hide ( "_action", EventType.DialogToggle )] public Image _dialogBox; // 대화창 오브젝트. Flag잘 쓰면 중복선언 안해도 될 것 같은데 아직은 안돼서 중복선언함..
	[Hide ( "_action", EventType.DialogToggle )] public bool _isEnable;  // 대화창 on/off

	[Hide ( "_action", EventType.Response )] public Image _responseBox; // 대답창 오브젝트
	[Hide ( "_action", EventType.Response )] public string[] _responseLog; // 대답 텍스트
	[Hide ( "_action", EventType.Response )] public int[] _route; // 점프 인덱스

	[Hide ( "_action", EventType.ConditionalJump )] public Condition _condition; // 점프 조건
	[Hide ( "_condition", Condition.Index )] public int _index; // 점프할 위치
	[Hide ( "_condition", Condition.IsFriend )] public int _isTrue; // 참일 때 점프할 위치
	[Hide ( "_condition", Condition.IsFriend )] public int _isFalse; // 거짓일 때 점프할 위치

    [Hide("_action", EventType.FadeInOout)] public bool _isRemoveNPCS; // true 이면 TempNPCS를 지운다.

    [Hide("_action", EventType.LoadScene)] public string _sceneName; // LoadScene name
}

[System.Serializable] 
public class Log
{
	public string[] logs;
}