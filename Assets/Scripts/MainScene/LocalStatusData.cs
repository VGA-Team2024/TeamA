/// <summary>
/// ゲームの進行度や音量設定などを保持する
/// </summary>
[System.Serializable]
public class LocalStatusData
{
    public string LastStageName;
    public float SEVolume;
    public float BGMVolume;
    public float VoiceVolume;
}
