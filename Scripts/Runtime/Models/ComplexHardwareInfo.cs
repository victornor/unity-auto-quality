using UnityEngine;

namespace AutoQuality.Scripts.Runtime.Models
{
    // Class for holding complex system info - all properties including 'useless' stuff
		[System.Serializable]
		public class ComplexHardwareInfo{
			[HideInInspector]
			public bool initialized = false;

			public string deviceModel = "";
			public string deviceName = "";
			public DeviceType deviceType = DeviceType.Unknown;
			public string deviceID = "";
			public int gpuDeviceID = -1;
			public string gpuDeviceName = "";
			public UnityEngine.Rendering.GraphicsDeviceType gpuDeviceType = UnityEngine.Rendering.GraphicsDeviceType.Null;
			public string gpuDeviceVendor = "";
			public int gpuDeviceVendorID = -1;
			public string gpuDeviceVersion = "";
			public int gpuMemory = -1;
			public bool gpuMultiThread = false;
			public int gpuShaderLevel = -1;
			public int maxTextureSize = -1;
			public NPOTSupport npotSupport = NPOTSupport.None;
			public string operatingSystem = "";
			// public UnityEngine.OperatingSystemFamily operatingSystemFamily = UnityEngine.OperatingSystemFamily.Other;
			public int processorCount = -1;
			public int processorFrequency = -1;
			public string processorType = "";
			public int supportedRenderTargetCount = -1;
			public bool supports2DArrayTextures = false;
			public bool supports3DTextures = false;
			public bool supportsAudio = false;
			public bool supportsComputeShaders = false;
			// public bool supportsCubemapArrayTextures = false;
			public bool supportsGyroscope = false;
			public bool supportsImageEffects = false;
			public bool supportsInstancing = false;
			public bool supportsLocationService = false;
			public bool supportsMotionVectors = false;
			public bool supportsRawShadowDepthSampling = false;
			public bool supportsRenderToCubemap = false;
			public bool supportsShadows = false;
			public bool supportsSparseTextures = false;
			public bool supportsVibration = false;
			// public bool usesReversedZBuffer = false;
			public int systemMemory = -1;

			public ComplexHardwareInfo(){
				initialized = true;
			}

			// Set the hardwareInfo using the current system config
			public void SetFromCurrentConfig(){
				deviceModel = SystemInfo.deviceModel;
				deviceName = SystemInfo.deviceName;
				deviceType = SystemInfo.deviceType;
				deviceID = SystemInfo.deviceUniqueIdentifier;
				gpuDeviceID = SystemInfo.graphicsDeviceID;
				gpuDeviceName = SystemInfo.graphicsDeviceName;
				gpuDeviceType = SystemInfo.graphicsDeviceType;
				gpuDeviceVendor = SystemInfo.graphicsDeviceVendor;
				gpuDeviceVendorID = SystemInfo.graphicsDeviceVendorID;
				gpuDeviceVersion = SystemInfo.graphicsDeviceVersion;
				gpuMemory = SystemInfo.graphicsMemorySize;
				gpuMultiThread = SystemInfo.graphicsMultiThreaded;
				gpuShaderLevel = SystemInfo.graphicsShaderLevel;
				maxTextureSize = SystemInfo.maxTextureSize;
				npotSupport = SystemInfo.npotSupport;
				operatingSystem = SystemInfo.operatingSystem;
				// operatingSystemFamily = SystemInfo.operatingSystemFamily;
				processorCount = SystemInfo.processorCount;
				processorFrequency = SystemInfo.processorFrequency;
				processorType = SystemInfo.processorType;
				supportedRenderTargetCount = SystemInfo.supportedRenderTargetCount;
				supports2DArrayTextures = SystemInfo.supports2DArrayTextures;
				supports3DTextures = SystemInfo.supports3DTextures;
				supportsAudio = SystemInfo.supportsAudio;
				supportsComputeShaders = SystemInfo.supportsComputeShaders;
				//supportsCubemapArrayTextures = SystemInfo.supportsCubemapArrayTextures;
				supportsGyroscope = SystemInfo.supportsGyroscope;
				supportsImageEffects = SystemInfo.supportsImageEffects;
				supportsInstancing = SystemInfo.supportsInstancing;
				supportsLocationService = SystemInfo.supportsLocationService;
				supportsMotionVectors = SystemInfo.supportsMotionVectors;
				supportsRawShadowDepthSampling = SystemInfo.supportsRawShadowDepthSampling;
				supportsRenderToCubemap = SystemInfo.supportsRenderToCubemap;
				supportsShadows = SystemInfo.supportsShadows;
				supportsSparseTextures = SystemInfo.supportsSparseTextures;
				supportsVibration = SystemInfo.supportsVibration;
				//usesReversedZBuffer = SystemInfo.usesReversedZBuffer;
				systemMemory = SystemInfo.systemMemorySize;
			}
		}
}