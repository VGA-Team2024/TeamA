
public class BuildState
{
    const string _hash = "40c91798-864e-45f8-b70f-6ccce1f2239f";
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