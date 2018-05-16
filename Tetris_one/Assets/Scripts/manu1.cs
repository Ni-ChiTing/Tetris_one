using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manu1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    public void OnClick()
    {
        Debug.Log("P1 OnClick");
        P1Start();
    }
    public void P1Start()
    {
        Application.LoadLevel("gameplay");
    }
    // Update is called once per frame
    void Update () {
		
	}
}
