using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcControl : Unit
{
	[SerializeField] private string _serialCode;
	[SerializeField] private string _phoneNumber;

	private bool _isFriend;

	private void Awake ()
	{
		SetStatus ( "Idle" );
		_reservation = transform.parent.Find ( "Reservation" ).gameObject;

		_deltaPosition = 0f;
		_isFriend = false;
	}

	/* 이동방향과 이동 횟수를 받음 */
	public override IEnumerator Move ( Vector2 direction, int count )
	{
		_unitData._direction = direction;

		if ( _unitData._isAnimPlaying == false )
		{
			if ( CheckCollider ( direction ) == false )
			{
				PlayAnim ( true );

				if ( GetCollider ( direction ).transform.root.name == "Player" )
					while ( CheckCollider ( direction ) == false ) yield return null;
			}

			yield return StartCoroutine ( base.Move ( direction, count ) );
		}
	}

	private GameObject GetCollider ( Vector2 direction )
	{
		GameObject obj = null;

		if ( direction == new Vector2 ( 0, 1 ) ) obj = _up;
		else if ( direction == new Vector2 ( 0, -1 ) ) obj = _down;
		else if ( direction == new Vector2 ( -1, 0 ) ) obj = _left;
		else if ( direction == new Vector2 ( 1, 0 ) ) obj = _right;

		return obj;
	}

	public string GetPhoneNumber()
	{
		return _serialCode + " : " + _phoneNumber;
	}

	public bool IsFriend
	{
		get { return _isFriend; }
		set { _isFriend = value; }
	}
}
