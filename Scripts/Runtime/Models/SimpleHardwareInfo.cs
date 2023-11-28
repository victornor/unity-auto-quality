using UnityEngine;

namespace AutoQuality.Scripts.Runtime.Models
{
    // Class for holding simple system info used for comparison
	[System.Serializable]
	public class SimpleHardwareInfo{
		[HideInInspector]
		public bool initialized = false;

		// SIMPLE HARDWRE PARAMETERS
		[TooltipAttribute("Device type. Options are 'Desktop' (for Laptop, Tablet, or Desktop), 'Console', 'Handheld', 'Unknown'.")]
		public DeviceType deviceType = DeviceType.Unknown;
		[TooltipAttribute("GPU Rendering API information. See Rendering.GraphicsDeviceType documentation for details.")]
		public UnityEngine.Rendering.GraphicsDeviceType gpuDeviceType = UnityEngine.Rendering.GraphicsDeviceType.Null;

		[SpaceAttribute(10)]

		[TooltipAttribute("Graphics memory (VRAM), in MB. A value of -1 indicates an error.")]
		public int gpuMemory = -1;
		[TooltipAttribute("Graphics ability to multithread. Default is false.")]
		public bool gpuMultiThread = false;
		[TooltipAttribute("Graphics Shader Level. See SystemInfo.GraphicsShaderLevel documentation for details. A value of -1 indicates an error.")]
		public int gpuShaderLevel = -1;
		[TooltipAttribute("Maximum texture size supported by the GPU, in pixels. A value of -1 indicates an error.")]
		public int maxTextureSize = -1;

		[SpaceAttribute(10)]

		[TooltipAttribute("System memory (RAM), in MB. A value of -1 indicates an error.")]
		public int systemMemory = -1;
		[TooltipAttribute("Logical processor threads. A value of -1 indicates an error.")]
		public int processorCount = -1;
		[TooltipAttribute("Processor speed, in MHz. A value of -1 indicates an error.")]
		public int processorFrequency = -1;
		[TooltipAttribute("Does the system support GPU Compute Shaders? Defaults to false.")]

		[SpaceAttribute(10)]

		public bool supportsComputeShaders = false;
		[TooltipAttribute("Does the system support Image Effects and GPU-accelerated Post Processing? Defaults to false.")]
		public bool supportsImageEffects = false;
		[TooltipAttribute("Does the system support shadows? Defaults to false.")]
		public bool supportsShadows = false;

		[SpaceAttribute(10)]
		[Range(1.0f,3.5f)]
		[TooltipAttribute("SLI cannot be detected by Unity, so this number will multiply the GPU score to 'approximate' effect of SLI. Default is 1.0. \nSLI scaling is non-linear and varies by GPU, but a safe number for a dual-card system would be about 1.6.")]
		public float SLIScalar = 1.0f;

		public SimpleHardwareInfo(){
			initialized = true;
		}
		
		// Set the hardwareInfo using the current system config
		public void SetFromCurrentConfig(){
			deviceType = SystemInfo.deviceType;
			gpuDeviceType = SystemInfo.graphicsDeviceType;
			gpuMemory = SystemInfo.graphicsMemorySize;
			gpuMultiThread = SystemInfo.graphicsMultiThreaded;
			gpuShaderLevel = SystemInfo.graphicsShaderLevel;
			maxTextureSize = SystemInfo.maxTextureSize;
			processorCount = SystemInfo.processorCount;
			processorFrequency = SystemInfo.processorFrequency;
			supportsComputeShaders = SystemInfo.supportsComputeShaders;
			supportsImageEffects = SystemInfo.supportsImageEffects;
			supportsShadows = SystemInfo.supportsShadows;
			systemMemory = SystemInfo.systemMemorySize;
		}

		// Clear the current hardwareInfo
		public void Clear(){
			deviceType = DeviceType.Unknown;
			gpuDeviceType = UnityEngine.Rendering.GraphicsDeviceType.Null;
			gpuMemory = -1;
			gpuMultiThread = false;
			gpuShaderLevel = -1;
			maxTextureSize = -1;
			processorCount = -1;
			processorFrequency = -1;
			supportsComputeShaders = false;
			supportsImageEffects = false;
			supportsShadows = false;
			systemMemory = -1;

			SLIScalar = 1.0f;

			initialized = false;
		}
	}
}