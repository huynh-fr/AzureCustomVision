using System.Collections;
using UnityEngine;
using HoloToolkit.Unity.SpatialMapping;
using System;

/// <summary>
/// Class created not to modify to much the Input Manager
/// </summary>
public class AfterGestureRecognition : MonoBehaviour {

    public static AfterGestureRecognition Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        
	}

    /// <summary>
    /// Respond to Tap Input.
    /// </summary>
    public void ExtraTapped()
    {
        //Debug.Log($"Extra Tapped called");

        //Vector3 cursorPosition = CursorManager.Instance.GetCursorPositionOnMesh();
        //Debug.Log($"Cursor position = {cursorPosition}");
        //DrawInSpace.Instance.DrawStaticRectangle(CursorManager.Instance.GetCursorPositionOnMesh(), 0.1f, 0.1f);        
    }

    /// <summary>
    /// Respond to 2 fast Tap Inputs.
    /// Not working in unity but works with Hololens
    /// </summary>
    public void DoubleTapped()
    {
        Debug.Log($"Double Tapped called");
        //DialogManager.Instance.LaunchBasicDialog(1, "Debug", "Double Tapped called");

        //Activate or Deactivate the 3D World Meshmap of the Hololens
        SpatialMappingManager.Instance.DrawVisualMeshes = !SpatialMappingManager.Instance.DrawVisualMeshes;

        RaycastHit hit;
        Ray landingRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        int layer = LayerMask.NameToLayer("Holograms");
        int layerMask = (1 << layer);

        if (Physics.Raycast(landingRay, out hit, Mathf.Infinity, layerMask))
        {
            //Raycast hit a gameObject set on the "Holograms" layer
            if (hit.collider.tag == "BoundingBox")
            {
                //Hit gameObject is tagged "BoundgingBox"
                //gameObject is dragged to the cursor position like a magnet
                hit.collider.gameObject.transform.position = CursorManager.Instance.GetCursorPositionOnMesh();
            }
        }
    }

    /// <summary>
    /// Respond to the start of Hold Gesture, Hold Started Input.
    /// </summary>
    public void ExtraHoldStarted()
    {
        //Takes a picture when you hold your fingers together
        ImageCapture.Instance.ExecuteImageCapture();
    }

    public void ExtraHoldCompleted()
    {
        //cursorAnim.SetBool(waitingHash, false);
        
    }

    public void ExtraHoldCanceled()
    {
        
    }
}
