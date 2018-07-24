using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

[RequireComponent(typeof(Camera))]
public class Renderator_FREE_PL : MonoBehaviour {
	
	public int mnoznikRozdzielczosci; // Mnoży wirtualnie okno Game
	public KeyCode render; // tym przyciskiem robimy render
	public bool renderNaStarcie; // jak jest włączony to renderuje pierwszą klatkę
	public string sciezka; // ścieżka, gdzie rendery będą zapisywane

	void Start () 
	{
		if(sciezka == "")
		{
			if(!Directory.Exists("Assets/Screenshots")) // sprawdzanie czy folder istnieje
				Directory.CreateDirectory("Assets/Screenshots"); // jeśli nie to go tworzy
		}else if(!Directory.Exists(sciezka)) // tak samo dla wybranego przez nas
			Directory.CreateDirectory(sciezka);

		if(renderNaStarcie)
		{
			if(sciezka == "") // zapisze w folderze z assetami
			{
				ScreenCapture.CaptureScreenshot("Assets/Screenshots/Render" // ścieżka do pliku oraz początek nazwy (Render)
					+ System.DateTime.Now.ToString("_yyyy-MM-dd_") // dodanie daty do nazwy
					+ System.DateTime.Now.ToString ("hh-mm-ss_") // dodanie obecnej godziny do nazwy
					+ ".png", mnoznikRozdzielczosci); //screenshot z mnożnikiem rozdzielczości
			}else
			{ // zapisze w wybranym folderze
				ScreenCapture.CaptureScreenshot(sciezka + "/Render"  // ścieżka do pliku oraz początek nazwy (Render)
					+ System.DateTime.Now.ToString("_yyyy-MM-dd_") // dodanie daty do nazwy
					+ System.DateTime.Now.ToString ("hh-mm-ss_") // dodanie obecnej godziny do nazwy
					+ ".png", mnoznikRozdzielczosci); //screenshot z mnożnikiem rozdzielczości);
			}
		}
	}
	void Update()
	{
		if(Input.GetKeyDown(render))
		{
			if(sciezka == "")
			{
				ScreenCapture.CaptureScreenshot("Assets/Screenshots/Render" // ścieżka do pliku oraz początek nazwy (Render)
					+ System.DateTime.Now.ToString("_yyyy-MM-dd_") // dodanie daty do nazwy
					+ System.DateTime.Now.ToString ("hh-mm-ss_") // dodanie obecnej godziny do nazwy
					+ ".png", mnoznikRozdzielczosci); //screenshot z mnożnikiem rozdzielczości
			}
			else
			{
				ScreenCapture.CaptureScreenshot(sciezka + "/Render"  // ścieżka do pliku oraz początek nazwy (Render)
					+ System.DateTime.Now.ToString("_yyyy-MM-dd_") // dodanie daty do nazwy
					+ System.DateTime.Now.ToString ("hh-mm-ss_") // dodanie obecnej godziny do nazwy
					+ ".png", mnoznikRozdzielczosci); //screenshot z mnożnikiem rozdzielczości);
			}
		}
	}
	[ContextMenu("Wybierz folder do zapisu")]
	void Kalesony()
	{
		string piach = EditorUtility.OpenFolderPanel("Wybierz folder do zapisu", "", "");
		if( piach.Length != 0 )
		{
			sciezka = piach;
		}
	}
}
