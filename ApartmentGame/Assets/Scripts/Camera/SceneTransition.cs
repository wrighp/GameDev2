using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/*
	Script to use for scene transitions
*/

[ExecuteInEditMode]
public class SceneTransition : MonoBehaviour
{
	public string scene;
	
    public Material TransitionMaterial;
	
	bool fadeOut = true;
	public bool play = false;
	public float cutoff = 0;
	
	//fade things back in if the screen is all black
	void Start()
	{
		cutoff = TransitionMaterial.GetFloat("_Cutoff");
		if(cutoff > 1)
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
			
			if(cutoff < 0)
			{
				play = false;
				cutoff = 0;
				fadeOut = true;
			}
		}
		
	}
}
