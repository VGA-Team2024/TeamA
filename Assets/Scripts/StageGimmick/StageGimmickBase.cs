using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageGimmickBase : MonoBehaviour, ISendCompleteFlag
{
    /// <summary>
    /// <para>�N���A���ɌĂяo������</para>
    /// <para>��override����ꍇ�͕K��base.ClearAction();���܂�ł�������</para>
    /// </summary>
    protected virtual void ClearAction()
    {
        SendFlag();
    }

    public void SendFlag()
    {
        
    }
}
