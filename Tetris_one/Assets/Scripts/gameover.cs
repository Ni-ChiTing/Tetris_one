using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class gameover : MonoBehaviour {
    public Text score;
    // Use this for initialization
    void Start () {
            score.text = Gameone.getscore().ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
