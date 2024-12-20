using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleGimmickChacker : StageGimmickBase
{
    protected override void ClearActive(bool changed)
    {
        base.ClearActive(changed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Donuts>(out var donutsComponent))
        {
            ClearActive(true);
            donutsComponent._isClear = true;
        }
    }
}
