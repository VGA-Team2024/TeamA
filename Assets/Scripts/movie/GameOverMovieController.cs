using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameOverMovieController : MonoBehaviour
{
    [LabelText("シーン移動用のボタンがまとめられてるCanvasGroup")]

    [SerializeField] private CanvasGroup _canvasGroup;
    [LabelText("ボタンのフェードインの時間")]
    [SerializeField] private float _fadeinDuration = 0.5f;
    [LabelText("フェード用のイメージ")]
    [SerializeField] private Image _panelImage;
    [LabelText("フェード時間")]
    [SerializeField] private float _fadeoutDuration = 1f;

    [LabelText("VideoPlayer")]
    [SerializeField] private VideoPlayer _videoPlayer;

    [FormerlySerializedAs("_videoClip")]
    [LabelText("差し替えたいVideoClip")]
    [SerializeField] private VideoClip _newVideoClip;

    private bool _isSkip;
    private bool _isActive;

    private string _sceneNameTitle = "TitleScene";

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
    public async void MoveSceneLastScene()
    {
        await FadeOutScene();
        try
        {
            SceneLoader.LoadSceneSimple(LocalDataManager.Instance.GetLocalData.LastStageName);
        }
        catch
        {
            Debug.LogWarning("LastStageName not exist");
            SceneLoader.LoadSceneSimple(_sceneNameTitle);
        }
    }

    public async void MoveSceneTitle()
    {
        await FadeOutScene();
        //ここでシーン名を受け取る
        SceneLoader.LoadSceneSimple(_sceneNameTitle);
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
        LMotion.Create(0f, 1f, _fadeinDuration).Bind(x => _canvasGroup.alpha = x);
        _canvasGroup.blocksRaycasts = true;
        Cursor.visible = true;
    }
    private async UniTask FadeOutScene()
    {
        await LMotion.Create(0f, 1f, _fadeoutDuration).BindToColorA(_panelImage);
        _videoPlayer.Pause();
    }

    private void OnDestroy()
    {
        _videoPlayer.prepareCompleted -= OnPrepareCompleted;
        _videoPlayer.loopPointReached -= ChangeVideo;
    }
}