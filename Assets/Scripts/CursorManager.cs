using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloToolkit.Unity.InputModule;
using Cursor = HoloToolkit.Unity.InputModule.Cursor;

public class CursorManager : MonoBehaviour {

    public static CursorManager Instance;

    [SerializeField]
    private Cursor cursor;

    private Animator cursorAnim;
    private int waitingHash = Animator.StringToHash("Waiting");

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cursorAnim = cursor.GetComponentInChildren<Animator>();
    }

    public void LoadingStart()
    {
        cursorAnim.SetBool(waitingHash, true);
    }

    public void LoadingStop()
    {
        cursorAnim.SetBool(waitingHash, false);
    }

    public Vector3 GetCursorPositionOnMesh()
    {
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            // If the raycast hit a hologram, use that as the focused object.
            return hitInfo.point;
        }
        else
        {
            Debug.Log("No mesh hit\n");
            DialogManager.Instance.LaunchBasicDialog(1, "Debug", "No mesh hit by the cursor");
            ImageCapture.Instance.ResetImageCapture();

            return new Vector3(0,0,0);
        }
    }
}
