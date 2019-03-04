using HoloToolkit.Unity.Buttons;
using HoloToolkit.UX.Dialog;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour {

    public static DialogManager Instance;

    [SerializeField]
    private Dialog dialogPrefab = null;

    private bool isDialogLaunched;

    [SerializeField]
    private GameObject resultText;
    /// <summary>
    /// Used to report the dialogResult. OK, Cancel etc.
    /// The button that was clicked to respond to the Dialog.
    /// Usefull if we want to display result directly in the scene
    /// </summary>
    public GameObject ResultText
    {
        get
        {
            return resultText;
        }

        set
        {
            resultText = value;
        }
    }

    [SerializeField]
    [Range(0, 2)]
    private int numButtons = 1;

    /// <summary>
    /// List of functions that will be called when a dialog button is clicked
    /// </summary>
    private Dictionary<string, Action> callbacks = new Dictionary<string, Action>();

    private TextMesh resultTextMesh;

    // Dialog button that will be clicked by user
    private Button button;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Action registeredAction;

        if (callbacks.TryGetValue(resultTextMesh.text, out registeredAction))
        {
            //A key equal to the clicked button text has been recognized,
            //the corresponding action will be called
            registeredAction.Invoke();
            resultTextMesh.text = "";
        }
    }

    /// <summary>
    /// This function is called to set the settings for the dialog and then open it.
    /// </summary>
    /// <param name="buttons">Enum describing the number of buttons that will be created on the Dialog</param>
    /// <param name="title">This string will appear at the top of the Dialog</param>
    /// <param name="message">This string will appear in the body of the Dialog</param>
    /// <returns>IEnumerator used for Coroutine funtions in Unity</returns>
    public IEnumerator LaunchDialog(DialogButtonType buttons, string title, string message)
    {
        isDialogLaunched = true;

        //Open Dialog by sending in prefab
        Dialog dialog = Dialog.Open(dialogPrefab.gameObject, buttons, title, message);

        if (dialog != null)
        {
            //listen for OnClosed Event
            dialog.OnClosed += OnClosed;
        }

        // Wait for dialog to close
        while (dialog.State < DialogState.InputReceived)
        {
            yield return null;
        }

        //only let one dialog be created at a time
        isDialogLaunched = false;

        yield break;
    }

    private void OnEnable()
    {
        resultTextMesh = ResultText.GetComponent<TextMesh>();
        button = GetComponent<Button>();
        if (button != null)
        {
            button.OnButtonClicked += OnButtonClicked;
        }
    }

    /// <summary>
    /// Function created to call LaunchDialog() more easily.
    /// Uses only OK and Yes/No dialogs.
    /// </summary>
    public void LaunchBasicDialog(int nbButtons, string title, string message)
    {
        if (nbButtons != 0)
            numButtons = nbButtons;

        if (isDialogLaunched == false)
        {
            if (numButtons == 1)
            {
                // Launch Dialog with single button
                StartCoroutine(LaunchDialog(DialogButtonType.OK, title, message));
            }
            else if (numButtons == 2)
            {
                // Launch Dialog with two buttons
                StartCoroutine(LaunchDialog(DialogButtonType.Yes | DialogButtonType.No, title, message));
            }
        }
    }

    /// <summary>
    /// If the clicked button has the sme name as dialogBtnName,
    /// the action method is called
    /// For example : "Yes" button and DoSomething method
    /// => StartCoroutine(RegisterActionForDialogButton("Yes", DoSomething));
    /// 
    /// Important : method is of type void method() = no return value, no parameters
    /// Other functions has te be created if you want to use parameters or return value
    /// => Action<int,string> = int and string params, Func<string,int> = string param and int return value
    /// </summary>
    public void RegisterActionForDialogButton(string dialogBtnName, Action method)
    {
        if (callbacks.ContainsKey(dialogBtnName))
        {
            //If already present, remove the key and the associated action 
            callbacks.Remove(dialogBtnName);
        }
        callbacks.Add(dialogBtnName, method);
    }

    private void OnButtonClicked(GameObject obj)
    {
        //Do nothing
    }

    /// <summary>
    /// Event Handler that fires when Dialog is closed- when a button on the Dialog is clicked.
    /// </summary>
    /// <param name="result">Returns a description of the result, which button was clicked</param>
    protected void OnClosed(DialogResult result)
    {
        // Get the result text from the Dialog
        resultTextMesh.text = result.Result.ToString();
        //LaunchBasicDialog(1, "Debug", resultTextMesh.text);
    }
}

