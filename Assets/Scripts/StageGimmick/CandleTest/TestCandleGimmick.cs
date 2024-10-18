using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCandleGimmick : StageGimmickBase
{
    private StageGimmickObserver _observer;

    private void Awake()
    {
        _observer = FindObjectOfType<StageGimmickObserver>();
        _observer.OnAllGimmicksClear += ClearTest;
    }
    protected override void ClearActive()
    {
        base.ClearActive();
    }
    private void ClearTest()
    {
        //�����ɃX�C�b�`���o�������鏈��
        Debug.Log("�X�C�b�`�o��!");
    }
}