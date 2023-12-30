using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Theremin
{
    [RequireComponent(typeof(CsoundUnity))]
    public class Theremin : MonoBehaviour
    {

        //public GameObject head;
  //private UnityEngine.Vector3 _headPosition = UnityEngine.Vector3.zero;
        // private float _headNote = 60;
        // private float _leftHandNote = 57;
        // private float _rightHandNote = 64;
        // private float _headNote1 = 60;
        // private float _leftHandNote1 = 57;
        // private float _rightHandNote1 = 64;
        // private float _leftHandNote2 = 57;
        // private float _rightHandNote2 = 64;
        // private float _leftDistance = 0f;
        // private float _rightDistance = 0f;
        public GameObject leftHand;
        public GameObject rightHand;
        public GameObject head;
        public GameObject leftfeet;

        private UnityEngine.Vector3 _leftHandPosition = UnityEngine.Vector3.zero;
        private UnityEngine.Vector3 _rightHandPosition = UnityEngine.Vector3.zero;
        private UnityEngine.Vector3 _headPosition = UnityEngine.Vector3.zero;
        private UnityEngine.Vector3 _leftfeetPosition = UnityEngine.Vector3.zero;
        private float _handDistance = 0f;
  private float fullDistance = 1.524972f;
        private float _headToFeet = 0f;
        private float headToFeetDistance = 1.620287f;
        private int timer = 0;
        private UnityEngine.Vector3 _currentPosition = UnityEngine.Vector3.zero;
  public float _out1 = 0;  
        public float _pitch1 = 0;
        public float _volume1 = 0;

        private bool _isSTWing = false;
        public DrawSineLine line;




        [SerializeField] Vector2 _freqRange = new Vector2(220, 1100);

        CsoundUnity _csound;

        IEnumerator Start()
        {
            _csound = GetComponent<CsoundUnity>();
            while (!_csound.IsInitialized)
                yield return null;

            _csound.SetChannel("Frequency", 440f);
            _csound.SetChannel("Amplitude", 0.7f);
            _csound.SetChannel("Noise", 0.1f);
        }

        void Update()
        {
            if (!_csound.IsInitialized)
                return;

            if(leftHand == null || rightHand == null) return;


            _leftHandPosition = leftHand.transform.position;
            _rightHandPosition = rightHand.transform.position;
            _headPosition = head.transform.position;
            _leftfeetPosition = leftfeet.transform.position;

            _headToFeet = _headPosition.y - _leftfeetPosition.y;

            _handDistance = UnityEngine.Vector3.Distance(_rightHandPosition , _leftHandPosition);


            _pitch1 = _handDistance / fullDistance;
            _volume1 = Math.Clamp(_headToFeet / headToFeetDistance , 0.5f , 1f);


            _isSTWing = line._isSTWing;
            if (_isSTWing)
                _csound.SetChannel("Noise", 0.2f);
            
            else
                _csound.SetChannel("Noise", 0f);

            _csound.SetChannel("Frequency", CsoundUnity.Remap(_pitch1, 0f, 1f, _freqRange.x, _freqRange.y, true));
            _csound.SetChannel("Amplitude", CsoundUnity.Remap(_volume1, 0.5f, 1f, 0f, 1f, true));
        }
    }
}