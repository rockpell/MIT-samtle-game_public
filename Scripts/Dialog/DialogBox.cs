using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
	[SerializeField] private int _selectIndex;

    public delegate void SomeDele();

    private float _leftTime;

	private void Awake ()
	{
		for ( int i = 0 ; i < transform.childCount ; i++ )
		{
			Text text = transform.GetChild ( i ).GetComponent<Text> ();
			text.text = "";
		}

		//DontDestroyOnLoad(transform.parent);
		//gameObject.SetActive(false);
	}

	public IEnumerator PrintLog ( string log )
	{
		Text text = transform.GetChild ( 0 ).GetComponent<Text> ();

		text.text = "";
		yield return new WaitForSeconds ( 0.5f );

		foreach ( char letter in log.ToCharArray () )
		{
			text.text += letter;
			yield return new WaitForSeconds ( 0.02f );
		}

		while ( !Input.GetButtonDown ( "Enter" ) )
		{
			yield return null;
		}
	}

    public IEnumerator PrintLog(SomeDele someDele, string log)
    {
        Text text = transform.GetChild(0).GetComponent<Text>();

        _leftTime = 1.2f;

        text.text = "";
        yield return new WaitForSeconds(0.5f);

        foreach (char letter in log.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(0.02f);
        }

        while (true)
        {
            _leftTime -= Time.deltaTime;
            if (Input.GetButtonDown("Enter"))
            {
                someDele();
                yield break;
            }

            if(_leftTime <= 0)
            {
                someDele();
                yield break;
            }
            yield return null;
        }
    }

    public IEnumerator SelectAnswer ( string[] responseText )
	{
		_selectIndex = 0;
		Text[] texts = new Text[responseText.Length];

		for (int i = 0 ; i < responseText.Length ; ++i )
		{
			texts[i] = transform.GetChild(i).GetComponent<Text> ();
			texts[i].text = responseText[i];
		}

		yield return null;

		while ( !Input.GetButtonDown ( "Enter" ) )
		{
			SetTextColor ( _selectIndex, texts );

			if ( Input.GetButtonDown ( "Vertical" ) )
			{
				_selectIndex++;
				if ( _selectIndex >= responseText.Length ) _selectIndex = 0;
			}

			yield return null;
		}
	}

	private void SetTextColor ( int index, Text[] texts )
	{
		for ( int i = 0 ; i < texts.Length ; ++i )
			texts[i].color = Color.gray;

		texts[index].color = Color.white;
	}

	public int SelectIndex
	{
		get { return _selectIndex; }
		private set { _selectIndex = value; }
	}
}
