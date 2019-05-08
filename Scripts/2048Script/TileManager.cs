using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour {

    public GameObject[] tilePositions;
    public GameObject game_end_object;
    public GameObject game_clear_object;
    public Text score_text;

    [SerializeField] private Text _nameText;
    [SerializeField] private GameObject _fade;
    [SerializeField] private GameObject _dialogBox;

    private DialogBoxController _dialogBoxController;
    private TileCreator tc;
    private List<TileClass> tileObjectList;

    private bool is_creatable_init_tile = false;
    private bool is_move_able = false;
    private bool is_game_end = false;

    private int col = 3;
    private int move_cool_time = 0;
    private int move_cool_tile_max = 15;
    private int game_score = 0;

    private int _clearTIleNumber;
    private int[] _clearTileNumbers;

    private StatusManager _statusManager;
    // Use this for initialization
    void Start () {
        tc = this.GetComponent<TileCreator>();
        tileObjectList = new List<TileClass>();

        createInitTile();
        createInitTile();

        move_cool_time = move_cool_tile_max;
        refreshScore();

        _dialogBoxController = _dialogBox.GetComponent<DialogBoxController>();

        GameObject game_manger = GameObject.Find("GameManager");
        if (game_manger != null) _statusManager = game_manger.GetComponent<StatusManager>();

        _clearTileNumbers = new int[] { 8, 16, 32, 64, 128, 256};
        _clearTIleNumber = ChoiceDifficultyLevel();
        //_clearTIleNumber = 0;
        RefreshNameText();

        GameStart();
    }
	
    private void RefreshNameText() {
        _nameText.text = _clearTileNumbers[_clearTIleNumber].ToString();
    }

    private int ChoiceDifficultyLevel() {
        if(_statusManager != null) {
            int coding = _statusManager.GetStatus("코딩력");
            int temp = 200 - coding;

            return temp / 40;
        }
        return 5;
    }

	// Update is called once per frame
	void Update () {
        if(!is_move_able) {
            move_cool_time += 1;

            if(move_cool_time > move_cool_tile_max) {
                is_move_able = true;
            }
        }

        if(is_move_able && !is_game_end) {
            if(Input.GetKeyDown(KeyCode.RightArrow)) {
                checkMove("right");
                checkCreateInitTile();
                is_move_able = false;
                move_cool_time = 0;
                refreshScore();
            } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                checkMove("left");
                checkCreateInitTile();
                is_move_able = false;
                move_cool_time = 0;
                refreshScore();
            } else if(Input.GetKeyDown(KeyCode.UpArrow)) {
                checkMove("up");
                checkCreateInitTile();
                is_move_able = false;
                move_cool_time = 0;
                refreshScore();
            } else if(Input.GetKeyDown(KeyCode.DownArrow)) {
                checkMove("down");
                checkCreateInitTile();
                is_move_able = false;
                move_cool_time = 0;
                refreshScore();
            }
        }
    }

    void checkCreateInitTile() {
        if(is_creatable_init_tile) {
            createInitTile();
            is_creatable_init_tile = false;
        }

        checkGameEnd();
    }

    void createInitTile() {
        int temp = setRandom();
        GameObject gtemp = null;

        while(!checkCreatableTile(temp)) {
            if(isFullTileList()) {
                return;
            }
            temp = setRandom();
        }

        
        gtemp = tc.createTile(2, getTilePosition(temp));
        tileObjectList.Add(new TileClass(gtemp, temp, 2));
        addScore(2);
    }

    int setRandom() {
        int rand = Random.Range(0, col * col);

        return rand;
    }

    int setRandom(int value) {
        int rand = Random.Range(0, value);

        return rand;
    }

    bool isFullTileList() {
        int[] temp_arry = getTileListArray();
        for(int i = 0; i < temp_arry.Length; i++) {
            if(temp_arry[i] == 0) {
                return false;
            }
        }
        return true;
    }

    int[] getTileListArray() {
        int[] result = new int[col*col];

        for(int i = 0; i < col * col; i++) {
            result[i] = 0;
        }
        
        for(int i = 0; i < tileObjectList.Count; i++) {
            result[tileObjectList[i].getPosition()] = tileObjectList[i].getValue();
        }

        return result;
    }
    void createTile(int value, int target_position) {
        Vector3 vtemp = getTilePosition(target_position);
        GameObject gtemp = tc.createTile(value, vtemp);
        tileObjectList.Add(new TileClass(gtemp, target_position, value));
        addScore(value);
    }

    bool checkCreatableTile(int num) {
        int[] temp_arry = getTileListArray();
        if(temp_arry[num] == 0) {
            return true;
        }

        return false;
    }

    Vector3 getTilePosition(int number) {
        Vector3 value = tilePositions[number].transform.position;

        return value;
    }
    
    void checkMove(string direction) {
        if(direction == "left") {
            Queue<TileClass> queue = new Queue<TileClass>();
            int[,] temp_arry = getTileArray();

            for(int i = 0; i < col; i++) {
                for(int p = 0; p < col; p++) {
                    if(temp_arry[i, p] != 0) {
                        queue.Enqueue(findTileClass(i * this.col + p));
                        temp_arry[i, p] = 0;
                    }
                }

                int num = 0;

                while(queue.Count > 0) {
                    TileClass temp_tile = queue.Dequeue();
                    if(temp_arry[i, num] == 0) {
                        temp_arry[i, num] = temp_tile.getValue();
                        moveTile(temp_tile, i, num);
                    } else if(temp_arry[i, num] == temp_tile.getValue()) {
                        temp_arry[i, num] *= temp_arry[i, num];
                        temp_tile.addValue();
                        combineTile(temp_tile, i, num);
                        num += 1;
                    } else {
                        temp_arry[i, ++num] = temp_tile.getValue();
                        moveTile(temp_tile, i, num);
                    }

                }
            }
        } else if(direction == "right") {
            Queue<TileClass> queue = new Queue<TileClass>();
            int[,] temp_arry = getTileArray();

            for(int i = 0; i < col; i++) {
                for(int p = col - 1; p >= 0; p--) {
                    if(temp_arry[i, p] != 0) {
                        queue.Enqueue(findTileClass(i * this.col + p));
                        temp_arry[i, p] = 0;
                    }
                }

                int num = col - 1;

                while(queue.Count > 0) {
                    TileClass temp_tile = queue.Dequeue();
                    if(temp_arry[i, num] == 0) {
                        temp_arry[i, num] = temp_tile.getValue();
                        moveTile(temp_tile, i, num);
                    } else if(temp_arry[i, num] == temp_tile.getValue()) {
                        temp_arry[i, num] *= temp_arry[i, num];
                        temp_tile.addValue();
                        combineTile(temp_tile, i, num);
                        num -= 1;
                    } else {
                        temp_arry[i, --num] = temp_tile.getValue();
                        moveTile(temp_tile, i, num);
                    }

                }
            }
        } else if(direction == "up") {
            Queue<TileClass> queue = new Queue<TileClass>();
            int[,] temp_arry = getTileArray();

            for(int i = 0; i < col; i++) {
                for(int p = 0; p < col; p++) {
                    if(temp_arry[p, i] != 0) {
                        queue.Enqueue(findTileClass(p * this.col + i));
                        temp_arry[p, i] = 0;
                    }
                }

                int num = 0;

                while(queue.Count > 0) {
                    TileClass temp_tile = queue.Dequeue();
                    if(temp_arry[num, i] == 0) {
                        temp_arry[num, i] = temp_tile.getValue();
                        moveTile(temp_tile, num, i);
                    } else if(temp_arry[num, i] == temp_tile.getValue()) {
                        temp_arry[num, i] *= temp_arry[num, i];
                        temp_tile.addValue();
                        combineTile(temp_tile, num, i);
                        num += 1;
                    } else {
                        temp_arry[++num, i] = temp_tile.getValue();
                        moveTile(temp_tile, num, i);
                    }

                }
            }
        } else if(direction == "down") {
            Queue<TileClass> queue = new Queue<TileClass>();
            int[,] temp_arry = getTileArray();

            for(int i = 0; i < col; i++) {
                for(int p = col - 1; p >= 0; p--) {
                    if(temp_arry[p, i] != 0) {
                        queue.Enqueue(findTileClass(p * this.col + i));
                        temp_arry[p, i] = 0;
                    }
                }

                int num = col - 1;

                while(queue.Count > 0) {
                    TileClass temp_tile = queue.Dequeue();
                    if(temp_arry[num, i] == 0) {
                        temp_arry[num, i] = temp_tile.getValue();
                        moveTile(temp_tile, num, i);
                    } else if(temp_arry[num, i] == temp_tile.getValue()) {
                        temp_arry[num, i] *= temp_arry[num, i];
                        temp_tile.addValue();
                        combineTile(temp_tile, num, i);
                        num -= 1;
                    } else {
                        temp_arry[--num, i] = temp_tile.getValue();
                        moveTile(temp_tile, num, i);
                    }

                }
            }
        }
    }

    void checkGameEnd() {
        bool result = true;

        for(int i = 0; i < tileObjectList.Count; i++) {
            int temp_position = tileObjectList[i].getPosition();

            if(temp_position + 1 < col * col) {
                if(temp_position / col == (temp_position + 1) / col) { // 오른쪽 검사
                    TileClass target_tile_class = findTileClass(temp_position + 1);
                    if(target_tile_class != null) {
                        if(tileObjectList[i].getValue() == target_tile_class.getValue()) {
                            result = false; 
                        }
                    } else {// 빈공간
                        result = false;
                    }
                }
            }

            if(temp_position - 1 >= 0) {
                if(temp_position / col == (temp_position - 1) / col) { // 왼쪽 검사
                    TileClass target_tile_class = findTileClass(temp_position - 1);
                    if(target_tile_class != null) {
                        if(tileObjectList[i].getValue() == target_tile_class.getValue()) {
                            result = false;
                        }
                    } else {
                        result = false;
                    }
                }
            }

            if(temp_position + col < col * col) { // 아래쪽 검사
                TileClass target_tile_class = findTileClass(temp_position + col);
                if(target_tile_class != null) {
                    if(tileObjectList[i].getValue() == target_tile_class.getValue()) {
                        result = false;
                    }
                } else {
                    result = false;
                }
            }

            if(temp_position - col > 0) { // 위쪽 검사
                TileClass target_tile_class = findTileClass(temp_position - col);
                if(target_tile_class != null) {
                    if(tileObjectList[i].getValue() == target_tile_class.getValue()) {
                        result = false;
                    }
                } else {
                    result = false;
                }
            }

            if(tileObjectList[i].getValue() == _clearTileNumbers[_clearTIleNumber]) {
                is_game_end = true;
                game_clear_object.SetActive(true);
                Invoke("GameEndAfter", 2.5f);
                ClearReward();
                return;
            }
        }

        is_game_end = result;

        if(is_game_end) {
            game_end_object.SetActive(true);
            Invoke("GameEndAfter", 1.5f);
        }
    }

    private void GameStart()
    {
        StartCoroutine(_fade.GetComponent<ScreenManager>().FadeImage(1));
    }

    private void GameEndAfter()
    {
        _fade.SetActive(true);
        StartCoroutine(_fade.GetComponent<ScreenManager>().FadeImage(LoadSceneLastInterview, - 1));
    }

    private void ClearReward()
    {
        if (_statusManager == null) {
            Debug.Log("StatusManager null");
            return;
        }

        _dialogBox.SetActive(true);
        _statusManager.AddValueStatus("코딩력", 20);
        _dialogBoxController.StartPrint("코딩력을 20 획득하였습니다.");
    }

    private void LoadSceneLastInterview()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LastInterview");
    }

    void moveTile(TileClass tile_class, int col, int row) {
        int position = col * this.col + row;
        if(tile_class.getPosition() != position) {
            tile_class.setPosition(position);
            //tile_class.getTile().transform.position = getTilePosition(position);
            moveViewTile(tile_class, position);
            is_creatable_init_tile = true; // 초기 타일 생성 가능
        }
    }

    void moveViewTile(TileClass tile_class, int position) {
        tile_class.getTile().SendMessage("setTargetPosition", getTilePosition(position));
    }

    void combineTile(TileClass tile_class, int col, int row) {
        int position = col * this.col + row;
        createTile(tile_class.getValue(), position);

        deleteTile(tile_class.getPosition());
        deleteTile(position);

        moveViewTile(tile_class, position);

        is_creatable_init_tile = true; // 초기 타일 생성 가능
    }

    void deleteTile(int target) {
        TileClass temp = findTileClass(target);
        temp.destroyTile();
        tileObjectList.Remove(temp);
    }

    TileClass findTileClass(int target_number) {
        for(int i = 0; i < tileObjectList.Count; i++) {
            if(tileObjectList[i].getPosition() == target_number) {
                return tileObjectList[i];
            }
        }
        return null;
    }

    Vector3 findTargetTilePositon(int target_number) {
        return findTileClass(target_number).getTile().transform.position;
    }

    int[,] getTileArray() {
        int[, ] result = new int[col , col];

        for(int i = 0; i < col; i++) {
            for(int p = 0; p < col; p++) {
                result[i, p] = 0;
            }
        }
        
        for(int i = 0; i < tileObjectList.Count; i++) {
            int temp = tileObjectList[i].getPosition();
            result[temp / col, temp % col] = tileObjectList[i].getValue();
        }

        return result;
    }

    void refreshScore() {
        score_text.GetComponent<Text>().text = "" + game_score;
    }

    void addScore(int value) {
        game_score += value;
    }

    class TileClass {
        GameObject tile;
        int position;
        int value;

        public TileClass(GameObject value, int num) {
            this.tile = value;
            this.position = num;
        }

        public TileClass(GameObject go, int num, int value) {
            this.tile = go;
            this.position = num;
            this.value = value;
        }

        public int getValue() {
            return this.value;
        }

        public void addValue() {
            this.value += this.value;
        }

        public int getPosition() {
            return position;
        }

        public void setPosition(int value) {
            this.position = value;
        }

        public GameObject getTile() {
            return tile;
        }

        void setTilePosition(Vector3 value) {
            this.tile.transform.position = value;
        }

        public void destroyTile() {
            //Destroy(this.tile);
            this.tile.SendMessage("setDestroy");
        }
    }
}

