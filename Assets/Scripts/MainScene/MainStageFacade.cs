using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 継承前提
/// 各ステージの最初にやりたい処理をそれぞれするやつ
/// </summary>
public class MainStageFacade : MonoBehaviour
{
    [SerializeField] private string _bgmCueName = string.Empty;
    /// <summary>
    /// 書くまでもない処理を登録する用
    /// </summary>
    [SerializeField] UnityEvent OnAwaked;
    /// <summary>
    /// <br>ローカルデータ更新とステージ最初の演出</br>
    /// <br>注意　Base.Startを呼び出すこと</br>
    /// </summary>
    protected virtual void Start()
    {
        if(LocalDataManager.Instance != null)
        {
            LocalDataManager.Instance.UpdateLastSceneName();
        }
        else
        {
            Debug.Log("Not Loaded LocalDataManager instance");
        }
        try
        {
            CRIAudioManager.BGM.Play("CueSheet_0", _bgmCueName);
        }
        catch
        {
            Debug.LogWarning($"{_bgmCueName}:this bgm doesn't exist");
        }
        if(PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SearchPlayerInScene();
        }
        PlayerEventHelper.OnPlayerDie += SubscribeGameOverLoad;
        OnAwaked?.Invoke();
    }
    private void SubscribeGameOverLoad()
    {
        SceneLoader.LoadSceneSimple("GameOverScene");
    }
    private void OnDisable()
    {
        PlayerEventHelper.OnPlayerDie -= SubscribeGameOverLoad;
    }
}
