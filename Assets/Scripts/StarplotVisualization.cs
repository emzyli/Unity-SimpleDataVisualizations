using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarplotVisualization : MonoBehaviour
{
    public String starplotName = "StarplotExample";
    public float axisMax = 100;
    public float radius = 3.0f;
    private Starplot starplot;
    private float[] data;
    private String unit;
 
    public Vector3[] axesPositions = new Vector3[] { Vector3.up, Vector3.right, Vector3.down, Vector3.left };

    // Use this for initialization
    protected void Start()
    {
        data = generateData();
        starplot = new Starplot(Vector3.zero, axesPositions, radius, starplotName, data, axisMax, gameObject);
        starplot.color = Color.green;
        starplot.LineWidth = 0.01f;
        starplot.unit = new string[] { "Ziegen", "Tauben", "Kartoffeln", "Zebras" };
    }

    protected void Update()
    {
        // update data every 3 seconds
        if (Time.unscaledTime % 3 < 0.02)
        {
            data = generateData();
        }
        starplot.Update(data);
    }

    private float[] generateData()
    {
        float[] data = new float[axesPositions.Length];
        for(int i = 0; i < axesPositions.Length; i++)
        {
            data[i] = UnityEngine.Random.Range(0, axisMax);
        }
        return data;
    }
}
