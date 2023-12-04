using System.Collections;
using System.Collections.Generic;
using AutoQuality.Scripts.Runtime.Models;
using UnityEngine;

	// HardwareInfo is a Singleton used to get information about the system hardware and software
	// This can then be used by other scripts to auto-set quality settings or for other convenience features

	// HardwareInfo will also compare a user's machine to a target "reference" machine (which can be defined
	// in the Inspector) and give it a comparative score based on that
	// Scores are out of 100.0; a score of 100 means the user machine is identical to the reference
	// Then, based on what graphics settings will run on the reference, graphics can be scaled on the user
	// NOTE: This comparison is a rough approximation only, and should not be taken as a truly accurate
	// performance benchmark of any sort
	public class HardwareInfo : MonoBehaviour 
	{
		// Singleton instance
		public static HardwareInfo Instance = null;

		[Header("Final User Score")]
		[ContextMenuItem ("Calculate Score", "CalculateHardwareScore")]
		[Tooltip("The final calculated User Hardware Score, calibrated to a scale of 100. \nA score of 100 means that the User hardware matches the Reference hardware exactly. \nLess than 100 means the User is a lesser system, and more than 100 means the User is a greater system.")]
		public float userHardwareScore = 0.0f;

		[Header("Breakdown")]

		[ContextMenuItem ("Calculate Score", "CalculateHardwareScore")]
		[Tooltip("The GPU score for the User System, calibrated to a scale of 100 where 100 = Reference.")]
		public float userGPUScore = 0.0f;
		[ContextMenuItem ("Calculate Score", "CalculateHardwareScore")]
		[Tooltip("The CPU score for the User System, calibrated to a scale of 100 where 100 = Reference.")]
		public float userCPUScore = 0.0f;

		[Header("Warnings")]

		[HideInInspector]
		//[TextArea(6,6)]
		//[ContextMenuItem ("Calculate Score", "CalculateHardwareScore")]
		public string compatibilityWarnings = "";

		[Space(20)]

		[SerializeField]
		private HardwareComparisonWeights comparisonWeights;

		// Toggle for SIMPLE vs COMPLEX data
		[SerializeField]
		[Space(10)]
		[ContextMenuItem ("Set Reference", "SetReference")]
		[ContextMenuItem ("Clear Reference", "ClearReference")]
		private bool useComplexHardwareData = false;
		// SIMPLE DATA is defined as what is necessary for the comparison operation:
		// Device Type
		// GPU memory
		// GPU multithreading
		// GPU shader level
		// GPU max texture size
		// CPU frequency
		// System Memory
		// Support for Compute Shaders
		// Support for Image Effects
		// Support for Shadows

		// COMPLEX DATA is defined as all other data

		// Add a ContextMenu item to set the reference configuration
		[Space(10)]
		[ContextMenuItem ("Set Reference", "SetReference")]
		[ContextMenuItem ("Clear Reference", "ClearReference")]
		public SimpleHardwareInfo referenceConfiguration;
		[ContextMenuItem ("Set Reference", "SetReference")]
		[ContextMenuItem ("Clear Reference", "ClearReference")]
		public SimpleHardwareInfo userConfiguration;

		[ContextMenuItem ("Set Reference", "SetReference")]
		[ContextMenuItem ("Clear Reference", "ClearReference")]
		public ComplexHardwareInfo complexUserConfiguration;

		void Awake (){
			// If the Instance doesn't already exist
			if(Instance == null){
				// If the instance doesn't already exist, set it to this
				Instance = this;
			}else if(Instance != this){
				// If an instance already exists that isn't this, destroy this instance and log what happened
				Destroy(gameObject);
				Debug.LogError("ERROR! The HardwareInfo script encountered another instance of HardwareInfo; it destroyed itself rather than overwrite the existing instance.", this);
			}

			// Create the Weights if they do not already exist
			if(comparisonWeights == null || comparisonWeights.initialized == false) comparisonWeights = new HardwareComparisonWeights();

			// Create the Reference info from the current config if it does not already exist
			if(referenceConfiguration == null || referenceConfiguration.initialized == false) referenceConfiguration = new SimpleHardwareInfo();
			// Create the User info from the current config
			userConfiguration = new SimpleHardwareInfo();
			userConfiguration.SetFromCurrentConfig();

			// If we are using complex info, create it from the current info
			if(useComplexHardwareData){
				complexUserConfiguration = new ComplexHardwareInfo();
				complexUserConfiguration.SetFromCurrentConfig();
			}else complexUserConfiguration = null;					// Otherwise set it to null for GC to deal with it later

			// Calculate user score
			userHardwareScore = CalculateHardwareScore();
		}

		// Calculate user hardware score
		public float CalculateHardwareScore(){

			// Clear compatibility warnings
			compatibilityWarnings = "";

			float basePoints = 100.0f;
			if(userConfiguration.deviceType == referenceConfiguration.deviceType) basePoints *= 1.0f;
			else{
				if(userConfiguration.deviceType == DeviceType.Desktop) basePoints *= comparisonWeights.desktopWeight;
				if(userConfiguration.deviceType == DeviceType.Console) basePoints *= comparisonWeights.consoleWeight;
				if(userConfiguration.deviceType == DeviceType.Handheld) basePoints *= comparisonWeights.handheldWeight;
			}

			// Check that they use the same graphics APIs and flag a warning if not
			if(userConfiguration.gpuDeviceType != referenceConfiguration.gpuDeviceType) compatibilityWarnings += "WARNING: The reference configration and user configuration are using different graphics APIs. This may cause incompatibility or rendering issues.\n";

			float GPUScore = 1.0f;
			GPUScore *= (float)(userConfiguration.gpuMemory)/(float)(referenceConfiguration.gpuMemory);
			GPUScore *= (float)(userConfiguration.gpuShaderLevel)/(float)(referenceConfiguration.gpuShaderLevel);
			// If there is a difference of more than 5 in reference and user GPU shader levels, create a warning
			if((referenceConfiguration.gpuShaderLevel - userConfiguration.gpuShaderLevel) > 5){
				compatibilityWarnings += "WARNING: The reference configuration and user configuration support different Shader Models. This may cause incompatibility or rendering issues.\n";
			}
			GPUScore *= (float)(userConfiguration.maxTextureSize)/(float)(referenceConfiguration.maxTextureSize);
			if(userConfiguration.gpuMultiThread == false && referenceConfiguration.gpuMultiThread == true){
				GPUScore -= (GPUScore * comparisonWeights.gpuMultithreadPenalty);
				compatibilityWarnings += "WARNING: The reference configuration supports GPU multithreading, but the user configuration does not!\n";
			}
			GPUScore *= userConfiguration.SLIScalar/referenceConfiguration.SLIScalar;
			//GPUScore *= comparisonWeights.GPUWeight;
			userGPUScore = GPUScore * 100.0f;

			float CPUScore = 1.0f;
			if(userConfiguration.processorCount > comparisonWeights.ignoreCoresAbove) CPUScore *= 1.0f;
			else CPUScore *= (float)userConfiguration.processorCount/(float)referenceConfiguration.processorCount;
			CPUScore *= (float)userConfiguration.processorFrequency/(float)referenceConfiguration.processorFrequency;
			//CPUScore *= comparisonWeights.CPUWeight;
			userCPUScore = CPUScore * 100.0f;

			float RAMScore = 1.0f;
			RAMScore *= (float)userConfiguration.systemMemory/(float)referenceConfiguration.systemMemory;
			//RAMScore *= comparisonWeights.RAMWeight;

			float avgScore = CPUScore + GPUScore + RAMScore;
			avgScore = avgScore / 3.0f;

			//float penalties = 0.0f;
			Vector3 penaltyVect = new Vector3(comparisonWeights.shadowPenalty, comparisonWeights.computePenalty, comparisonWeights.imageEffectPenalty);
			//float penaltySize = penaltyVect.x + penaltyVect.y + penaltyVect.z;
			//penaltyVect *= 1.0f/penaltySize;
			// penaltyVect.Normalize();
			if(userConfiguration.supportsShadows == referenceConfiguration.supportsShadows) avgScore *= 1.0f;
			else{
				if(userConfiguration.supportsShadows == false && referenceConfiguration.supportsShadows == true){
					avgScore *= (1.0f - penaltyVect.x);
					compatibilityWarnings += "WARNING: The reference configuration supports shadows, but the user configuration does not!\n";
				}
			}
			if(userConfiguration.supportsComputeShaders == referenceConfiguration.supportsComputeShaders) avgScore *= 1.0f;
			else{
				if(userConfiguration.supportsComputeShaders == false && referenceConfiguration.supportsComputeShaders == true){
					avgScore *= (1.0f - penaltyVect.y);
					compatibilityWarnings += "WARNING: The reference configuration supports Compute Shaders, but the user configuration does not!\n";
				}
			}
			if(userConfiguration.supportsImageEffects == referenceConfiguration.supportsImageEffects) avgScore *= 1.0f;
			else{
				if(userConfiguration.supportsImageEffects == false && referenceConfiguration.supportsImageEffects == true){
					avgScore *= (1.0f - penaltyVect.z);
					compatibilityWarnings += "WARNING: The reference configuration supports Image Effects, but the user configuration does not!\n";
				}
			}

			//avgScore *= (1.0f - penalties);
			

			return avgScore*basePoints;
			//userHardwareScore = penalties;
		}

		// Get Compatibility Warnings, or check if they exist at all
		public static bool CompatibilityCheck(){
			if(Instance.compatibilityWarnings.Length > 0) return false;

			return true;
		}
		public static string GetWarnings(){
			return Instance.compatibilityWarnings;
		}

		// Get User, Reference, and Complex User Data
		public static SimpleHardwareInfo GetUserHardwareInfo(){
			return Instance.userConfiguration;
		}

		public static SimpleHardwareInfo GetReferenceHardwareInfo(){
			return Instance.referenceConfiguration;
		}

		public static ComplexHardwareInfo GetComplexUserHardwareInfo(){
			if(Instance.complexUserConfiguration != null) return Instance.complexUserConfiguration;

			return null;
		}

		// Set and clear reference values
		public void SetReference(){
			referenceConfiguration.SetFromCurrentConfig();
		}

		public void ClearReference(){
			referenceConfiguration.Clear();
			referenceConfiguration = null;
		}

	}