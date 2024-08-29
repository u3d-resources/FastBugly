using UnityEngine;

    public partial class BuglyAgent
    {
        
#if UNITY_ANDROID
        
    private static readonly string GAME_AGENT_CLASS = "com.tencent.bugly.agent.GameAgent";
    private static readonly int TYPE_U3D_CRASH = 4;
    private static readonly int GAME_TYPE_UNITY = 2;
    private static bool hasSetGameType = false;
    private static AndroidJavaClass _gameAgentClass = null;
    
    public static AndroidJavaClass GameAgent {
        get {
            if (_gameAgentClass == null) {
                _gameAgentClass = new AndroidJavaClass(GAME_AGENT_CLASS);
//                using (AndroidJavaClass clazz = new AndroidJavaClass(CLASS_UNITYAGENT)) {
//                    _gameAgentClass = clazz.CallStatic<AndroidJavaObject> ("getInstance");
//                }
            }
            if (!hasSetGameType) {
                // set game type: unity(2).
                _gameAgentClass.CallStatic ("setGameType", GAME_TYPE_UNITY);
                hasSetGameType = true;
            }
            return _gameAgentClass;
        }
    }
    
    private static string _configChannel;
    private static string _configVersion;
    private static string _configUser;
    private static long _configDelayTime;
    
    private static void ConfigDefaultBeforeInit(string channel, string version, string user, long delay){
        _configChannel = channel;
        _configVersion = version;
        _configUser = user;
        _configDelayTime = delay;
    }

    private static bool _configCrashReporterPackage = false;
    
    private static void ConfigCrashReporterPackage(){
        
        if (!_configCrashReporterPackage) {
            try {
                GameAgent.CallStatic("setSdkPackageName", _crashReporterPackage);
                _configCrashReporterPackage = true;
            } catch {
                
            }
        }
        
    }

    private static void InitBuglyAgent(string appId)
    {
        if (IsInitialized) {
            return;
        }
        
        ConfigCrashReporterPackage();

        try {
            GameAgent.CallStatic("initCrashReport", appId, _configChannel, _configVersion, _configUser, _configDelayTime);
            _isInitialized = true;
        } catch {
            
        }
    }
    
    private static void EnableDebugMode(bool enable){
        _debugMode = enable;

        ConfigCrashReporterPackage();

        try {
            GameAgent.CallStatic("setLogEnable", enable);
        } catch {
            
        }
    }
    
    private static void SetUserInfo(string userInfo){
        ConfigCrashReporterPackage();

        try {
            GameAgent.CallStatic("setUserId", userInfo);
        } catch {
        }
    }
    
    private static void ReportException (int type, string name, string reason, string stackTrace, bool quitProgram)
    {
        ConfigCrashReporterPackage();

        try {
            GameAgent.CallStatic("postException", TYPE_U3D_CRASH, name, reason, stackTrace, quitProgram);
        } catch {
            
        }
    }
    
    private static void SetCurrentScene(int sceneId) {
        ConfigCrashReporterPackage();

        try {
            GameAgent.CallStatic("setUserSceneTag", sceneId);
        } catch {
            
        }
    }
    
    private static void SetUnityVersion(){
        ConfigCrashReporterPackage();

        try {
            GameAgent.CallStatic("setSdkConfig", "UnityVersion", Application.unityVersion);
        } catch {
            
        }
    }
    
    private static void AddKeyAndValueInScene(string key, string value){
        ConfigCrashReporterPackage();

        try {
            GameAgent.CallStatic("putUserData", key, value);
        } catch {
            
        }
    }
    
    private static void AddExtraDataWithException(string key, string value) {
        // no impl
    }
    
    private static void LogRecord(LogSeverity level, string message){
        if (level < LogSeverity.LogWarning) {
            DebugLog (level.ToString (), message);
        }

        ConfigCrashReporterPackage();
        
        try {
            GameAgent.CallStatic("printLog", string.Format ("<{0}> - {1}", level.ToString (), message));
        } catch {
            
        }
    }
        
#endif
        
        
    }
