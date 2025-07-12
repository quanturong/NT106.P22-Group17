
#if true // MODULE_MARKER
using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening
{
	public static class DOTweenModulePhysics
    {
        #region Shortcuts

        #region Rigidbody
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOMove(this Rigidbody target, Vector3 endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> t = DOTween.To(() => target.position, target.MovePosition, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveX(this Rigidbody target, float endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> t = DOTween.To(() => target.position, target.MovePosition, new Vector3(endValue, 0, 0), duration);
            t.SetOptions(AxisConstraint.X, snapping).SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveY(this Rigidbody target, float endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> t = DOTween.To(() => target.position, target.MovePosition, new Vector3(0, endValue, 0), duration);
            t.SetOptions(AxisConstraint.Y, snapping).SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveZ(this Rigidbody target, float endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> t = DOTween.To(() => target.position, target.MovePosition, new Vector3(0, 0, endValue), duration);
            t.SetOptions(AxisConstraint.Z, snapping).SetTarget(target);
            return t;
        }
        public static TweenerCore<Quaternion, Vector3, QuaternionOptions> DORotate(this Rigidbody target, Vector3 endValue, float duration, RotateMode mode = RotateMode.Fast)
        {
            TweenerCore<Quaternion, Vector3, QuaternionOptions> t = DOTween.To(() => target.rotation, target.MoveRotation, endValue, duration);
            t.SetTarget(target);
            t.plugOptions.rotateMode = mode;
            return t;
        }
        public static TweenerCore<Quaternion, Vector3, QuaternionOptions> DOLookAt(this Rigidbody target, Vector3 towards, float duration, AxisConstraint axisConstraint = AxisConstraint.None, Vector3? up = null)
        {
            TweenerCore<Quaternion, Vector3, QuaternionOptions> t = DOTween.To(() => target.rotation, target.MoveRotation, towards, duration)
                .SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetLookAt);
            t.plugOptions.axisConstraint = axisConstraint;
            t.plugOptions.up = (up == null) ? Vector3.up : (Vector3)up;
            return t;
        }

        #region Special
        public static Sequence DOJump(this Rigidbody target, Vector3 endValue, float jumpPower, int numJumps, float duration, bool snapping = false)
        {
            if (numJumps < 1) numJumps = 1;
            float startPosY = 0;
            float offsetY = -1;
            bool offsetYSet = false;
            Sequence s = DOTween.Sequence();
            Tween yTween = DOTween.To(() => target.position, target.MovePosition, new Vector3(0, jumpPower, 0), duration / (numJumps * 2))
                .SetOptions(AxisConstraint.Y, snapping).SetEase(Ease.OutQuad).SetRelative()
                .SetLoops(numJumps * 2, LoopType.Yoyo)
                .OnStart(() => startPosY = target.position.y);
            s.Append(DOTween.To(() => target.position, target.MovePosition, new Vector3(endValue.x, 0, 0), duration)
                    .SetOptions(AxisConstraint.X, snapping).SetEase(Ease.Linear)
                ).Join(DOTween.To(() => target.position, target.MovePosition, new Vector3(0, 0, endValue.z), duration)
                    .SetOptions(AxisConstraint.Z, snapping).SetEase(Ease.Linear)
                ).Join(yTween)
                .SetTarget(target).SetEase(DOTween.defaultEaseType);
            yTween.OnUpdate(() => {
                if (!offsetYSet) {
                    offsetYSet = true;
                    offsetY = s.isRelative ? endValue.y : endValue.y - startPosY;
                }
                Vector3 pos = target.position;
                pos.y += DOVirtual.EasedValue(0, offsetY, yTween.ElapsedPercentage(), Ease.OutQuad);
                target.MovePosition(pos);
            });
            return s;
        }
        public static TweenerCore<Vector3, Path, PathOptions> DOPath(
            this Rigidbody target, Vector3[] path, float duration, PathType pathType = PathType.Linear,
            PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null
        )
        {
            if (resolution < 1) resolution = 1;
            TweenerCore<Vector3, Path, PathOptions> t = DOTween.To(PathPlugin.Get(), () => target.position, target.MovePosition, new Path(pathType, path, resolution, gizmoColor), duration)
                .SetTarget(target).SetUpdate(UpdateType.Fixed);

            t.plugOptions.isRigidbody = true;
            t.plugOptions.mode = pathMode;
            return t;
        }
        public static TweenerCore<Vector3, Path, PathOptions> DOLocalPath(
            this Rigidbody target, Vector3[] path, float duration, PathType pathType = PathType.Linear,
            PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null
        )
        {
            if (resolution < 1) resolution = 1;
            Transform trans = target.transform;
            TweenerCore<Vector3, Path, PathOptions> t = DOTween.To(PathPlugin.Get(), () => trans.localPosition, x => target.MovePosition(trans.parent == null ? x : trans.parent.TransformPoint(x)), new Path(pathType, path, resolution, gizmoColor), duration)
                .SetTarget(target).SetUpdate(UpdateType.Fixed);

            t.plugOptions.isRigidbody = true;
            t.plugOptions.mode = pathMode;
            t.plugOptions.useLocalPosition = true;
            return t;
        }
        internal static TweenerCore<Vector3, Path, PathOptions> DOPath(
            this Rigidbody target, Path path, float duration, PathMode pathMode = PathMode.Full3D
        )
        {
            TweenerCore<Vector3, Path, PathOptions> t = DOTween.To(PathPlugin.Get(), () => target.position, target.MovePosition, path, duration)
                .SetTarget(target);

            t.plugOptions.isRigidbody = true;
            t.plugOptions.mode = pathMode;
            return t;
        }
        internal static TweenerCore<Vector3, Path, PathOptions> DOLocalPath(
            this Rigidbody target, Path path, float duration, PathMode pathMode = PathMode.Full3D
        )
        {
            Transform trans = target.transform;
            TweenerCore<Vector3, Path, PathOptions> t = DOTween.To(PathPlugin.Get(), () => trans.localPosition, x => target.MovePosition(trans.parent == null ? x : trans.parent.TransformPoint(x)), path, duration)
                .SetTarget(target);

            t.plugOptions.isRigidbody = true;
            t.plugOptions.mode = pathMode;
            t.plugOptions.useLocalPosition = true;
            return t;
        }

        #endregion

        #endregion

        #endregion
	}
}
#endif
