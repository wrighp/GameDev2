using UnityEngine;
using System.Collections;

/*
	Script to use for scene transitions
*/

[ExecuteInEditMode]
public class SceneTransition : MonoBehaviour
{
    public Material TransitionMaterial;
	
	public bool fadeIn = true;
	public bool play = false;
	public float cutoff = 0;

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (TransitionMaterial != null)
            Graphics.Blit(src, dst, TransitionMaterial);
    }
	
	void Update()
	{
		if(play)
		{
			if(fadeIn)
				cutoff+=Time.deltaTime;
			else
				cutoff-=Time.deltaTime;
			
			TransitionMaterial.SetFloat("_Cutoff", cutoff);
			
			if(cutoff > 1)
			{
				play = false;
				cutoff = 1;
			}
			
			if(cutoff < 0)
			{
				play = false;
				cutoff = 0;
			}
		}
		
	}
}
