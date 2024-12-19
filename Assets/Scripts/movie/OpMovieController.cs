using Alchemy.Inspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OpMovieController : MonoBehaviour
{
    [LabelText("フェード用のイメージ")]
    [SerializeField] private Image _panelImage;
    [LabelText("フェード時間")]
    [SerializeField] private float fadeDuration = 1f;

    bool _isSkip;
    private void Awake()
    {
        //レコードスタート
        GameEventRecorder.GameStart();
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
        SceneLoader.LoadSceneSimple("Stage_1");
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
}