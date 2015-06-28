using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GazeLaser : MonoBehaviour {
	private LineRenderer gazeRenderer;
	private LineRenderer PlayerMovPathRenderer;
	private Transform origin;
	private Transform destination;
	private bool wasReleased = true;
	private bool isRec = true;
	public List<RecordData> PlayerMovPath = new List<RecordData>();

	// Use this for initialization
	void Start () {
		//if(gazeRenderer == null)
		//{
		origin = this.transform;


		gazeRenderer = this.GetComponent<LineRenderer> ();
		if (gazeRenderer != null) {
		gazeRenderer.enabled = true;
		gazeRenderer.useWorldSpace = true;
		gazeRenderer.SetVertexCount (2);
		gazeRenderer.SetPosition (0, origin.position);
		}

		PlayerMovPathRenderer = Camera.main.GetComponent<LineRenderer>();
		if (PlayerMovPathRenderer) {
			PlayerMovPathRenderer.enabled = true;
			PlayerMovPathRenderer.useWorldSpace = true;
			
		}

		//MouseMov1 = GetComponent<MouseLook>();



		//}
	}
	
	// Update is called once per frame
	void Update () {

		Ray gaze = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		//SimulateGaze();
		RecordPlayerPos();


		if (true){
			if(Physics.Raycast(gaze, out hit) == true)
			{
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
	//MouseLook mouseLook = this.GetComponent<MouseLook>();

	KeyPressed (KeyCode.G);
	if ((Input.GetKeyDown (KeyCode.G)) && wasReleased == false) {
			this.GetComponent<MouseLook>().enabled = false;
			Camera.main.GetComponent<MouseLook>().enabled = false;
		} 
	if (wasReleased == true) 
		{
			this.GetComponent<MouseLook>().enabled = true;	
			Camera.main.GetComponent<MouseLook>().enabled = true;
		}
	}



	void endRec(){
		if ((Input.GetKeyDown (KeyCode.E))) {
			isRec = false;
			ShowMovPath();

		}
	}

	void RecordPlayerPos() {
		endRec ();
		if(isRec == true){ 
			PlayerMovPath.Add(new RecordData(this.transform.position));
			Debug.Log("Test");
		}
	}

	void ShowMovPath(){			
		if (PlayerMovPathRenderer) {
			PlayerMovPathRenderer.SetVertexCount (PlayerMovPath.Count);
			//Debug.Log("Rec End");
		
			for (int i=0; i<PlayerMovPath.Count; i++) {
				PlayerMovPathRenderer.SetPosition (i, PlayerMovPath [i].PlayerPosition);
				//Debug.Log(PlayerMovPath[i].PlayerPosition);
				
			}
		}




	}



}
//https://www.youtube.com/watch?v=P0PHY1hJp5k
//https://www.youtube.com/watch?v=Bqcu94VuVOI
//http://answers.unity3d.com/questions/782409/how-to-recordsave-and-replayload-a-players-movemen.html


//Gathering all Data about Position, direction and gaze for every Frame
public class RecordData {
	
	private int dataID;
	private Vector3 playerPosition;
	
	public RecordData(Vector3 currentPlayerPosition){
		//dataID = currentDataID;
		playerPosition = currentPlayerPosition;
		
	}
	
	
	public Vector3 PlayerPosition {
		get {
			return playerPosition;
		}
	}
	//https://www.youtube.com/watch?v=Ul_rdZqGhJw
}





