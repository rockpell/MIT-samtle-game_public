using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelf : MonoBehaviour {

    private Vector3 completeScale = new Vector3(1, 1, 1);
    private Vector3 targetPosition;
    private Vector3 startPosition;

    private bool is_move = false;
    private bool destroy_switch = false;

    private float moveFloat = 0;
    private float speed = 6.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(is_move && this.transform.position == targetPosition) {
            is_move = false;
        }

        if(!is_move && destroy_switch) {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate() {
        if(!checkScale()) {
            Vector3 temp = this.transform.localScale;

            this.transform.localScale = new Vector3(temp.x + 0.02f, temp.y + 0.02f, 1);
        }

        if(is_move) {
            moveFloat += Time.deltaTime * speed;
            this.transform.position = Vector3.Lerp(startPosition, targetPosition, moveFloat);
        }
    }

    bool checkScale() {
        if(this.transform.localScale.x >= completeScale.x){
            return true;
        }
        return false;
    }

    void setTargetPosition(Vector3 target) {
        targetPosition = target;
        startPosition = this.transform.position;
        is_move = true;
    }

    void setDestroy() {
        destroy_switch = true;
    }
}
