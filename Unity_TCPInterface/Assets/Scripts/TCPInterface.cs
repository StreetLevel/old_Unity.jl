
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPInterface : MonoBehaviour
{	

		private TcpClient socketConnection;
		private Thread clientReceiveThread;
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

				// Connect to Server
				ConnectToTcpServer ();
		}
	
		// Update is called once per frame
		void Update ()
		{
		
				while (meshqueue.Count > 0) {
					
					UnityMesh rec_msh = meshqueue.Dequeue();

						if (rec_msh.triangles.Length > 2) {
								//triangle mesh

								string id_tri_msh = rec_msh.id + id_tri;
								//Debug.Log(rec_msh.options["shader"]);
								if (meshdict.ContainsKey (id_tri_msh)) {
										//update mesh
										//Debug.Log("Update Mesh "+id_tri_msh);
										Mesh msh = meshdict [id_tri_msh];
										rec_msh.update_tri_mesh (msh);

								} else {
										//new mesh
										//Debug.Log("New Mesh "+id_tri_msh);
										GameObject go = new GameObject (id_tri_msh);
										godict [id_tri_msh] = go;
										Mesh msh = rec_msh.new_tri_mesh (go);
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
								} else {
										//new mesh
										GameObject go = new GameObject (id_line_msh);
										godict [id_line_msh] = go;
										Mesh msh = rec_msh.new_line_mesh (go);
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
								} else {
										//new mesh
										GameObject go = new GameObject (id_vert_msh);
										godict [id_vert_msh] = go;
										Mesh msh = rec_msh.new_vert_mesh (go);
										meshdict [id_vert_msh] = msh;
								}

								rec_msh.process_options(godict [id_vert_msh], "point");
						}

						rec_msh = null;
				}
		
				/*if (Input.GetKeyDown(KeyCode.Space)) {             
			SendMessage();         
		}*/   
		}

		/// <summary> 	
		/// Setup socket connection. 	
		/// </summary> 	
		private void ConnectToTcpServer ()
		{ 		
				try {  			
						clientReceiveThread = new Thread (new ThreadStart (ListenForData)); 			
						clientReceiveThread.IsBackground = true; 			
						clientReceiveThread.Start ();  		
				} catch (Exception e) { 			
						Debug.Log ("On client connect exception " + e); 		
				} 	
		}

		/// <summary> 	
		/// Runs in background clientReceiveThread; Listens for incomming data. 	
		/// </summary>     
		private void ListenForData ()
		{ 		
				try { 			
						//socketConnection = new TcpClient("130.75.53.89", 8053);  			
						//socketConnection = new TcpClient("192.168.1.3", 8053);  			
						socketConnection = new TcpClient ("localhost", 8052);  			
						Byte[] bytes = new Byte[1024]; 
						StringBuilder sb = new StringBuilder ();
			
						while (true) { 				
								using (NetworkStream stream = socketConnection.GetStream ()) { 					
										int length; 					
										while ((length = stream.Read (bytes, 0, bytes.Length)) != 0) { 
												var incommingData = new byte[length]; 						
												Array.Copy (bytes, 0, incommingData, 0, length); 						
												// Convert byte array to string message. 						
												string serverMessage = Encoding.ASCII.GetString (incommingData);	
												//Debug.Log(serverMessage);
												//End of msg
												if (serverMessage.Length > 24 && serverMessage.Substring(serverMessage.Length - 25,25).Equals("UNITY_MESH_JSON_FORMATTED") )
												{
														//Debug.Log ("End of Message");
														//Debug.Log(serverMessage);
														sb.Append (serverMessage.Substring(0,serverMessage.Length - 25));
														UnityMesh rec_msh = JsonUtility.FromJson<UnityMesh> (sb.ToString ());
														meshqueue.Enqueue(rec_msh);	
														sb = new StringBuilder ();

												}
												else
												{
													sb.Append (serverMessage);	
												}
										} 				
								} 			
						}         
				} catch (SocketException socketException) {             
						Debug.Log ("Socket exception: " + socketException);         
				}     
		}

		/// <summary> 	
		/// Send message to server using socket connection. 	
		/// </summary> 	
		private void SendMessage ()
		{         
				if (socketConnection == null) {             
						return;         
				}  		
				try { 			
						// Get a stream object for writing. 			
						NetworkStream stream = socketConnection.GetStream (); 			
						if (stream.CanWrite) {                 
								string clientMessage = "This is a message from one of your clients."; 				
								// Convert string message to byte array.                 
								byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes (clientMessage); 				
								// Write byte array to socketConnection stream.                 
								stream.Write (clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);                 
								Debug.Log ("Client sent his message - should be received by server");             
						}         
				} catch (SocketException socketException) {             
						Debug.Log ("Socket exception: " + socketException);         
				}     
		}
}