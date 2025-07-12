
#if true // MODULE_MARKER
using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening
{
	public static class DOTweenModulePhysics2D
    {
        #region Shortcuts

        #region Rigidbody2D Shortcuts
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOMove(this Rigidbody2D target, Vector2 endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => target.position, target.MovePosition, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOMoveX(this Rigidbody2D target, float endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => target.position, target.MovePosition, new Vector2(endValue, 0), duration);
            t.SetOptions(AxisConstraint.X, snapping).SetTarget(target);
            return t;
        }
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOMoveY(this Rigidbody2D target, float endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => target.position, target.MovePosition, new Vector2(0, endValue), duration);
            t.SetOptions(AxisConstraint.Y, snapping).SetTarget(target);
            return t;
        }
        public static TweenerCore<float, float, FloatOptions> DORotate(this Rigidbody2D target, float endValue, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.rotation, target.MoveRotation, endValue, duration);
            t.SetTarget(target);
            return t;
        }

        #region Special
        public static Sequence DOJump(this Rigidbody2D target, Vector2 endValue, float jumpPower, int numJumps, float duration, bool snapping = false)
        {
            if (numJumps < 1) numJumps = 1;
            float startPosY = 0;
            float offsetY = -1;
            bool offsetYSet = false;
            Sequence s = DOTween.Sequence();
            Tween yTween = DOTween.To(() => target.position, x => target.position = x, new Vector2(0, jumpPower), duration / (numJumps * 2))
                .SetOptions(AxisConstraint.Y, snapping).SetEase(Ease.OutQuad).SetRelative()
                .SetLoops(numJumps * 2, LoopType.Yoyo)
                .OnStart(() => startPosY = target.position.y);
            s.Append(DOTween.To(() => target.position, x => target.position = x, new Vector2(endValue.x, 0), duration)
                    .SetOptions(AxisConstraint.X, snapping).SetEase(Ease.Linear)
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
            this Rigidbody2D target, Vector2[] path, float duration, PathType pathType = PathType.Linear,
            PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null
        )
        {
            if (resolution < 1) resolution = 1;
            int len = path.Length;
            Vector3[] path3D = new Vector3[len];
            for (int i = 0; i < len; ++i) path3D[i] = path[i];
            TweenerCore<Vector3, Path, PathOptions> t = DOTween.To(PathPlugin.Get(), () => target.position, x => target.MovePosition(x), new Path(pathType, path3D, resolution, gizmoColor), duration)
                .SetTarget(target).SetUpdate(UpdateType.Fixed);

            t.plugOptions.isRigidbody2D = true;
            t.plugOptions.mode = pathMode;
            return t;
        }
        public static TweenerCore<Vector3, Path, PathOptions> DOLocalPath(
            this Rigidbody2D target, Vector2[] path, float duration, PathType pathType = PathType.Linear,
            PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null
        )
        {
            if (resolution < 1) resolution = 1;
            int len = path.Length;
            Vector3[] path3D = new Vector3[len];
            for (int i = 0; i < len; ++i) path3D[i] = path[i];
            Transform trans = target.transform;
            TweenerCore<Vector3, Path, PathOptions> t = DOTween.To(PathPlugin.Get(), () => trans.localPosition, x => target.MovePosition(trans.parent == null ? x : trans.parent.TransformPoint(x)), new Path(pathType, path3D, resolution, gizmoColor), duration)
                .SetTarget(target).SetUpdate(UpdateType.Fixed);

            t.plugOptions.isRigidbody2D = true;
            t.plugOptions.mode = pathMode;
            t.plugOptions.useLocalPosition = true;
            return t;
        }
        internal static TweenerCore<Vector3, Path, PathOptions> DOPath(
            this Rigidbody2D target, Path path, float duration, PathMode pathMode = PathMode.Full3D
        )
        {
            TweenerCore<Vector3, Path, PathOptions> t = DOTween.To(PathPlugin.Get(), () => target.position, x => target.MovePosition(x), path, duration)
                .SetTarget(target);

            t.plugOptions.isRigidbody2D = true;
            t.plugOptions.mode = pathMode;
            return t;
        }
        internal static TweenerCore<Vector3, Path, PathOptions> DOLocalPath(
            this Rigidbody2D target, Path path, float duration, PathMode pathMode = PathMode.Full3D
        )
        {
            Transform trans = target.transform;
            TweenerCore<Vector3, Path, PathOptions> t = DOTween.To(PathPlugin.Get(), () => trans.localPosition, x => target.MovePosition(trans.parent == null ? x : trans.parent.TransformPoint(x)), path, duration)
                .SetTarget(target);

            t.plugOptions.isRigidbody2D = true;
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
