using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject _linkedDoor;
    [SerializeField] private Direction _direction;
    [SerializeField] private bool _isActive = true;
    [SerializeField] private GameObject[] _activeObjects;

	public IEnumerator Teleport ( GameObject player )
	{
        DoorSound();

        player.GetComponent<PlayerControl> ().IsControlable = false;
		GameObject panel = player.transform.Find("Body").Find ( "Main Camera" ).Find ( "Fade" ).gameObject;
		panel.SetActive ( true );

		yield return StartCoroutine ( panel.GetComponent<ScreenManager> ().Fade ( -1 ) );
		yield return new WaitForSeconds ( 0.8f );

        Vector2 myVector = DirectionToVector ( _direction );
		Vector2 playerVector2 = player.GetComponent<Unit> ().UnitData._direction;

		if ( myVector + playerVector2 == Vector2.zero )
		{
			Vector2 deltaVector = DirectionToVector ( _linkedDoor.GetComponent<Door> ().Direction );
			player.transform.position = _linkedDoor.transform.position + (Vector3) deltaVector;

			//player.GetComponent<PlayerControl> ().IsControlable = true;
		}

		yield return StartCoroutine ( panel.GetComponent<ScreenManager> ().Fade ( 1 ) );
        player.GetComponent<PlayerControl>().IsControlable = true;

        GetComponent<Door>().ActiveObjects();
        _linkedDoor.GetComponent<Door>().ActiveObjects();
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

	public Direction Direction
	{
		get { return _direction; }
		private set { _direction = value; }
	}

    public bool GetIsActive()
    {
        return _isActive;
    }

    private void ActiveObjects()
    {
        for (int i = 0; i < _activeObjects.Length; i++)
        {
            if(_activeObjects[i] != null)
                _activeObjects[i].SetActive(!_activeObjects[i].activeSelf);
        }
    }

    private void DoorSound()
    {
        GameObject.Find("SoundManger").GetComponent<SoundManager>().DoorSound();
    }

}
