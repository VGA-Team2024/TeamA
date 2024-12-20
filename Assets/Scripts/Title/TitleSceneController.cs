using System.Collections;
using System.Collections.Generic;
using Alchemy.Inspector;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneController : MonoBehaviour
{
    [LabelText("再生したいオブジェクト")] 
    [SerializeField] private GameObject[] _targetObject;

    [LabelText("再生したいアニメーションのトリガー名")] 
    [SerializeField] private string _animationTrigger = "Active";
    
    [LabelText("フェード用のイメージ")]
    [SerializeField] private Image _panelImage;
    [LabelText("スタート時のフェード時間")]
    [SerializeField] private float _startFadeDuration = 1f;
    [LabelText("ゲーム終了時のフェード時間")]
    [SerializeField] private float _endFadeDuration = 1f;

    private bool _isButton;
    
    /// <summary>
    /// ゲームスタート時のアニメーションを再生
    /// </summary>
    public void PlayTitleAnimation()
    {
        if (_isButton)
            return;
        
        _isButton = true;
        //クリック音を流す
        CRIAudioManager.SE.Play3D(Vector3.zero, "CueSheet_0", "jingle_start");
        
        // //各オブジェクトのアニメーションを再生
        // foreach (GameObject obj in _targetObject)
        // {
        //     Animator animator = obj.GetComponent<Animator>();
        //     if(animator != null)
        //     {
        //         animator.SetTrigger(_animationTrigger);
        //     }
        // }
        
        //テスト用にシーン遷移
        MoveScene();
    }

    /// <summary>
    /// フェードをスタートさせる処理
    /// </summary>
    public void MoveScene()
    {
        StartCoroutine("FadeOutScene");
    }
    
    private IEnumerator FadeOutScene()
    {
        float timer = 0f;
        while (timer < _startFadeDuration)
        {
            timer += Time.deltaTime;
            Color color = _panelImage.color;
            color.a = Mathf.Lerp(0, 1, timer / _startFadeDuration);
            _panelImage.color = color;
            yield return null;
        }
        
        SceneLoader.LoadSceneSimple("OpScene");
    }

    /// <summary>
    /// ゲーム終了処理
    /// </summary>
    public void FinishGame()
    {
        if (_isButton)
            return;
        _isButton = true;
        CRIAudioManager.SE.Play3D(Vector3.zero, "CueSheet_0", "SE_click");
        StartCoroutine("FadeOutGame");
    }
    
    private IEnumerator FadeOutGame()
    {
        float timer = 0f;
        while (timer < _endFadeDuration)
        {
            timer += Time.deltaTime;
            Color color = _panelImage.color;
            color.a = Mathf.Lerp(0, 1, timer / _endFadeDuration);
            _panelImage.color = color;
            yield return null;
        }
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
        Application.Quit();//ゲームプレイ終了
#endif
    }
}
