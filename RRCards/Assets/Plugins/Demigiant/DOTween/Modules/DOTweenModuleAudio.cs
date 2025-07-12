
#if true // MODULE_MARKER
using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Audio; // Required for AudioMixer

#pragma warning disable 1591
namespace DG.Tweening
{
	public static class DOTweenModuleAudio
    {
        #region Shortcuts

        #region Audio
        public static TweenerCore<float, float, FloatOptions> DOFade(this AudioSource target, float endValue, float duration)
        {
            if (endValue < 0) endValue = 0;
            else if (endValue > 1) endValue = 1;
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.volume, x => target.volume = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        public static TweenerCore<float, float, FloatOptions> DOPitch(this AudioSource target, float endValue, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.pitch, x => target.pitch = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }

        #endregion

        #region AudioMixer
        public static TweenerCore<float, float, FloatOptions> DOSetFloat(this AudioMixer target, string floatName, float endValue, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(()=> {
                    float currVal;
                    target.GetFloat(floatName, out currVal);
                    return currVal;
                }, x=> target.SetFloat(floatName, x), endValue, duration);
            t.SetTarget(target);
            return t;
        }

        #region Operation Shortcuts
        public static int DOComplete(this AudioMixer target, bool withCallbacks = false)
        {
            return DOTween.Complete(target, withCallbacks);
        }
        public static int DOKill(this AudioMixer target, bool complete = false)
        {
            return DOTween.Kill(target, complete);
        }
        public static int DOFlip(this AudioMixer target)
        {
            return DOTween.Flip(target);
        }
        public static int DOGoto(this AudioMixer target, float to, bool andPlay = false)
        {
            return DOTween.Goto(target, to, andPlay);
        }
        public static int DOPause(this AudioMixer target)
        {
            return DOTween.Pause(target);
        }
        public static int DOPlay(this AudioMixer target)
        {
            return DOTween.Play(target);
        }
        public static int DOPlayBackwards(this AudioMixer target)
        {
            return DOTween.PlayBackwards(target);
        }
        public static int DOPlayForward(this AudioMixer target)
        {
            return DOTween.PlayForward(target);
        }
        public static int DORestart(this AudioMixer target)
        {
            return DOTween.Restart(target);
        }
        public static int DORewind(this AudioMixer target)
        {
            return DOTween.Rewind(target);
        }
        public static int DOSmoothRewind(this AudioMixer target)
        {
            return DOTween.SmoothRewind(target);
        }
        public static int DOTogglePause(this AudioMixer target)
        {
            return DOTween.TogglePause(target);
        }

        #endregion

        #endregion

        #endregion
    }
}
#endif
