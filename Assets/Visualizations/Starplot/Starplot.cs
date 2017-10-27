using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Starplot
{
	private GameObject background;
	private List<GameObject> axes;
	private List<GameObject> meshes;
	private int amountOfAxes;
	private Color[] lookuptable;
	private float[] data;
	private String name;
	private Boolean flexibleAxes;
	private Vector3[] axisPositions;
	private GameObject parent;
	private Boolean isVisible;

    /// <summary>
    /// Gets or sets the axis max.
    /// </summary>
    /// <value>The axis max.</value>
    public float axisMax { get; set; }

    /// <summary>
    /// Gets or sets the radius.
    /// </summary>
    /// <value>The radius.</value>
    public float radius { get; set; }

    /// <summary>
    /// Gets or sets the unit.
    /// </summary>
    /// <value>The unit.</value>
	public String[] unit { get; set; }

    public Color color { get; set; }

    private float _lineWidth;
    public float LineWidth
    {
        get { return LineWidth; }
        set {
            if (this.axes != null)
            {
                foreach (GameObject gameObject in this.axes)
                {
                    LineRenderer lr = gameObject.GetComponent<LineRenderer>();
                    lr.startWidth = value;
                    lr.endWidth = value;
                }
            }
            _lineWidth = value;

        }
    }

	/// <summary>
	/// Creates a new star plot.
	/// </summary>
	/// <param name="amountOfAxes">The number of axes <b>greater then 2.</b></param>
	/// <param name="position">The position of the star plot.</param>
	/// <param name="data">The data of each axes.</param>
	/// <param name="axisMax">the maximum value of the axes</param>
	/// <param name="parent">the parent Object</param>
	public Starplot(int amountOfAxes, Vector3 position, String name, float[] data, float axisMax, GameObject parent)
	{
		this.parent = parent;

		defaultSetUp(amountOfAxes, position, name, data, axisMax, 5, false, null);

		background.transform.parent = parent.transform;
		background.transform.localPosition = position;
		background.transform.localScale = Vector3.one * 5;
		Vector3 rotation = new Vector3(0, 0, 0);
		SetRotation(Quaternion.Euler(rotation));
	}

	public Starplot(int amountOfAxes, Vector3 position, float radius, String name, float[] data, float axisMax)
	{
        defaultSetUp(amountOfAxes, position, name, data, axisMax, radius, false, null);

		background.transform.position = position;
		background.transform.localScale = new Vector3(radius, radius, radius);
		Vector3 rotation = new Vector3(90, 0, 0);
		SetRotation(Quaternion.Euler(rotation));
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="T:Starplot"/> class.
	/// </summary>
	/// <param name="position">the local position on parent.</param>
	/// <param name="axisPositions">world positions of axes.</param>
	/// <param name="radius">Radius.</param>
	/// <param name="name">Name.</param>
	/// <param name="data">Data for each axis</param>
	/// <param name="axisMax">Axis max.</param>
	/// <param name="parent">the parent GameObject</param>
	public Starplot(Vector3 position, Vector3[] axisPositions, float radius, String name, float[] data, float axisMax, GameObject parent)
	{
		this.parent = parent;

        this.defaultSetUp(axisPositions.Length, position, name, data, axisMax, radius, true, axisPositions);

		Vector3 rotation = new Vector3(0, 0, 0);
		SetRotation(Quaternion.Euler(rotation));
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="T:Starplot"/> class.
	/// </summary>
	/// <param name="position">the local position on parent.</param>
	/// <param name="axisPositions">world positions of axes.</param>
	/// <param name="radius">Radius.</param>
	/// <param name="name">Name.</param>
	/// <param name="data">Data for each axis</param>
	/// <param name="axisMax">Axis max.</param>
	/// <param name="parent">the parent GameObject</param>
	/// <param name="unit">the unit of the values</param>
	public Starplot(Vector3 position, Vector3[] axisPositions, float radius, String name, float[] data, float axisMax, GameObject parent, String[] unit)
	{
		this.parent = parent;
		this.unit = unit;

        this.defaultSetUp(axisPositions.Length, position, name, data, axisMax, radius, true, axisPositions);

		Vector3 rotation = new Vector3(0, 0, 0);
		SetRotation(Quaternion.Euler(rotation));
	}
	/// <summary>
	/// Destroy the star graph.
	/// </summary>
	public void Destroy()
	{
		foreach (Transform transform in background.transform)
		{
			GameObject.Destroy(transform.gameObject);
		}
		GameObject.Destroy(background);

	}

    /// <summary>
    /// Default set up of the starplot.
    /// </summary>
    /// <param name="amountOfAxes">Amount of axes.</param>
    /// <param name="position">Position.</param>
    /// <param name="name">Name.</param>
    /// <param name="data">Data.</param>
    /// <param name="axisMax">Axis max.</param>
    /// <param name="radius">Radius.</param>
    /// <param name="flexibleAxes">If set to <c>true</c> flexible axes.</param>
    /// <param name="axisPositions">Axis positions.</param>
	private void defaultSetUp(int amountOfAxes, Vector3 position, String name, float[] data, float axisMax, float radius, bool flexibleAxes, Vector3[] axisPositions)
	{
        if (amountOfAxes < 3)
		{
			throw new ArgumentOutOfRangeException("amountOfAxes");
		}
        this.isVisible = true;
		this.axisMax = axisMax;
		this.amountOfAxes = amountOfAxes;
		this.data = data;
		this.name = name;
		this.radius = radius;
        this.flexibleAxes = flexibleAxes;
        this._lineWidth = 0.0025f;
        this.color = Color.cyan;
        axes = new List<GameObject>();
		meshes = new List<GameObject>();

		createBackground(position);
        if(axisPositions != null){
       //     this.axisPositions = tranformToLocalPositions(axisPositions);    
        }
		createAxes();
		renderGraph();
	}
    /// <summary>
    /// Creates the background.
    /// </summary>
    /// <param name="position">the position of the starplot.</param>
	private void createBackground(Vector3 position)
	{
		background = GameObject.Find(name);
		if (background == null)
		{
			Color color = new Color32(255, 255, 255, 150);

			background = MonoBehaviour.Instantiate(Resources.Load("Starplot/cirlce")) as GameObject;
			GameObject def = GameObject.Find("default");
			foreach (Transform child in background.transform)
			{
				if (child.name.Equals("default"))
				{
					def = child.gameObject;
				}
			}
			def.name = name + "_default";
			background.name = name;
			if (parent != null)
			{
				background.transform.parent = parent.transform;
			}

			background.transform.localScale = Vector3.one * radius;
			background.transform.localPosition = position;
		}
		else
		{
			throw new Exception("An object with the name '" + name + "' already exists!");
		}
	}

    /// <summary>
    /// Creates the axes.
    /// </summary>
	private void createAxes()
	{
		var length = 0.0f;
		if (axisPositions == null) axisPositions = new Vector3[amountOfAxes];

        for (int i = 0; i < amountOfAxes; i++)
		{
            float angle = 0;
			GameObject axis = GameObject.Find(name + "_Axis" + i);
			if (axis == null)
			{
                length = radius;
                Vector3 position;
				if (axisPositions[i] == Vector3.zero)
				{
					position = new Vector3(0, 0, 0.9f);
					angle = 360.0f / amountOfAxes * i;
					axisPositions[i] = position;
					Array.Sort(axisPositions, compareClockwise);
				}
				else
				{
					position = axisPositions[i];
				}

				axis = new GameObject(name + "_Axis" + i);
				LineRenderer lr = axis.AddComponent<LineRenderer>();
				lr.material = new Material(Shader.Find("Diffuse"));
				lr.material.color = Color.black;
				lr.startWidth = _lineWidth;
				lr.endWidth = _lineWidth;
				lr.useWorldSpace = false;
				lr.SetPosition(0, Vector3.zero);
				lr.SetPosition(1, position * radius);

                axis.transform.parent = background.transform;
                axis.transform.localScale = Vector3.one / radius;
				axis.transform.localPosition = new Vector3(0, 0.03f, 0);

				axis.transform.localRotation = Quaternion.Euler(0, angle, 0);
				axes.Add(axis);
			}
			else
			{
				LineRenderer lr = axis.GetComponent<LineRenderer>();
				lr.SetPosition(1, axisPositions[i] * radius);
				axis.transform.localPosition = new Vector3(0, 0.03f, 0);

				var currentLength = (lr.GetPosition(1) - lr.GetPosition(0)).magnitude;
				if (currentLength > length)
				{
					length = currentLength;
				}
			}
		}
		background.transform.localScale = new Vector3(length, length, length);
	}

    /// <summary>
    /// Creates the data labels.
    /// </summary>
	private void createDataLabels()
	{
		for (int i = 0; i < amountOfAxes; i++)
		{
			GameObject labelObj = GameObject.Find(axes[i].name + "_label_main");
			if (labelObj == null)
			{
				labelObj = new GameObject(axes[i].name + "_label_main");
				labelObj.transform.parent = background.transform;

				GameObject bg = GameObject.CreatePrimitive(PrimitiveType.Plane);
                bg.name = "labelBackground";
				bg.transform.parent = labelObj.transform;

				bg.transform.localScale = new Vector3(0.03f, 0.03f, 0.01f);
				Renderer renderer = bg.GetComponent<Renderer>();
				Material mat = new Material(Shader.Find("Diffuse"));
				mat.color = Color.white;
				renderer.material = mat;
			}
			GameObject dataLabel = GameObject.Find(axes[i].name + "_label");
			if (dataLabel == null)
			{
				dataLabel = new GameObject();
				dataLabel.transform.parent = labelObj.transform;
				dataLabel.name = axes[i].name + "_label";
			}
			Vector3 position = axes[i].transform.localRotation * axisPositions[i].normalized;
			labelObj.transform.localPosition = Vector3.zero + position + new Vector3(0, 0.01f, 0);
			dataLabel.transform.localPosition = new Vector3(0, 0.01f, 0);

			Canvas can = dataLabel.GetComponent<Canvas>();
			if (can == null)
			{
				can = dataLabel.AddComponent<Canvas>();
			}

			Text text = dataLabel.GetComponent<Text>();
			if (text == null)
			{
				text = dataLabel.AddComponent<Text>();
				text.transform.SetParent(can.transform, false);
				text.transform.localScale = new Vector3(0.002f, 0.002f, 0.001f);
				text.alignment = TextAnchor.MiddleCenter;
				text.color = Color.black;
				text.font = Font.CreateDynamicFontFromOSFont("Arial", (int)radius * 7);
				text.fontSize = 34;
				text.fontStyle = FontStyle.Bold;
				text.verticalOverflow = VerticalWrapMode.Overflow;
				text.horizontalOverflow = HorizontalWrapMode.Overflow;
			}
            String labelText = Math.Round(data[i]) + "";
            if(unit != null){
                labelText += " " + unit[i];
            }
            text.text = labelText;
            foreach(Transform transform in labelObj.transform)
            {
                if (transform.gameObject.name.Equals("labelBackground"))
                {
                    transform.localScale = (new Vector3(0.005f * labelText.Length, 0.03f, 0.01f));
                }                
            }
          
			text.resizeTextForBestFit = true;
			dataLabel.transform.localRotation = Quaternion.Euler(90, 0, 0);
			labelObj.transform.localScale = Vector3.one;
			labelObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
		}
	}

    /// <summary>
    /// Compares given vectors clockwise.
    /// </summary>
    /// <param name="v1">Vector 1.</param>
    /// <param name="v2">Vector 2.</param>
	private int compareClockwise(Vector3 v1, Vector3 v2)
	{
		if (v1.x >= 0)
		{
			if (v2.x < 0)
			{
				return -1;
			}
			return -Comparer<float>.Default.Compare(v1.z, v2.z);
		}
		else
		{
			if (v2.x >= 0)
			{
				return 1;
			}
			return Comparer<float>.Default.Compare(v1.z, v2.z);
		}
	}


    /// <summary>
    /// Tranforms to local positions.
    /// </summary>
    /// <returns>The to local positions.</returns>
    /// <param name="worldPositions">World positions.</param>
	private Vector3[] tranformToLocalPositions(Vector3[] worldPositions)
	{
		Vector3[] localPositions = new Vector3[worldPositions.Length];
		for (int i = 0; i < worldPositions.Length; i++)
		{
			Vector3 localPosition = background.transform.InverseTransformPoint(worldPositions[i]);
			localPositions[i] = localPosition;
		}
		return localPositions;
	}

    /// <summary>
    /// Gets the point on the axis.
    /// </summary>
    /// <returns>The point on theaxis.</returns>
    /// <param name="data">the data.</param>
    /// <param name="postion">the position.</param>
    private Vector3 getPointOnAxis(float data, Vector3 postion)
	{
		// position.x -> end of Axes
		float x = data * (postion.x / radius) / axisMax;
		float y = data * (postion.y / radius) / axisMax;
		float z = data * (postion.z / radius) / axisMax;
		return new Vector3(x, y, z);
	}

    /// <summary>
    /// Renders the graph.
    /// </summary>
	private void renderGraph()
	{
		if (axes.ToArray().Length > 0)
		{
			for (int i = 0; i < data.Length; i++)
			{
				Vector3[] positions = new Vector3[2];

				axes[i].GetComponent<LineRenderer>().GetPositions(positions);
				positions[1] = axes[i].transform.localRotation * positions[1];
				// Point on current axes           
				Vector3 pointA = getPointOnAxis(data[i], positions[1]);

				// point on next Axis
				Vector3 pointB;
				if (i + 1 < data.Length)
				{
					Vector3[] positions1 = new Vector3[2];
					axes[i + 1].GetComponent<LineRenderer>().GetPositions(positions1);
					positions1[1] = axes[i + 1].transform.localRotation * positions1[1];                   
					pointB = getPointOnAxis(data[i + 1], positions1[1]);
				}
				else
				{
					Vector3[] positions1 = new Vector3[2];
					axes[0].GetComponent<LineRenderer>().GetPositions(positions1);
					positions1[1] = axes[0].transform.localRotation * positions1[1];                    
					pointB = getPointOnAxis(data[0], positions1[1]);
				}
               
                // get Mesh
                GameObject obj = GameObject.Find(name + "_Triangle" + i);

				MeshRenderer renderer;
				Mesh mesh;
				Boolean objectIsNew = false;

				// if mesh is not existing
				if (obj == null)
				{
					objectIsNew = true;
					// create mesh
					obj = new GameObject(name + "_Triangle" + i);
					renderer = obj.AddComponent<MeshRenderer>();
					mesh = obj.AddComponent<MeshFilter>().mesh;
				}
				else
				{
					renderer = obj.GetComponent<MeshRenderer>();
					mesh = obj.GetComponent<MeshFilter>().mesh;

					mesh.Clear();
				}
				obj.transform.parent = background.transform;
				// position mesh before axis
				obj.transform.localPosition = new Vector3(0, axes[i].transform.localPosition.y - 0.01f, 0);

				// triangle points: point on current axis, point of origin, point on next axis
				mesh.vertices = new Vector3[]{
					pointA,
					Vector3.zero,
					pointB
				};
				mesh.uv = new Vector2[]{
					new Vector2(0,1), new Vector2(0,1), new Vector2(0,1)
				};
				mesh.triangles = new int[] { 0, 2, 1 };

				mesh.RecalculateNormals();

				Color[] colors = new Color[]{
					color,
					color,
					color,
				 };

				mesh.colors = colors;

				Material material = new Material(Shader.Find("AAA/StarplotShader"));
				renderer.material = material;
				if (objectIsNew)
				{
					meshes.Add(obj);
				}
				obj.transform.localScale = new Vector3(1, 1, 1);
			}
		}
	}
	/// <summary>
	/// Update the graph for the given data.
	/// ATTENTION: Call this in the Update() Function of the MonoBehaviour.
	/// </summary>
	/// <param name="data">the new data.</param>
	public void Update(float[] data)
	{
		if (data.Length != amountOfAxes)
		{
			throw new Exception("Please indicate a value for every axis!");
		}

		// for a smooth movement:
		// - approximate the old value to the new value
		// - by calling this method every frame, the new value is set slowly and 
		//   it looks like a smooth movement
		for (int i = 0; i < amountOfAxes; i++)
		{
			// difference between old and new value on axis
			float difference = Math.Abs(this.data[i] - data[i]) / 30.0f;

			// if old value is greater then the new value, minimize old data 
			if (this.data[i] > data[i])
			{
				this.data[i] -= difference;
			}
			// if old value is less then new value, maximize old data
			else if (this.data[i] < data[i])
			{
				this.data[i] += difference;
			}
		}

		if (!this.IsHidden())
		{
			createDataLabels();

			renderGraph();
		}



	}

    /// <summary>
    /// Sets the rotation.
    /// </summary>
    /// <param name="rotation">Rotation.</param>
	public void SetRotation(Quaternion rotation)
	{
		if (background.transform.parent != null)
		{
			background.transform.localRotation = rotation;

		}
		else
		{
			background.transform.rotation = rotation;
		}

	}

	/// <summary>
	/// sets the position of the axis end. It starts in the middle of the starplot.
	/// </summary>
	/// <param name="axisPositions">The axispositions.</param>
	public void SetAxisPositions(Vector3[] axisPositions)
	{
		// transform position from world space to local spacd
		Vector3[] localPositions = tranformToLocalPositions(axisPositions);
		// Array.Sort(localPositions, (a, b) => compareClockwise(a, b));

		Boolean different = false;
		for (int i = 0; i < axisPositions.Length; i++)
		{
			if (localPositions[i] != this.axisPositions[i])
			{
				different = true;
			}
		}
		if (different)
		{
			// for a smooth movement:
			// - approximate the old value to the new value
			// - by calling this method every frame, the new value is set slowly and 
			//   it looks like a smooth movement
			for (int i = 0; i < amountOfAxes; i++)
			{

				Vector3 localPosition = localPositions[i];

				if (localPosition.x != this.axisPositions[i].x)
				{
					float diff = Math.Abs(localPosition.x - this.axisPositions[i].x) / 5.0f;
					if (this.axisPositions[i].x > localPosition.x)
					{
						this.axisPositions[i].x -= diff;
					}
					// if old value is less then new value, maximize old data
					else if (this.axisPositions[i].x < localPosition.x)
					{
						this.axisPositions[i].x += diff;
					}
				}
				if (localPosition.y != this.axisPositions[i].y)
				{
					float diff = Math.Abs(localPosition.y - this.axisPositions[i].y) / 5.0f;
					if (this.axisPositions[i].y > localPosition.y)
					{
						this.axisPositions[i].y -= diff;
					}
					// if old value is less then new value, maximize old data
					else if (this.axisPositions[i].y < localPosition.y)
					{
						this.axisPositions[i].y += diff;
					}
				}
				if (localPosition.z != this.axisPositions[i].z)
				{
					float diff = Math.Abs(localPosition.z - this.axisPositions[i].z) / 5.0f;
					if (this.axisPositions[i].z > localPosition.z)
					{
						this.axisPositions[i].z -= diff;
					}
					// if old value is less then new value, maximize old data
					else if (this.axisPositions[i].z < localPosition.z)
					{
						this.axisPositions[i].z += diff;
					}
				}
			}
			createAxes();
			renderGraph();
			foreach (GameObject axis in axes)
			{
				axis.transform.localRotation = Quaternion.Euler(0, 0, 0);
			}
			foreach (GameObject mesh in meshes)
			{
				mesh.transform.localRotation = Quaternion.Euler(0, 0, 0);
			}
		}
	}

	/// <summary>
    /// Hide the Starplot instance.
    /// </summary>
	public void Hide()
	{
		if (this.isVisible)
		{
			this.isVisible = false;
			foreach (GameObject obj in meshes)
			{
				obj.GetComponent<Renderer>().enabled = false;
			}
			foreach (GameObject axis in axes)
			{
				axis.GetComponent<Renderer>().enabled = false;
			}
		}
	}

	/// <summary>
    /// Show this Starplot instance.
    /// </summary>
	public void Show()
	{
		if (this.IsHidden())
		{
			this.isVisible = true;
			foreach (GameObject obj in meshes)
			{
				obj.GetComponent<Renderer>().enabled = true;
			}
			foreach (GameObject axis in axes)
			{
				axis.GetComponent<Renderer>().enabled = true;
			}

			createDataLabels();
			renderGraph();
		}
	}

	/// <summary>
	/// Says if starplot is invisible.
	/// </summary>
	/// <returns><c>true</c>, if this starplot instance is hidden, <c>false</c> otherwise.</returns>
	public bool IsHidden()
	{
		return !isVisible;
	}

}
