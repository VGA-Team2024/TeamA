using Alchemy.Inspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class EndMovieController : MonoBehaviour
{
    [LabelText("フェード用のイメージ")]
    [SerializeField] private Image _panelImage;
    [LabelText("フェード時間")]
    [SerializeField] private float fadeDuration = 1f;

    [LabelText("VideoPlayer")] 
    [SerializeField] private VideoPlayer _videoPlayer;
    bool _isSkip;
    private bool _isActive;
    

    private void Start()
    {
        _videoPlayer.isLooping = false; // ループ再生を無効化
        _videoPlayer.Prepare();
        _videoPlayer.prepareCompleted += OnPrepareCompleted;
        _videoPlayer.loopPointReached += GameEnd;
    }

    private void OnPrepareCompleted(VideoPlayer vp)
    {
        Color color = _panelImage.color;
        color.a = 0;
        _panelImage.color = color;
        _isActive = true;
        // 動画再生を開始
        _videoPlayer.time = 0;
        _videoPlayer.Play();
    }
    
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && _isActive)
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
    /// ゲーム終了時の処理
    /// </summary>
    public void GameEnd()
    {
        _videoPlayer.Pause();
        GameEventRecorder.GameEnd(MoveScene);
    }

    private void GameEnd(VideoPlayer vp)
    {
        _isSkip = true;
        _videoPlayer.Pause();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameEventRecorder.GameEnd(MoveScene);
    }
    
    /// <summary>
    /// シーン遷移
    /// </summary>
    private void MoveScene()
    {
        SceneLoader.LoadScene("FirstStageSystem");
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
        
        GameEnd();
    }

    private void OnDestroy()
    {
        _videoPlayer.prepareCompleted -= OnPrepareCompleted;
        _videoPlayer.loopPointReached -= GameEnd;
    }
}