using LitMotion;
using LitMotion.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryUI : MonoBehaviour
{
    [SerializeField] string _path = "Image";
    [SerializeField] TextMeshProUGUI _lineText;//文字表示するテキスト
    [SerializeField] UnityEngine.UI.Image _bgImage;//背景イメージ
    [SerializeField] UnityEngine.UI.Image _animImage;//アニメーション処理用イメージ
    [SerializeField] int _textSpeed = 5;

    [SerializeField] private float _fadeDuration;//画像のフェードにかかる時間
    [SerializeField] private Ease _ease;

    [SerializeField, MultilineAttribute] string[] _lines;

    MotionHandle _handle;//テキストのモーションハンドル
    int _count = 0;
    void Start()
    {
        StoryText();
    }

    // Update is called once per frame
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
        //「/」の記号で画像変更かテキスト変更かを判別
        string[] line = _lines[_count].Split('/');
        if (line[0] == _path)
        {
            //画像変更する
            Debug.Log("画像変更");

            _animImage.sprite = Resources.Load<Sprite>(_lines[_count]);
            LMotion.Create(0f, 1f, _fadeDuration)//アルファ値をフェードさせる
            .WithEase(_ease)
            .WithOnComplete(() => FadeIn())//モーションが完了したときの処理
            .BindToColorA(_animImage);

            _count++;
            StoryText();//イメージ変更とともにテキストも変えるため、StoryText()を再呼び出し
        }
        else
        {

            //テキスト表示アニメーションのハンドルを入れる
            _handle = LMotion.String.Create512Bytes("", _lines[_count], _lines[_count].Length / _textSpeed).WithRichText().WithScrambleChars(ScrambleMode.None)
            .BindToText(_lineText); ;

            _count++;
        }

    }
    public void FadeIn()
    {
        //フェードインが完了したら、アニメーション用のimageのスプライトを_bgImageに移行
        //その後_animImageのアルファ値を0にして非表示
        _bgImage.sprite = _animImage.sprite;
        _animImage.color = new Color(1, 1, 1, 0);
    }
}