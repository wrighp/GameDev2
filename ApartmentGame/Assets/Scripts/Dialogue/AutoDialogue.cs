using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	Dialogue that runs automatically, independent of player interaction
	it can either run in the background (npc chatter) or be like
	an unskippable cutscene
	
	will be linear, doesn't rely on player interaction
*/

public class AutoDialogue : MonoBehaviour {
	
	//the dialogue tree that's being referenced
	Dialogue dialogue;
	//and its path
	public Color color = new Color(0.2F, 0.3F, 0.4F, 0.5F);
	public string Path;
	
	//the dialogue window to be used
	public GameObject dialogueWindow;
	
	//dialogue boxes to be instantiated with the correct text
	public GameObject bigBox;
	public GameObject smallBox;
	
	//================================================================TEXT DELAYS ==========
	public float dDelay = 0.1f;
	public float pDelay = 0.3f;
	public float nDelay = 0.75f;
	
	public float yOffset = 1.5f;
	
	GameObject nodeText;
	
	//whether or not this is going to be running automatically in the background
	//of game events
	public bool background = false;
	public bool auto = false;
	
	public List<GameObject> scripts;
	
	//if there's an animation to play
	//public Animator animator;
	
	//keep track of the coroutine
	private IEnumerator runCoroutine;
	private IEnumerator displayCoroutine;
	private bool textScroll = false;
	
	int nodeID = -1;
	int scriptIndex = 0;

	// Use this for initialization
	//determine whether or not we're in cutscene mode or worldspace mode
	void Start () {
		
		//load the dialogue from the given path
		dialogue = Dialogue.Load(Path);
		
		//find the required components and initialize the private variables
		var canvas = GameObject.Find("Canvas");
		
		if(!background)
		{
			dialogueWindow = Instantiate<GameObject>(dialogueWindow);
			dialogueWindow.transform.SetParent(canvas.transform, false);
		}
		else
		{
			bigBox = Instantiate<GameObject>(bigBox);
			smallBox = Instantiate<GameObject>(smallBox);
			
			bigBox.transform.SetParent(transform, false);
			smallBox.transform.SetParent(transform, false);
			
			bigBox.GetComponent<LookAtCamera>().target = transform;
			bigBox.GetComponent<LookAtCamera>().yOffset = yOffset;
			
			smallBox.GetComponent<LookAtCamera>().target = transform;
			smallBox.GetComponent<LookAtCamera>().yOffset = yOffset;
			
			nodeText = bigBox.transform.Find("Box").gameObject.transform.Find("Text").gameObject;
			
			bigBox.transform.Find("Box").gameObject.GetComponent<Image>().color = color;
			
			bigBox.SetActive(false);
			smallBox.SetActive(false);
		}
		
		
		if(auto)
			runDialogue();
	}
	
	public void runDialogue()
	{
		bigBox.SetActive(true);
		//smallBox.SetActive(true);
		
		runCoroutine = run();
		StartCoroutine(runCoroutine);
	}
	
	public void stopDialogue()
	{
		if(runCoroutine!=null)
			StopCoroutine(runCoroutine);
		
		bigBox.SetActive(false);
		//smallBox.SetActive(false);
	}
	
	//run the dialogue tree coroutine
	//scroll the text, wait a moment, then go to the next node
	public IEnumerator run(){
		nodeID = dialogue._next;
		Node current;
		
		while(nodeID!=-1)
		{
			current = dialogue._nodes[nodeID];
			dialogue._next = current._reset;
			
			if(current._options[0]._dest != nodeID)
				updateText(current);
			
			while(textScroll)
			{
				yield return null;
			}
			nodeID = current._options[0]._dest;
			yield return new WaitForSeconds(nDelay);
		}
	}
	
	//update the text
	private void updateText(Node node){
		if(displayCoroutine!=null)
			StopCoroutine(displayCoroutine);
		
		displayCoroutine = DisplayText(node._text);
		StartCoroutine(displayCoroutine);
	}
	
	//coroutine for displaying the text
	//handle text wrapping too
	//size box based on size?
	private IEnumerator DisplayText(string displayText){
		int strLen = displayText.Length;
		int index = 0;
		
		nodeText.GetComponent<Text>().text = "";
		textScroll = true;
		
		while(true){
			//deal with newline character
			if(displayText[index] == '\\' && index<strLen-1 && displayText[index+1] == 'n'){
				index++;
				nodeText.GetComponent<Text>().text+='\n';
			}
			else if(displayText[index] == '%' && index<strLen-1 && displayText[index+1] == 'f'
				&& scriptIndex<scripts.Count)
			{
				index ++;
				scripts[scriptIndex].SetActive(true);
				scriptIndex++;
			}
			//otherwise go normally
			else{
				nodeText.GetComponent<Text>().text += displayText[index];
				//Source.PlayOneShot(Voice);
			}
			
			if((displayText[index] == '!' || displayText[index] == '?' ||
				displayText[index] == '.') && index<strLen-1 && 
				(displayText[index+1] == ' '|| displayText[index+1] == '\n')){
					yield return new WaitForSeconds(pDelay);
				}
			
			index++;
			
			if(index<strLen){
				//play a sound potentially
				//wait for a moment before adding next character
				yield return new WaitForSeconds(dDelay);
			}
			else{
				break;
			}
		}
		textScroll = false;
	}
	
}
