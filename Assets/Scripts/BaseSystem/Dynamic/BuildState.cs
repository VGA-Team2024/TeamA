
public class BuildState
{
    const string _hash = "407bcba0-016a-4a5a-af43-6968666e8998";
    public const string TeamID = "TeamA2024";

    public static string BuildHash
    {
        get
        {
#if UNITY_EDITOR
            return "UNITY_EDITOR";
#else
            return _hash;
#endif
        }
    }
};