using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class wireframe : MonoBehaviour {

	public KeyCode wireFrameKey;
	public bool wireFrameMode;

	public void Start()
	{
		
	}

	void OnPreRender() {
		if(wireFrameMode)
			GL.wireframe = true;
		else
			GL.wireframe = false;
	}
	void Update()
	{
		if(Input.GetKeyDown(wireFrameKey))
			wireFrameMode = !wireFrameMode;
	}
}
