using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    //public TMP_Text statusDisplay;
    
    private void Awake()
    {
        if (Display.displays.Length > 1)
        {
            // Activate the display 1 (second monitor connected to the system).
            Display.displays[1].Activate();
            print("Dual Screen Mode activated!");
            //statusDisplay.text = "Display Mode: Dualscreen";
        }
        else
        {
            print("Split Screen Mode activated!");
            //statusDisplay.text = "Display Mode: Splitscreen";
        }
    }
}
