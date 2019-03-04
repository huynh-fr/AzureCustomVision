using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* All the scripts make calls between each other
* If you want understand the code, 
* you can start with AfterGestureRecognition class / ExtraHoldStarted()
* and follow which functions are then called
* 
* The code that inspired this App can be found on 
* https://docs.microsoft.com/en-us/windows/mixed-reality/mr-azure-310
* 
* 
* When you import this project in Unity for the first time,
* you may need to set class attributes in the Unity Inspector.
* Check each gameObject and make sure that you have the least "None (Transform)" possible
* In each script that I wrote, I told in the comments if the attribute needs to be set in the Inspector
*/

public class Main : MonoBehaviour {

    private void Awake()
    {
        /*
         * Add those scripts (classes) to the gameObject
         * Classes whose attributes don't need to be set in the Unity Inspector
         */

        gameObject.AddComponent<CustomVisionAnalyser>();
        
        gameObject.AddComponent<CustomVisionTrainer>();

        gameObject.AddComponent<CustomVisionObjects>();

        gameObject.AddComponent<ImageCapture>();

        gameObject.AddComponent<SceneOrganiser>();

        gameObject.AddComponent<VoiceRecognizer>();

        gameObject.AddComponent<AfterGestureRecognition>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
