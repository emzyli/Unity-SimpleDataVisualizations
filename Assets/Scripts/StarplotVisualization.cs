using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarplotVisualization : MonoBehaviour
{
    public String starplotName = "StarplotExample";  
    public float radius = 3.0f;
    public int amountOfAxes = 4;

    private Starplot starplot;
    private float[] data;
    private String unit;
    
    public float[] axisMax = new float[] { 100.0f, 550, 250.5f, 10 };
    public Vector3[] axesPositions = new Vector3[] { Vector3.up, Vector3.right, Vector3.down, Vector3.left };

    // Use this for initialization
    protected void Start()
    {
        if(axisMax.Length < amountOfAxes)
        {
            throw new Exception("Please indicate a max value for every axis!");
        }
        data = GenerateData();
        if (amountOfAxes != axesPositions.Length)
        {
            starplot = new Starplot(Vector3.zero, axisMax.Length, radius, starplotName, data, axisMax, gameObject)
            {
                Color = new Color32(0, 255, 0, 100),
                LineWidth = 0.01f,
                Units = new string[] { "Ziegen", "Tauben", "g Kartoffeln", "Zebras", "Karotten" }
            };
        }
        else
        {
            starplot = new Starplot(Vector3.zero, axesPositions, radius, starplotName, data, axisMax, gameObject)
            {
                Color = new Color32(0, 255, 0, 100),
                LineWidth = 0.01f,
                Units = new string[] { "Ziegen", "Tauben", "g Kartoffeln", "Zebras", "Karotten" }
            };
        }
    }

    protected void Update()
    {
        // update data every 3 seconds
        if (Time.unscaledTime % 3 < 0.02)
        {
            data = GenerateData();
        }
        starplot.Update(data);
    }

    private float[] GenerateData()
    {
        float[] data = new float[amountOfAxes];
        for(int i = 0; i < amountOfAxes; i++)
        {
            data[i] = UnityEngine.Random.Range(0, axisMax[i]);
        }
        return data;
    }
}
