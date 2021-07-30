namespace UnityEngine.Rendering.Universal
{
    public class CloudsFeature : DrawFullscreenFeature
    {
        public CloudUrpProxy cloudSettings;
        public NoiseGenerator noiseGenerator;
        public WeatherMap weatherMap;
        
        public override void Create()
        {
            base.Create();
            cloudSettings = FindObjectOfType<CloudUrpProxy>();
            if (cloudSettings == null)
            {
                Debug.LogError("Could not find Cloud URP Proxy Script in Scene!");
                return;
            }
            noiseGenerator = FindObjectOfType<NoiseGenerator>();
            if (noiseGenerator == null)
            {
                Debug.LogError("Could not find Noise Generator Script in Scene!");
                return;
            }
            weatherMap = FindObjectOfType<WeatherMap>();
            if (weatherMap == null)
            {
                Debug.LogError("Could not find Weather Map Script in Scene!");
                return;
            }

            cloudSettings.TriggerUpdate = true;
        }

        private void UpdateBasics()
        {
            noiseGenerator.UpdateNoise();
            if (!Application.isPlaying)
            {
                weatherMap.UpdateMap();
            }
            
            settings.blitMaterial.SetTexture("NoiseTex", noiseGenerator.shapeTexture);
            settings.blitMaterial.SetTexture("DetailNoiseTex", noiseGenerator.detailTexture);
            settings.blitMaterial.SetTexture("WeatherMap", weatherMap.weatherMap);
        }

        private void UpdateEverything()
        {
            UpdateBasics();
            
            settings.blitMaterial.SetTexture("BlueNoise", cloudSettings.blueNoise);
            
            cloudSettings.numStepsLight = Mathf.Max(1, cloudSettings.numStepsLight);
            
            Vector3 size = cloudSettings.MaxBounds - cloudSettings.MinBounds;
            int width = Mathf.CeilToInt(size.x);
            int height = Mathf.CeilToInt(size.y);
            int depth = Mathf.CeilToInt(size.z);

            settings.blitMaterial.SetFloat("scale", cloudSettings.cloudScale);
            settings.blitMaterial.SetFloat("densityMultiplier", cloudSettings.densityMultiplier);
            settings.blitMaterial.SetFloat("densityOffset", cloudSettings.densityOffset);
            settings.blitMaterial.SetFloat("lightAbsorptionThroughCloud", cloudSettings.lightAbsorptionThroughCloud);
            settings.blitMaterial.SetFloat("lightAbsorptionTowardSun", cloudSettings.lightAbsorptionTowardSun);
            settings.blitMaterial.SetFloat("darknessThreshold", cloudSettings.darknessThreshold);
            settings.blitMaterial.SetVector("params", cloudSettings.cloudTestParams);
            settings.blitMaterial.SetFloat("rayOffsetStrength", cloudSettings.rayOffsetStrength);

            settings.blitMaterial.SetFloat("detailNoiseScale", cloudSettings.detailNoiseScale);
            settings.blitMaterial.SetFloat("detailNoiseWeight", cloudSettings.detailNoiseWeight);
            settings.blitMaterial.SetVector("shapeOffset", cloudSettings.shapeOffset);
            settings.blitMaterial.SetVector("detailOffset", cloudSettings.detailOffset);
            settings.blitMaterial.SetVector("detailWeights", cloudSettings.detailNoiseWeights);
            settings.blitMaterial.SetVector("shapeNoiseWeights", cloudSettings.shapeNoiseWeights);
            settings.blitMaterial.SetVector("phaseParams",
                new Vector4(cloudSettings.forwardScattering, cloudSettings.backScattering, cloudSettings.baseBrightness, cloudSettings.phaseFactor));

            settings.blitMaterial.SetVector("boundsMin", cloudSettings.MinBounds);
            settings.blitMaterial.SetVector("boundsMax", cloudSettings.MaxBounds);

            settings.blitMaterial.SetInt("numStepsLight", cloudSettings.numStepsLight);

            settings.blitMaterial.SetVector("mapSize", new Vector4(width, height, depth, 0));

            settings.blitMaterial.SetFloat("timeScale", (Application.isPlaying) ? cloudSettings.timeScale : 0);
            settings.blitMaterial.SetFloat("baseSpeed", cloudSettings.baseSpeed);
            settings.blitMaterial.SetFloat("detailSpeed", cloudSettings.detailSpeed);

            settings.blitMaterial.SetColor("colA", cloudSettings.colA);
            settings.blitMaterial.SetColor("colB", cloudSettings.colB);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (cloudSettings.UpdateAllParametersInRealtime)
            {
                UpdateEverything();
            }
            else
            {
                if (cloudSettings.TriggerUpdate)
                {
                    UpdateEverything();
                }
                else
                {
                    UpdateBasics();
                }
            }
            
            blitPass.renderPassEvent = settings.renderPassEvent;
            blitPass.settings = settings;
            renderer.EnqueuePass(blitPass);
        }
    }
}