using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialog : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DialogManager.Instance.LaunchBasicDialog(1, "Test", "This is a test");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
