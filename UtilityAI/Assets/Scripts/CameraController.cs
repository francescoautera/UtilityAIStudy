using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CharacterData;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {

	[SerializeField] List<CameraButtonMap> cameraButtonMaps = new List<CameraButtonMap>();
	
	[SerializeField,ReadOnly] CinemachineVirtualCamera masterCamera;
	[SerializeField] private Camera mainCamera;
	private CharacterInfo characterInfo;
	private Character currentCharacter;
	private IEnumerable<KeyCode> allKeys;
	
	
	private void Awake() {
		characterInfo = FindObjectOfType<CharacterInfo>();
		characterInfo.OnCloseRequested += ResetMasterCamera;
		allKeys = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>();
		foreach (var cameraButton in cameraButtonMaps) {
			cameraButton.AudioListener.enabled = false;
		}
		masterCamera.GetComponent<AudioListener>().enabled = true;

	}

	private void Update() {
		if (masterCamera.Priority <= 10) {
			return;
		}
		
		foreach (var key in allKeys) {
			if (Input.GetKeyDown(key)) {
				FindKeyCode(key);
				break;
			}
		}

		if (Input.GetMouseButtonDown(0)) {
			TryHitCharacter(Input.mousePosition);
		}
	}

	private void FindKeyCode(KeyCode keyCode) {
		
		foreach (var cameraButton in cameraButtonMaps) {
			if (cameraButton.KeyCode == keyCode) {
				cameraButton.cameraPriority.Priority = 11;
				masterCamera = cameraButton.cameraPriority;
				cameraButton.AudioListener.enabled = true;
				StartCoroutine(WaitReset());
				break;
			}
		}

		
	}

	IEnumerator WaitReset() {
		yield return null;
		ResetCameras();
	}

	private void ResetCameras() {
		foreach (var cameraButton in cameraButtonMaps) {
			if(cameraButton.cameraPriority == masterCamera)
				continue;
			cameraButton.cameraPriority.Priority = 10;
			cameraButton.AudioListener.enabled = false;
		}
	}


	private void TryHitCharacter(Vector3 position) {
		
		Ray ray = mainCamera.ScreenPointToRay(position);
		if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 1000, LayerMask.GetMask("Character"))) {
			var character = hit.collider.GetComponent<Character>();
			character.SetCharacterInfo();
			currentCharacter = character;
			characterInfo.Init(character);
			masterCamera.Priority = 10;
			masterCamera.GetComponent<AudioListener>().enabled = false;
		}
	}

	private void ResetMasterCamera() { 
		currentCharacter.ResetCamera();
		currentCharacter = null;
		masterCamera.Priority = 11;
		masterCamera.GetComponent<AudioListener>().enabled = true;
	}
	
	
	[Serializable]
	public class CameraButtonMap {
		
		public KeyCode KeyCode;
		public CinemachineVirtualCamera cameraPriority;
		public AudioListener AudioListener;
	}
}
