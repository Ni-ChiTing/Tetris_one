using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class gameovertwo : MonoBehaviour {
    public Text score;
    public Text whowin;
    public static int win=0;
    // Use this for initialization
    void Start () {
        if (win==0)
        {
            whowin.text = "1p is Wins";
            score.text = Gameone.getscore().ToString();
        }  
        else
        {
            whowin.text = "2p is Wins";
            score.text = Gametwo.getscore().ToString();
        }
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
