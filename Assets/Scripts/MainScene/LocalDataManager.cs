using System.IO;
using System.Linq;
using UnityEngine;

public class LocalDataManager : MonoBehaviour
{
    static LocalDataManager _instance;
    public static LocalDataManager Instance => _instance;

    LocalStatusData _localData;
    public LocalStatusData GetLocalData => _localData;
    string _filepath;
    string _fileName = "SaveData.json";
    [SerializeField] string[] _disableSceneNames;
    void Awake()
    {
        //シングルトン化
        if(_instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        //セーブファイルを探して存在するならロード
        _filepath = Application.dataPath + "/" + _fileName;
        // ファイルがないとき、ファイル作成
        if (!File.Exists(_filepath))
        {
            //初期設定用のクラス作成
            LocalStatusData initialSaveData = new LocalStatusData();
            Save(initialSaveData);
        }
        // ファイルを読み込んでdataに格納
        _localData = Load(_filepath);
    }
    private void Start()
    {
        //InitializeGameSetting();
    }
    private void InitializeGameSetting()
    {
        CRIAudioManager.BGM.SetVolume(_localData.BGMVolume);
        CRIAudioManager.SE.SetVolume(_localData.SEVolume);
        CRIAudioManager.VOICE.SetVolume(_localData.VoiceVolume);
    }
    /// <summary>
    /// 指定したデータをファイルに書き込み
    /// </summary>
    /// <param name="saveData"></param>
    void Save(LocalStatusData saveData)
    {
        string json = JsonUtility.ToJson(saveData);
        StreamWriter wr = new StreamWriter(_filepath, false);
        wr.WriteLine(json);
        wr.Close();
    }
    /// <summary>
    /// ファイルからデータ読み込み
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    LocalStatusData Load(string filePath)
    {
        StreamReader rd = new StreamReader(filePath);
        string json = rd.ReadToEnd();
        rd.Close();

        return JsonUtility.FromJson<LocalStatusData>(json);
    }
    /// <summary>
    /// <br>最後にプレイしたステージ状態を更新する</br>
    /// <br>各StageFacadeで呼び出す</br>
    /// </summary>
    public void UpdateLastSceneName()
    {
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            string readSceneName = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).name;
            if (!_disableSceneNames.Any(x => x == readSceneName))
            {
                _localData.LastStageName = readSceneName;
#if UNITY_EDITOR
                Debug.Log($"LastStageName updated : {readSceneName}");
#endif
                break;
            }
        }
    }
    private void OnDisable()
    {
        _localData.BGMVolume = CRIAudioManager.BGM.GetAtomExPlayerVol;
        _localData.SEVolume = CRIAudioManager.SE.GetAtomExPlayerVol;
        _localData.VoiceVolume = CRIAudioManager.VOICE.GetAtomExPlayerVol;
        Save(_localData);
    }
}
