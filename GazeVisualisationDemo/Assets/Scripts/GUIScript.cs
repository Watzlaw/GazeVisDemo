using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GUIScript : MonoBehaviour {

	public bool display;
	public Button endRecord;
	public Text recordingInfo;
	public Toggle playerMovPath;
	public Toggle playerGazeOnMovPath;
	public Toggle playerGazeTime;
	public GameObject recorder;
	public GameObject canvas;


	// Use this for initialization
	void Start () {

	}

	void Update () {
		
		showRecordingStatus ();
		hideGUI ();
		if (display == true) {
			canvas.GetComponent<Canvas>().enabled = true;
		} else {
			canvas.GetComponent<Canvas>().enabled = false;
		}
	
	}
	
	
	private void OnGUI(){
	}
	
	
	private void showRecordingStatus(){
		if (isRecording()) {
			recordingInfo.text = "Recording Data";
			recordingInfo.color = Color.red;
			endRecord.GetComponentInChildren<Text> ().color = Color.red;
			endRecord.GetComponentInChildren<Text>().text = "End Recording";
			playerMovPath.interactable = false;
			playerGazeOnMovPath.interactable = false;;
			if (playerGazeOnMovPath.isOn == false){
				playerGazeTime.interactable = false;
				playerGazeTime.isOn = false;
			}


		} else {
			recordingInfo.text = "Not recording...";
			recordingInfo.color = Color.white;
			endRecord.GetComponentInChildren<Text> ().color = Color.green;
			endRecord.GetComponentInChildren<Text>().text = "Start Recording";
			playerMovPath.interactable = true;
			playerGazeOnMovPath.interactable = true;

			if (playerGazeOnMovPath.isOn == true){
			playerGazeTime.interactable = true;
			}
		}
	}

	private bool isRecording(){		
		if (recorder.GetComponent<GazeLaser>().IsRec) {
			return true;
		} else {
			return false;
		}			
	}

	private void hideGUI(){
		if (Input.GetKeyDown (KeyCode.Tab)) {
			display = !display;
		}	
	}
		


}
