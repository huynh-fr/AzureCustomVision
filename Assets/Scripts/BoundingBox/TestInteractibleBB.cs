using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractibleBB : MonoBehaviour {

	// Use this for initialization
	void Start () {
        BoundingBoxManager.Instance.MakeBoundingBoxInteractible(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
