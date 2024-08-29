public partial class BuglyAgent
{

#if UNITY_IOS || UNITY_IPHONE
    private static bool _crashReporterTypeConfiged = false;

    private static void ConfigCrashReporterType(){
        if (!_crashReporterTypeConfiged) {
            try {
                _BuglyConfigCrashReporterType(_crashReporterType);
                _crashReporterTypeConfiged = true;
             } catch {
                
            }
        }
    }

    private static void ConfigDefaultBeforeInit(string channel, string version, string user, long delay){
        ConfigCrashReporterType();
        
        try {
            _BuglyDefaultConfig(channel, version, user, null);
        } catch {
            
        }
    }
    
    private static void EnableDebugMode(bool enable){
        _debugMode = enable;
    }
    
    private static void InitBuglyAgent (string appId)
    {
        ConfigCrashReporterType();
        
        if(!string.IsNullOrEmpty(appId)) {
            
            _BuglyInit(appId, _debugMode, _crashReproterCustomizedLogLevel); // Log level 
        }
    }
    
    private static void SetUnityVersion(){
        ConfigCrashReporterType();
        
        _BuglySetExtraConfig("UnityVersion", Application.unityVersion);
    }
    
    private static void SetUserInfo(string userInfo){
        if(!string.IsNullOrEmpty(userInfo)) {
            ConfigCrashReporterType();
            
            _BuglySetUserId(userInfo);
        }
    }
    
    private static void ReportException (int type, string name, string reason, string stackTrace, bool quitProgram)
    {
        ConfigCrashReporterType();
        
        string extraInfo = "";
        Dictionary<string, string> extras = null;
        if (_LogCallbackExtrasHandler != null) {
            extras = _LogCallbackExtrasHandler();
        }
        if (extras == null || extras.Count == 0) {
            extras = new Dictionary<string, string> ();
            extras.Add ("UnityVersion", Application.unityVersion);
        }
        
        if (extras != null && extras.Count > 0) {
            if (!extras.ContainsKey("UnityVersion")) {
                extras.Add ("UnityVersion", Application.unityVersion);
            }
            
            StringBuilder builder = new StringBuilder();
            foreach(KeyValuePair<string,string> kvp in extras){
                builder.Append(string.Format("\"{0}\" : \"{1}\"", kvp.Key, kvp.Value)).Append(" , ");
            }
            extraInfo = string.Format("{{ {0} }}", builder.ToString().TrimEnd(" , ".ToCharArray()));
        }
        
        // 4 is C# exception
        _BuglyReportException(4, name, reason, stackTrace, extraInfo, quitProgram);
    }
    
    private static void SetCurrentScene(int sceneId) {
        ConfigCrashReporterType();
        
        _BuglySetTag(sceneId);
    }
    
    private static void AddKeyAndValueInScene(string key, string value){
        ConfigCrashReporterType();
        
        _BuglySetKeyValue(key, value);
    }
    
    private static void AddExtraDataWithException(string key, string value) {
        
    }
    
    private static void LogRecord(LogSeverity level, string message){
        if (level < LogSeverity.LogWarning) {
            DebugLog (level.ToString (), message);
        }
        
        ConfigCrashReporterType();
        
        _BuglyLogMessage(LogSeverityToInt(level), null, message);
    }
    
    private static int LogSeverityToInt(LogSeverity logLevel){
        int level = 5;
        switch(logLevel) {
        case LogSeverity.Log:
            level = 5;
            break;
        case LogSeverity.LogDebug:
            level = 4;
            break;
        case LogSeverity.LogInfo:
            level = 3;
            break;
        case LogSeverity.LogWarning:
        case LogSeverity.LogAssert:
            level = 2;
            break;
        case LogSeverity.LogError:
        case LogSeverity.LogException:
            level = 1;
            break;
        default:
            level = 0;
            break;
        }
        return level;
    }
    
    // --- dllimport start ---
    [DllImport("__Internal")]
    private static extern void _BuglyInit(string appId, bool debug, int level);
    
    [DllImport("__Internal")]
    private static extern void _BuglySetUserId(string userId);
    
    [DllImport("__Internal")]
    private static extern void _BuglySetTag(int tag);
    
    [DllImport("__Internal")]
    private static extern void _BuglySetKeyValue(string key, string value);
    
    [DllImport("__Internal")]
    private static extern void _BuglyReportException(int type, string name, string reason, string stackTrace, string extras, bool quit);
    
    [DllImport("__Internal")]
    private static extern void _BuglyDefaultConfig(string channel, string version, string user, string deviceId);
    
    [DllImport("__Internal")]
    private static extern void _BuglyLogMessage(int level, string tag, string log);
    
    [DllImport("__Internal")]
    private static extern void _BuglyConfigCrashReporterType(int type);
    
    [DllImport("__Internal")]
    private static extern void _BuglySetExtraConfig(string key, string value);
    
    // dllimport end
#endif
}