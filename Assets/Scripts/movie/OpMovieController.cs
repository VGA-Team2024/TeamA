using System;
using Alchemy.Inspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class OpMovieController : MonoBehaviour
{
    [LabelText("フェード用のイメージ")]
    [SerializeField] private Image _panelImage;
    [LabelText("フェード時間")]
    [SerializeField] private float fadeDuration = 1f;

    [LabelText("VideoPlayer")] 
    [SerializeField] private VideoPlayer _videoPlayer;

    bool _isSkip;
    private void Awake()
    {
        //レコードスタート
        GameEventRecorder.GameStart();
        
        _videoPlayer.Prepare();
    }

    private void Start()
    {
        _videoPlayer.loopPointReached += FinishMovie;
        //ビデオプレイヤーを再生
        _videoPlayer.Play();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SkipMove();
        }
    }

    /// <summary>
    /// Moveスキップを行う
    /// </summary>
    public void SkipMove()
    {
        if(!_isSkip)
        {
            _isSkip = true;
            StartCoroutine("FadeOut");
        }
    }

    /// <summary>
    /// シーン遷移
    /// </summary>
    public void MoveScene()
    {
        ResetVideo();
        SceneLoader.LoadSceneSimple("FirstStageSystem");
    }

    /// <summary>
    /// ビデオ終了時に呼び出す
    /// </summary>
    private void FinishMovie(VideoPlayer vp)
    {
        _isSkip = true;
        ResetVideo();
        SceneLoader.LoadSceneSimple("FirstStageSystem");
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            Color color = _panelImage.color;
            color.a = Mathf.Lerp(0, 1, timer / fadeDuration);
            _panelImage.color = color;
            yield return null;
        }

        MoveScene();
    }

    /// <summary>
    /// ビデオの初期化
    /// </summary>
    private void ResetVideo()
    {
        _videoPlayer.frame = 0;
        _videoPlayer.Pause();
    }
}