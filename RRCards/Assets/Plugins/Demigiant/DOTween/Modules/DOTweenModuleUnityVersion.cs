
using System;
using UnityEngine;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

#pragma warning disable 1591
namespace DG.Tweening
{
	public static class DOTweenModuleUnityVersion
    {
        #region Material
        public static Sequence DOGradientColor(this Material target, Gradient gradient, float duration)
        {
            Sequence s = DOTween.Sequence();
            GradientColorKey[] colors = gradient.colorKeys;
            int len = colors.Length;
            for (int i = 0; i < len; ++i) {
                GradientColorKey c = colors[i];
                if (i == 0 && c.time <= 0) {
                    target.color = c.color;
                    continue;
                }
                float colorDuration = i == len - 1
                    ? duration - s.Duration(false) // Verifies that total duration is correct
                    : duration * (i == 0 ? c.time : c.time - colors[i - 1].time);
                s.Append(target.DOColor(c.color, colorDuration).SetEase(Ease.Linear));
            }
            s.SetTarget(target);
            return s;
        }
        public static Sequence DOGradientColor(this Material target, Gradient gradient, string property, float duration)
        {
            Sequence s = DOTween.Sequence();
            GradientColorKey[] colors = gradient.colorKeys;
            int len = colors.Length;
            for (int i = 0; i < len; ++i) {
                GradientColorKey c = colors[i];
                if (i == 0 && c.time <= 0) {
                    target.SetColor(property, c.color);
                    continue;
                }
                float colorDuration = i == len - 1
                    ? duration - s.Duration(false) // Verifies that total duration is correct
                    : duration * (i == 0 ? c.time : c.time - colors[i - 1].time);
                s.Append(target.DOColor(c.color, property, colorDuration).SetEase(Ease.Linear));
            }
            s.SetTarget(target);
            return s;
        }

        #endregion

        #region CustomYieldInstructions
        public static CustomYieldInstruction WaitForCompletion(this Tween t, bool returnCustomYieldInstruction)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }
            return new DOTweenCYInstruction.WaitForCompletion(t);
        }
        public static CustomYieldInstruction WaitForRewind(this Tween t, bool returnCustomYieldInstruction)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }
            return new DOTweenCYInstruction.WaitForRewind(t);
        }
        public static CustomYieldInstruction WaitForKill(this Tween t, bool returnCustomYieldInstruction)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }
            return new DOTweenCYInstruction.WaitForKill(t);
        }
        public static CustomYieldInstruction WaitForElapsedLoops(this Tween t, int elapsedLoops, bool returnCustomYieldInstruction)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }
            return new DOTweenCYInstruction.WaitForElapsedLoops(t, elapsedLoops);
        }
        public static CustomYieldInstruction WaitForPosition(this Tween t, float position, bool returnCustomYieldInstruction)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }
            return new DOTweenCYInstruction.WaitForPosition(t, position);
        }
        public static CustomYieldInstruction WaitForStart(this Tween t, bool returnCustomYieldInstruction)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }
            return new DOTweenCYInstruction.WaitForStart(t);
        }

        #endregion

#if UNITY_2018_1_OR_NEWER
        #region Unity 2018.1 or Newer

        #region Material
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOOffset(this Material target, Vector2 endValue, int propertyID, float duration)
        {
            if (!target.HasProperty(propertyID)) {
                if (Debugger.logPriority > 0) Debugger.LogMissingMaterialProperty(propertyID);
                return null;
            }
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => target.GetTextureOffset(propertyID), x => target.SetTextureOffset(propertyID, x), endValue, duration);
            t.SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOTiling(this Material target, Vector2 endValue, int propertyID, float duration)
        {
            if (!target.HasProperty(propertyID)) {
                if (Debugger.logPriority > 0) Debugger.LogMissingMaterialProperty(propertyID);
                return null;
            }
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => target.GetTextureScale(propertyID), x => target.SetTextureScale(propertyID, x), endValue, duration);
            t.SetTarget(target);
            return t;
        }

        #endregion

        #region .NET 4.6 or Newer

#if UNITY_2018_1_OR_NEWER && (NET_4_6 || NET_STANDARD_2_0)

        #region Async Instructions
        public static async System.Threading.Tasks.Task AsyncWaitForCompletion(this Tween t)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }
            while (t.active && !t.IsComplete()) await System.Threading.Tasks.Task.Yield();
        }
        public static async System.Threading.Tasks.Task AsyncWaitForRewind(this Tween t)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }
            while (t.active && (!t.playedOnce || t.position * (t.CompletedLoops() + 1) > 0)) await System.Threading.Tasks.Task.Yield();
        }
        public static async System.Threading.Tasks.Task AsyncWaitForKill(this Tween t)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }
            while (t.active) await System.Threading.Tasks.Task.Yield();
        }
        public static async System.Threading.Tasks.Task AsyncWaitForElapsedLoops(this Tween t, int elapsedLoops)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }
            while (t.active && t.CompletedLoops() < elapsedLoops) await System.Threading.Tasks.Task.Yield();
        }
        public static async System.Threading.Tasks.Task AsyncWaitForPosition(this Tween t, float position)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }
            while (t.active && t.position * (t.CompletedLoops() + 1) < position) await System.Threading.Tasks.Task.Yield();
        }
        public static async System.Threading.Tasks.Task AsyncWaitForStart(this Tween t)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }
            while (t.active && !t.playedOnce) await System.Threading.Tasks.Task.Yield();
        }

        #endregion
#endif

        #endregion

        #endregion
#endif
    }

    public static class DOTweenCYInstruction
    {
        public class WaitForCompletion : CustomYieldInstruction
        {
            public override bool keepWaiting { get {
                return t.active && !t.IsComplete();
            }}
            readonly Tween t;
            public WaitForCompletion(Tween tween)
            {
                t = tween;
            }
        }

        public class WaitForRewind : CustomYieldInstruction
        {
            public override bool keepWaiting { get {
                return t.active && (!t.playedOnce || t.position * (t.CompletedLoops() + 1) > 0);
            }}
            readonly Tween t;
            public WaitForRewind(Tween tween)
            {
                t = tween;
            }
        }

        public class WaitForKill : CustomYieldInstruction
        {
            public override bool keepWaiting { get {
                return t.active;
            }}
            readonly Tween t;
            public WaitForKill(Tween tween)
            {
                t = tween;
            }
        }

        public class WaitForElapsedLoops : CustomYieldInstruction
        {
            public override bool keepWaiting { get {
                return t.active && t.CompletedLoops() < elapsedLoops;
            }}
            readonly Tween t;
            readonly int elapsedLoops;
            public WaitForElapsedLoops(Tween tween, int elapsedLoops)
            {
                t = tween;
                this.elapsedLoops = elapsedLoops;
            }
        }

        public class WaitForPosition : CustomYieldInstruction
        {
            public override bool keepWaiting { get {
                return t.active && t.position * (t.CompletedLoops() + 1) < position;
            }}
            readonly Tween t;
            readonly float position;
            public WaitForPosition(Tween tween, float position)
            {
                t = tween;
                this.position = position;
            }
        }

        public class WaitForStart : CustomYieldInstruction
        {
            public override bool keepWaiting { get {
                return t.active && !t.playedOnce;
            }}
            readonly Tween t;
            public WaitForStart(Tween tween)
            {
                t = tween;
            }
        }
    }
}
