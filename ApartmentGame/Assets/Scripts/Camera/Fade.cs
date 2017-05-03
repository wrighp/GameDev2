using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {

	// Use this for initialization
    // public Camera activeCamera;

    // public Collider trigger;

    public string NextScene;
       
    public Texture2D fadeTexture;
    float fadeSpeed = 0.2f;
    int drawDepth = -1000;

    float alpha = 1.0f;
    public float fadeDir = -1;

    void Start()
    {
        alpha = 1 - fadeDir;
    }

    void Update()
    {
        if(alpha >= 1 && fadeDir == 1)
        {
            //SceneManager.LoadScene(NextScene);
        }
    }

    void OnGUI()
    {
        if(this.GetComponent<Camera>() != Camera.current)
        {
            return;
        }
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b,alpha);

        GUI.depth = drawDepth;

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
    }
}
