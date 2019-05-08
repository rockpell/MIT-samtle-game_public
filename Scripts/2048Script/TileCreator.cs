using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCreator : MonoBehaviour {
    public GameObject[] obj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject createTile(int value, Vector3 position) {
        int number = 0;
        switch(value) {
            case 2:
                number = 0;
                break;
            case 4:
                number = 1;
                break;
            case 8:
                number = 2;
                break;
            case 16:
                number = 3;
                break;
            case 32:
                number = 4;
                break;
            case 64:
                number = 5;
                break;
            case 128:
                number = 6;
                break;
            case 256:
                number = 7;
                break;
            case 512:
                number = 8;
                break;
            case 1024:
                number = 9;
                break;
            case 2048:
                number = 10;
                break;

        }
        return Instantiate(obj[number], position, Quaternion.identity);
    }
}