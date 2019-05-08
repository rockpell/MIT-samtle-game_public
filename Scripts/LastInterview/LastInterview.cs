using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastInterview : MonoBehaviour {

    public GameObject _dialogBox;
    private DialogBoxController _dialogBoxController;

    // Use this for initialization
    void Start () {
        string text = DecideComent();
        _dialogBoxController = _dialogBox.GetComponent<DialogBoxController>();
        _dialogBoxController.StartPrint(LoadSceneEnding, "모여 앉아 있는 MIT동아리 사람들", "??? : …..그렇군….그럼 다음",
            "??? : 입생이는 어떤거 같아?", "??? : " + text, "??? : 그럼 입생이는 최종적으로…");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LoadSceneEnding()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
    }

    private string DecideComent()
    {
        string bestStatus = FindBestStatus();
        string result = null;

        if(bestStatus == "코딩력")
        {
            result = "입생이는 코딩을 제법 잘 하더라구요";
        } else if(bestStatus == "대인관계")
        {
            result = "입생이는 친화력이 좋더라구요";
        } else if(bestStatus == "신앙심")
        {
            result = "입생이는 신앙심이 뛰어나더군요";
        }

        return result;
    }

    private string FindBestStatus()
    {
        StatusManager statusManager;
        if (GameManager.Instance != null)
        {
            statusManager = GameManager.Instance.gameObject.GetComponent<StatusManager>();
            Dictionary<string, int> repository = statusManager.GetStatusRepository();
            List<int> tempList = new List<int>();
            string result = null;
            foreach (KeyValuePair<string, int> p in repository)
            {
                if(p.Key == "코딩력" || p.Key == "대인관계" || p.Key == "신앙심")
                {
                    tempList.Add(p.Value);
                }
                
            }
            tempList.Sort();

            foreach (KeyValuePair<string, int> p in repository)
            {
                if (p.Value == tempList[tempList.Count - 1])
                {
                    result = p.Key;
                }
            }

            return result;
        }
        return null;
    }
}
