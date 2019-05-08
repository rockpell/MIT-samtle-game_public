using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterviewMainController : MonoBehaviour {

    public Slider npc_gage_1;
    public Slider npc_gage_2;
    public Slider npc_gage_3;
    public Slider npc_gage_4;

    public Text _leftSelect;

    public GameObject _dialogBox;

    [SerializeField] private TextController_Interview[] words;

    private WordDepository word_depository;
    private DialogBoxController _dialogBoxController;
    [SerializeField] private GameObject[] npc;

    private GameManager _gameManager;
    private StatusManager _statusManger;

    private List<int> randomBox;

    private int _selectWordCount;
    private bool _isSelectOver = true;

    // Use this for initialization
    void Start () {
        word_depository = new WordDepository();

        _gameManager = GameManager.Instance;
        if (GameManager.Instance != null)
            _statusManger = GameManager.Instance.gameObject.GetComponent<StatusManager>();

        randomBox = new List<int>();
        initiList();
        Invoke("loadWords", 0.5f);

        _selectWordCount = 1;
        RefreshLeftSelectUI();
        _dialogBoxController = _dialogBox.GetComponent<DialogBoxController>();

        //GameObject.Find("Main Camera").transform.Find("Fade").gameObject.SetActive(true);
        //StartCoroutine(screen_manager.FadeImage(-1));
        _dialogBoxController.StartPrint(TempMehod, "몇 일 후...", "면접 당일");
        
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private void TempMehod()
    {
        _dialogBoxController.StartPrint("");
        SceenFadeIn(SelectStart);
    }

    public void SelectStart() {
        _dialogBoxController.StartPrint(UIOff, "마음에 드는 단어를 선택하세요", "선택한 단어에 따라 결과가 달라집니다.");
    }

    private void UIOff() {
        _dialogBox.SetActive(false);
        ActiveSlectOver();
    }

    void loadWords() {
        for(int i = 0; i < words.Length; i++) {
            int value = selectNumber();
            words[i].setText(word_depository.getWord(value));
            words[i].setProperty(value);
        }
        initiList();
    }
    
    int selectNumber() {
        int temp = Random.Range(0, randomBox.Count);
        int result = randomBox[temp];
        randomBox.RemoveAt(temp);
        return result;
    }

    void initiList() {
        for(int i = 1; i < 5; i++) {
            randomBox.Add(i);
            randomBox.Add(i);
        }
    }

    public void reLoadWords() {
        loadWords();
    }

    public void affectGage(int property) {
        switch(property) {
            case 1:
                npc_gage_1.value += 1;
                npc[0].SendMessage("react");
                break;
            case 2:
                npc_gage_2.value += 1;
                npc[1].SendMessage("react");
                break;
            case 3:
                npc_gage_3.value += 1;
                npc[2].SendMessage("react");
                break;
            case 4:
                npc_gage_4.value += 1;
                npc[3].SendMessage("react");
                break;
            default:
                break;
        }
    }

    public void AddSelectWordTime() {
        _selectWordCount += 1;
        RefreshLeftSelectUI();

        if(_selectWordCount >= 20) { // interview 씬 종료
            _isSelectOver = true;
            _dialogBox.SetActive(true);
            SceenFadeOut(null);
            ActiveSlectOver();
            _dialogBoxController.StartPrint(LoadScenePrincessMaker, "입생이는 면접을 마치고 기숙사로 돌아왔다.", "다음주부터 교육이라고 하니 힘내도록 하자!",
                "입생이는 잠에 들었다.", "그리고 주말이 지나고 월요일이 되었다.");
            AffectStatus();
        }
    }

    private void AffectStatus() // 면접 결과에 따른 스테이터스 증가
    {
        if (_statusManger == null) return;

        _statusManger.AddValueStatus("코딩력", (int)npc_gage_1.value);
        _statusManger.AddValueStatus("신앙심", (int)npc_gage_2.value);
        _statusManger.AddValueStatus("대인관계", (int)npc_gage_3.value);
        _statusManger.AddValueStatus("감수성", (int)npc_gage_4.value);
    }

    private void SceenFadeIn(ScreenManager.SomeDele some_dele) {
        ScreenManager screen_manager = GameObject.Find("DialogBoxCanvas").transform.Find("Fade").GetComponent<ScreenManager>();
        if(some_dele == null)
        {
            StartCoroutine(screen_manager.FadeImage(1));
        } else
        {
            StartCoroutine(screen_manager.FadeImage(some_dele, 1));
        }
        
    }

    private void SceenFadeOut(ScreenManager.SomeDele some_dele) {
        ScreenManager screen_manager = GameObject.Find("DialogBoxCanvas").transform.Find("Fade").GetComponent<ScreenManager>();
        if (some_dele == null)
        {
            StartCoroutine(screen_manager.FadeImage(-1));
        }
        else
        {
            StartCoroutine(screen_manager.FadeImage(some_dele, -1));
        }
    }

    private void LoadScenePrincessMaker() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("PrincessMaker");
    }

    private void RefreshLeftSelectUI() {
        _leftSelect.text = _selectWordCount + " / 20";
    }

    public bool GetIsSelectOver() {
        return _isSelectOver;
    }

    public void ActiveSlectOver() {
        _isSelectOver = !_isSelectOver;
    }
}

class WordDepository {
    private string[] npc_1 = {"스팀", "바이오쇼크", "데드셀", "어햇인타임", "OneShot", "더바인딩오브아이작", "슬라이 쿠퍼",
        "와치독스", "월-E", "본아이덴티티", "후드", "타라덩컨", "웹툰", "집돌이" }; // 김동현
    private string[] npc_2 = {"드립커피", "빵", "햄스터", "피어싱", "보라색", "검은색", "태양", "앤드류가필드", "라미말렉",
        "부엉이", "테일즈런너", "자취생", "모자", "동양풍", "음양사", "아이폰", "메가박스" }; // 이가희
    private string[] npc_3 = {"사이퍼즈", "세일러문", "그래퍼", "요괴워치", "연어", "일어일문학과", "김케장", "레벨파이브",
        "라멘", "부리또", "가면라이더", "심즈", "얼음", "사이툴", "아보카도", "캬라멜우유", "인문대", "브릿지", "테니스" }; // 안예지
    private string[] npc_4 = { "하스스톤", "스카이림", "블리자드", "로봇", "제어", "집부", "집", "맥주", "사이다", "아이패드",
        "디아블로", "15", "트수", "유튜브", "어바웃타임", "일본", "너구리", "던그리드", "휴강", "공강", "탑신병자", "하이머딩거" }; // 주성현

    public string getWord(int num) {
        int temp;
        switch(num) {
            case 1:
                temp = randSelect(npc_1.Length);
                return npc_1[temp];
            case 2:
                temp = randSelect(npc_2.Length);
                return npc_2[temp];
            case 3:
                temp = randSelect(npc_3.Length);
                return npc_3[temp];
            case 4:
                temp = randSelect(npc_4.Length);
                return npc_4[temp];
            default:
                return null;
        }
    }

    int randSelect(int max) {
        return Random.Range(0, max);
    }
}
