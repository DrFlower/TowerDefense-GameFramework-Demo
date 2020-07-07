using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Flower.Data;
using GameFramework;

namespace Flower
{
    class WaveControl : IReference
    {
        private Queue<WaveData> waveDatas;

        public WaveControl()
        {
            waveDatas = new Queue<WaveData>();
        }

        public void StartWave()
        {

        }

        public void OnPause()
        {

        }

        public void OnResume()
        {

        }

        public void OnRestart()
        {

        }

        public void OnGameover()
        {

        }

        public void OnQuick()
        {

        }

        public static WaveControl Create(WaveData [] waveDatas)
        {
            WaveControl waveControl = ReferencePool.Acquire<WaveControl>();
            for (int i = 0; i < waveDatas.Length; i++)
            {
                waveControl.waveDatas.Enqueue(waveDatas[i]);
            }

            return waveControl;
        }

        public void Clear()
        {
            waveDatas.Clear();
        }
    }
}
