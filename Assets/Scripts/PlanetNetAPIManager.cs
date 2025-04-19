using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PlanetNetAPIManager : MonoBehaviour
{
    [SerializeField] private string project = "all";
    [SerializeField] private string includeRelatedImages = "false";
    [SerializeField] private string noReject = "false";
    [SerializeField] private int nbResults = 10;
    [SerializeField] private string lang = "en";
    [SerializeField] private string type = "kt";
    [SerializeField] private string apiKey = "2b10YHauwHrZMTuGC0ehcsEuqe";
    [SerializeField] private string baseUrl = "https://my-api.plantnet.org";
    [SerializeField] private string endPoint = "v2/identify";
    
    [SerializeField] private CameraManager cameraManager;

    [ContextMenu("UploadPlantImages")]
    public void UploadPlantImages()
    {
        StartCoroutine(PostPlantIdentification(GetUrl()));
    }

    private string GetUrl()
    {
        //https://my-api.plantnet.org/v2/identify/all?include-related-images=false&no-reject=false&nb-results=10&lang=en&type=kt&api-key=2b10YHauwHrZMTuGC0ehcsEuqe

        return $"{baseUrl}/{endPoint}/{project}?" +
               $"include-related-images={includeRelatedImages}" +
               $"&no-reject={noReject}" +
               $"&nb-results={nbResults}" +
               $"&lang={lang}" +
               $"&type={type}" +
               $"&api-key={apiKey}";

    }

    private IEnumerator PostPlantIdentification(string url)
    {
        WWWForm form = new WWWForm();
        
        form.AddBinaryData("images", File.ReadAllBytes(cameraManager.imagePath), "image.jpg");
        
        Debug.Log($"URL: {url}");
        string boundary = "------------------------" + System.DateTime.Now.Ticks.ToString("x");
        byte[] body = BuildMultipartFormData(cameraManager.capturedTexture, boundary);
        
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "multipart/form-data; boundary=" + boundary);
        
        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Upload success: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError($"Upload failed: {request.error}, Code: {request.responseCode}");
        }
    }
    
    private byte[] BuildMultipartFormData(Texture2D texture, string boundary)
    {
        List<byte> body = new List<byte>();
        string newLine = "\r\n";

        // IMAGE PART
        string headerImage = $"--{boundary}{newLine}" +
                             $"Content-Disposition: form-data; name=\"images\"; filename=\"image.jpg\"{newLine}" +
                             $"Content-Type: image/jpeg{newLine}{newLine}";

        byte[] imageBytes = texture.EncodeToJPG();
        body.AddRange(Encoding.UTF8.GetBytes(headerImage));
        body.AddRange(imageBytes);
        body.AddRange(Encoding.UTF8.GetBytes(newLine));

        // FINAL BOUNDARY
        string endBoundary = $"--{boundary}--{newLine}";
        body.AddRange(Encoding.UTF8.GetBytes(endBoundary));

        return body.ToArray();
    }
}
