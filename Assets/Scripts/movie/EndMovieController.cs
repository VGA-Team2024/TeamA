using System;
using Alchemy.Inspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndMovieController : MonoBehaviour
{
    [LabelText("フェード用のイメージ")]
    [SerializeField] private Image _panelImage;
    [LabelText("フェード時間")]
    [SerializeField] private float fadeDuration = 1f;

    private Action _action;

    bool _isSkip;

    private void Awake()
    {
        _action += MoveScene;
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
    /// ゲーム終了時の処理
    /// </summary>
    private void GameEnd()
    {
        GameEventRecorder.GameEnd(MoveScene);
    }
    
    /// <summary>
    /// シーン遷移
    /// </summary>
    public void MoveScene()
    {
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

        GameEnd();
    }

    private void OnDestroy()
    {
        _action -= MoveScene;
    }
}