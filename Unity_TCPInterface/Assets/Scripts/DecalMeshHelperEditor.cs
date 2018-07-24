using UnityEngine;
using System.Collections;

public class DecalMeshHelperEditor : MonoBehaviour
{
    public Texture btnTexture;
    public GameObject[] gos;

    void OnGUI()
    {
        if (!btnTexture)
        {
            Debug.LogError("Please assign a texture on the inspector");
            return;
        }

        if (GUI.Button(new Rect(10, 10, 50, 50), btnTexture)){
        	gos = new GameObject[10];
            Debug.Log("Clicked the button with an image");
            for(int i = 1; i <= 10; i++){
            	string str = string.Format("Mesh {0}", i);
            	GameObject go = GameObject.Find(str);
            	gos[i-1] = go;
            }

            for(int i = 1; i <= 10; i++){
            for(int j = 1; j <= 10; j++){
            		
            		if(i==j)
            			gos[j-1].SetActiveRecursively(true);
            		else
            			gos[j-1].SetActiveRecursively(false);
            		
            	}
            	Debug.Log("Mesh");
            	Debug.Log(i);
            	Debug.Log("visible");
            }


        }

        if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
            Debug.Log("Clicked the button with text");
    }
}
