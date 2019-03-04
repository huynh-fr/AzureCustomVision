using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using HoloToolkit.UI.Keyboard;

/// <summary>
/// has to be attached to PanelWithInputField gameObject
/// </summary>

public class InputFieldManager : MonoBehaviour {

    public static InputFieldManager Instance;

    private Text title, description, inputText;
    private KeyboardInputField kbInputField;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        //isActive = false;
        
        title = transform.Find("WindowCanvas/Background/Title").GetComponent<Text>();
        description = transform.Find("WindowCanvas/Background/Description").GetComponent<Text>();
        inputText = transform.Find("WindowCanvas/Background/MessageInputField/InputFieldText").GetComponent<Text>();
        kbInputField = GameObject.Find("WindowCanvas/Background/MessageInputField").GetComponent<KeyboardInputField>() ;

        if (title != null)
        {
            title.text = "Add a new tag";
        }

        if (description != null)
        {
            description.text = "Write the name of the new tag or close this window.";
        }

        GameObject panelWithInputField = transform.parent.gameObject;
        panelWithInputField.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void OpenInputFieldDialog()
    {
        //Open dialog
        GameObject panelWithInputField = transform.parent.gameObject;
        panelWithInputField.SetActive(true);

        //Empty input field
        kbInputField.Keyboard_OnTextUpdated(string.Empty);

        //Place the dialog in front of user
        Vector3 headPosition = Camera.main.transform.position;
        Vector3 gazeDirection = Camera.main.transform.forward;

        panelWithInputField.transform.position = headPosition;
        panelWithInputField.transform.Translate(new Vector3(gazeDirection.x, 0, gazeDirection.z)*2);
    }

    public void CloseInputFieldDialog()
    {
        //Close dialog
        GameObject panelWithInputField = transform.parent.gameObject;
        panelWithInputField.SetActive(false);
    }

    public void ValidateButtonClicked()
    {
        CustomVisionTrainer.Instance.AddNewTag(inputText.text);
        ImageCapture.Instance.UploadPhoto(inputText.text);

        CloseInputFieldDialog();
    }
}
