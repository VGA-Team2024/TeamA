using System.Collections;
using UnityEngine;

public class BossAreaAttack : MonoBehaviour
{
    [SerializeField] private GameObject _warningIndicator;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _warningDuration = 3f;

    private BoxCollider _warningIndicatorCollider;

    private void Start()
    {
        SetupWarningIndicator();
    }
    private void Update() //でばっく
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShowWarning();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            ExecuteAttack(1);
        }
    }
    /// <summary>
    /// 警告表示オブジェクトの初期設定
    /// </summary>
    private void SetupWarningIndicator()
    {
        if (_warningIndicator == null)
        {
            Debug.LogError("警告表示用のオブジェクトが設定されていません。");
            return;
        }

        _warningIndicatorCollider = _warningIndicator.GetComponent<BoxCollider>();
        if (_warningIndicatorCollider == null)
        {
            Debug.LogError("警告表示用オブジェクトにBoxColliderがありません。");
            return;
        }

        _warningIndicator.SetActive(false); // 初期状態で非アクティブ
    }

    /// <summary>
    /// 警告表示を開始
    /// </summary>
    public void ShowWarning()
    {
        if (_warningIndicator == null) return;

        _warningIndicator.SetActive(true);
        StartCoroutine(HideWarningAfterDelay(_warningDuration));
    }

    /// <summary>
    /// 実際の攻撃処理
    /// </summary>
    public void ExecuteAttack(float damage)
    {
        // 警告表示用オブジェクトの位置・サイズ・回転を取得
        Vector3 boxCenter = _warningIndicator.transform.position;
        Vector3 boxHalfExtents = _warningIndicator.transform.lossyScale / 2f; // グローバルスケールを考慮
        Quaternion boxRotation = _warningIndicator.transform.rotation;

        // OverlapBoxで判定
        Collider[] colliders = Physics.OverlapBox(boxCenter, boxHalfExtents, boxRotation, _targetLayer);
        foreach (Collider collider in colliders)
        {
            Debug.Log($"攻撃範囲内にプレイヤーを検知: {collider.name}");
            // ダメージ処理
            if (collider.TryGetComponent<PlayerDamageReceiver>(out PlayerDamageReceiver damageReceiver))
            {
                damageReceiver.ApplyDamage(damage);
                Debug.Log($"{collider.name}に{damage}ダメージ与えた");
            }
        }
    }


    /// <summary>
    /// 警告表示を非アクティブ化
    /// </summary>
    private IEnumerator HideWarningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (_warningIndicator != null)
        {
            _warningIndicator.SetActive(false);
        }
    }

    /// <summary>
    /// エディタ上で範囲を視覚化
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        var cube = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(this.transform.localPosition, this.transform.localRotation, this.transform.localScale);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_warningIndicator.transform.localPosition, _warningIndicator.transform.localScale);
        Gizmos.matrix = cube;
    }
}
