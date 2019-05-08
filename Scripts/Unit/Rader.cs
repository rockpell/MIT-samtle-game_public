using UnityEngine;

public class Rader : MonoBehaviour
{
	private Unit _parent;

	private void Start ()
	{
        //_parent = transform.root.Find("Body").GetComponent<Unit>();
        if (transform.parent.parent.parent.name == "Player")
        {
            _parent = transform.parent.parent.parent.gameObject.GetComponent<Unit>();
        } else
        {
            _parent = transform.parent.parent.gameObject.GetComponent<Unit>();
        }
        

    }

	private void OnTriggerEnter2D ( Collider2D col )
	{
		if ( col.name == "Reservation" || col.tag == "Obstacle" || col.tag == "Door" || col.tag == "Interactable" )
		{
            //Debug.Log("gameObject.name : " + gameObject.name + "  col.gameObject :   " + col.gameObject.tag);
            if (gameObject.name == "Up" && col.tag != _parent.gameObject.tag)
            {
                _parent.Up = col.gameObject;
            }
            if (gameObject.name == "Down" && col.tag != _parent.gameObject.tag) {
                _parent.Down = col.gameObject;
            }
            if (gameObject.name == "Left" && col.tag != _parent.gameObject.tag) {
                _parent.Left = col.gameObject;
            }
            if (gameObject.name == "Right" && col.tag != _parent.gameObject.tag) {
                _parent.Right = col.gameObject;
            } 
		}
	}

	private void OnTriggerExit2D ( Collider2D col )
	{
		if ( col.name == "Reservation" || col.tag == "Obstacle" || col.tag == "Door" || col.tag == "Interactable" )
		{
			if ( gameObject.name == "Up" && col.gameObject == _parent.Up) _parent.Up = null;
			if ( gameObject.name == "Down" && col.gameObject == _parent.Down ) _parent.Down = null;
			if ( gameObject.name == "Left" && col.gameObject == _parent.Left ) _parent.Left = null;
			if ( gameObject.name == "Right" && col.gameObject == _parent.Right ) _parent.Right = null;
		}
	}
}
