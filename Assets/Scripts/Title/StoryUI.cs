using LitMotion;
using LitMotion.Extensions;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SpriteData
{
    public string Key;
    public Sprite Sprite;
}

public class StoryUI : MonoBehaviour
{
    [SerializeField] string _path = "Image";
    [SerializeField] TextMeshProUGUI _lineText;//文字表示するテキスト
    [SerializeField] Image _bgImage;//背景イメージ
    [SerializeField] Image _animImage;//アニメーション処理用イメージ
    [SerializeField] int _textSpeed = 5;
    [SerializeField] private float _fadeDuration;//画像のフェードにかかる時間
    [SerializeField] private Ease _ease;
    [SerializeField, Multiline] string[] _lines; // "Image/キー名" で画像変更行, それ以外はテキスト行

    [SerializeField] SpriteData[] _spriteDatas; // InspectorでKeyとSpriteを紐づけ

    private Dictionary<string, Sprite> _spriteDictionary;
    private MotionHandle _handle;
    private int _count = 0;
    void Awake()
    {
        // SpriteData配列からDictionaryへ変換
        _spriteDictionary = _spriteDatas.ToDictionary(x => x.Key, x => x.Sprite);
    }
    void Start()
    {
        StoryText();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //テキスト表示アニメーション中にクリックされたらテキストアニメーションを完了させる
            if (_handle.IsActive())
            {
                _handle.Complete();
            }
            else
            {
                StoryText();
            }
        }
    }
    public void StoryText()
    {
        //ストーリーのデータの_lines[]が全て終わっていたらreturn
        if (_count >= _lines.Length)
        {
            Debug.Log("ストーリー終了");
            return;
        }
        string[] line = _lines[_count].Split('/');
        // line[0] == "Image" の場合はline[1]がキー名、それ以外はテキスト表示
        if (line[0] == _path && line.Length > 1)
        {
            //画像変更する
            Debug.Log("画像変更");
            string key = line[1];
            if (_spriteDictionary.TryGetValue(key, out var sprite))
            {
                _animImage.sprite = sprite;
                LMotion.Create(0f, 1f, _fadeDuration)//アルファ値をフェードさせる
                      .WithEase(_ease)
                      .WithOnComplete(() => FadeIn())//モーションが完了したときの処理
                      .BindToColorA(_animImage);
            }
            else
            {
                Debug.LogWarning($"キー '{key}' に対応する画像が見つかりません");
            }
            _count++;
            StoryText();
        }
        else
        {
            string text = _lines[_count];
            //テキスト表示アニメーションのハンドルを入れる
            _handle = LMotion.String.Create512Bytes("", text, text.Length / _textSpeed)
                     .WithRichText()
                     .WithScrambleChars(ScrambleMode.None)
                     .BindToText(_lineText);
            _count++;
        }
    }
    public void FadeIn()
    {
        // フェードイン完了後、_animImageの画像を_bgImageへ移し、_animImageは透明化
        _bgImage.sprite = _animImage.sprite;
        _animImage.color = new Color(1, 1, 1, 0);
    }
}
