using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCandleGimmick : StageGimmickBase
{
    private StageGimmickObserver _observer;
    protected override void ClearActive(bool changeIsClear)
    {
        base.ClearActive(changeIsClear);
    }
    public void OnFire()
    {
        ClearActive(true);
    }
}
