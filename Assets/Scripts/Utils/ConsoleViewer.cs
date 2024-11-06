using Events;
using Events.ScriptableObjects;
using UnityEngine;
public class ConsoleViewer : MonoBehaviour
{
    [SerializeField] private BoolEventChannelSO OnCheatToggleConsole;
    string myLog = "*begin log";
    string filename = "";
    bool doShow = false;
    int kChars = 700;
    void OnEnable()
    {
        Application.logMessageReceived += Log;
        OnCheatToggleConsole.onTypedEvent.AddListener(ToggleConsole);
    }
    void OnDisable()
    {
        Application.logMessageReceived -= Log;
        OnCheatToggleConsole.onTypedEvent.RemoveListener(ToggleConsole);
    }
    void ToggleConsole(bool value)
    { 
        doShow = value;
    }
    public void Log(string logString, string stackTrace, LogType type)
    {
        // for onscreen...
        myLog = myLog + " " + logString + "\n";
        if (myLog.Length > kChars) { myLog = myLog.Substring(myLog.Length - kChars); }
    }

    void OnGUI()
    {
        if (!doShow) { return; }
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
           new Vector3(Screen.width / 1200.0f, Screen.height / 800.0f, 1.0f));
        GUI.TextArea(new Rect(10, 10, 540, 370), myLog);
    }
}
