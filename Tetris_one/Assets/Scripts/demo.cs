using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    public void OnClick()
    {
        Debug.Log("Demo OnClick");
        DemoStart();
    }
    public void DemoStart()
    {
        Application.LoadLevel("demo");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
