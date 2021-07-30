using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudUrpProxy : MonoBehaviour
{
    const string headerDecoration = " --- ";

    [Header(headerDecoration + "Main" + headerDecoration)]
    public Transform container;
    public Vector3 cloudTestParams;

    [Header("March settings" + headerDecoration)]
    public int numStepsLight = 8;
    public float rayOffsetStrength = 9.19f;
    public Texture2D blueNoise;

    [Header(headerDecoration + "Base Shape" + headerDecoration)]
    public float cloudScale = 0.6f;

    public float densityMultiplier = 0.82f;
    public float densityOffset = -3.64f;
    public Vector3 shapeOffset = new Vector3(29.4f, -47.87f, 1.11f);
    public Vector2 heightOffset = new Vector2(0, 0);
    public Vector4 shapeNoiseWeights = new Vector4(2.51f, 0.89f, 1.37f, 0.57f);

    [Header(headerDecoration + "Detail" + headerDecoration)]
    public float detailNoiseScale = 4;

    public float detailNoiseWeight = 1.5f;
    public Vector3 detailNoiseWeights = new Vector3(2.57f, 0.89f, 1.37f);
    public Vector3 detailOffset = new Vector3(130.23f, 0, 0);


    [Header(headerDecoration + "Lighting" + headerDecoration)]
    public float lightAbsorptionThroughCloud = 1.05f;

    public float lightAbsorptionTowardSun = 1.6f;
    [Range(0, 1)] public float darknessThreshold = .28f;
    [Range(0, 1)] public float forwardScattering = .72f;
    [Range(0, 1)] public float backScattering = .33f;
    [Range(0, 1)] public float baseBrightness = 1f;
    [Range(0, 1)] public float phaseFactor = .83f;

    [Header(headerDecoration + "Animation" + headerDecoration)]
    public float timeScale = 1;

    public float baseSpeed = 0.5f;
    public float detailSpeed = 1f;

    [Header(headerDecoration + "Sky" + headerDecoration)]
    public Color colA = Color.white;
    public Color colB = Color.cyan;

    [Header("Optimizations")] 
    [Tooltip("Enable this if you want to see changes immediately. If all the configuration is done, please uncheck it.")]
    public bool UpdateAllParametersInRealtime;
    [HideInInspector]
    public bool TriggerUpdate = true;

    public Vector3 MinBounds
    {
        get
        {
            if (container == null)
            {
                return new Vector3(-2048, 500, -2048);
            }

            return container.position - container.localScale / 2f;
        }
    }
    public Vector3 MaxBounds
    {
        get
        {
            if (container == null)
            {
                return new Vector3(2048, 700, 2048);
            }

            return container.position + container.localScale / 2f;
        }
    }
}
