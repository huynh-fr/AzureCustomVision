using UnityEngine;
using UnityEngine.EventSystems;

using HoloToolkit.Unity.SpatialMapping;

/// <summary>
/// Attach this script to the GameObject you would like to detect dragging on.
/// Make sure the Camera you are using has a Physics Raycaster 
/// (Click the Add Component button and go to Event>Physics Raycaster) so it can detect clicks on GameObjects.
/// 
/// IMPORTANT : this class isn't a general purpose class.
/// If you want to have holograms with different actions called when the same event happens,
/// you can use RegisterActionForDialogButton() of the DialogManager class as an inspiration
/// and modify this class.
/// Each hologram should have its own dictionary of delegate callbacks (Action).
/// </summary>
public class EventTriggerDelegate : MonoBehaviour {

    // Use this for initialization
    void Start() {
       
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry;

        //Pointer Down Event
        //entry = new EventTrigger.Entry();
        //entry.eventID = EventTriggerType.PointerDown;
        //entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
        //trigger.triggers.Add(entry);

        //Begin Drag Event
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.BeginDrag;
        entry.callback.AddListener((data) => { OnBeginDragDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);

        //End Drag Event
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.EndDrag;
        entry.callback.AddListener((data) => { OnEndDragDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Called when the gameObject is clicked on
    /// </summary>
    public void OnPointerDownDelegate(PointerEventData data)
    {
        Debug.Log("OnPointerDownDelegate called.");
    }

    /// <summary>
    /// Called when the gameObject is dragged over (Beginning of the event)
    /// </summary>
    public void OnBeginDragDelegate(PointerEventData data)
    {
        Debug.Log("OnBeginDragDelegate called.");
        DialogManager.Instance.LaunchBasicDialog(1, "Debug", "OnBeginDragDelegate called");

        try { ImageCapture.Instance.CanTakePicture = false; }
        catch { Debug.Log("Add ImageCapture script"); }
    }

    /// <summary>
    /// Called when the gameObject is released (End of the event)
    /// </summary>
    public void OnEndDragDelegate(PointerEventData data)
    {
        Debug.Log("OnEndDragDelegate called.");
        DialogManager.Instance.LaunchBasicDialog(1, "Debug", "OnEndDragDelegate called");

        SpatialMappingManager.Instance.DrawVisualMeshes = false;

        try { ImageCapture.Instance.CanTakePicture = true; }
        catch { Debug.Log("Add ImageCapture script"); }
    }
}
