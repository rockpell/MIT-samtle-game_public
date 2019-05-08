using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour {

    private int _codingPower;
    private int _sensitivity;
    private int _religiosity;
    private int _interpersonal;
    private int _grade;
    private int _alcohol;
    private int _stress;
    private int _phoneNumberCount;
    private int _steamGame;
    private int _arbeit;

    private int _stessDecrease;
    private int _money;
    private bool _house;

    private int _dateCounter;
    private int _scheduleIndex;
    List<string> _selectedSchedule;

    private bool _isCodingStudy;
    private bool _isSelectCodingStudy;

    private Calendar _calendar;

    private Dictionary<string, int> _statusRepository;

    private static StatusManager _instance;

    public static StatusManager Instance
    {
        get { return _instance; }
        private set{ _instance = value; }
    }

    // Use this for initialization
    void Awake () {
        if (_instance == null)
        {
            _instance = this;
        }
        
        _calendar = Calendar.GetInstance();

        _codingPower = 0;
        _sensitivity = 0;
        _religiosity = 0;
        _interpersonal = 0;
        _grade = 0;
        _alcohol = 0;
        _stress = 0;
        _phoneNumberCount = 0;
        _steamGame = 0;
        _arbeit = 0;

        _dateCounter = 0;

        _stessDecrease = 0;
        _money = 100;
        _house = false;

        _scheduleIndex = 0;
        _dateCounter = 0;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Dictionary<string, int> GetStatusRepository() {
        _statusRepository = new Dictionary<string, int>();

        _statusRepository.Add("코딩력", _codingPower);
        _statusRepository.Add("감수성", _sensitivity);
        _statusRepository.Add("신앙심", _religiosity);
        _statusRepository.Add("대인관계", _interpersonal);
        _statusRepository.Add("학점", _grade);
        _statusRepository.Add("알콜수치", _alcohol);
        _statusRepository.Add("스트레스", _stress);
        _statusRepository.Add("전화번호수", _phoneNumberCount);
        _statusRepository.Add("스팀게임", _steamGame);

        return _statusRepository;
    }

    public void AddValueStatus(string text, int value) {
        SetStatus(text, GetStatus(text) + value);

        if(GetStatus(text) > 200) {
            SetStatus(text, 200);
        }
    }

    public void SetStatus(string text, int value) {
        switch(text) {
            case "코딩력":
                _codingPower = value;
                break;
            case "감수성":
                _sensitivity = value;
                break;
            case "신앙심":
                _religiosity = value;
                break;
            case "대인관계":
                _interpersonal = value;
                break;
            case "학점":
                _grade = value;
                break;
            case "알콜수치":
                _alcohol = value;
                break;
            case "스트레스":
                _stress = value;
                break;
            case "전화번호수":
                _phoneNumberCount = value;
                break;
            case "스팀게임":
                _steamGame = value;
                break;
            case "스트레스감소":
                _stessDecrease = value;
                break;
            case "집":
                _house = true;
                break;
            case "알바횟수":
                _arbeit = value;
                break;
            default:
                break;
        }
    }

    public int GetStatus(string text) {
        switch(text) {
            case "코딩력":
                return _codingPower;
            case "감수성":
                return _sensitivity;
            case "신앙심":
                return _religiosity;
            case "대인관계":
                return _interpersonal;
            case "학점":
                return _grade;
            case "알콜수치":
                return _alcohol;
            case "스트레스":
                return _stress;
            case "전화번호수":
                return _phoneNumberCount;
            case "스팀게임":
                return _steamGame;
            case "알바횟수":
                return _arbeit;
            default:
                return 0;
        }
    }

    public void SetStatus(Dictionary<string, int> dictionary) {
        foreach(KeyValuePair<string, int> dic in dictionary) {
            SetStatus(dic.Key, dic.Value);
        }
    }

    public void AddMoney(int value) {
        _money += value;
    }

    public bool IsStress() {
        if(_stress == 200) {
            return true;
        } else {
            return false;
        }
    }

    public bool IsStress(int value)
    {
        if (_stress + value >= 200)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsStress(string text)
    {
        if(text == "알바")
        {
            return IsStress(25);
        } else if(text == "공부")
        {
            return IsStress(30);
        }

        return false;
    }

    public int CodingPower {
        get { return _codingPower; }
        set { _codingPower = value; }
    }

    public int Sensitivity {
        get { return _sensitivity; }
        set { _sensitivity = value; }
    }

    public int Religiosity {
        get { return _religiosity; }
        set { _religiosity = value; }
    }

    public int Interpersonal {
        get { return _interpersonal; }
        set { _interpersonal = value; }
    }

    public int Grade {
        get { return _grade; }
        set { _grade = value; }
    }

    public int Alcohol {
        get { return _alcohol; }
        set { _alcohol = value; }
    }

    public int Stress {
        get { return _stress; }
        set { _stress = value; }
    }

    public int PhoneNumberCount {
        get { return _phoneNumberCount; }
        set { _phoneNumberCount = value; }
    }

    public int SteamGame {
        get { return _steamGame; }
        set { _steamGame = value; }
    }

    public int Money {
        get { return _money; }
        set { _money = value; }
    }

    public int StessDecrease {
        get { return _stessDecrease; }
        set { _stessDecrease = value; }
    }

    public bool GetHouse() {
        return _house;
    }

    public int GetCalendarMonth() {
        return _calendar.GetMonth();
    }

    public int GetCalendarDay() {
        return _calendar.GetDay();
    }

    public string GetCalendarTextDay() {
        return _calendar.GetTextday();
    }

    public void CalendarAddMonth() {
        _calendar.AddMonth();
    }

    public void CalendarAddDay() {
        _calendar.AddDay();
    }

    public int DateCounter {
        get { return _dateCounter; }
        set { _dateCounter = value; }
    }

    public int ScheduleIndex {
        get { return _scheduleIndex; }
        set { _scheduleIndex = value; }
    }

    public List<string> SelectedSchedule {
        get { return _selectedSchedule; }
        set {
            _selectedSchedule = new List<string>();
            for(int i = 0; i < value.Count; i++)
            {
                _selectedSchedule.Add(value[i]);
            }
        }
    }

    public bool IsCodingStudy {
        get { return _isCodingStudy; }
        set { _isCodingStudy = value; }
    }

    public bool IsSelectCodingStudy {
        get { return _isSelectCodingStudy; }
        set { _isSelectCodingStudy = value; }
    }
}

class Calendar {
    private static Calendar _instance = new Calendar();

    private int _year;
    private int _month;
    private int _day;
    private string _textDay;

    enum Day {일, 월, 화, 수, 목, 금, 토};

    private Calendar() {
        _year = 2019;
        _month = 3;
        _day = 18;
        _textDay = (Day.월).ToString();
    }

    public static Calendar GetInstance() {
        return _instance;
    }
    
    public void AddYear() {
        _year += 1;
    }

    public void AddMonth() {
        if(_month == 12) {
            AddYear();
            _month = 0;
        }
        _month += 1;
    }

    public void AddDay() {
        if((_month == 3 && _day == 31) || (_month == 4 && _day == 30)) {
            AddMonth();
            _day = 0;
        }
        _day += 1;
    }

    public void SetCalendar(int month, int day, string text_day)
    {
        _month = month;
        _day = day;
        _textDay = text_day;
    }

    public int GetYear() {
        return _year;
    }

    public int GetMonth() {
        return _month;
    }

    public int GetDay() {
        return _day;
    }

    public string GetTextday() {
        return _textDay;
    }
}