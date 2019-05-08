using System.Collections;
using UnityEngine;

public class PlayerControl : Unit
{
	private static PlayerControl _instance;
	private bool _isControlable; // 플레이어 조작이 가능한가
	private bool _isMoving; // 플레이어가 이동중인가

	private void Awake ()
	{
		//DontDestroyOnLoad ( transform.root.gameObject );
        if(_instance == null)
        {
            _instance = this;
        }
		
		SetStatus ( "Idle" );

        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "Reservation")
            {
                _reservation = transform.GetChild(i).gameObject;
            }
        }

        //_reservation = transform.Find("Reservation").gameObject;
        _deltaPosition = 0f;

		_isControlable = true;
		_isMoving = false;
	}

	private void FixedUpdate ()
	{
        //Debug.Log("_isMoving: " + _isMoving);
		if ( _isControlable == true )
		{

			if ( _isMoving == false )
			{
				InputMovement ();
				InputAdditionalKey ();
			}
		}
	}

	/* 기본적인 상/하/좌/우 움직임을 입력받는 함수 */
	private void InputMovement ()
	{
		if ( IsControlable == false ) return;

		Vector2 direction = Vector2.zero;
		float horizontal = Input.GetAxis ( "Horizontal" );
		float vertical = Input.GetAxis ( "Vertical" );

		if ( vertical > 0 ) direction = new Vector2 ( 0, 1 ); // 상
		else if ( vertical < 0 ) direction = new Vector2 ( 0, -1 ); // 하
		else if ( horizontal < 0 ) direction = new Vector2 ( -1, 0 ); // 좌
		else if ( horizontal > 0 ) direction = new Vector2 ( 1, 0 ); // 우

		if ( horizontal != 0 || vertical != 0 )
		{
			SetStatus ( "Walk" );
			StartCoroutine ( Move ( direction ) );
		}

        if (Input.GetButtonDown("Interact"))
        {
            StartCoroutine(Interact());
        }
	}

	/* 달리기 등의 특수 키를 입력받는 함수 */
	private void InputAdditionalKey ()
	{
		if ( IsControlable == false ) return;

		if ( Input.GetAxis ( "Run" ) > 0 ) _unitData._runSpeed = 2;
		else _unitData._runSpeed = 1;
	}

	/* 이동방향과 이동 횟수를 받음 */
	public override IEnumerator Move ( Vector2 direction, int count = 1 )
	{
		_unitData._direction = direction;

		if ( _isMoving == false )
		{
			if ( CheckCollider ( direction ) == true )
			{
				_isMoving = true;

				yield return StartCoroutine ( base.Move ( direction, count ) );

				if ( _isControlable == true) {
                    if(GoldManager.Instance != null)
                        GoldManager.Instance.RemainedStep--;
                }
					

				_isMoving = false;
				InputMovement ();
				InputAdditionalKey ();
			}

			else PlayAnim ( true );
		}
	}

	protected override bool CheckCollider ( Vector2 direction )
	{
		GameObject obj = null;

		if ( direction == new Vector2 ( 0, 1 ) ) obj = _up;
		else if ( direction == new Vector2 ( 0, -1 ) ) obj = _down;
		else if ( direction == new Vector2 ( -1, 0 ) ) obj = _left;
		else if ( direction == new Vector2 ( 1, 0 ) ) obj = _right;

		if ( obj == null ) return true;

		else if ( obj.tag == "Door") {
            Door door = obj.GetComponent<Door>();
            if (door.GetIsActive())
            {
                StartCoroutine(door.Teleport(gameObject));
            }
        }
			

		return false;
	}

	private IEnumerator Interact ()
	{
		GameObject target = null;
        Debug.Log("Interact");
		if ( _unitData._direction == new Vector2 ( 0, 1 ) ) target = _up; // 상
		else if ( _unitData._direction == new Vector2 ( 0, -1 ) ) target = _down; // 하
		else if ( _unitData._direction == new Vector2 ( -1, 0 ) ) target = _left; // 좌
		else if ( _unitData._direction == new Vector2 ( 1, 0 ) ) target = _right; // 우

		if ( target != null )
		{
            if (target.tag == "Interactable")
            {
                if (target.GetComponent<ObjectInteract>())
                {
                    _isControlable = false;
                    yield return StartCoroutine(target.GetComponent<ObjectInteract>().Interact());
                    _isControlable = true;
                }
            }
            else
            {
                if (target.tag == "NPC")
                {
                    _isControlable = false;

                    NpcControl script = target.transform.parent.Find("Body").GetComponent<NpcControl>();
                    script.IsFriend = CheckPhoneNumber(script);

                    if (!script.IsFriend && script.GetPhoneNumber() != "-9999 : -9999")
                    {
                        GainPhoneNumber(script);
                    }

                    yield return StartCoroutine(target.transform.parent.Find("Body").GetComponent<EventHandler>().InteractEvent());

                    _isControlable = true;
                }
            }
		}
	}

	private bool CheckPhoneNumber(NpcControl script)
	{
		if ( GameManager.Instance.PhoneNumbers.Contains ( script.GetPhoneNumber () )) return true;
		else return false;
	}

    private void GainPhoneNumber(NpcControl script)
    {
        GameManager.Instance.GainPhoneNumber(script.GetPhoneNumber());
        StatusManager.Instance.AddValueStatus("전화번호수", 1);
    }

	public static PlayerControl Instance
	{
		get { return _instance; }
		private set { _instance = value; }
	}

	public bool IsControlable
	{
		get { return _isControlable; }
		set { _isControlable = value; }
	}
}
