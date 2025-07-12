
using System;
using System.Reflection;
using UnityEngine;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;

#pragma warning disable 1591
namespace DG.Tweening
{
	public static class DOTweenModuleUtils
    {
        static bool _initialized;

        #region Reflection
#if UNITY_2018_1_OR_NEWER
        [UnityEngine.Scripting.Preserve]
#endif
        public static void Init()
        {
            if (_initialized) return;

            _initialized = true;
            DOTweenExternalCommand.SetOrientationOnPath += Physics.SetOrientationOnPath;

#if UNITY_EDITOR
#if UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_2017_1
            UnityEditor.EditorApplication.playmodeStateChanged += PlaymodeStateChanged;
#else
            UnityEditor.EditorApplication.playModeStateChanged += PlaymodeStateChanged;
#endif
#endif
        }

#if UNITY_2018_1_OR_NEWER
#pragma warning disable
        [UnityEngine.Scripting.Preserve]
        static void Preserver()
        {
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            MethodInfo mi = typeof(MonoBehaviour).GetMethod("Stub");
        }
#pragma warning restore
#endif

        #endregion

#if UNITY_EDITOR
#if UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_2017_1
        static void PlaymodeStateChanged()
        #else
        static void PlaymodeStateChanged(UnityEditor.PlayModeStateChange state)
#endif
        {
            if (DOTween.instance == null) return;
            DOTween.instance.OnApplicationPause(UnityEditor.EditorApplication.isPaused);
        }
#endif

        public static class Physics
        {
            public static void SetOrientationOnPath(PathOptions options, Tween t, Quaternion newRot, Transform trans)
            {
#if true // PHYSICS_MARKER
                if (options.isRigidbody) ((Rigidbody)t.target).rotation = newRot;
                else trans.rotation = newRot;
#else
                trans.rotation = newRot;
#endif
            }
            public static bool HasRigidbody2D(Component target)
            {
#if true // PHYSICS2D_MARKER
                return target.GetComponent<Rigidbody2D>() != null;
#else
                return false;
#endif
            }

            #region Called via Reflection
#if UNITY_2018_1_OR_NEWER
            [UnityEngine.Scripting.Preserve]
#endif
            public static bool HasRigidbody(Component target)
            {
#if true // PHYSICS_MARKER
                return target.GetComponent<Rigidbody>() != null;
#else
                return false;
#endif
            }
#if UNITY_2018_1_OR_NEWER
            [UnityEngine.Scripting.Preserve]
#endif
            public static TweenerCore<Vector3, Path, PathOptions> CreateDOTweenPathTween(
                MonoBehaviour target, bool tweenRigidbody, bool isLocal, Path path, float duration, PathMode pathMode
            ){
                TweenerCore<Vector3, Path, PathOptions> t = null;
                bool rBodyFoundAndTweened = false;
#if true // PHYSICS_MARKER
                if (tweenRigidbody) {
                    Rigidbody rBody = target.GetComponent<Rigidbody>();
                    if (rBody != null) {
                        rBodyFoundAndTweened = true;
                        t = isLocal
                            ? rBody.DOLocalPath(path, duration, pathMode)
                            : rBody.DOPath(path, duration, pathMode);
                    }
                }
#endif
#if true // PHYSICS2D_MARKER
                if (!rBodyFoundAndTweened && tweenRigidbody) {
                    Rigidbody2D rBody2D = target.GetComponent<Rigidbody2D>();
                    if (rBody2D != null) {
                        rBodyFoundAndTweened = true;
                        t = isLocal
                            ? rBody2D.DOLocalPath(path, duration, pathMode)
                            : rBody2D.DOPath(path, duration, pathMode);
                    }
                }
#endif
                if (!rBodyFoundAndTweened) {
                    t = isLocal
                        ? target.transform.DOLocalPath(path, duration, pathMode)
                        : target.transform.DOPath(path, duration, pathMode);
                }
                return t;
            }

            #endregion
        }
    }
}
