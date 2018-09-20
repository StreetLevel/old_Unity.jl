using System.Collections.Generic;
using UnityEngine;

public class AnimateMesh : MonoBehaviour {

    //soll animation laufen
    private bool b_animate = false;
    //framerate (kann aus inspektor angepasst werden)
    public float framerate = 0.5F;
    //
    private float nextframe = 0.0F;
    private int step = 0;
    private List<GameObject> meshlist;

    void OnGUI()
    {
        //Hier wird der Button definiert
        if (GUI.Button(new Rect(10, 10, 50, 30), "Go"))
        {   
            //Animation deaktivieren
            if (b_animate) b_animate = false;
            //Animation aktivieren
            else
            {
                //Sequenz auf start
                step = 0;
                b_animate = true;
                //Alle Meshes abfragen (Meshes müssen den Namen "Mesh i" wober i ein Int ist, im Inspektor haben)
                meshlist = get_all_meshes();
            }
        }
    }

    //Wird einmal pro update aufgerufen
    void Update()
    {
        //Ist die definierte Zeit bis zum nächsten Animationsschritt abgelaufen?
        if (b_animate && Time.time > nextframe)
        {
            //Nächster Animationsschritt
            step++;

            //Wenn keine neuen Netze mehr vorhanden sind starte von vorne
            if (step > meshlist.Count) step = 1;

            //Hier werden alle netze bis auf eins deaktiviert
            for (int i = 0; i < meshlist.Count; i++)
            {
                if (i==step-1) //die minus 1, da meshlist mit dem Index 0 anfängt
                {
                    meshlist[i].SetActive(true);
                }
                else
                {
                    meshlist[i].SetActive(false);
                }
            }

            // Zeit bis zum nächsten Zeitschritt definieren
            nextframe = Time.time + framerate;
        }
       
    }

    //Hier werden alle Meshes gesammelt
    private List<GameObject> get_all_meshes(){

        List<GameObject> ret = new List<GameObject>();
        int imesh = 0;
        bool foundmesh = true;
            while(foundmesh)
            {
                imesh++;
                var str_mesh = string.Format("Mesh {0}", imesh);
                GameObject mesh = GameObject.Find(str_mesh);
                if (mesh == null) foundmesh = false;
                else ret.Add(mesh);
            }
        return ret;
    }

    
    // Use this for initialization
    void Start () {
		
	}
	
}
