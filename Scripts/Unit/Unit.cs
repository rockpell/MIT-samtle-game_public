using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
	[SerializeField] protected UnitData _unitData; // 유닛 데이터

	protected GameObject _reservation;

	protected float _deltaPosition; // 움직인 양

	/* 상하좌우 콜라이더 체크 오브젝트 */
	[SerializeField] protected GameObject _up;
	[SerializeField] protected GameObject _down;
	[SerializeField] protected GameObject _left;
	[SerializeField] protected GameObject _right;

	private void Awake ()
	{
		_unitData = new UnitData ();
		_unitData._runSpeed = 1;
		_unitData._curSpeed = _unitData._walkSpeed * _unitData._runSpeed;
	}
	private void Update ()
	{
		_unitData._curSpeed = _unitData._walkSpeed * _unitData._runSpeed;
	}

	protected void PlayAnim ( bool isIdle )
	{
		int direction = 1;

		if ( _unitData._direction == new Vector2 ( 0, 1 ) ) direction = (int) Direction.Up;
		else if ( _unitData._direction == new Vector2 ( 0, -1 ) ) direction = (int) Direction.Down;
		else if ( _unitData._direction == new Vector2 ( -1, 0 ) ) direction = (int) Direction.Left;
		else if ( _unitData._direction == new Vector2 ( 1, 0 ) ) direction = (int) Direction.Right;

		Sprite idle = _unitData._animImages[direction]._idle;
		Sprite[] walk = _unitData._animImages[direction]._walk;

		if ( _unitData._isIdle == true || isIdle )
		{
			if ( idle == null ) Debug.Log ( "Player Idle Sprite가 존재하지 않습니다!" );

			else
			{
				_unitData._isAnimPlaying = false;
                if (!transform.parent)
                    transform.Find("Body").GetComponent<SpriteRenderer> ().sprite = idle;
                else
                    transform.GetComponent<SpriteRenderer>().sprite = idle;
            }
		}

		else if ( _unitData._isWalking == true || _unitData._isRunning == true )
		{
			if ( walk == null ) Debug.Log ( "Player Walk Sprite가 존재하지 않습니다!" );
			else StartCoroutine ( PlayAnimSprites ( walk ) );
		}
	}

	protected IEnumerator PlayAnimSprites ( Sprite[] sprites )
	{
		if ( _unitData._isAnimPlaying == false )
		{
			float animSpeed = _unitData._curSpeed / _unitData._walkSpeed;
			_unitData._isAnimPlaying = true;

			for ( int i = 0 ; i < sprites.Length / (int) animSpeed ; ++i )
			{
                if(!transform.parent)
                {
                    transform.Find("Body").GetComponent<SpriteRenderer>().sprite = sprites[_unitData._animIndex];
                } else
                {
                    transform.GetComponent<SpriteRenderer>().sprite = sprites[_unitData._animIndex];
                }
				_unitData.NextAnimIndex ();

				if ( i == sprites.Length / (int) animSpeed - 1 ) break;

				yield return new WaitForSeconds ( 1.0f / (_unitData._curSpeed * sprites.Length * 60) );
			}

			_unitData._isAnimPlaying = false;
			_unitData._animIndex = 0;
		}
	}

	/* 이동방향과 이동 횟수를 받음 */
	public virtual IEnumerator Move ( Vector2 direction, int count = 1 )
	{
        Transform tempTransform;
        if (!transform.parent)//Player
        {
            tempTransform = transform;
        }
        else
        {
            tempTransform = transform.parent;
        }

		for ( int i = 0 ; i < count ; ++i )
		{
			PlayAnim ( false );

            if(transform.name != "Player")
                _reservation.transform.position += (Vector3)direction;

            while ( Mathf.Abs ( _deltaPosition ) < 1 )
			{
				Vector2 delta = _unitData._direction * _unitData._curSpeed;
				_deltaPosition += (delta.x != 0 ? delta.x : delta.y);
                tempTransform.position += (Vector3) delta;

				if ( Mathf.Abs ( _deltaPosition ) < 1 ) yield return null;
			}

			//if ( gameObject.tag != "Player" ) while ( CheckCollider ( direction ) == false ) yield return null;

			_deltaPosition = 0;
		}

        tempTransform.position = new Vector3 ( Mathf.Floor ( transform.position.x ) + 0.5f, Mathf.Floor ( transform.position.y ) + 0.5f, 0 );
		SetStatus ( "Idle" );
	}

	/* 받은 시간동안 기다림 */
	public IEnumerator Wait ( float time )
	{
		yield return new WaitForSeconds ( time );
	}

	/* 대화하는 함수 */
	public IEnumerator Dialog ( UnityEngine.UI.Image dialogBox, string log )
	{
		yield return StartCoroutine ( dialogBox.GetComponent<DialogBox> ().PrintLog ( log ) );
	}

	/* 대답하는 함수 */
	public IEnumerator Response ( UnityEngine.UI.Image responseBox, string[] responseText )
	{
		yield return StartCoroutine ( responseBox.GetComponent<DialogBox> ().SelectAnswer ( responseText ) );
	}

	protected virtual bool CheckCollider ( Vector2 direction )
	{
		GameObject obj = null;

		if ( direction == new Vector2 ( 0, 1 ) ) obj = _up;
		else if ( direction == new Vector2 ( 0, -1 ) ) obj = _down;
		else if ( direction == new Vector2 ( -1, 0 ) ) obj = _left;
		else if ( direction == new Vector2 ( 1, 0 ) ) obj = _right;

		if ( obj == null ) return true;
		else return false;
	}

	public void SetStatus ( string status )
	{
		_unitData._isIdle = false;
		_unitData._isWalking = false;
		_unitData._isRunning = false;

		if ( status == "Idle" ) _unitData._isIdle = true;
		else if ( status == "Walk" ) _unitData._isWalking = true;
		else if ( status == "Run" ) _unitData._isRunning = true;

		else Debug.Log ( status + "와 일치하는 상태값이 존재하지 않습니다!" );
	}

	public string GetStatus ()
	{
		if ( _unitData._isIdle == true ) return "Idle";
		else if ( _unitData._isWalking == true ) return "Walk";
		else if ( _unitData._isRunning == true ) return "Run";

		else return "None";
	}

	public GameObject Up
	{
		get { return _up; }
		set { _up = value; }
	}

	public GameObject Down
	{
		get { return _down; }
		set { _down = value; }
	}

	public GameObject Left
	{
		get { return _left; }
		set { _left = value; }
	}

	public GameObject Right
	{
		get { return _right; }
		set { _right = value; }
	}

	public UnitData UnitData
	{
		get { return _unitData; }
		private set { _unitData = value; }
	}
}
