using System;
using Alchemy.Inspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameOverMovieController : MonoBehaviour
{
    [LabelText("フェード用のイメージ")]
    [SerializeField] private Image _panelImage;
    [LabelText("フェード時間")]
    [SerializeField] private float fadeDuration = 1f;

    [LabelText("VideoPlayer")] 
    [SerializeField] private VideoPlayer _videoPlayer;

    [FormerlySerializedAs("_videoClip")]
    [LabelText("差し替えたいVideoClip")] 
    [SerializeField] private VideoClip _newVideoClip;

    private bool _isSkip;
    private bool _isActive;

    private string _sceneName = "FirstStageSystem";
    
    private void Start()
    {
        //レコードスタート
        GameEventRecorder.GameStart();
        _videoPlayer.isLooping = false; // ループ再生を無効化
        _videoPlayer.Prepare();
        _videoPlayer.prepareCompleted += OnPrepareCompleted;
        _videoPlayer.loopPointReached += ChangeVideo;
    }

    private void OnPrepareCompleted(VideoPlayer vp)
    {
        Color color = _panelImage.color;
        color.a = 0;
        _panelImage.color = color;
        // 動画再生を開始
        _videoPlayer.time = 0;
        _videoPlayer.Play();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && _isActive)
        {
            ActiveButton();
        }
    }

    /// <summary>
    /// シーン遷移
    /// </summary>
    public void ActiveButton()
    {
        if(!_isSkip)
        {
            _isSkip = true;
            StartCoroutine("FadeOut");
        }
        
    }
    
    private void MoveScene()
    {
        //ここでシーン名を受け取る
        
        _videoPlayer.Pause();
        SceneLoader.LoadSceneSimple(_sceneName);
    }

    private void ChangeVideo(VideoPlayer vp)
    {
        _videoPlayer.loopPointReached -= ChangeVideo;
        _isActive = true;
        _videoPlayer.clip = _newVideoClip;
        
        //再準備
        _videoPlayer.Prepare();
        _videoPlayer.prepareCompleted += OnNewClipPrepared;
    }
    
    private void OnNewClipPrepared(VideoPlayer vp)
    {
        _videoPlayer.prepareCompleted -= OnNewClipPrepared;

        // 再生を開始
        _videoPlayer.time = 0;
        _videoPlayer.isLooping = true;
        _videoPlayer.Play();
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
    
    private void OnDestroy()
    {
        _videoPlayer.prepareCompleted -= OnPrepareCompleted;
        _videoPlayer.loopPointReached -= ChangeVideo;
    }
}