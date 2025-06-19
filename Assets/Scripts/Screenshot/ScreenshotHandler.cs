using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;

public class ScreenshotHandler : MonoBehaviour
{
    private bool isProcessing = false;

    public void CaptureScreenshot()
    {
        if (!isProcessing)
        {
            StartCoroutine(CaptureScreenshotCoroutine());
        }
    }

    private IEnumerator CaptureScreenshotCoroutine()
    {
        isProcessing = true;

        // Wait for the end of the frame to capture the screenshot
        yield return new WaitForEndOfFrame();

        // Capture the screenshot into a texture
        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshotTexture.Apply();

        // Encode the texture into PNG format
        byte[] screenshotBytes = screenshotTexture.EncodeToPNG();
        Destroy(screenshotTexture);

        // Convert the screenshot to Base64 string
        string base64Screenshot = System.Convert.ToBase64String(screenshotBytes);

        // Call the JavaScript function to trigger download
        string fileName = "screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        TriggerScreenshotDownload(base64Screenshot, fileName);

        isProcessing = false;
    }

    private void TriggerScreenshotDownload(string base64Image, string fileName)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        WebGLScreenshotDownload(base64Image, fileName);
#else
        Debug.Log("Screenshot captured but automatic download is only available in WebGL builds.");
#endif
    }

    [DllImport("__Internal")]
    private static extern void WebGLScreenshotDownload(string base64Image, string fileName);
}
