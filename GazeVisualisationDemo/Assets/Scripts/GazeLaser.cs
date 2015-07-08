using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;





//namespace UnityStandardAssets.Characters.FirstPerson
//{
	
	public class GazeLaser : MonoBehaviour {
		
		private LineRenderer gazeRenderer;
		private LineRenderer PlayerMovPathRenderer;
		private Transform origin;
		private Vector3 destination;
		private bool wasReleased = true;
		private bool isRec = true;
		public List<RecordData> PlayerMovPath = new List<RecordData>();
		public GameObject test;
		private GameObject gazeEmpty;
		private GUI GazeInfo;
		private bool showGazeInfo;
		private bool ShowGUI;
		
		// Use this for initialization
		void Start () {			
			origin = this.transform;
			
		if (this.GetComponent<LineRenderer>() == null) gameObject.AddComponent<LineRenderer>();
			gazeRenderer = this.GetComponent<LineRenderer> ();
			gazeRenderer.enabled = true;
			gazeRenderer.SetWidth(0.05f,0.01f);
			gazeRenderer.useWorldSpace = true;
			gazeRenderer.SetVertexCount (2);
			gazeRenderer.SetPosition (0, origin.position);

			
			PlayerMovPathRenderer = Camera.main.GetComponent<LineRenderer>();
			if (PlayerMovPathRenderer) {
				PlayerMovPathRenderer.enabled = false;
				PlayerMovPathRenderer.useWorldSpace = true;
				
			}
			
		gazeEmpty = new GameObject("gazeEmpty");
		showGazeInfo = false;
		}

	void OnGUI(){

		if (IsRec == false &&  showGazeInfo == true){
			for (int i=0; i<PlayerMovPath.Count; i++) {
				Vector3 toTarget = (PlayerMovPath[i].GazePoint - transform.position).normalized;
				
				if (Vector3.Dot(toTarget, transform.forward) > 0)               //Check if object is in front of player. If it's...
				{
					ShowGUI = true;
				} else {                //if it's not...
					ShowGUI = false;
				}
				if (ShowGUI == true)ShowGazeTime(PlayerMovPath[i].GazePoint, i);
			}

		}
	}


		
		// Update is called once per frame
		void Update () {
			Ray gaze = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			
			RecordPlayerPos();
			SimulateGaze();
			
			
			if (true){
				if(Physics.Raycast(gaze, out hit) == true)
				{
					destination =  hit.point;
					Debug.DrawRay(gaze.origin, gaze.direction*hit.distance, Color.red);
					//Debug.Log("Gaze hit = " + hit.transform.gameObject.tag);
					//Debug.Log("Gaze origin = " + gaze.origin);
					if (gazeRenderer) {
						gazeRenderer.enabled = true;
						gazeRenderer.SetPosition(0,origin.position);
						gazeRenderer.SetPosition(1,hit.point);
					}
					
				}
				else{
					Debug.DrawRay(gaze.origin, gaze.direction*100, Color.cyan);
					
					gazeRenderer.enabled = false;
				}
			}
		}
		
		
		
		void KeyPressed (KeyCode keyToPress){
			
			if (Input.GetKeyDown (keyToPress)) {
				wasReleased = false;				
			} 
			
			if( Input.GetKeyUp (keyToPress) ){
				wasReleased = true;
				
			}	
			
		}	
		
		void SimulateGaze(){
			
//			KeyPressed (KeyCode.G);
//			if ((Input.GetKeyDown (KeyCode.G)) && wasReleased == false) {
//				if(GetComponent<FirstPersonController>())
//					this.GetComponent<FirstPersonController>().enabled = false;
//				//Camera.main.GetComponent<MouseLook>().enabled = false;
//			} 
//			if (wasReleased == true) 
//			{
//				if(GetComponent<FirstPersonController>())
//					this.GetComponent<FirstPersonController>().enabled = true;	
//			}
		}
		
		
		
		void endRec(){
			if ((Input.GetKeyDown (KeyCode.E))) {
			GameObject.Find("TogglePlayerMovPath").GetComponent<Toggle>().isOn = !GameObject.Find("TogglePlayerMovPath").GetComponent<Toggle>().isOn;
			IsRec = false;
				
			}
			if (Input.GetKeyDown (KeyCode.R)) {
			GameObject.Find("TogglePlayerGazeOnMovePath").GetComponent<Toggle>().isOn = !GameObject.Find("TogglePlayerGazeOnMovePath").GetComponent<Toggle>().isOn;
			}
			if (Input.GetKeyDown (KeyCode.T)) {
			GameObject.Find("TogglePlayerGazeTime").GetComponent<Toggle>().isOn = !GameObject.Find("TogglePlayerGazeTime").GetComponent<Toggle>().isOn;
			}
	}
	
		void RecordPlayerPos() {
			endRec ();
			if(isRec == true){ 
				PlayerMovPath.Add(new RecordData(this.transform.position, destination));
			}
		}
		
		public void ShowMovPath(){			
			if (PlayerMovPathRenderer) {
				PlayerMovPathRenderer.SetVertexCount (PlayerMovPath.Count);
				PlayerMovPathRenderer.enabled = !PlayerMovPathRenderer.enabled;
				
			if(IsRec == true){
				for (int i=0; i<PlayerMovPath.Count; i++) {
					PlayerMovPathRenderer.SetPosition (i, PlayerMovPath[i].PlayerPosition);
					CreateGazeEmpty(PlayerMovPath[i].PlayerPosition, PlayerMovPath[i].GazePoint);
				}
			}
			}
		}

	void CreateGazeEmpty(Vector3 currentPlayerPosition, Vector3 currentGazePoint){
		GameObject gazeEmptyInstance;
		gazeEmptyInstance = Instantiate (gazeEmpty) as GameObject;
		gazeEmptyInstance.transform.position = currentPlayerPosition;
		gazeEmptyInstance.AddComponent<LineRenderer>();
		gazeEmptyInstance.tag = "GazeEmpty";
		LineRenderer emptyGazeRenderer = gazeEmptyInstance.GetComponent<LineRenderer>();
		if (emptyGazeRenderer) {
			emptyGazeRenderer.enabled = false;
			emptyGazeRenderer.useWorldSpace = true;
			emptyGazeRenderer.SetWidth(0.01f,0.01f);
			emptyGazeRenderer.SetVertexCount (2);
			emptyGazeRenderer.SetPosition (0, currentPlayerPosition);
			emptyGazeRenderer.SetPosition (1, currentGazePoint);
		}
	}
	

		private bool gazeVisible = false;
		public void ShowGazePath(){		
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("GazeEmpty")) {
					go.GetComponent<LineRenderer>().enabled = !go.GetComponent<LineRenderer>().enabled;
					gazeVisible = !gazeVisible;	
					
			}
			
		}


	float boxW = 50f;
	float boxH = 25f;
	private void ShowGazeTime(Vector3 pos, int i)
	{

			Vector2 TextLocation = Camera.main.WorldToScreenPoint(pos);			
			TextLocation.y = Screen.height - TextLocation.y;			
			TextLocation.x -= boxW * 0.5f;
			TextLocation.y -= boxH * 0.5f;			
			GUI.Box(new Rect(TextLocation.x, TextLocation.y, boxW, boxH), i.ToString());

	}

	public void ShowGazeInfo(){
		showGazeInfo = !showGazeInfo;
	}


	//http://pastebin.com/2eBVsR4z
	
	public bool IsRec {
		get {
			return isRec;
		}
		set {
			isRec = value;
		}
	}
	}
//}
//https://www.youtube.com/watch?v=P0PHY1hJp5k
//https://www.youtube.com/watch?v=Bqcu94VuVOI
//http://answers.unity3d.com/questions/782409/how-to-recordsave-and-replayload-a-players-movemen.html


//Gathering all Data about Position, direction and gaze for every Frame
public class RecordData {
	
	private int dataID;
	private Vector3 playerPosition;
	private Vector3 gazePoint;
	
	
	
	public RecordData(Vector3 currentPlayerPosition, Vector3 currentGazePoint){
		//dataID = currentDataID;
		playerPosition = currentPlayerPosition;
		gazePoint = currentGazePoint;
		
	}
	
	
	
	
	public Vector3 PlayerPosition {
		get {
			return playerPosition;
		}
	}
	
	public Vector3 GazePoint {
		get {
			return gazePoint;
		}
	}
	//https://www.youtube.com/watch?v=Ul_rdZqGhJw
}
//}



/// Open tag manager
//	SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
//SerializedProperty tagsProp = tagManager.FindProperty("tags");
//
//// For Unity 5 we need this too
//SerializedProperty layersProp = tagManager.FindProperty("layers");
//
//// Adding a Tag
//string s = "the_tag_i_want_to_add";
//
//// First check if it is not already present
//bool found = false;
//for (int i = 0; i < tagsProp.arraySize; i++)
//{
//	SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
//	if (t.stringValue.Equals(s)) { found = true; break; }
//}
//
//// if not found, add it
//if (!found)
//{
//	tagsProp.InsertArrayElementAtIndex(0);
//	SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
//	n.stringValue = s;
//}






