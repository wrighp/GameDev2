using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
	Script to use for scene transitions
*/

[ExecuteInEditMode]
public class SceneTransition : MonoBehaviour
{
	//get that canvas and the popup
	public Canvas canvas;
	GameObject popup;
	
	public static string scene;
	Scene currentScene;
	
    public Material TransitionMaterial;
	
	public bool fadeOut = true;
	public bool play = false;
	public bool playOnStart = false;
	public float cutoff = 0;
	bool showPopup = false;
	
	//fade things back in if the screen is all black
	void Start()
	{
		currentScene = SceneManager.GetActiveScene();
		//call scene.name
		if(canvas.transform.Find("Popup")!=null)
		{
			popup = canvas.transform.Find("Popup").gameObject;
			popup.transform.Find("[popupText]").gameObject.GetComponent<Text>().text = 
						currentScene.name;
		}
		
		cutoff = TransitionMaterial.GetFloat("_Cutoff");
		if(cutoff > 1 && playOnStart)
		{
			cutoff = 1.5f;
			fadeOut = false;
			play = true;
		}
	}

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (TransitionMaterial != null)
            Graphics.Blit(src, dst, TransitionMaterial);
    }
	
	void Update()
	{
		//Debug.Log(cutoff);
		if(play)
		{
			Play();
		}
		if(showPopup)
		{
			Debug.Log(popup);
			popup.GetComponent<Animator>().Play("PopupFade");
			showPopup = false;
		}
	}
	
	//manually play
	public void Play()
	{
		if(fadeOut)
				cutoff+=Time.deltaTime;
			else
				cutoff-=Time.deltaTime;
			
			TransitionMaterial.SetFloat("_Cutoff", cutoff);
			
			if(cutoff > 1 && fadeOut)
			{
				play = false;
				cutoff = 1;
				SceneManager.LoadScene(scene);
				fadeOut = false;
			}
			
			if(cutoff < 0 && ! fadeOut)
			{
				//change the popup text
				play = false;
				cutoff = 0;
				fadeOut = true;
				showPopup = true;
			}
	}
	
	public static void setScene(string s)
	{
		scene = s;
	}
}
