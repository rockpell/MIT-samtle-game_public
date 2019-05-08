using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteract : MonoBehaviour
{
	[SerializeField] private InteractFunction _function;

	[Hide ( "_function", InteractFunction.ShowImage )]
	[SerializeField] private UnityEngine.UI.Image _image;
    [SerializeField] private string _imageText;

    [Hide ( "_function", InteractFunction.LoadScene )]
	[SerializeField] private string _sceneName;

    [Hide("_function", InteractFunction.PrintLog)]
    [SerializeField] private string _text;

    private IEnumerator _coroutineEnumerator;

    private bool isRunning = false;

    public IEnumerator Interact()
	{
        isRunning = false;

        if (_function == InteractFunction.ShowImage) ShowImage();
        else if (_function == InteractFunction.LoadScene) LoadScene();
        else if (_function == InteractFunction.PrintLog) StartCoroutine(PrintLog(_text));

        yield return new WaitUntil(()=> isRunning);
    }

	private void ShowImage()
	{
        _image.gameObject.SetActive(true);
        StartCoroutine(PrintLog(_imageText));
    }

	private void LoadScene()
	{
		PlayerControl.Instance.gameObject.SetActive ( false );
        isRunning = true;
		UnityEngine.SceneManagement.SceneManager.LoadScene (_sceneName);
	}

    private IEnumerator PrintLog(string text)
    {
        GameObject DialogBoxUI = GameObject.Find("Canvas").transform.Find("DialogBox1").gameObject;
        DialogBox dialogBox = DialogBoxUI.GetComponent<DialogBox>();

        if( !(_text == null && _imageText == null) )
            DialogBoxUI.SetActive(true);

        if(_coroutineEnumerator != null)
            StopCoroutine(_coroutineEnumerator);

        _coroutineEnumerator = dialogBox.PrintLog(HIdeUI, text);
        yield return StartCoroutine(_coroutineEnumerator);
        isRunning = true;
    }

    private void HIdeUI()
    {
        GameObject DialogBoxUI = GameObject.Find("Canvas").transform.Find("DialogBox1").gameObject;
        DialogBoxUI.SetActive(false);
        if(_image != null)
            _image.gameObject.SetActive(false);
    }

}

public enum InteractFunction
{
	None = 0,
	ShowImage = 1,
	LoadScene = 2,
    PrintLog = 3
}
