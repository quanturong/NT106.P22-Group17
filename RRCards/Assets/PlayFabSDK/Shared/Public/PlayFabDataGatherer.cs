using UnityEngine;
using System.Text;
using PlayFab.SharedModels;
using UnityEngine.Rendering;
#if NETFX_CORE
using System.Reflection;
#endif

namespace PlayFab
{
    public class PlayFabDataGatherer
    {
#if UNITY_5 || UNITY_5_3_OR_NEWER
        public string ProductName;
        public string ProductBundle;
        public string Version;
        public string Company;
        public RuntimePlatform Platform;
        public bool GraphicsMultiThreaded;
#else
        public enum GraphicsDeviceType
        {
            OpenGL2 = 0, Direct3D9 = 1, Direct3D11 = 2, PlayStation3 = 3, Null = 4, Xbox360 = 6, OpenGLES2 = 8, OpenGLES3 = 11, PlayStationVita = 12,
            PlayStation4 = 13, XboxOne = 14, PlayStationMobile = 15, Metal = 16, OpenGLCore = 17, Direct3D12 = 18, Nintendo3DS = 19
        }
#endif
#if !UNITY_5_0 && (UNITY_5 || UNITY_5_3_OR_NEWER)
        public GraphicsDeviceType GraphicsType;
#endif
        public string DataPath;
        public string PersistentDataPath;
        public string StreamingAssetsPath;
        public int TargetFrameRate;
        public string UnityVersion;
        public bool RunInBackground;
        public string DeviceModel;
        public DeviceType DeviceType;
        public string DeviceUniqueId;
        public string OperatingSystem;
        public int GraphicsDeviceId;
        public string GraphicsDeviceName;
        public int GraphicsMemorySize;
        public int GraphicsShaderLevel;
        public int SystemMemorySize;
        public int ProcessorCount;
        public int ProcessorFrequency;
        public string ProcessorType;
        public bool SupportsAccelerometer;
        public bool SupportsGyroscope;
        public bool SupportsLocationService;

        public PlayFabDataGatherer()
        {
#if UNITY_5 || UNITY_5_3_OR_NEWER
            ProductName = Application.productName;
            Version = Application.version;
            Company = Application.companyName;
            Platform = Application.platform;
            GraphicsMultiThreaded = SystemInfo.graphicsMultiThreaded;
#endif
#if !UNITY_5_0 && (UNITY_5 || UNITY_5_3_OR_NEWER)
            GraphicsType = SystemInfo.graphicsDeviceType;
#endif
#if UNITY_5_6_OR_NEWER && (UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE)
            ProductBundle = Application.identifier;
#elif UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
            ProductBundle = Application.bundleIdentifier;
#endif
            DataPath = Application.dataPath;
#if !UNITY_SWITCH
            PersistentDataPath = Application.persistentDataPath;
#endif
            StreamingAssetsPath = Application.streamingAssetsPath;
            TargetFrameRate = Application.targetFrameRate;
            UnityVersion = Application.unityVersion;
            DeviceModel = SystemInfo.deviceModel;
            DeviceType = SystemInfo.deviceType;

            DeviceUniqueId = PlayFabSettings.DeviceUniqueIdentifier;
            OperatingSystem = SystemInfo.operatingSystem;
            GraphicsDeviceId = SystemInfo.graphicsDeviceID;
            GraphicsDeviceName = SystemInfo.graphicsDeviceName;
            GraphicsMemorySize = SystemInfo.graphicsMemorySize;
            GraphicsShaderLevel = SystemInfo.graphicsShaderLevel;
            SystemMemorySize = SystemInfo.systemMemorySize;
            ProcessorCount = SystemInfo.processorCount;
#if UNITY_5_3_OR_NEWER
            ProcessorFrequency = SystemInfo.processorFrequency; // Not Supported in PRE Unity 5_2
#endif
            ProcessorType = SystemInfo.processorType;
            SupportsAccelerometer = SystemInfo.supportsAccelerometer;
            SupportsGyroscope = SystemInfo.supportsGyroscope;
            SupportsLocationService = SystemInfo.supportsLocationService;
        }

        public string GenerateReport()
        {
            var sb = new StringBuilder();
            sb.Append("Logging System Info: ========================================\n");
            foreach (var field in GetType().GetTypeInfo().GetFields())
            {
                var fld = field.GetValue(this).ToString();
                sb.AppendFormat("System Info - {0}: {1}\n", field.Name, fld);
            }
            return sb.ToString();
        }
    }
}
