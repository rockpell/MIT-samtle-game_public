using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogBoxController : MonoBehaviour {

    private IEnumerator coruoutine;
    public delegate void SomeDele();

    // Use this for initialization
    void Start () {
        for(int i = 0; i < transform.childCount; i++) {
            Text text = transform.GetChild(i).GetComponent<Text>();
            text.text = "";
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator PrintLog(params string[] log) {
        Text text = transform.GetChild(0).GetComponent<Text>();
        text.text = "";
        yield return new WaitForSeconds(0.5f);

        foreach(char letter in log[0].ToCharArray()) {
            text.text += letter;
            yield return new WaitForSeconds(0.02f);
        }

        if(log.Length > 1) {
            string[] result = new string[log.Length - 1];
            for(int i = 0; i < result.Length; i++) {
                result[i] = log[i + 1];
            }
            yield return new WaitForSeconds(1.5f);
            StartPrint(result);
        }
    }

    private IEnumerator PrintLog(SomeDele some_dele, params string[] log) {
        Text text = transform.GetChild(0).GetComponent<Text>();
        text.text = "";
        yield return new WaitForSeconds(0.5f);

        foreach(char letter in log[0].ToCharArray()) {
            text.text += letter;
            yield return new WaitForSeconds(0.02f);
        }

        if(log.Length > 1) {
            string[] result = new string[log.Length - 1];
            for(int i = 0; i < result.Length; i++) {
                result[i] = log[i + 1];
            }
            yield return new WaitForSeconds(1.5f);
            StartPrint(some_dele, result);
        } else {
            yield return new WaitForSeconds(1);
            some_dele();
        }
    }

    public void StartPrint(params string[] text) {
        if(coruoutine != null) StopCoroutine(coruoutine);

        coruoutine = PrintLog(text);
        StartCoroutine(coruoutine);
    }

    public void StartPrint(SomeDele some_dele, params string[] text) {
        if(coruoutine != null) StopCoroutine(coruoutine);

        coruoutine = PrintLog(some_dele, text);
        StartCoroutine(coruoutine);
    }
}
