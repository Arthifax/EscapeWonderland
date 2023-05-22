using System;
using UnityEditor;
using UnityEngine;

namespace TinyGiantStudio.PackageImporter
{
    /// <summary>
    /// Contains information about a package
    /// </summary>
    [Serializable]
    internal class TGS_Package
    {
        public string packageName;
        public DefaultAsset package;
        [Tooltip("The description of the unitypackage")]
        public string Description;

        public bool hasPipelineDependancy = false;
        public Pipeline targetPipeline;

        public int[] minimumPipelineVersion;
        public int[] maximumPipelineVersion;

        public bool hasUnityVersionDependancy = false;
        public double minimumUnityVersion;
        public double maximumUnityVersion;



        public float currentVersion = 1;
        public float lastInstalledVersion = 0;

        public bool ignoreUpdates;

        public enum Pipeline
        {
            SRP,
            URP,
            HDRP
        }
    }
}

