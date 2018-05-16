using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manu2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    public void OnClick()
    {
        Debug.Log("P2 OnClick");
        P2Start();
    }
    public void P2Start()
    {
        Application.LoadLevel("gameplay2");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
