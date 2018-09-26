
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net; 
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
  

public class TCPInterface : MonoBehaviour
{	

		private TcpListener tcpListener; 
		private Thread tcpListenerThread;  	
		private TcpClient connectedTcpClient; 	
		private Dictionary<string,Mesh> meshdict;
		private Dictionary<string,GameObject> godict;
		private Queue<UnityMesh> meshqueue;
        private Queue<UnityCameraSettings> camsetqueue;
		private static string id_tri = " Surface";
		private static string id_line = " Line";
		private static string id_vert = " Point";
        //public GameObject SampleButton;
		
	
		// Use this for initialization
		void Start ()
		{
                GameObject button_clear = GameObject.Find("Button_clear");
                button_clear.GetComponent<Button>().onClick.AddListener(reset_all);

				meshdict = new Dictionary<string,Mesh> ();
				godict = new Dictionary<string,GameObject> ();
				meshqueue = new Queue<UnityMesh> ();
                camsetqueue = new Queue<UnityCameraSettings>();

				// Start Server
				tcpListenerThread = new Thread (new ThreadStart(ListenForIncommingRequests)); 		
				tcpListenerThread.IsBackground = true; 		
				tcpListenerThread.Start(); 

		}


    public void reset_all ()
    {
        foreach(KeyValuePair<string,GameObject> entry in godict)
        {
            Destroy(entry.Value);      
        }
        godict.Clear();
        meshdict.Clear();
    }
	
		// Update is called once per frame
		void Update ()
    {       
        while (camsetqueue.Count > 0)
            { 
            UnityCameraSettings rec_set = camsetqueue.Dequeue();
            GameObject cam = GameObject.Find(rec_set.id);
            rec_set.process_command(cam);
            }
		
				while (meshqueue.Count > 0) {
					
					UnityMesh rec_msh = meshqueue.Dequeue();
					GameObject ago = null;
					GameObject parent = null;
					string[] rec_ids = rec_msh.id.Split(":"[0]);
					string rec_msh_id = rec_ids[0];
					string id_spec = "";
					if (rec_ids.Length>1){
						id_spec = rec_ids[1];
					}

					if (godict.ContainsKey (rec_msh_id))
					{
						parent = godict[rec_msh_id];
					}
					else
					{
                
						parent = new GameObject();
        				parent.name = rec_msh_id;
						godict[rec_msh_id] = parent;
                                           
                        //GameObject button = (GameObject)Instantiate(SampleButton);
                        GameObject prefab = GameObject.Find("Button_clear");
                        GameObject button = (GameObject)Instantiate(prefab);
                        GameObject panel = GameObject.Find("Panel_visible");
                        button.transform.SetParent(panel.transform);//Setting button parent
                                                                    //button.SetActive(true);
                        button.GetComponent<Button>().onClick.RemoveAllListeners();
                    
                        //button.GetComponent<Button>().onClick.AddListener(OnClick);//Setting what button does when clicked
                        button.GetComponent<Button>().onClick.AddListener(() => on_button_click(parent,button) );
                        button.transform.GetChild(0).GetComponent<Text>().text = rec_msh_id;//Changing text

                        Button thebutton = button.GetComponent<Button>();
                        ColorBlock thecolor = button.GetComponent<Button>().colors;
                        thecolor.normalColor = Color.green;
                        thecolor.highlightedColor = Color.green;
                        thecolor.pressedColor = Color.green;
                        thebutton.colors = thecolor;

                        godict[rec_msh_id + ":Button"] = button;

					}

					if (rec_msh.triangles.Length > 2) {
							//triangle mesh
							string id_tri_msh = rec_msh_id + id_tri + " " + id_spec;
							
							if (meshdict.ContainsKey (id_tri_msh) && godict.ContainsKey (id_tri_msh)) {
									
									Mesh msh = meshdict [id_tri_msh];
									rec_msh.update_tri_mesh (msh);
									ago = godict [id_tri_msh];

							} else {
									
									ago = new GameObject (id_tri_msh);
									//ago.transform.SetParent(parent.transform);
									godict [id_tri_msh] = ago;
									Mesh msh = rec_msh.new_tri_mesh (ago,parent);
									meshdict [id_tri_msh] = msh;
									//EditorGUIUtility.PingObject(ago);
									//Selection.activeGameObject = ago;
									//ago.transform.SetParent(parent.transform, false);

									ago.transform.parent = parent.transform;

									
							}
							rec_msh.process_options(godict [id_tri_msh], "surface");
					}
					if (rec_msh.lines.Length > 1) {
							//line mesh
							//string id_tri_msh = rec_msh_id + id_tri + " " + id_spec;
							string id_line_msh = rec_msh_id + " " + id_spec + " "+ id_line;
							if (meshdict.ContainsKey (id_line_msh) && godict.ContainsKey (id_line_msh)) {
									
									//update mesh
									Mesh msh = meshdict [id_line_msh];
									rec_msh.update_line_mesh (msh);
									ago = godict [id_line_msh]; //?

							} else {
									
									//new mesh
									ago = new GameObject (id_line_msh);
									//ago.transform.SetParent(parent.transform);
									//ago.transform.parent = parent.transform;
									godict [id_line_msh] = ago;
									Mesh msh = rec_msh.new_line_mesh (ago);
									meshdict [id_line_msh] = msh;
									ago.transform.parent = parent.transform;


							}
							rec_msh.process_options(godict [id_line_msh], "line");
					}
					if (rec_msh.points.Length > 0) {
							//vertex mesh
							string id_vert_msh = rec_msh_id + id_vert + " " + id_spec;
							if (meshdict.ContainsKey (id_vert_msh) && godict.ContainsKey (id_vert_msh)) {
									
									//update mesh
									Mesh msh = meshdict [id_vert_msh];
									rec_msh.update_vert_mesh (msh);
									ago = godict [id_vert_msh];

							} else {
									
									//new mesh
									ago = new GameObject (id_vert_msh);
									godict [id_vert_msh] = ago;
									//ago.transform.SetParent(parent.transform);
									Mesh msh = rec_msh.new_vert_mesh (ago);
									meshdict [id_vert_msh] = msh;
									ago.transform.parent = parent.transform;
							}
							rec_msh.process_options(godict [id_vert_msh], "point");
					}
					if (ago!=null){
                        var children = new List<GameObject>();
                        foreach (Transform child in ago.transform) children.Add(child.gameObject);
                        children.ForEach(child => Destroy(child));
						rec_msh.draw_text(ago);
					}
					ago = null;
					rec_msh = null;
						
				}
		 
		}

		private void ListenForIncommingRequests () { 		
		try { 			
			// Create listener on localhost port 8052. 			
			tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8052); 			
			tcpListener.Start();              
			Debug.Log("Server is listening");              
			//Byte[] bytes = new Byte[1024];  
			Byte[] bytes = new Byte[64000];
			StringBuilder sb = new StringBuilder ();			
			while (true) { 				
				using (connectedTcpClient = tcpListener.AcceptTcpClient()) { 					
					// Get a stream object for reading 					
					using (NetworkStream stream = connectedTcpClient.GetStream()) { 						
						int length; 						
						// Read incomming stream into byte arrary. 						
						while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 							
							var incommingData = new byte[length]; 							
							Array.Copy(bytes, 0, incommingData, 0, length);  							
							// Convert byte array to string message. 							
							string clientMessage = Encoding.ASCII.GetString(incommingData);
                            //Debug.Log("client message received as: " + clientMessage); 						
                            if (clientMessage.Length > 24 && clientMessage.Substring(clientMessage.Length - 25, 25).Equals("UNITY_MESH_JSON_FORMATTED"))
                            {
                                //Debug.Log ("End of Message");
                                sb.Append(clientMessage.Substring(0, clientMessage.Length - 25));
                                UnityMesh rec_msh = JsonUtility.FromJson<UnityMesh>(sb.ToString());
                                meshqueue.Enqueue(rec_msh);
                                sb = new StringBuilder();
                            }
                            else
                            {
                                if (clientMessage.Length > 20 && clientMessage.Substring(clientMessage.Length - 21, 21).Equals("UNITY_CAMERA_SETTINGS"))
                                {
                                    sb.Append(clientMessage.Substring(0, clientMessage.Length - 21));
                                    UnityCameraSettings cam_settings = JsonUtility.FromJson<UnityCameraSettings>(sb.ToString());
                                    camsetqueue.Enqueue(cam_settings);
                                    sb = new StringBuilder();
                                }
                                else
                                {
                                    sb.Append(clientMessage);
                                }
                            }
						} 					
					} 				
				} 			
			} 		
		} 		
		catch (SocketException socketException) { 			
			Debug.Log("SocketException " + socketException.ToString()); 		
		}     
	}
		private void SendMessage() { 		
		if (connectedTcpClient == null) {             
			return;         
		}  		
		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = connectedTcpClient.GetStream(); 			
			if (stream.CanWrite) {                 
				string serverMessage = "This is a message from your server."; 			
				// Convert string message to byte array.                 
				byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage); 				
				// Write byte array to socketConnection stream.               
				stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);               
				Debug.Log("Server sent his message - should be received by client");           
			}       
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		} 	
	} 


    public void on_button_click(GameObject o, GameObject b)
    {
        if (o.activeSelf)
        {
            o.SetActive(false);
            Button thebutton = b.GetComponent<Button>();
            ColorBlock thecolor = b.GetComponent<Button>().colors;
            thecolor.normalColor = Color.red;
            thecolor.highlightedColor = Color.red;
            thecolor.pressedColor = Color.red;
            thebutton.colors = thecolor;


        }
        else
        {
            o.SetActive(true);
            Button thebutton = b.GetComponent<Button>();
            ColorBlock thecolor = b.GetComponent<Button>().colors;
            thecolor.normalColor = Color.green;
            thecolor.highlightedColor = Color.green;
            thecolor.pressedColor = Color.green;
            thebutton.colors = thecolor;

        }
    }

}