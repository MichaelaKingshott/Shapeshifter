using Unity.VisualScripting;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) { 
            TakeScreenshot();
        }
    }
    public void TakeScreenshot()
    {
        Debug.Log("take screenshot");
        ScreenCapture.CaptureScreenshot("C:\\Users\\michaela.kingshott.YORKSJ\\Downloads\\myscreenshot.png", 4);
    }
}
