using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

[RequireComponent(typeof(Camera))]
public class Renderator_FREE_EN : MonoBehaviour {
	
	public int multipler; // multipling Game window resolution
	public KeyCode render; // press it to make render
	public bool renderAtStart; // if true it will render at start
	public string path; // path where renders will be saved

	void Start () 
	{
		if(path == "")
		{
			if(!Directory.Exists("Assets/Screenshots")) // check if folder exist
				Directory.CreateDirectory("Assets/Screenshots"); // if not it creates one
		}else if(!Directory.Exists(path)) // the same for our path
			Directory.CreateDirectory(path);
		

		if(renderAtStart)
		{
			if(path == "") // saving in Asset folder
			{
				ScreenCapture.CaptureScreenshot("Assets/Screenshots/Render"  // path to folder with renders and firts part of the name (Render)
					+ System.DateTime.Now.ToString("_yyyy-MM-dd_") // giving date to name
					+ System.DateTime.Now.ToString ("hh-mm-ss_") // giving current time to name
					+ ".png", multipler); //screenshot with resolution multipler
			}else
			{ // saving in our folder we choose
				ScreenCapture.CaptureScreenshot(path + "/Render" // path to folder with renders and firts part of the name (Render)
					+ System.DateTime.Now.ToString("_yyyy-MM-dd_") // giving date to name
					+ System.DateTime.Now.ToString ("hh-mm-ss_") // giving current time to name
					+ ".png", multipler); //screenshot with resolution multipler
			}
		}
	}

	void Update()
	{
		if(Input.GetKeyDown(render))
		{
			if(path == "") // saving in Asset folder	
			{
				ScreenCapture.CaptureScreenshot("Assets/Screenshots/Render"  // path to folder with renders and firts part of the name (Render)
					+ System.DateTime.Now.ToString("_yyyy-MM-dd_") // giving date to name
					+ System.DateTime.Now.ToString ("hh-mm-ss_") // giving current time to name
					+ ".png", multipler); //screenshot with resolution multipler
			}else
			{ // saving in our folder we choose
				ScreenCapture.CaptureScreenshot(path + "/Render" // path to folder with renders and firts part of the name (Render)
					+ System.DateTime.Now.ToString("_yyyy-MM-dd_") // giving date to name
					+ System.DateTime.Now.ToString ("hh-mm-ss_") // giving current time to name
					+ ".png", multipler); //screenshot with resolution multipler
			}
		}
	}
	[ContextMenu("Choose save folder")]
	void Kalesony()
	{
		string piach = EditorUtility.OpenFolderPanel("Choose save folder", "", "");
		if( piach.Length != 0 )
		{
			path = piach;
		}
	}
}
