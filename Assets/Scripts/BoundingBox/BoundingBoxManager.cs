using UnityEngine;
using HoloToolkit.Unity.UX;
using HoloToolkit.Unity.InputModule.Utilities.Interactions;

public class BoundingBoxManager : MonoBehaviour {

    public static BoundingBoxManager Instance;

    [SerializeField]
    /// <summary>
    /// IMPORTANT : needs to be set in the Unity Inspector
    ///             find BoundingBoxBasic.prefab
    /// </summary>
    private BoundingBox BoundingBoxPrefab;

    [SerializeField]
    /// <summary>
    /// Appbar of the boundingbox 
    /// IMPORTANT : needs to be set in the Unity Inspector
    ///             find AppBar.prefab
    /// </summary>
    private AppBar AppBarPrefab;

    [SerializeField]
    /// <summary>
    /// Material for the interactible parts of the boundingbox 
    /// IMPORTANT : needs to be set in the Unity Inspector
    ///             find BoundingBoxHandle.mat
    /// </summary>
    private Material handle;

    [SerializeField]
    /// <summary>
    /// Material for the interactible parts of the boundingbox when hold
    /// IMPORTANT : needs to be set in the Unity Inspector
    ///             find BoundingBoxHandleGrabbed.mat
    /// </summary>
    private Material handleGrabbed;

    private BoundingBoxRig bbRig;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        //GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //MakeBoundingBoxInteractible(shape);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Makes any hologram into an interactible one
    /// You can now move, rotate or scale it with your hands
    /// </summary>
    public void MakeBoundingBoxInteractible(GameObject obj)
    {
        Rigidbody body = obj.AddComponent<Rigidbody>();
        body.mass = 100;
        body.useGravity = false;
        body.isKinematic = true;

        //Set all the boundingBox rig's settings
        bbRig = obj.AddComponent<BoundingBoxRig>();
        bbRig.ScaleHandleMaterial = handle;
        bbRig.RotateHandleMaterial = handle;
        bbRig.InteractingMaterial = handleGrabbed;
        bbRig.BoundingBoxPrefab = BoundingBoxPrefab;
        bbRig.AppBarPrefab = AppBarPrefab;

        TwoHandManipulatable twoHand = obj.AddComponent<TwoHandManipulatable>();
        twoHand.HostTransform = obj.transform;
        twoHand.BoundingBoxPrefab = BoundingBoxPrefab;

        obj.tag = "BoundingBox";
    }

    public void ActivateBoundingBox(bool state)
    {
        if (state == true)
            bbRig.Activate();
        else
            bbRig.Deactivate();
    }
}
