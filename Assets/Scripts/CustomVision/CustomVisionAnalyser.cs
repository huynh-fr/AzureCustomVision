using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class CustomVisionAnalyser : MonoBehaviour {

    /// <summary>
    /// Unique instance of this class
    /// </summary>
    public static CustomVisionAnalyser Instance;

    /// <summary>
    /// Insert your Prediction Key here
    /// </summary>
    private string predictionKey = "50e64ae28c444e059a3747013d4b65ea";

    /// <summary>
    /// Insert your prediction endpoint here
    /// </summary>
    private string predictionEndpoint;

    /// <summary>
    /// Byte array of the image to submit for analysis
    /// </summary>
    [HideInInspector] public byte[] imageBytes;

    /// <summary>
    /// Initialises this class
    /// </summary>
    private void Awake()
    {
        // Allows this instance to behave like a singleton
        Instance = this;

        predictionEndpoint = "https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/1f2ce23d-d05f-46b6-9592-5441df2c3991/image";
    }

    /// <summary>
    /// Call the Computer Vision Service to submit the image.
    /// </summary>
    public IEnumerator AnalyseLastImageCaptured(string imagePath)
    {
        // Update camera status to analysis.
        SceneOrganiser.Instance.SetCameraStatus("Analysis");

        //Makes call to the API to analyse the picture and find a tag
        //When it is done, SceneOrganiser.Instance.FinaliseLabel is called
        WWWForm webForm = new WWWForm();
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(predictionEndpoint, webForm))
        {            
            // Gets a byte array out of the saved image
            imageBytes = GetImageAsByteArray(imagePath);

            unityWebRequest.SetRequestHeader("Content-Type", "application/octet-stream");
            unityWebRequest.SetRequestHeader("Prediction-Key", predictionKey);

            // The upload handler will help uploading the byte array with the request
            unityWebRequest.uploadHandler = new UploadHandlerRaw(imageBytes);
            unityWebRequest.uploadHandler.contentType = "application/octet-stream";

            // The download handler will help receiving the analysis from Azure
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();

            // Send the request
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.error != "")
                DialogManager.Instance.LaunchBasicDialog(1, "Internet Error", "Please verify your internet connection");

            string jsonResponse = unityWebRequest.downloadHandler.text;

            Debug.Log("response: " + jsonResponse);

            // Create a texture. Texture size does not matter, since
            // LoadImage will replace with the incoming image size.
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(imageBytes);
            SceneOrganiser.Instance.quadRenderer.material.SetTexture("_MainTex", tex);

            // The response will be in JSON format, therefore it needs to be deserialized  
            AnalysisObject analysisObject = new AnalysisObject();
            analysisObject = JsonConvert.DeserializeObject<AnalysisObject>(jsonResponse);

            SceneOrganiser.Instance.FinaliseLabel(analysisObject);
        }
    }

    /// <summary>
    /// Returns the contents of the specified image file as a byte array.
    /// </summary>
    static byte[] GetImageAsByteArray(string imageFilePath)
    {
        FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);

        BinaryReader binaryReader = new BinaryReader(fileStream);

        return binaryReader.ReadBytes((int)fileStream.Length);
    }
}
