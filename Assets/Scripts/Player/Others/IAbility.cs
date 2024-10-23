using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ability
{
    //�v���C���[�̃A�r���e�B(�X�L��)�𒊏ۓI�Ɉ������߂�interface�ł�
    public interface IPlayerAbility 
    {
        /// <summary>
        /// ���݂�Ability��NoneAbility(�A�r���e�B���ݒ肳��Ă��Ȃ����)�łȂ��Ƃ�true��Ԃ�masu
        /// </summary>
        /// <returns></returns>
        bool HasAbility() { return true; }

        /// <summary>
        /// �ݒ肳�ꂽ�A�r���e�B�����s���܂�
        /// </summary>
        void PerformAbility();
    }
    //�Ȃ�̃A�r���e�B���ݒ肳��Ă��Ȃ����Ƃ�\��Ability�ł�
    public sealed class NoneAbility : IPlayerAbility
    {
        public bool HasAbility() { return false; }
        public void PerformAbility() { }
    }

    //�e�X�g�p
    public class CandleAbility : IPlayerAbility
    {
        public void PerformAbility()
        {
            Debug.Log("CandleAbility���ݒ肳��Ă��܂�");
        }
    }
}


