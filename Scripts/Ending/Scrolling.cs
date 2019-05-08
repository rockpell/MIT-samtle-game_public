using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
	[SerializeField] private float _speed;
	private GameObject _lastCredit;
	private bool _isScrollable;

	private void Start ()
	{
		_lastCredit = GameObject.Find ( "Canvas" ).GetComponent<EndingCredit> ().GetLastCredit ();
		_isScrollable = true;
        Debug.Log(_lastCredit);
	}

	private void FixedUpdate ()
	{
		float accel;
		if ( Input.anyKey ) accel = 3;
		else accel = 1;

		if(transform.position.y <= _lastCredit.transform.position.y)
		{
			_isScrollable = false;
			transform.position = new Vector3 ( transform.position.x, _lastCredit.transform.position.y, transform.position.z );
		}

		if ( _isScrollable == true ) transform.position -= new Vector3 ( 0, _speed * accel, 0 );
	}


}
