
#if true // MODULE_MARKER

using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Options;
using Outline = UnityEngine.UI.Outline;
using Text = UnityEngine.UI.Text;

#pragma warning disable 1591
namespace DG.Tweening
{
	public static class DOTweenModuleUI
    {
        #region Shortcuts

        #region CanvasGroup
        public static TweenerCore<float, float, FloatOptions> DOFade(this CanvasGroup target, float endValue, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.alpha, x => target.alpha = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }

        #endregion

        #region Graphic
        public static TweenerCore<Color, Color, ColorOptions> DOColor(this Graphic target, Color endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> t = DOTween.To(() => target.color, x => target.color = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        public static TweenerCore<Color, Color, ColorOptions> DOFade(this Graphic target, float endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> t = DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }

        #endregion

        #region Image
        public static TweenerCore<Color, Color, ColorOptions> DOColor(this Image target, Color endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> t = DOTween.To(() => target.color, x => target.color = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        public static TweenerCore<Color, Color, ColorOptions> DOFade(this Image target, float endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> t = DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        public static TweenerCore<float, float, FloatOptions> DOFillAmount(this Image target, float endValue, float duration)
        {
            if (endValue > 1) endValue = 1;
            else if (endValue < 0) endValue = 0;
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.fillAmount, x => target.fillAmount = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        public static Sequence DOGradientColor(this Image target, Gradient gradient, float duration)
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

        #endregion

        #region LayoutElement
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOFlexibleSize(this LayoutElement target, Vector2 endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => new Vector2(target.flexibleWidth, target.flexibleHeight), x => {
                    target.flexibleWidth = x.x;
                    target.flexibleHeight = x.y;
                }, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOMinSize(this LayoutElement target, Vector2 endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => new Vector2(target.minWidth, target.minHeight), x => {
                target.minWidth = x.x;
                target.minHeight = x.y;
            }, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOPreferredSize(this LayoutElement target, Vector2 endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => new Vector2(target.preferredWidth, target.preferredHeight), x => {
                target.preferredWidth = x.x;
                target.preferredHeight = x.y;
            }, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }

        #endregion

        #region Outline
        public static TweenerCore<Color, Color, ColorOptions> DOColor(this Outline target, Color endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> t = DOTween.To(() => target.effectColor, x => target.effectColor = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        public static TweenerCore<Color, Color, ColorOptions> DOFade(this Outline target, float endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> t = DOTween.ToAlpha(() => target.effectColor, x => target.effectColor = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOScale(this Outline target, Vector2 endValue, float duration)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => target.effectDistance, x => target.effectDistance = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }

        #endregion

        #region RectTransform
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOAnchorPos(this RectTransform target, Vector2 endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => target.anchoredPosition, x => target.anchoredPosition = x, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOAnchorPosX(this RectTransform target, float endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => target.anchoredPosition, x => target.anchoredPosition = x, new Vector2(endValue, 0), duration);
            t.SetOptions(AxisConstraint.X, snapping).SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOAnchorPosY(this RectTransform target, float endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => target.anchoredPosition, x => target.anchoredPosition = x, new Vector2(0, endValue), duration);
            t.SetOptions(AxisConstraint.Y, snapping).SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOAnchorPos3D(this RectTransform target, Vector3 endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> t = DOTween.To(() => target.anchoredPosition3D, x => target.anchoredPosition3D = x, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOAnchorPos3DX(this RectTransform target, float endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> t = DOTween.To(() => target.anchoredPosition3D, x => target.anchoredPosition3D = x, new Vector3(endValue, 0, 0), duration);
            t.SetOptions(AxisConstraint.X, snapping).SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOAnchorPos3DY(this RectTransform target, float endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> t = DOTween.To(() => target.anchoredPosition3D, x => target.anchoredPosition3D = x, new Vector3(0, endValue, 0), duration);
            t.SetOptions(AxisConstraint.Y, snapping).SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOAnchorPos3DZ(this RectTransform target, float endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> t = DOTween.To(() => target.anchoredPosition3D, x => target.anchoredPosition3D = x, new Vector3(0, 0, endValue), duration);
            t.SetOptions(AxisConstraint.Z, snapping).SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOAnchorMax(this RectTransform target, Vector2 endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => target.anchorMax, x => target.anchorMax = x, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOAnchorMin(this RectTransform target, Vector2 endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => target.anchorMin, x => target.anchorMin = x, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOPivot(this RectTransform target, Vector2 endValue, float duration)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => target.pivot, x => target.pivot = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOPivotX(this RectTransform target, float endValue, float duration)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => target.pivot, x => target.pivot = x, new Vector2(endValue, 0), duration);
            t.SetOptions(AxisConstraint.X).SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOPivotY(this RectTransform target, float endValue, float duration)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => target.pivot, x => target.pivot = x, new Vector2(0, endValue), duration);
            t.SetOptions(AxisConstraint.Y).SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOSizeDelta(this RectTransform target, Vector2 endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => target.sizeDelta, x => target.sizeDelta = x, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }
        public static Tweener DOPunchAnchorPos(this RectTransform target, Vector2 punch, float duration, int vibrato = 10, float elasticity = 1, bool snapping = false)
        {
            return DOTween.Punch(() => target.anchoredPosition, x => target.anchoredPosition = x, punch, duration, vibrato, elasticity)
                .SetTarget(target).SetOptions(snapping);
        }
        public static Tweener DOShakeAnchorPos(this RectTransform target, float duration, float strength = 100, int vibrato = 10, float randomness = 90, bool snapping = false, bool fadeOut = true, ShakeRandomnessMode randomnessMode = ShakeRandomnessMode.Full)
        {
            return DOTween.Shake(() => target.anchoredPosition, x => target.anchoredPosition = x, duration, strength, vibrato, randomness, true, fadeOut, randomnessMode)
                .SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake).SetOptions(snapping);
        }
        public static Tweener DOShakeAnchorPos(this RectTransform target, float duration, Vector2 strength, int vibrato = 10, float randomness = 90, bool snapping = false, bool fadeOut = true, ShakeRandomnessMode randomnessMode = ShakeRandomnessMode.Full)
        {
            return DOTween.Shake(() => target.anchoredPosition, x => target.anchoredPosition = x, duration, strength, vibrato, randomness, fadeOut, randomnessMode)
                .SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake).SetOptions(snapping);
        }

        #region Special
        public static Sequence DOJumpAnchorPos(this RectTransform target, Vector2 endValue, float jumpPower, int numJumps, float duration, bool snapping = false)
        {
            if (numJumps < 1) numJumps = 1;
            float startPosY = 0;
            float offsetY = -1;
            bool offsetYSet = false;
            Sequence s = DOTween.Sequence();
            Tween yTween = DOTween.To(() => target.anchoredPosition, x => target.anchoredPosition = x, new Vector2(0, jumpPower), duration / (numJumps * 2))
                .SetOptions(AxisConstraint.Y, snapping).SetEase(Ease.OutQuad).SetRelative()
                .SetLoops(numJumps * 2, LoopType.Yoyo)
                .OnStart(()=> startPosY = target.anchoredPosition.y);
            s.Append(DOTween.To(() => target.anchoredPosition, x => target.anchoredPosition = x, new Vector2(endValue.x, 0), duration)
                    .SetOptions(AxisConstraint.X, snapping).SetEase(Ease.Linear)
                ).Join(yTween)
                .SetTarget(target).SetEase(DOTween.defaultEaseType);
            s.OnUpdate(() => {
                if (!offsetYSet) {
                    offsetYSet = true;
                    offsetY = s.isRelative ? endValue.y : endValue.y - startPosY;
                }
                Vector2 pos = target.anchoredPosition;
                pos.y += DOVirtual.EasedValue(0, offsetY, s.ElapsedDirectionalPercentage(), Ease.OutQuad);
                target.anchoredPosition = pos;
            });
            return s;
        }

        #endregion

        #endregion

        #region ScrollRect
        public static Tweener DONormalizedPos(this ScrollRect target, Vector2 endValue, float duration, bool snapping = false)
        {
            return DOTween.To(() => new Vector2(target.horizontalNormalizedPosition, target.verticalNormalizedPosition),
                x => {
                    target.horizontalNormalizedPosition = x.x;
                    target.verticalNormalizedPosition = x.y;
                }, endValue, duration)
                .SetOptions(snapping).SetTarget(target);
        }
        public static Tweener DOHorizontalNormalizedPos(this ScrollRect target, float endValue, float duration, bool snapping = false)
        {
            return DOTween.To(() => target.horizontalNormalizedPosition, x => target.horizontalNormalizedPosition = x, endValue, duration)
                .SetOptions(snapping).SetTarget(target);
        }
        public static Tweener DOVerticalNormalizedPos(this ScrollRect target, float endValue, float duration, bool snapping = false)
        {
            return DOTween.To(() => target.verticalNormalizedPosition, x => target.verticalNormalizedPosition = x, endValue, duration)
                .SetOptions(snapping).SetTarget(target);
        }

        #endregion

        #region Slider
        public static TweenerCore<float, float, FloatOptions> DOValue(this Slider target, float endValue, float duration, bool snapping = false)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.value, x => target.value = x, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }

        #endregion

        #region Text
        public static TweenerCore<Color, Color, ColorOptions> DOColor(this Text target, Color endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> t = DOTween.To(() => target.color, x => target.color = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        public static TweenerCore<int, int, NoOptions> DOCounter(
            this Text target, int fromValue, int endValue, float duration, bool addThousandsSeparator = true, CultureInfo culture = null
        ){
            int v = fromValue;
            CultureInfo cInfo = !addThousandsSeparator ? null : culture ?? CultureInfo.InvariantCulture;
            TweenerCore<int, int, NoOptions> t = DOTween.To(() => v, x => {
                v = x;
                target.text = addThousandsSeparator
                    ? v.ToString("N0", cInfo)
                    : v.ToString();
            }, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        public static TweenerCore<Color, Color, ColorOptions> DOFade(this Text target, float endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> t = DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        public static TweenerCore<string, string, StringOptions> DOText(this Text target, string endValue, float duration, bool richTextEnabled = true, ScrambleMode scrambleMode = ScrambleMode.None, string scrambleChars = null)
        {
            if (endValue == null) {
                if (Debugger.logPriority > 0) Debugger.LogWarning("You can't pass a NULL string to DOText: an empty string will be used instead to avoid errors");
                endValue = "";
            }
            TweenerCore<string, string, StringOptions> t = DOTween.To(() => target.text, x => target.text = x, endValue, duration);
            t.SetOptions(richTextEnabled, scrambleMode, scrambleChars)
                .SetTarget(target);
            return t;
        }

        #endregion

        #region Blendables

        #region Graphic
        public static Tweener DOBlendableColor(this Graphic target, Color endValue, float duration)
        {
            endValue = endValue - target.color;
            Color to = new Color(0, 0, 0, 0);
            return DOTween.To(() => to, x => {
                Color diff = x - to;
                to = x;
                target.color += diff;
            }, endValue, duration)
                .Blendable().SetTarget(target);
        }

        #endregion

        #region Image
        public static Tweener DOBlendableColor(this Image target, Color endValue, float duration)
        {
            endValue = endValue - target.color;
            Color to = new Color(0, 0, 0, 0);
            return DOTween.To(() => to, x => {
                Color diff = x - to;
                to = x;
                target.color += diff;
            }, endValue, duration)
                .Blendable().SetTarget(target);
        }

        #endregion

        #region Text
        public static Tweener DOBlendableColor(this Text target, Color endValue, float duration)
        {
            endValue = endValue - target.color;
            Color to = new Color(0, 0, 0, 0);
            return DOTween.To(() => to, x => {
                Color diff = x - to;
                to = x;
                target.color += diff;
            }, endValue, duration)
                .Blendable().SetTarget(target);
        }

        #endregion

        #endregion

        #region Shapes
        public static TweenerCore<Vector2, Vector2, CircleOptions> DOShapeCircle(
            this RectTransform target, Vector2 center, float endValueDegrees, float duration, bool relativeCenter = false, bool snapping = false
        )
        {
            TweenerCore<Vector2, Vector2, CircleOptions> t = DOTween.To(
                CirclePlugin.Get(), () => target.anchoredPosition, x => target.anchoredPosition = x, center, duration
            );
            t.SetOptions(endValueDegrees, relativeCenter, snapping).SetTarget(target);
            return t;
        }

        #endregion

        #endregion

        public static class Utils
        {
            public static Vector2 SwitchToRectTransform(RectTransform from, RectTransform to)
            {
                Vector2 localPoint;
                Vector2 fromPivotDerivedOffset = new Vector2(from.rect.width * 0.5f + from.rect.xMin, from.rect.height * 0.5f + from.rect.yMin);
                Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, from.position);
                screenP += fromPivotDerivedOffset;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(to, screenP, null, out localPoint);
                Vector2 pivotDerivedOffset = new Vector2(to.rect.width * 0.5f + to.rect.xMin, to.rect.height * 0.5f + to.rect.yMin);
                return to.anchoredPosition + localPoint - pivotDerivedOffset;
            }
        }
	}
}
#endif
