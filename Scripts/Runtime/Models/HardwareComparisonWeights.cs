using UnityEngine;

namespace AutoQuality.Scripts.Runtime.Models
{
    // Class for storing the weighting information for different parameters
		[System.Serializable]
		public class HardwareComparisonWeights
		{
			[HideInInspector]
			public bool initialized = false;

			[HeaderAttribute("Device Type Weights")]
			// Weights for device type
			[Range(0.0f, 1.0f)]
			[TooltipAttribute("Weight for the Desktop device type (which includes Laptops and some Tablets). Points are set to 100 * this value.")]
			public float desktopWeight = 1.0f;
			[Range(0.0f, 1.0f)]
			[TooltipAttribute("Weight for the Console device type. Points are set to 100 * this value.")]
			public float consoleWeight = 1.0f;
			[Range(0.0f, 1.0f)]
			[TooltipAttribute("Weight for the Console device type. Points are set to 100 * this value.")]
			public float handheldWeight = 0.7f;

			//[SpaceAttribute(10)]
			//[HeaderAttribute("Main Property Weights")]
			// Weights for main categories - CPU, GPU
			//[Range(0.0f, 1.0f)]
			//public float CPUWeight = 0.5f;
			//[Range(0.0f, 1.0f)]
			//public float GPUWeight = 0.5f;
			// Weight for RAM
			//[Range(0.0f, 1.0f)]
			//public float RAMWeight = 0.25f;
			[SpaceAttribute(10)]
			[Range(1,8)]
			[TooltipAttribute("If the User's Processor has more than this many logical cores, the additional ones do not factor into the score. \nThis is helpful since Unity primarily stresses only one thread.")]
			public int ignoreCoresAbove = 2;

			[SpaceAttribute(10)]
			[HeaderAttribute("Penalties")]
			[Range(0.0f, 1.0f)]
			[TooltipAttribute("Penalty for failing to support GPU multithreading. GPU score -= (GPU score * this value).")]
			public float gpuMultithreadPenalty = 0.5f;
			// Weight for Compute Shaders
			[Range(0.0f, 1.0f)]
			[TooltipAttribute("Penalty for failing to support Compute Shaders. Score -= (score * this value).")]
			public float computePenalty = 0.5f;
			// Weight for Image Effects
			[Range(0.0f, 1.0f)]
			[TooltipAttribute("Penalty for failing to support Image Effects. Score -= (score * this value).")]
			public float imageEffectPenalty = 0.25f;
			// Weight for Shadow support
			[Range(0.0f, 1.0f)]
			[TooltipAttribute("Penalty for failing to support Shadows. Score -= (score * this value).")]
			public float shadowPenalty = 0.9f;

			public HardwareComparisonWeights(){
				initialized = true;
			}

		}
}