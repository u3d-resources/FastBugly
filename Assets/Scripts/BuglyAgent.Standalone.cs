public partial class BuglyAgent
{
    
#if (UNITY_EDITOR || UNITY_STANDALONE) && !UNITY_ANDROID
    private static void InitBuglyAgent (string appId)
    {
    }
    
    private static void ConfigDefaultBeforeInit(string channel, string version, string user, long delay){
    }
    
    private static void EnableDebugMode(bool enable){
    }
    
    private static void SetUserInfo(string userInfo){
    }
    
    private static void ReportException (int type,string name, string message, string stackTrace, bool quitProgram)
    {
    }
    
    private static void SetCurrentScene(int sceneId) {
    }
    
    private static void AddKeyAndValueInScene(string key, string value){
    }
    
    private static void AddExtraDataWithException(string key, string value) {
        // only impl for iOS
    }
    
    private static void LogRecord(LogSeverity level, string message){
    }
    
    private static void SetUnityVersion(){
        
    }

#endif

}