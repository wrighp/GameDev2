using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

//Needs to have a canvas in the scene (doesn't have to be attached to camera)
//player could have a hash table <string, bool> of tasks to perform
//for dialogue options and triggers, could check event name as such to see if 
//the player has done it

public class npcDialogue : MonoBehaviour {
	
	//the dialogue tree that's being referenced
	Dialogue dialogue;
	
	public LayerMask layer;
	public float talkDistance = 5f;
	public float talkCooldown = 2f;
	bool coolingDown = false;
	
	//the dialogue box info
	GameObject Name;
	GameObject nodeText;
	
	GameObject[] options;
	GameObject[] availableOptions;
	
	public GameObject buttonPrefab;
	RectTransform ParentPanel;

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
	private bool protection = false;
	
	//the list of tasks and achievements the player has accomplished or rather, hasn't
	public static Dictionary<string, bool> tasks;
	public static Dictionary<string, bool> chapterTasks;
	public static string[] taskList;
	public static string[] chapterTaskList;
	public static int numTasks = 4;
	
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
		
		ParentPanel = dialogueWindow.transform.Find("ButtonPanel").gameObject.
			GetComponent<RectTransform>();
		
		nodeText = dialogueWindow.transform.Find("[dialogue]").gameObject;
		
		dialogueWindow.SetActive(false);
		
		//to test
		//move this runDialogue to a different function later
		//runDialogue();
		
	}
	
	void Update()
	{
		if(coolingDown)
		{
			talkCooldown+=Time.deltaTime;
		}
		if(coolingDown && talkCooldown>0.5f)
		{
			talkCooldown = 0.5f;
			coolingDown = false;
		}
	}
	
	private void initiateTasks(){
		tasks = new Dictionary<string, bool>();
		taskList = new string[numTasks];
		tasks["A"] = false;
		tasks["B"] = false;
		tasks["C"] = false;
		tasks["D"] = false;
		
		taskList[0] = "A";
		taskList[1] = "B";
		taskList[2] = "C";
		taskList[3] = "D";
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
		
		options = new GameObject[node._options.Count];
		
		//loop through all of this node's possible options and display them
		for(int i=0;i<node._options.Count;i++){
			updateButton(node, node._options[i], i);
		}
	}
	
	void updateButton(Node node, dialogueOption option, int index)
	{
		if(option._req != null && option._req != ""){
			//if the dictionary[option._req] == false, return
			if(!tasks[option._req])
				return;
		}
		
		GameObject newButton = (GameObject) Instantiate(buttonPrefab);
		newButton.transform.SetParent(ParentPanel, false);
		newButton.transform.localScale = new Vector3(1, 1, 1);
			
		newButton.GetComponentInChildren<Text>().text = option._text;
		Button tmpButton = newButton.GetComponent<Button>();
		tmpButton.onClick.AddListener(delegate{
		setSelect(option._dest);});
		
		options[index] = newButton;
		
		//if there's more than one option, display it
		if(node._options.Count > 1)
		{
			newButton.SetActive(true);
		}
		else
		{
			newButton.SetActive(false);
		}
	}
	
	//run the dialogue tree coroutine
	public IEnumerator run(){
		//THIS WILL DO FOR NOW
		yield return new WaitForEndOfFrame();

		dialogueWindow.SetActive(true);
		
		nodeID = dialogue._next;
		
		Node current;
		//while the node isn't an exit node...
		while(nodeID!=-1){
			current = dialogue._nodes[nodeID];
			updateText(current);
			//testing out the execute function
			current._precalls.ForEach ((Call c) => c.execute ());
			
			//add accomplishment to dictionary
			if(current._accomplish!=null && 
				current._accomplish!=""){
				tasks[current._accomplish] = true;
				Debug.Log("ACCOMPLISHED " + current._accomplish);
				checkStatus();
			}
			
			//set the corresponding button active
			//for some reason, this works,  not complaining
			int j=0;
			for(int i=0; i<current._options.Count;i++){
				if(options[i].activeSelf){
					//options[i].GetComponent<Button>().Select();
					availableOptions[j] = options[i];
					j++;
				}
			}
			
			
			select = -2;
			
			int index = 0;
			int direction;
			float selCooldown = 0.25f;
			
			//only one option, fire1 goes straight to the next one
			if(j == 0){
				while(select == -2){
					if (Input.GetButtonDown("Fire1")&& !textScroll && running
						&& selCooldown<0){
						//invoke a click through script
						options[0].GetComponent<Button>().onClick.Invoke();
						
					}
					selCooldown -=Time.deltaTime;
					yield return /*new WaitForEndOfFrame()*/null;
				}
			}
			else{
				availableOptions[0].GetComponent<Button>().Select();
				
				//cooldown to make selection less sensitive
				while(select == -2){
						
						availableOptions[index].GetComponent<Button>().Select();
						
						direction = -(int) Input.GetAxisRaw("Vertical");
						
						if(selCooldown<0){
							index+=direction;
							
							//bind the indices
							if(index<0)
								index = 0;
							if(index>j-1)
								index = j-1;
						
						}

						//testing
						if(direction!=0 && selCooldown<0){
							selCooldown = 0.25f;
						}
						
						selCooldown -= Time.deltaTime;

						if (Input.GetButtonDown("Fire1") && !textScroll && running 
							&& selCooldown<0){
							availableOptions[index].GetComponent<Button>().onClick.Invoke();
							selCooldown = 0.25f;
							
						}
						yield return /*new WaitForEndOfFrame()*/null;
				}
			}
			//destroy the buttons
			for(int i=0; i<current._options.Count;i++)
			{
				Destroy(options[i]);
			}
			
			current._postcalls.ForEach ((Call c) => c.execute ());
			dialogue._next = current._reset;
			nodeID = select;
		}
		running = false;
		coolingDown = true;
		talkCooldown = 0f;
		mainCamera.gameObject.SetActive(true);
		dialogueCamera.gameObject.SetActive(false);
		dialogueWindow.SetActive(false); 
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = true;
	}
	
	//coroutine for displaying the text
	//handle text wrapping too
	private IEnumerator DisplayText(string displayText){
		int strLen = displayText.Length;
		int index = 0;
		
		nodeText.GetComponent<Text>().text = "";
		textScroll = true;
		
		while(true){
			//if the player presses fire1, just put the text and get out
			if (Input.GetButtonDown("Fire1") && running){
				nodeText.GetComponent<Text>().text = (string)displayText;
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
	
	void OnTriggerEnter(Collider col){
		//Debug.Log(col.tag);
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
	
	//have a talking cooldown so the player doesn't automatically jump back when they
	//exit conversation
	void OnTriggerStay(Collider col){
		if(auto || col.tag!="Player")
			return;

		//if the player presses "interact"/fire1, disable player movement and
		//run the dialogue tree
		if (Input.GetButtonDown("Fire1") && !running && !coolingDown){
			//get player's forward facing direction
			Vector3 p4 = col.transform.TransformDirection(Vector3.forward);
			Debug.Log("Player in area");
			
			RaycastHit hit;
			//raycast from player, the player's forward, store it in hit, of distance hit
			if(Physics.SphereCast(col.transform.position, 1, p4, out hit, talkDistance, layer))
			{
				Debug.Log("Let's talk?");
				Debug.Log(hit.collider.gameObject);
				Debug.Log(this);
				//if the ray hits this character, run the thing
				if(hit.collider.gameObject == this.transform.parent.gameObject)
				{
					mainCamera.gameObject.SetActive(false);
					dialogueCamera.gameObject.SetActive(true);
					running = true;
					GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = false;
					runDialogue();
				}
			}
			else if(Vector3.Distance(col.transform.position, transform.position) < 1)
			{
				mainCamera.gameObject.SetActive(false);
				dialogueCamera.gameObject.SetActive(true);
				running = true;
				GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = false;
				runDialogue();
			}
		}
			
	}
	
	//cycle through all tasks, if they're all complete, you win!
	void checkStatus(){
		for(int i=0; i<numTasks;i++){
			Debug.Log("CYCLING..." + taskList[i]);
			if(tasks[taskList[i]] == false)
				return;
		}
		Debug.Log("YOU DID IT!");
		//end the game
		SceneManager.LoadScene("Win");
	}
	
}
