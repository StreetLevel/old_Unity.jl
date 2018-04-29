
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net; 
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
  

public class TCPInterface : MonoBehaviour
{	

		private TcpListener tcpListener; 
		private Thread tcpListenerThread;  	
		private TcpClient connectedTcpClient; 	
		private Dictionary<string,Mesh> meshdict;
		private Dictionary<string,GameObject> godict;
		private Queue<UnityMesh> meshqueue;
		private static string id_tri = " Surface";
		private static string id_line = " Line";
		private static string id_vert = " Point";
	
		// Use this for initialization
		void Start ()
		{

				meshdict = new Dictionary<string,Mesh> ();
				godict = new Dictionary<string,GameObject> ();
				meshqueue = new Queue<UnityMesh> ();

				// Start Server
				tcpListenerThread = new Thread (new ThreadStart(ListenForIncommingRequests)); 		
				tcpListenerThread.IsBackground = true; 		
				tcpListenerThread.Start(); 

		}
	
		// Update is called once per frame
		void Update ()
		{
		
				while (meshqueue.Count > 0) {
					
					UnityMesh rec_msh = meshqueue.Dequeue();
					GameObject ago = null;

						if (rec_msh.triangles.Length > 2) {
								//triangle mesh
								string id_tri_msh = rec_msh.id + id_tri;
								
								if (meshdict.ContainsKey (id_tri_msh)) {
										
										Mesh msh = meshdict [id_tri_msh];
										rec_msh.update_tri_mesh (msh);
										ago = godict [id_tri_msh];

								} else {
										
										ago = new GameObject (id_tri_msh);
										godict [id_tri_msh] = ago;
										Mesh msh = rec_msh.new_tri_mesh (ago);
										meshdict [id_tri_msh] = msh;
										
								}

								rec_msh.process_options(godict [id_tri_msh], "surface");
						}

						if (rec_msh.lines.Length > 1) {
								//line mesh
								string id_line_msh = rec_msh.id + id_line;
								if (meshdict.ContainsKey (id_line_msh)) {
										//update mesh
										Mesh msh = meshdict [id_line_msh];
										rec_msh.update_line_mesh (msh);
										ago = godict [id_line_msh];
								} else {
										//new mesh
										ago = new GameObject (id_line_msh);
										godict [id_line_msh] = ago;
										Mesh msh = rec_msh.new_line_mesh (ago);
										meshdict [id_line_msh] = msh;
								}

								rec_msh.process_options(godict [id_line_msh], "line");
						}

						if (rec_msh.points.Length > 0) {
								//vertex mesh
								string id_vert_msh = rec_msh.id + id_vert;
								if (meshdict.ContainsKey (id_vert_msh)) {
										//update mesh
										Mesh msh = meshdict [id_vert_msh];
										rec_msh.update_vert_mesh (msh);
										ago = godict [id_vert_msh];
								} else {
										//new mesh
										ago = new GameObject (id_vert_msh);
										godict [id_vert_msh] = ago;
										Mesh msh = rec_msh.new_vert_mesh (ago);
										meshdict [id_vert_msh] = msh;
								}

								rec_msh.process_options(godict [id_vert_msh], "point");
						}

						if (ago!=null){
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
			Byte[] bytes = new Byte[1024];  
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
							if (clientMessage.Length > 24 && clientMessage.Substring(clientMessage.Length - 25,25).Equals("UNITY_MESH_JSON_FORMATTED") )
							{
									//Debug.Log ("End of Message");
									//Debug.Log(serverMessage);
									sb.Append (clientMessage.Substring(0,clientMessage.Length - 25));
									UnityMesh rec_msh = JsonUtility.FromJson<UnityMesh> (sb.ToString ());
									meshqueue.Enqueue(rec_msh);	
									sb = new StringBuilder ();
							}
							else
							{
								sb.Append (clientMessage);	
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

}