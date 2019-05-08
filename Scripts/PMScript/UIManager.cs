using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject _mainButtonUI;
    public GameObject _statusBoxUI;
    public GameObject _scheduleUI;
    public GameObject _shoppingUI;
    public GameObject _dialogBox;
    public GameObject _backButton;
    public GameObject _acceptButton;
    public GameObject _moneyValueUI;
    public GameObject _calendarBox;
    public GameObject _dialogBoxSchedule;
    public GameObject _miniStatusBoxUI;
    public GameObject _scheduleMoneyBoxUI;
    public GameObject _scheduleProgressUI;

    public GameObject _scheduleBottle;
    public GameObject[] _scheduleTargetPositions;

    private Text _gamePriceUI;

    private delegate void Dele();
    private Dictionary<string, int> _statusRepository;

    private GameObject[] _inBottleSchedule;
    private List<string> _selectedSchedule;

    private delegate void SomeDele();

    private StatusManager _statusManger;
    private Item[] _items;

    private string _selectItem;
    private int[] _statusValues;
    private bool _isCodingStudy; // 코딩 수업 참가 여부
    private bool _isSelectCodingStudy; // 코딩 수업 참가 선택지 선택 유무
    private int _dateCounter; //날짜 흐름
    private int _scheduleIndex; //현재 스케쥴 목록 번호

    private int _date;
    private int _month;
    private string _day;

    private CutSceneSwap _cutSceneSwap;

    public static UIManager _instance;

    public static UIManager Instance {
        get { return _instance; }
        private set { _instance = value; }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        _statusRepository = new Dictionary<string, int>();
        _selectedSchedule = new List<string>();
        _inBottleSchedule = new GameObject[3];
        _statusValues = new int[8];
        _items = new Item[10];
        _gamePriceUI = _shoppingUI.transform.Find("ShoppingItem_10").Find("Item_price").GetComponent<Text>();
        _cutSceneSwap = GameObject.Find("Canvas").transform.Find("ScheduleProgressUI").Find("CutSceneImage").GetComponent<CutSceneSwap>();
        InitItems();

        _statusManger = StatusManager.Instance;

        _dateCounter = _statusManger.DateCounter;
        _scheduleIndex = _statusManger.ScheduleIndex;
        _isCodingStudy = StatusManager.Instance.IsCodingStudy;
        _isSelectCodingStudy = StatusManager.Instance.IsSelectCodingStudy;

        _date = Calendar.GetInstance().GetDay();
        _month = Calendar.GetInstance().GetMonth();
        _day = Calendar.GetInstance().GetTextday();

        SetCalendarUI(_month, _date, _day);
        ChangeDiaglogText("무엇을 할까?");
        Invoke("RefreshUI", 0.5f);

        if (_statusManger.SelectedSchedule != null)
        { // 프메씬에 재 진입시에 남아있는 스케줄을 진행
            _mainButtonUI.SetActive(false);
            ChangeDiaglogText(LeftScheduleStart, "자유행동이 종료되었습니다.");
        } else
        {
            _mainButtonUI.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LeftScheduleStart()
    {
        _dialogBox.SetActive(false);
        _cutSceneSwap.SceneActive(false);
        ChangeAnswerButtonBasic();
        _selectedSchedule = _statusManger.SelectedSchedule;
        InvokeRepeating("ScheduleStart", 0f, 3);
    }

    public void selectSchedule(GameObject target) {
        string name = target.transform.Find("Text").GetComponent<Text>().text;
        if(_selectedSchedule.Count < 3) {
            AddSelectedSchedule(name);

            for(int i = 0; i < _inBottleSchedule.Length; i++) {
                if(_inBottleSchedule[i] == null) {
                    _inBottleSchedule[i] = Instantiate(target, _scheduleBottle.transform, true);
                    _inBottleSchedule[i].GetComponent<ScheduleUIInteration>().SetIsSelected();
                    MoveToPositionObject(_inBottleSchedule[i], _scheduleTargetPositions[i].GetComponent<RectTransform>().localPosition);
                    break;
                }
            }
            
        } else {
            Debug.Log("schedule full");
        }
        
    }
    
    public void DecideAcceptButton() {
        if(_selectedSchedule.Count == 3) {
            _acceptButton.SetActive(true);
        } else {
            _acceptButton.SetActive(false);
        }
    }

    void MoveToPositionObject(GameObject target, Vector3 position) {
        RectTransform temp = target.GetComponent<RectTransform>();
        temp.localPosition = position;
    }

    void AddSelectedSchedule(string text) {
        _selectedSchedule.Add(text);
    }

    void RemoveSelectedSchedule(string text) {
        _selectedSchedule.Remove(text);
    }

    public void RemoveInBottleSchedule(GameObject target) {
        for(int i = 0; i < _inBottleSchedule.Length; i++) {
            if(target.Equals(_inBottleSchedule[i])){
                string name = target.transform.Find("Text").GetComponent<Text>().text;
                _inBottleSchedule[i] = null;
                RemoveSelectedSchedule(name);
            }
        }
    }

    private void ResetSchedule() {
        _selectedSchedule = new List<string>();
        for(int i = 0; i < _inBottleSchedule.Length; i++) {
            if(_inBottleSchedule[i] != null) {
                _inBottleSchedule[i].GetComponent<ScheduleUIInteration>().SelfDestroy();
                //_inBottleSchedule[i] = null;
            }
        }
    }

    public void WorkUI(string text) {
        if(text == "스테이터스") {
            _mainButtonUI.SetActive(false);
            _statusBoxUI.SetActive(true);
            _dialogBox.SetActive(false);
            _backButton.SetActive(true);
            RefreshUI();
        } else if(text == "스케줄") {
            if(!_isSelectCodingStudy) {
                SelectCodingStudy();
                return;
            }
            ChangeAnswerButtonBasic();
            _mainButtonUI.SetActive(false);
            _scheduleUI.SetActive(true);
            _backButton.SetActive(true);
            _acceptButton.SetActive(false);
            RefreshUI();
            ChangeDiaglogText("이번주 스케줄을 선택하세요");
        } else if(text == "쇼핑") {
            _backButton.SetActive(true);
            _shoppingUI.SetActive(true);
            _mainButtonUI.SetActive(false);
            ChangeDiaglogText("무엇을 살까?");
            GameSale();
        } else if(text == "뒤로가기") {
            _mainButtonUI.SetActive(true);
            _scheduleUI.SetActive(false);
            _statusBoxUI.SetActive(false);
            _dialogBox.SetActive(true);
            _backButton.SetActive(false);
            _acceptButton.SetActive(false);
            _shoppingUI.SetActive(false);
            ChangeDiaglogText("무엇을 할까?");
            ResetSchedule();
        } else if(text == "완료") {
            ChangeDiaglogText("이대로 스케줄을 진행하시겠습니까?");
            ChangeAnswerButtonSchedule();
        } else if(text == "진행") { // 스케줄 진행
            _backButton.SetActive(false);
            _acceptButton.SetActive(false);
            _scheduleUI.SetActive(false);
            _dialogBox.SetActive(false);
            ChangeAnswerButtonBasic();
            InvokeRepeating("ScheduleStart", 0f, 3);
            _statusManger.SelectedSchedule = _selectedSchedule; // status manager에 선택된 스케줄 리스트 저장
        } else if(text == "취소") { // 스케줄 진행 취소
            ChangeAnswerButtonBasic();
            ChangeDiaglogText("이번주 스케줄을 선택하세요");
        } else if(text == "구매") { // 아이템 구매
            ChangeAnswerButtonBasic();
            _acceptButton.SetActive(false);
            ChangeDiaglogText(_selectItem + "을(를) 구매하였다.", "무엇을 살까?");
            PurchaseItem();
            RefreshUI();
        } else if(text == "보류") { // 아이템 구매 취소
            ChangeAnswerButtonBasic();
            _acceptButton.SetActive(false);
            ChangeDiaglogText("무엇을 살까?");
        } else if(text == "참가한다") {
            _isCodingStudy = true;
            StatusManager.Instance.IsCodingStudy = _isCodingStudy;
            _backButton.SetActive(false);
            _acceptButton.SetActive(false);
            ChangeDiaglogText(ActWorkUISchedule , "앞으로 동아리 코딩 수업에 참가합니다.", "");
        } else if(text == "안간다") {
            _isCodingStudy = false;
            StatusManager.Instance.IsCodingStudy = _isCodingStudy;
            _backButton.SetActive(false);
            _acceptButton.SetActive(false);
            ChangeDiaglogText(ActWorkUISchedule, "동아리 코딩 수업에 참가하지 않습니다.", "");
        }
    }

    public void SelectShoppingItem(string text) {
        _selectItem = text;
        Item target = GetItem(text);
        if(target.Price > _statusManger.Money) { // money check
            ChangeDiaglogText("돈이 부족합니다.");
            _acceptButton.SetActive(false);
            ChangeAnswerButtonBasic();
            return;
        } else {
            ChangeDiaglogText(text + "을(를) 구매하시겠습니까?");
            ChangeAnswerButtonShopping();
            _backButton.SetActive(true);
            _acceptButton.SetActive(true);
        }
    }

    private void PurchaseItem() {
        Item target = GetItem(_selectItem);
        _statusManger.AddMoney(-1 * target.Price);

        if(target.IsItemPropoertys()) {
            foreach(KeyValuePair<string, int> dic in target.GetItemPropertys()) {
                _statusManger.AddValueStatus(dic.Key, dic.Value);
            }
        }

        if(target.Name == "로또복권") {
            int rand_num = RandomNumber(1000);
            if(rand_num == 0) {
                _statusManger.AddMoney(10000);
                ChangeDiaglogText("10000G 당첨되었습니다.", "무엇을 살까?");
            } else if(0 < rand_num && rand_num < 5) {
                _statusManger.AddMoney(1000);
                ChangeDiaglogText("1000G 당첨되었습니다.", "무엇을 살까?");
            } else if(5 <= rand_num && rand_num < 20) {
                _statusManger.AddMoney(200);
                ChangeDiaglogText("200G 당첨되었습니다.", "무엇을 살까?");
            } else if(20 <= rand_num && rand_num < 50) {
                _statusManger.AddMoney(50);
                ChangeDiaglogText("50G 당첨되었습니다.", "무엇을 살까?");
            } else if(50 <= rand_num && rand_num < 300) {
                _statusManger.AddMoney(5);
                ChangeDiaglogText("5G 당첨되었습니다.", "무엇을 살까?");
            } else {
                ChangeDiaglogText("꽝입니다.", "무엇을 살까?");
            }
        }
        _selectItem = null;
    }

    private int RandomNumber(int num) {
        int result = Random.Range(0, num);
        return result;
    }

    private void GameSale() {
        int rand_temp = RandomNumber(15);
        GetItem("스팀게임").Price = 20 - rand_temp;
        _gamePriceUI.text = (20 - rand_temp).ToString() + "G";
    }

    private void ActWorkUISchedule() {
        WorkUI("스케줄");
    }

    private void SelectCodingStudy() {
        ChangeDiaglogText("정기적으로 동아리 코딩 수업에 참가 할 수 있습니다.", "스케줄 선택에 영향을 주지 않습니다.", "앞으로 동아리 코딩 수업에 참가 하시겠습니까?");
        ChangeAnswerCodingStudy();
        _isSelectCodingStudy = true;
        StatusManager.Instance.IsSelectCodingStudy = _isSelectCodingStudy;
        _backButton.SetActive(true);
        _acceptButton.SetActive(true);
        _mainButtonUI.SetActive(false);
    }

    private void ChangeAnswerButtonShopping() {
        _acceptButton.GetComponent<UIChanger>().ChangName("구매");
        _backButton.GetComponent<UIChanger>().ChangName("보류");
    }

    private void ChangeAnswerButtonSchedule() {
        _acceptButton.GetComponent<UIChanger>().ChangName("진행");
        _backButton.GetComponent<UIChanger>().ChangName("취소");
    }

    private void ChangeAnswerButtonBasic() {
        _acceptButton.GetComponent<UIChanger>().ChangName("완료");
        _backButton.GetComponent<UIChanger>().ChangName("뒤로가기");
    }

    private void ChangeAnswerCodingStudy() {
        _acceptButton.GetComponent<UIChanger>().ChangName("참가한다");
        _backButton.GetComponent<UIChanger>().ChangName("안간다");
    }

    public void RefreshUI() {
        RefreshUIMoney();
        RefreshStatusRepository();
    }

    public void RefreshStatusRepository() {
        SetStatusRepository(_statusManger.GetStatusRepository());
    }

    private void SetStatusRepository(Dictionary<string, int> statusDic) {
        foreach(KeyValuePair<string, int> dic in statusDic) {
            if(_statusRepository.ContainsKey(dic.Key)) {
                _statusRepository[dic.Key] = dic.Value;
            } else {
                _statusRepository.Add(dic.Key, dic.Value);
            }
        }
        StatusBoxReFresh();
    }

    private void StatusBoxReFresh() {
        for(int i = 0; i < _statusBoxUI.transform.childCount; i++) { // 스테이터스 창 갱신
            Transform temp_transform =  _statusBoxUI.transform.GetChild(i);
            string this_ui_name = temp_transform.Find("StatusName").GetComponent<Text>().text;
            Slider this_ui_value = temp_transform.Find("StatusBar").GetComponent<Slider>();
            if(_statusRepository.ContainsKey(this_ui_name))
                this_ui_value.value = _statusRepository[this_ui_name];
        }

        for(int i = 0; i < _miniStatusBoxUI.transform.childCount; i++) { //  미니 스테이터스 갱신
            Transform temp_transform = _miniStatusBoxUI.transform.GetChild(i);
            string temp_name = temp_transform.Find("StatusName").GetComponent<Text>().text;
            if(_statusRepository.ContainsKey(temp_name)) {
                temp_transform.Find("StatusBar").GetComponent<Slider>().value = _statusRepository[temp_name];
            }
            
        }
    }

    private void ChangeDiaglogText(params string[] text) {
        DialogBoxController _dialogBox_controller = GameObject.Find("DialogBox").GetComponent<DialogBoxController>();
        _dialogBox_controller.StartPrint(text);
    }

    private void ChangeDiaglogText(DialogBoxController.SomeDele some_dele, params string[] text) {
        DialogBoxController _dialogBox_controller = GameObject.Find("DialogBox").GetComponent<DialogBoxController>();
        _dialogBox_controller.StartPrint(some_dele, text);
    }

    public void ChangeDiaglogTextSchedule(params string[] text) { // 스케줄용
        DialogBoxController _dialogBox_controller = GameObject.Find("DialogBox_Schedule").GetComponent<DialogBoxController>();
        _dialogBox_controller.StartPrint(text);
    }

    public void SetUIMoney(int num) {
        _moneyValueUI.GetComponent<Text>().text = num.ToString() + "G";
    }

    public void RefreshUIMoney() {
        SetUIMoney(_statusManger.Money);
    }

    public void SetCalendarUI(int month, int day, string text) {
        SetMonthUI(month);
        SetDayUI(day);
        SetTextDayUI(text);
    }

    private void SetMonthUI(int num) {
        _calendarBox.transform.GetChild(1).GetComponent<Text>().text = num.ToString() + "월";
    }

    private void SetDayUI(int num) {
        _calendarBox.transform.GetChild(2).GetComponent<Text>().text = num.ToString() + "일";
    }

    private void SetTextDayUI(string text) {
        _calendarBox.transform.GetChild(3).GetComponent<Text>().text = text + "요일";
    }

    private void SetMiniStatus(params string[] texts) {
        _miniStatusBoxUI.gameObject.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            Transform temp_transform = _miniStatusBoxUI.transform.GetChild(i);
            temp_transform.gameObject.SetActive(false);
        }

        if(texts.Length < 4) {
            
            for(int i = 0; i < texts.Length; i++) {
                Transform temp_transform = _miniStatusBoxUI.transform.GetChild(i);
                temp_transform.Find("StatusName").GetComponent<Text>().text = texts[i];
                temp_transform.Find("StatusBar").GetComponent<Slider>().value = _statusRepository[texts[i]];
                temp_transform.gameObject.SetActive(true);
            }
        }
    }

    private void ActiveMiniStatus(string name)  // 스케줄 이름에 따라 필요한 미니 스테이터스창 활성화
    {
        switch (name)
        {
            case "술자리":
                Invoke("RefreshUI", 1f);
                SetMiniStatus("알콜수치", "대인관계", "스트레스");
                break;
            case "휴식":
                Invoke("RefreshUI", 1f);
                SetMiniStatus("신앙심", "스트레스");
                break;
            case "여행":
                Invoke("RefreshUI", 1f);
                SetMiniStatus("감수성", "대인관계", "스트레스");
                break;
            case "공부":
                Invoke("RefreshUI", 1f);
                SetMiniStatus("학점", "스트레스");
                break;
            case "알바":
                Invoke("RefreshUI", 1f);
                SetMiniStatus("스트레스");
                break;
            case "수업":
                Invoke("RefreshUI", 1f);
                SetMiniStatus("코딩력", "대인관계", "스트레스");
                break;
        }
    }

    private void HIdeMiniStatus() {
        _miniStatusBoxUI.gameObject.SetActive(false);
        for(int i =0; i < _miniStatusBoxUI.transform.childCount; i++) {
            Transform temp_transform = _miniStatusBoxUI.transform.GetChild(i);
            temp_transform.gameObject.SetActive(false);
        }
    }

    public void ActiveProfitUI(int value) {
        _scheduleMoneyBoxUI.SetActive(true);
        _scheduleMoneyBoxUI.transform.Find("ScheduleMoneyValue").GetComponent<Text>().text = value.ToString() + "G";
    }

    public void DeActiveProfitUI() {
        _scheduleMoneyBoxUI.SetActive(false);
    }

    public void ScheduleEnd() {
        _dialogBox.SetActive(true);
        HIdeMiniStatus();
        _scheduleProgressUI.SetActive(false);
        ChangeDiaglogText(ScheduleEndAfterUI, "스케줄이 종료되었습니다.", "무엇을 할까?");
    }

    private void ScheduleEndAfterUI() {
        _mainButtonUI.SetActive(true);
        DeActiveProfitUI();
    }

    private void GameEndUI()
    {
        _dialogBox.SetActive(true);
        _scheduleProgressUI.SetActive(false);
        ChangeDiaglogText(FadeOut, "최종과제", "과제를 달성하면 추가로 코딩력이 올라갑니다.");
    }

    private void FadeOut()
    {
        ScreenManager screenManger = GameObject.Find("FadeCanvas").transform.Find("Fade").GetComponent<ScreenManager>();
        StartCoroutine(screenManger.FadeImage(GameEnd, -1));
    }

    private void GameEnd()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("2048(256)_revise");
    }

    public void SetDate()
    {
        if ((_date == 31) && (_month == 3))
        {
            _month++;
            _date = 0;
        }
        if ((_date == 30) && (_month == 4))
        {
            _month++;
            _date = 0;
        }
        _date++;
        DayChange();

        SetCalendarUI(_month, _date, _day);
        Calendar.GetInstance().SetCalendar(_month, _date, _day);
    }
    
    private void DayChange()
    {
        switch (_day)
        {
            case "월":
                _day = "화";
                break;
            case "화":
                _day = "수";
                break;
            case "수":
                _day = "목";
                break;
            case "목":
                _day = "금";
                break;
            case "금":
                _day = "토";
                break;
            case "토":
                _day = "일";
                break;
            case "일":
                _day = "월";
                break;
            default:
                Debug.Log("잘못된 요일");
                break;
        }
    }

    public void ScheduleStart() { // 스케줄 선택 완료후 스케줄 진행
        if(_selectedSchedule.Count > _scheduleIndex)
            _scheduleProgressUI.SetActive(true);

        if (_scheduleIndex < 3)
        {
            if (_selectedSchedule[_scheduleIndex] != "자유행동")
                _cutSceneSwap.SceneActive(true);
        }
        
        if (_dateCounter == 0)
        {
            _cutSceneSwap.SwapCutScene(_selectedSchedule[_scheduleIndex]);
            
            ActiveMiniStatus(_selectedSchedule[_scheduleIndex]);
            
            if(_selectedSchedule[_scheduleIndex] == "자유행동") // 자유행동 스케줄의 시간 흐름 적용
            {
                _cutSceneSwap.SceneActive(false);
                SetDate();
                _dateCounter++;
            }

            _scheduleIndex++;
            _statusManger.ScheduleIndex = _scheduleIndex;
        }
        else
        {
            SetDate();
        }

        if (((_dateCounter == 5)) || (!_isCodingStudy && (_dateCounter == 2)) || (_isCodingStudy && (_dateCounter == 3)))
        {
            _cutSceneSwap.SwapCutScene(_selectedSchedule[_scheduleIndex]);
            ActiveMiniStatus(_selectedSchedule[_scheduleIndex]);

            if (_selectedSchedule[_scheduleIndex] == "자유행동" && _dateCounter == 5)// 자유행동 스케줄의 시간 흐름 적용
            {
                _dateCounter++;
                SetDate();
            }
            else if (_selectedSchedule[_scheduleIndex] == "자유행동" && (!_isCodingStudy && _dateCounter == 2))
            {
                SetDate();
                SetDate();
                _dateCounter += 2;
            }

            _scheduleIndex++;
            _statusManger.ScheduleIndex = _scheduleIndex;
        }
        if (_isCodingStudy && ((_dateCounter == 2) || (_dateCounter == 4)))
        {
            _cutSceneSwap.SceneActive(true);
            if(_dateCounter == 2)
            {
                _cutSceneSwap.SwapCutScene("수업");
            } else if(_dateCounter == 4)
            {
                _cutSceneSwap.SwapCutScene("수업2");
            }
            ActiveMiniStatus("수업");
        }
        _dateCounter++;

        if (_dateCounter == 8)
        {
            _dateCounter = 0;
            _scheduleIndex = 0;
            _statusManger.ScheduleIndex = _scheduleIndex;
            _cutSceneSwap.SceneActive(false);
            ResetSchedule();

            CancelInvoke("ScheduleStart");
            if (!GameManager.Instance.CheckEndDay())
            {
                Invoke("ScheduleEnd", 0f);
            } else // 지정된 날짜가 되면 프메 종료
            {
                Invoke("GameEndUI", 0f);
            }
            
        }
        _statusManger.DateCounter = _dateCounter; // status manger에 값 저장
        //Debug.Log(_dateCounter);
    }

    private void InitItems() {
        _items[0] = new Item("핸드폰", 100); _items[0].AddItemProperty("대인관계", 10);
        _items[1] = new Item("노트북", 150); _items[1].AddItemProperty("코딩력", 8); _items[1].AddItemProperty("학점", 8);
        _items[2] = new Item("콘솔게임기", 60); _items[2].AddItemProperty("스트레스감소", 20);
        _items[3] = new Item("책", 10); _items[3].AddItemProperty("감수성", 1);
        _items[4] = new Item("앨범", 20); _items[4].AddItemProperty("감수성", 1); _items[4].AddItemProperty("신앙심", 1);
        _items[5] = new Item("기계식키보드", 40); _items[5].AddItemProperty("감수성", 2); _items[5].AddItemProperty("코딩력", 2);
        _items[6] = new Item("집", 10000); _items[6].AddItemProperty("집", 1);
        _items[7] = new Item("로또복권", 5); 
        _items[8] = new Item("피규어", 20); _items[8].AddItemProperty("신앙심", 2);
        _items[9] = new Item("스팀게임", 20); _items[9].AddItemProperty("스트레스감소", 2); _items[9].AddItemProperty("스팀게임", 1);

    }

    private Item GetItem(string text) {
        for(int i = 0; i < _items.Length; i++) {
            if(_items[i].Name == text) {
                return _items[i];
            }
        }
        return null;
    }
}

public class Item{
    private string _name;
    private int _price;
    private Dictionary<string, int> _itemPropertys; // 스트레스감소는 휴식시 스트레스 감소치 증가


    public Item(string name, int price) {
        _name = name;
        _price = price;
        _itemPropertys = new Dictionary<string, int>();
    }

    public void AddItemProperty(string text, int value) {
        _itemPropertys.Add(text, value);
    }

    public Dictionary<string, int> GetItemPropertys() {
        return _itemPropertys;
    }

    public bool IsItemPropoertys() {
        if(_itemPropertys == null) {
            return false;
        } else {
            return true;
        }
    }

    public int GetPropertyValue(string text) {
        int result;

        _itemPropertys.TryGetValue(text, out result);

        return result;
    }

    public string Name {
        get { return _name; }
        set { _name = value; }
    }

    public int Price {
        get { return _price; }
        set { _price = value; }
    }
}
