﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sharklib.ShineShader.Examples {
    public class ManualControls : MonoBehaviour
    {
        [SerializeField] ShineControl shineControl;
        [SerializeField] Text loopState;
        [SerializeField] Text loopCounter;
        [SerializeField] Slider loopSlider;

        int numLoops = 1;

        void Start() {
            if (loopSlider != null)
                ChangeLoops(loopSlider.value);

            EventLoopEnded();
        }

        public void ChangeLoops(float loops) {
            numLoops = Mathf.RoundToInt(loops);

            if (loopCounter != null)
                loopCounter.text = "Loops: " + numLoops;
        }

        public void TriggerLoops() {
            shineControl.TriggerShine(numLoops);
        }

        public void EventShineStart(int loop, float duration) {
            if (loopState != null)
                loopState.text = string.Format("Loop {0} Started (duration: {1:0.00}s)", loop, duration);
        }

        public void EventPauseStart(int loop, float duration) {
            if (loopState != null)
                loopState.text = string.Format("Loop {0} Paused (duration: {1:0.00}s)", loop, duration);
        }

        public void EventLoopEnded() {
            if (loopState != null)
                loopState.text = "Loops ended";
        }

    }
}