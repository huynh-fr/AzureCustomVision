using System;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.XR.WSA.WebCam;
using TMPro;

public class ImageCapture : MonoBehaviour {

    /// <summary>
    /// Allows this class to behave like a singleton
    /// </summary>
    public static ImageCapture Instance;

    /// <summary>
    /// Allows this class to take a picture or not
    /// Used for not taking pictures while moving a Hologram
    /// </summary>
    public bool CanTakePicture { get; set; }

    /// <summary>
    /// Keep counts of the taps for image renaming
    /// </summary>
    private int captureCount = 0;

    /// <summary>
    /// Photo Capture object
    /// </summary>
    private PhotoCapture photoCaptureObject = null;

    /// <summary>
    /// Loop timer
    /// </summary>
    //private float secondsBetweenCaptures = 10f;

    /// <summary>
    /// Application main functionalities switch
    /// - Analysis : takes picture and analyse it, find a tag
    /// - Training : takes a picture and upload it with a tag
    /// - Smart : takes a picture, analyse it and find a tag, then upload it with found tag
    /// </summary>
    public enum AppModes { Analysis, Training, Smart }

    /// <summary>
    /// Local variable for current AppMode
    /// </summary>
    public AppModes AppMode { get; private set; }

    /// <summary>
    /// Flagging if the capture loop is running
    /// </summary>
    internal bool captureIsActive;

    /// <summary>
    /// File path of current analysed photo
    /// </summary>
    internal string filePath = string.Empty;

    /// <summary>
    /// Called on initialization
    /// </summary>
    private void Awake()
    {
        Instance = this;

        // Change this flag to switch between Analysis, Training or Smart Mode 
        AppMode = AppModes.Training;
    }

    /// <summary>
    /// Runs at initialization right after Awake method
    /// </summary>
    void Start()
    {
        // Clean up the LocalState folder of this application from all photos stored
        DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath);
        var fileInfo = info.GetFiles();
        foreach (var file in fileInfo)
        {
            try
            {
                file.Delete();
            }
            catch (Exception)
            {
                Debug.LogFormat("Cannot delete file: ", file.Name);
            }
        }
        CanTakePicture = true;
        SceneOrganiser.Instance.SetCameraStatus("Ready");
    }

    /// <summary>
    /// Begin process of Image Capturing and send To Azure Custom Vision Service.
    /// </summary>
    public void ExecuteImageCapture()
    {
        if (CanTakePicture == true)
        {
            // Create a label in world space using the SceneOrganiser class 
            // Invisible at this point but correctly positioned where the image was taken
            SceneOrganiser.Instance.PlaceAnalysisLabel();

            // Set the camera resolution to be the highest possible
            Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

            Texture2D targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

            // Begin capture process, set the image format
            PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject)
            {
                photoCaptureObject = captureObject;

                CameraParameters camParameters = new CameraParameters
                {
                    hologramOpacity = 0.0f,
                    cameraResolutionWidth = targetTexture.width,
                    cameraResolutionHeight = targetTexture.height,
                    pixelFormat = CapturePixelFormat.BGRA32
                };

                // Capture the image from the camera and save it in the App internal folder
                captureObject.StartPhotoModeAsync(camParameters, delegate (PhotoCapture.PhotoCaptureResult result)
                {
                    string filename = string.Format(@"CapturedImage{0}.jpg", captureCount);
                    filePath = Path.Combine(Application.persistentDataPath, filename);
                    captureCount++;
                    photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
                });
            });
        }
    }

    /// <summary>
    /// Register the full execution of the Photo Capture. 
    /// </summary>
    void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        // Call StopPhotoMode once the image has successfully captured
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }


    /// <summary>
    /// The camera photo mode has stopped after the capture.
    /// Begin the Image Analysis process.
    /// </summary>
    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        //Play sound since the object has been recognized
        //AudioPlay.Instance.Play();

        Debug.LogFormat("Stopped Photo Mode");

        // Dispose from the object in memory and request the image analysis 
        photoCaptureObject.Dispose();
        photoCaptureObject = null;

        switch (AppMode)
        {
            case AppModes.Analysis:
            case AppModes.Smart:
                // Call the image analysis
                StartCoroutine(CustomVisionAnalyser.Instance.AnalyseLastImageCaptured(filePath));
                break;

            case AppModes.Training:
                // Call training using captured image
                UploadPhoto(null);
                break;
        }
    }

    /// <summary>
    /// Begin the Image Upload process.
    /// </summary>
    public void UploadPhoto(string tag)
    {
        // Call training using captured image
        if (tag == null)
        {
            // Update camera status.
            SceneOrganiser.Instance.SetCameraStatus("Choosing");

            CustomVisionTrainer.Instance.AddNewTagRequest();
            //CustomVisionTrainer.Instance.EnableTextDisplay(true);
        }    
        else
        {
            // Update camera status.
            SceneOrganiser.Instance.SetCameraStatus("Uploading Image");

            //Upload photo with recognized tag
            CustomVisionTrainer.Instance.EnableTextDisplay(true);
            CustomVisionTrainer.Instance.VerifyTag(tag);
        } 
    }

    /// <summary>
    /// Stops all capture pending actions
    /// </summary>
    internal void ResetImageCapture()
    {
        captureIsActive = false;

        //Disable the Training Text
        //CustomVisionTrainer.Instance.EnableTextDisplay(false);

        // Update camera status to ready.
        SceneOrganiser.Instance.SetCameraStatus("Ready");

        // Stop the capture loop if active
        CancelInvoke();
    }
}
