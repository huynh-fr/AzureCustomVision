using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using HoloToolkit.Unity.SpatialMapping;
using System;


public class SceneOrganiser : MonoBehaviour {

    /// <summary>
    /// Allows this class to behave like a singleton
    /// </summary>
    public static SceneOrganiser Instance;

    /// <summary>
    /// Reference to the label currently used to display the analysis on the objects in the real world
    /// Equal to null if analysis completed
    /// </summary>
    internal GameObject currentLabel = null;

    /// <summary>
    /// Object providing the current status of the camera.
    /// </summary>
    [SerializeField]
    internal TextMeshPro cameraStatusIndicator;

    [SerializeField]
    internal SpatialMappingManager SpatialMapping;

    /// <summary>
    /// Name of the recognized object /!\ Only in AppModes.Smart mode
    /// </summary>
    internal string RecognizedObject { get; set; }

    internal int recognizedObjects;

    /// <summary>
    /// Reference to the last label positioned
    /// </summary>
    public TextMeshPro LastLabelPlaced { get; set; }

    /// <summary>
    /// Current threshold accepted for displaying the label
    /// Reduce this value to display the recognition more often
    /// </summary>
    internal float probabilityThreshold = 0.5f;

    /// <summary>
    /// The quad object hosting the imposed image captured
    /// </summary>
    private GameObject quad;

    /// <summary>
    /// Renderer of the quad object
    /// </summary>
    internal Renderer quadRenderer;


    /// <summary>
    /// Called on initialization
    /// </summary>
    private void Awake()
    {
        // Use this class instance as singleton
        Instance = this;

        // Create the camera status indicator label, and place it above where Predictions
        // and training UI will appear.

        // Set camera status indicator to loading.
        SetCameraStatus("Loading");
    }

    void Start()
    {
        
    }

    void Update()
    {
        cameraStatusIndicator.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }

    /// <summary>
    /// Set the camera status to a provided string. Will be coloured if it matches a keyword.
    /// </summary>
    /// <param name="statusText">Input string</param>
    public void SetCameraStatus(string statusText)
    {
        if (string.IsNullOrEmpty(statusText) == false)
        {
            string textColor = "white";

            switch (statusText.ToLower())
            {
                case "loading":
                    textColor = "yellow";
                    break;

                case "ready":
                    textColor = "green";
                    break;

                case "uploading image":
                    textColor = "red";
                    break;

                case "looping capture":
                    textColor = "yellow";
                    break;

                case "analysis":
                    textColor = "red";
                    break;
                case "choosing":
                    textColor = "blue";
                    break;
            }

            cameraStatusIndicator.GetComponent<TextMeshPro>().text = $"Camera Status:\n<color={textColor}>{statusText}..</color>";

            if(statusText.ToLower() == "ready")
            {
                // Set the cursor to Normal state (Idle)
                CursorManager.Instance.LoadingStop();
            }
            else
            {
                //Change cursor animation to Loading status
                CursorManager.Instance.LoadingStart();
            }
        }
    }

    public void SetLastLabel(string text)
    {
        if(text == "Unknown")
        {
            GameObject obj = DrawInSpace.Instance.DrawCube(0.05f, 0.05f, 0.05f, LastLabelPlaced.transform.position);
            LastLabelPlaced.transform.Translate(Vector3.up * 0.1f, Space.World);
            DrawInSpace.Instance.ChooseMaterial(obj, "AppBarBackgroundBar");
            BoundingBoxManager.Instance.MakeBoundingBoxInteractible(obj);
            LastLabelPlaced.transform.SetParent(obj.transform);
            obj.layer = LayerMask.NameToLayer("Holograms");

            ImageCapture.Instance.ResetImageCapture();
        }
        
        LastLabelPlaced.text = text;
    }

    /// <summary>
    /// Instantiate a label in the appropriate location relative to the Main Camera.
    /// </summary>
    public void PlaceAnalysisLabel()
    {
        GameObject panel = new GameObject("Label");
        TextMeshPro label = panel.AddComponent<TextMeshPro>();
        label.fontSizeMin = 1;
        label.fontSizeMax = 70;
        label.enableAutoSizing = true;
        label.rectTransform.sizeDelta = new Vector2(2, 1);
        label.transform.localScale = new Vector3(0.1f, 0.1f, 0.01f);
        label.alignment = TextAlignmentOptions.Midline;

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            // If the raycast hit a hologram, use that as the focused object.
            label.transform.position = hitInfo.point;
            label.transform.Translate(Vector3.back * 0.1f, Space.World);
            label.transform.Translate(Vector3.up * 0.1f, Space.World);
            label.transform.rotation = Quaternion.LookRotation(gazeDirection);

            LastLabelPlaced = label;
        }

        // Create a GameObject to which the texture can be applied
        quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quadRenderer = quad.GetComponent<Renderer>() as Renderer;
        Material m = new Material(Shader.Find("Legacy Shaders/Transparent/Diffuse")); 
        quadRenderer.material = m;

        // Here you can set the transparency of the quad. Useful for debugging
        // Allows you to see the picture taken in the real world
        float transparency = 0.0f;
        quadRenderer.material.color = new Color(1, 1, 1, transparency);

        //Set the position and scale of the quad depending on user position
        quad.transform.SetParent(transform);

        //Set the quad (screen for picture) at the cursor position and make it face the user
        quad.transform.position = CursorManager.Instance.GetCursorPositionOnMesh();
        quad.transform.rotation = Quaternion.LookRotation(gazeDirection);

        // The quad scale has been set with the following value following experimentation,  
        // to allow the image on the quad to be as precisely imposed to the real world as possible
        quad.transform.localScale = new Vector3(hitInfo.distance, 1.65f / 3f * hitInfo.distance, 1f); 

        quad.transform.parent = null;        
    }

    /// <summary>
    /// Set the Tags as Text of the last label created. 
    /// </summary>
    public void FinaliseLabel(AnalysisObject analysisObject)
    {
        if (analysisObject.Predictions != null)
        {
            // Sort the Predictions to locate the highest one
            List<Prediction> sortedPredictions = new List<Prediction>();
            sortedPredictions = analysisObject.Predictions.OrderBy(p => p.Probability).ToList();
            Prediction bestPrediction = new Prediction();
            bestPrediction = sortedPredictions[sortedPredictions.Count - 1];

            if (bestPrediction.Probability > probabilityThreshold)
            {
                // The prediction is considered good enough
                quadRenderer = quad.GetComponent<Renderer>() as Renderer;
                Bounds quadBounds = quadRenderer.bounds;

                //Draw a cube acting like a visible boundingBox for the recognized object
                GameObject objBoundingBox = DrawInSpace.Instance.DrawCube((float)bestPrediction.BoundingBox.Width, (float)bestPrediction.BoundingBox.Height);
                objBoundingBox.transform.parent = quad.transform;
                objBoundingBox.transform.localPosition = CalculateBoundingBoxPosition(quadBounds, bestPrediction.BoundingBox);
                DrawInSpace.Instance.ChooseMaterial(objBoundingBox, "BoundingBoxTransparentFlashy"); //optional

                //Set the position and scale of the quad depending on user position
                objBoundingBox.transform.SetParent(transform); //break the link with quad (picture)
                BoundingBoxManager.Instance.MakeBoundingBoxInteractible(objBoundingBox);
                objBoundingBox.AddComponent<EventTriggerDelegate>(); //to block picture functions while moving bounding box

                // Move the label upward in world space just above the boundingBox.
                LastLabelPlaced.transform.position = objBoundingBox.transform.position;
                LastLabelPlaced.transform.Translate(Vector3.up * (float)(bestPrediction.BoundingBox.Height / 2 + 0.1f), Space.World); //Vector3 = World Space and Transform = Local Space
                LastLabelPlaced.transform.parent = objBoundingBox.transform; //Link the Text label to the object bounding box

                // Set the label text and make it face the user
                SetLastLabel(bestPrediction.TagName);
                LastLabelPlaced.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

                RecognizedObject = bestPrediction.TagName;
            }
            else
                RecognizedObject = null;
        }
        else
            RecognizedObject = null;

        if(ImageCapture.Instance.AppMode == ImageCapture.AppModes.Analysis)
        {
            if(RecognizedObject == null)
            {
                SetLastLabel("Unknown");
            }
        }
        else if (ImageCapture.Instance.AppMode == ImageCapture.AppModes.Smart)
        {
            ImageCapture.Instance.UploadPhoto(RecognizedObject);
        }
        

        //Make a sound to notify the user of the new label
        AudioPlay.Instance.PlayWithVolume("Bell", 80);

        ImageCapture.Instance.ResetImageCapture();
    }

    /// <summary>
    /// This method hosts a series of calculations to determine the position 
    /// of the Bounding Box on the quad created in the real world
    /// by using the Bounding Box received back alongside the Best Prediction
    /// </summary>
    public Vector3 CalculateBoundingBoxPosition(Bounds b, BoundingBox2D boundingBox)
    {
        Debug.Log($"BB: left {boundingBox.Left}, top {boundingBox.Top}, width {boundingBox.Width}, height {boundingBox.Height}");

        double centerFromLeft = boundingBox.Left + (boundingBox.Width / 2);
        double centerFromTop = boundingBox.Top + (boundingBox.Height / 2);
        Debug.Log($"BB CenterFromLeft {centerFromLeft}, CenterFromTop {centerFromTop}");

        double quadWidth = b.size.normalized.x;
        double quadHeight = b.size.normalized.y;
        Debug.Log($"Quad Width {b.size.normalized.x}, Quad Height {b.size.normalized.y}");

        double normalisedPos_X = (quadWidth * centerFromLeft) - (quadWidth / 2);
        double normalisedPos_Y = (quadHeight * centerFromTop) - (quadHeight / 2);

        return new Vector3((float)normalisedPos_X, (float)normalisedPos_Y, 0);
    }
}
