using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController_Interview : MonoBehaviour {

    bool is_moveable, is_moveable2;

    Vector2 startPosition;
    Vector2 endPosition;
    float move_value;
    // Use this for initialization
    void Start () {
        is_moveable = false;
        is_moveable2 = false;
        move_value = 0;
        startPosition = this.transform.position;
        endPosition = new Vector2(startPosition.x, startPosition.y + 0.3f);
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("move_value : " + move_value);
        if(is_moveable) {
            move_value += Time.deltaTime * 6f;
            this.transform.position = Vector2.Lerp(startPosition, endPosition, move_value);
            if(move_value > 1) {
                is_moveable2 = true;
                is_moveable = false;
            }
        }
        if(is_moveable2) {
            move_value -= Time.deltaTime * 6f;
            this.transform.position = Vector2.Lerp(startPosition, endPosition, move_value);
            if(move_value <= 0) {
                move_value = 0;
                is_moveable2 = false;
            }
        }
    }

    public void react() {
        is_moveable = true;
    }
}
