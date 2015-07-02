using UnityEngine;
using System.Collections.Generic;





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
				PlayerMovPathRenderer.enabled = true;
				PlayerMovPathRenderer.useWorldSpace = true;
				
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
				isRec = false;
				ShowMovPath();		
			}
			ShowGazePath();
		}
		
		void RecordPlayerPos() {
			endRec ();
			if(isRec == true){ 
				PlayerMovPath.Add(new RecordData(this.transform.position, destination));
			}
		}
		
		void ShowMovPath(){			
			if (PlayerMovPathRenderer) {
				PlayerMovPathRenderer.SetVertexCount (PlayerMovPath.Count);
				//Debug.Log("Rec End");
				
				for (int i=0; i<PlayerMovPath.Count; i++) {
					PlayerMovPathRenderer.SetPosition (i, PlayerMovPath[i].PlayerPosition);
					CreateGazeEmpty(PlayerMovPath[i].PlayerPosition, PlayerMovPath[i].GazePoint);
				}
			}
		}
		
		void CreateGazeEmpty(Vector3 currentPlayerPosition, Vector3 currentGazePoint){
			GameObject gazeEmpty = new GameObject("gazeEmpty");
			gazeEmpty.transform.position = currentPlayerPosition;
			gazeEmpty.AddComponent<LineRenderer>();
			gazeEmpty.tag = "GazeEmpty";
			LineRenderer emptyGazeRenderer = gazeEmpty.GetComponent<LineRenderer>();
			if (emptyGazeRenderer) {
				emptyGazeRenderer.enabled = false;
				emptyGazeRenderer.useWorldSpace = true;
				emptyGazeRenderer.SetWidth(0.01f,0.01f);
				//emptyGazeRenderer.SetColors(Color.FromKnownColor(KnownColor.Aqua), Color.FromKnownColor(KnownColor.Aqua));
				emptyGazeRenderer.SetVertexCount (2);
				emptyGazeRenderer.SetPosition (0, currentPlayerPosition);
				emptyGazeRenderer.SetPosition (1, currentGazePoint);
			}
		}
		
		private bool gazeVisible = false;
		void ShowGazePath(){		
			
			if (Input.GetKeyDown (KeyCode.R)) {
				
				//Debug.Log ("Test");
				foreach (GameObject go in GameObject.FindGameObjectsWithTag("GazeEmpty")) {
					go.GetComponent<LineRenderer>().enabled = !go.GetComponent<LineRenderer>().enabled;
					gazeVisible = !gazeVisible;
				}
				
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






