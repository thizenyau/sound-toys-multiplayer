using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Theremin
{
    [RequireComponent(typeof(CsoundUnity))]
    public class Csound : MonoBehaviour
    {
        public BodyData bodyData ;
        CsoundUnity _csound;
        private float freq;
        private bool noise;
        private float ampl;


        IEnumerator Start()
        {
            _csound = GetComponent<CsoundUnity>();
            while (!_csound.IsInitialized)
                yield return null;

            _csound.SetChannel("Frequency", 1f);
            _csound.SetChannel("Amplitude", 0.7f);
            _csound.SetChannel("Noise", 0.1f);
        }

        void Update()
        {
            if (!_csound.IsInitialized)
                return;
            freq = bodyData.HandDistance0_1();
            noise = bodyData.isSurrounding;
            ampl = bodyData.LeftHandHeight0_1();


            if (noise)
                _csound.SetChannel("Noise", 0.2f);
            else
                _csound.SetChannel("Noise", 0f);

            _csound.SetChannel("Frequency", CsoundUnity.Remap(freq, 0f, 1f, 220f, 1100f, true));
            _csound.SetChannel("Amplitude", ampl);
        }
    }
}