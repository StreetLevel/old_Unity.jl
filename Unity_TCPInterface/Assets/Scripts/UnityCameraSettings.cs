using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UnityCameraSettings
{
	
	//all vectors of length one
	public string Id;
	public Vector3[] main_camera_position;
	public Vector3[] main_camera_rotation;
	public Vector3[] main_camera_scale;
	public Color[] background_color;
	
    

    public UnityCameraSettings(){
	}

	public void process_command(){

		GameObject maincam = GameObject.Find(Id);
		if (this.main_camera_position.Length > 0 && this.main_camera_position.Length < 2)
		{
            maincam.transform.localPosition = this.main_camera_position[0];
		}
		if (this.main_camera_rotation.Length > 0 && this.main_camera_rotation.Length < 2)
		{
            maincam.transform.localEulerAngles = this.main_camera_rotation[0];
		}
		if (this.main_camera_scale.Length > 0 && this.main_camera_scale.Length < 2)
		{
            maincam.transform.localScale = this.main_camera_scale[0];
		}
		if (this.background_color.Length > 0 && this.background_color.Length < 2)
		{
            maincam.GetComponent<Camera>().backgroundColor = this.background_color[0];
		}


	}

	
}
