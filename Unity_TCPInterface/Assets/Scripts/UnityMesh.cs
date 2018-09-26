using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UnityMesh
{
	public string id;
	public Vector3[] vertices;
	public int[] points;
	public int[] lines;
	public int[] triangles;
	public Color[] colors;
	public string[] options;
	public UnityText[] text;
    

	public UnityMesh(){
	}

	public Mesh new_tri_mesh(GameObject gameObject, GameObject parent){

		Mesh msh = new Mesh();
		this.update_tri_mesh (msh);
		// Set up game object with mesh;
		gameObject.AddComponent(typeof(MeshRenderer));
		MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
		//filter.transform.parent = parent.transform;

		filter.mesh = msh;
		gameObject.GetComponent<Renderer>().material = new Material( Shader.Find("Custom/Flat Wireframe") );


		return msh;
	}
		
	public void update_tri_mesh(Mesh msh){
		bool facecolor = this.get_option("color") == "facecolor";
		if (!facecolor){
			msh.triangles = new int[]{};
			msh.vertices = this.vertices;
			msh.triangles = this.triangles;
			msh.colors = this.colors;
			msh.RecalculateNormals();
			msh.RecalculateBounds();
			msh.RecalculateTangents();
		}
		if(facecolor)
		{
			int len_tri = this.triangles.Length;
			int facenum = 0;

			Vector3[] new_vertices = new Vector3[len_tri];
			Color[] new_colors = new Color[len_tri];
			int[] new_triangles = new int[len_tri];

			for (int i = 0; i < len_tri; i++){
				facenum = i/3;
				new_vertices[i] = this.vertices[this.triangles[i]];
				new_colors[i] = this.colors[facenum];
				new_triangles[i] = i;
			}
			msh.vertices = new_vertices;
			msh.triangles = new_triangles;
			msh.colors = new_colors;
			msh.RecalculateNormals();
			msh.RecalculateBounds();
			msh.RecalculateTangents();
		}
	}

	public Mesh new_line_mesh(GameObject gameObject){
		//Debug.Log(this.id);
		Mesh msh = new Mesh();
		this.update_line_mesh (msh);
		// Set up game object with mesh;
		gameObject.AddComponent(typeof(MeshRenderer));
		MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
		filter.mesh = msh;
		gameObject.GetComponent<Renderer>().material = new Material( Shader.Find("Unlit/ThickLine") );
		return msh;
	}

	public void update_line_mesh(Mesh msh){
		msh.vertices = this.vertices;
		msh.SetIndices(this.lines, MeshTopology.Lines, 0);
		msh.colors = this.colors;
		msh.RecalculateBounds();
	}

	public Mesh new_vert_mesh(GameObject gameObject){
		Mesh msh = new Mesh();
		this.update_vert_mesh (msh);
		// Set up game object with mesh;
		gameObject.AddComponent(typeof(MeshRenderer));
		MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
		filter.mesh = msh;
		//gameObject.GetComponent<Renderer>().material = new Material( Shader.Find("Point Cloud/Disk") );
		gameObject.GetComponent<Renderer>().material = new Material( Shader.Find("Custom/CubeShader") );
		return msh;
	}

	public void update_vert_mesh(Mesh msh){
		msh.vertices = this.vertices;
		msh.SetIndices(this.points, MeshTopology.Points, 0);
		msh.colors = this.colors;
		msh.RecalculateBounds();
	}

	public string get_option(string str){

		for(int i = 0; i < this.options.Length; i++){
			string[] strArr = this.options[i].Split("="[0]);
			if (strArr[0].Trim().Equals(str)){
				return strArr[1].Trim();
			}
		}
		return "not found";
	}

	public void process_options(GameObject gameObject, string str)
	{
		for(int i = 0; i < this.options.Length; i++)
		{	
			
			string[] strArr = this.options[i].Split("="[0]);
			if (strArr[0].Trim().Equals(str+"_shader") && str.Equals("surface")){
				//Debug.Log(strArr[1].Trim());
				set_surface_shader_options(gameObject,strArr[1].Trim());
			}
			if (strArr[0].Trim().Equals(str+"_size") && str.Equals("point") ){
				//Debug.Log(strArr[1].Trim());
				set_point_shader_size(gameObject, float.Parse( strArr[1].Trim() ) );
			}
			if (strArr[0].Trim().Equals(str+"_width") && str.Equals("line") ){
				//Debug.Log(strArr[1].Trim());
				set_line_shader_width(gameObject, float.Parse( strArr[1].Trim() ) );
			}

		}

	}

	public void set_surface_shader_options(GameObject gameObject, string option){
		if (option.Equals("wireframe")){
			gameObject.GetComponent<Renderer>().material = new Material( Shader.Find("Custom/Flat Wireframe") );
		}
		if (option.Equals("flat")){
			gameObject.GetComponent<Renderer>().material = new Material( Shader.Find("Custom/Flat") );
		}
		if (option.Equals("smooth")){
			gameObject.GetComponent<Renderer>().material = new Material( Shader.Find("Custom/StandardVertex") );
		}
		if (option.Equals("transparent")){
			gameObject.GetComponent<Renderer>().material = new Material( Shader.Find("Custom/VertexColors2") );
		}

	}

	public void set_point_shader_size(GameObject gameObject, float size){
		gameObject.GetComponent<Renderer>().material.SetFloat("_PointSize", size);
	}

		public void set_line_shader_width(GameObject gameObject, float size){
		gameObject.GetComponent<Renderer>().material.SetFloat("_Width", size);
	}

	public void draw_text(GameObject parent){
		//foreach (Transform child in parent.transform) GameObject.Destroy(child.gameObject);
		for (int i = 0; i < this.text.Length; i++) draw_text(parent, this.text[i], i);
	}

	public void draw_text(GameObject parent, UnityText text, int i){
		GameObject textobject = new GameObject ("Text "+ i.ToString());
		TextMesh t = textobject.AddComponent<TextMesh>();
		t.text = text.text;
        t.color = text.color;
 		t.fontSize = 20;
 		t.transform.localEulerAngles = text.rot;
 		t.transform.position = text.pos;
 		t.transform.localScale = text.scale;
 		t.transform.parent = parent.transform;
	}	
	
}
