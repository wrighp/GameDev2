using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

//Needs to have a canvas in the scene (doesn't have to be attached to camera)
//player could have a hash table <string, bool> of tasks to perform
//for dialogue options and triggers, could check event name as such to see if 
//the player has done it

public class npcDialogue : MonoBehaviour {
	
	//the dialogue tree that's being referenced
	Dialogue dialogue;
	
	//the dialogue box info
	GameObject Name;
	GameObject nodeText;
	
	GameObject[] options;
	GameObject[] availableOptions;
	
	GameObject option1;
	GameObject option2;
	GameObject option3;
	
	GameObject exit;
	
	//cameras to use
	public Camera mainCamera;
	public Camera dialogueCamera;
	public List<GameObject> speakers;
	
	//coroutine stuff
	private IEnumerator runCoroutine;
	private IEnumerator displayCoroutine;
	
	//node selection stuff
	private int select = -2;
	int nodeID = -1;
	
	public string Path;
	public GameObject dialogueWindow;
	
	//Auto would cause the dialogue to run once the player enters the
	//trigger area
	public bool auto = false;
	private bool running = false;
	private bool textScroll = false;
	
	public static Dictionary<string, bool> tasks;
	
	// Use this for initialization
	void Start () {
		//load the dialogue from the given path
		dialogue = Dialogue.Load(Path);
		
		//find the required components and initialize the private variables
		var canvas = GameObject.Find("Canvas");
		
		initiateTasks();
		
		dialogueWindow = Instantiate<GameObject>(dialogueWindow);
		dialogueWindow.transform.SetParent(canvas.transform, false);
		
		RectTransform windowTrans = (RectTransform)dialogueWindow.transform;
		windowTrans.localPosition = new Vector3(0,-100,0);
		
		
		Name = dialogueWindow.transform.Find("[npc]").gameObject;
		
		options = new GameObject[4];
		availableOptions = new GameObject[4];
		
		options[0] = option1 = dialogueWindow.transform.Find("[option 1]").gameObject;
		options[1] = option2 = dialogueWindow.transform.Find("[option 2]").gameObject;
		options[2] = option3 = dialogueWindow.transform.Find("[option 3]").gameObject;
		options[3] = exit = dialogueWindow.transform.Find("[end convo]").gameObject;
		
		nodeText = dialogueWindow.transform.Find("[dialogue]").gameObject;
		
		//add an action to the exit button
		exit.GetComponent<Button>().onClick.AddListener(delegate {
				setSelect(-1);
		});
		
		dialogueWindow.SetActive(false);
		
		//to test
		//move this runDialogue to a different function later
		//runDialogue();
		
	}
	
	private void initiateTasks(){
		tasks = new Dictionary<string, bool>();
		tasks["A"] = false;
		tasks["B"] = false;
		tasks["C"] = false;
		tasks["D"] = false;
	}
	
	//bla bla bla
	public void achieve(string thing){
		tasks[thing] = true;
	}
	
	public void runDialogue(){
		runCoroutine = run();
		StartCoroutine(runCoroutine);
	}
	
	public void loadDialogue(string newPath){
		Path = newPath;
		
		//load the dialogue from the given path
		dialogue = Dialogue.Load(Path);
		select = -2;
		nodeID = -1;
	}
	
	//method for selecting the next dialogue node to load
	public void setSelect(int x){
		select = x;
	}
	
	public void setNext(int x){
		dialogue._next = x;
	}
	
	//update the text
	private void updateText(Node node){
		Name.GetComponent<Text>().text = node._name;
		if(displayCoroutine!=null)
			StopCoroutine(displayCoroutine);
		
		displayCoroutine = DisplayText(node._text);
		StartCoroutine(displayCoroutine);
		
		//nodeText.GetComponent<Text>().text = node._text;
		
		option1.SetActive(false);
		option2.SetActive(false);
		option3.SetActive(false);
		exit.SetActive(false);
		
		//loop through all of this node's possible options and display them, currently
		//a maximum of 3 options per node
		for(int i=0;i<node._options.Count/*||i<2for later expansion?*/;i++){
			switch(i){
				case 0:
					updateButton(node, option1, node._options[i]);
				break;
				
				case 1:
					updateButton(node, option2, node._options[i]);
				break;
				
				case 2:
					updateButton(node, option3, node._options[i]);
				break;
				
			}
		}
		
		if(node._options.Count>1){
			exit.SetActive(true);
		}
	}
	
	//make the buttons visible
	private void updateButton(Node node, GameObject button, dialogueOption option){
		//check to see if the option has any requirements
		//if it does, check the hash table
		if(option._req != null && option._req != ""){
			//if the dictionary[option._req] == false, return
			if(!tasks[option._req])
				return;
			//else setActive and continue
		}
		
		if(node._options.Count > 1){
			button.SetActive(true);
		}
		
		button.GetComponentInChildren<Text>().text = option._text;
		button.GetComponent<Button>().onClick.AddListener(delegate {
			//Debug.Log(option._dest);
			setSelect(option._dest);});
	}
	
	//run the dialogue tree coroutine
	public IEnumerator run(){
		//THIS WILL DO FOR NOW
		yield return new WaitForEndOfFrame();
		
		//lock the cursor
		//Cursor.lockState = CursorLockMode.Locked;
		
		dialogueWindow.SetActive(true);
		
		nodeID = dialogue._next;
		
		//while the node isn't an exit node...
		while(nodeID!=-1){
			updateText(dialogue._nodes[nodeID]);
			//testing out the execute function
			dialogue._nodes [nodeID]._precalls.ForEach ((Call c) => c.execute ());
			
			//testing buttons
			//set the corresponding button active
			//for some reason, this works
			//CHECK THIS OUT ============================================================
			int j=0;
			for(int i=0; i<4;i++){
				if(options[i].activeSelf){
					//options[i].GetComponent<Button>().Select();
					availableOptions[j] = options[i];
					j++;
					Debug.Log( j + " " + options[i].GetComponentInChildren<Text>().text);
					//Debug.Log("FUCK " + j);
				}
			}
			
			Debug.Log("J IS EQUAL TO " + j);
			Debug.Log("THE DIALOGUE OPTIONS ARE ");
			
			for(int i=0; i<j;i++){
				Debug.Log( i + " " + availableOptions[i].GetComponentInChildren<Text>().text);
			}
			
			
			select = -2;
			
			int index = 0;
			int direction;
			
			//only one option
			if(j == 0){
				while(select == -2){
					if (Input.GetButtonDown("Fire1")&& !textScroll){
						//invoke a click through script
						// referenceToTheButton.onClick.Invoke();
						options[0].GetComponent<Button>().onClick.Invoke();
						
					}
					yield return /*new WaitForEndOfFrame()*/null;
				}
			}
			else{
				availableOptions[0].GetComponent<Button>().Select();
				
				while(select == -2){
						availableOptions[index].GetComponent<Button>().Select();
							
						direction = -(int) Input.GetAxisRaw("Vertical");
						index+=direction;
						
						//bind the indices
						if(index<0)
							index = 0;
						if(index>j-1)
							index = j-1;
						
						//Debug.Log( index + " " + options[index].GetComponentInChildren<Text>().text);
						
						//testing
						if(direction!=0){
							Debug.Log(direction);
							Debug.Log( "INDEX " + index + " " + 
							availableOptions[index].GetComponentInChildren<Text>().text);
						}

						//look for player input
						if (Input.GetButtonDown("Fire1") && !textScroll){
							//invoke a click through script
							// referenceToTheButton.onClick.Invoke();
							availableOptions[index].GetComponent<Button>().onClick.Invoke();
							
						}
						yield return /*new WaitForEndOfFrame()*/null;
				}
			}
			dialogue._nodes [nodeID]._postcalls.ForEach ((Call c) => c.execute ());
			dialogue._next = dialogue._nodes[nodeID]._reset;
			nodeID = select;
		}
		running = false;
		mainCamera.gameObject.SetActive(true);
		dialogueCamera.gameObject.SetActive(false);
		dialogueWindow.SetActive(false); 
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = true;
	}
	
	//coroutine for displaying the text, add something to allow the player to skip
	//forward later
	//handle text wrapping too
	//also, break out of coroutine when the player advances
	private IEnumerator DisplayText(string displayText){
		int strLen = displayText.Length;
		int index = 0;
		
		nodeText.GetComponent<Text>().text = "";
		textScroll = true;
		
		while(true){
			//if the player presses fire1, just put the text and get out
			if (Input.GetButtonDown("Fire1")){
				nodeText.GetComponent<Text>().text = displayText;
				break;
			}
			
			//deal with newline character
			if(displayText[index] == '\\' && index<strLen-1 && displayText[index+1] == 'n'){
				index++;
				nodeText.GetComponent<Text>().text+='\n';
			}
			//otherwise go normally
			else{
				nodeText.GetComponent<Text>().text += displayText[index];
			}
			
			if((displayText[index] == '!' || displayText[index] == '?' ||
				displayText[index] == '.') && index<strLen-1 && 
				(displayText[index+1] == ' '|| displayText[index+1] == '\n')){
					yield return new WaitForSeconds(0.3f);
				}
			
			index++;
			
			if(index<strLen){
				//play a sound potentially
				//wait for a moment before adding next character
				yield return new WaitForSeconds(0.02f);
			}
			else{
				break;
			}
		}
		textScroll = false;
	}
	
	//shake the dialogue box, maybe play a noise
	void shake(){
		
	}
	
	void OnTriggerEnter(Collider col){
		if(col.tag == "Player" && auto && !running){
			//run the dialogue
			mainCamera.gameObject.SetActive(false);
			dialogueCamera.gameObject.SetActive(true);
			running = true;
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = false;
			runDialogue();
			auto = false;
		}
	}
	
	void OnTriggerStay(Collider col){
		if(col.tag!="Player" && !auto)
			return;
		
		//if the player presses "interact"/fire1, disable player movement and
		//run the dialogue tree
		if (Input.GetButtonDown("Fire1") && !running){
			mainCamera.gameObject.SetActive(false);
			dialogueCamera.gameObject.SetActive(true);
			running = true;
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = false;
			runDialogue();
		}
	}
	
}
