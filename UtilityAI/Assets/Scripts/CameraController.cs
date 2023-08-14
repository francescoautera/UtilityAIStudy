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
			cameraButton.cameraPriority.Priority = 10;
			if (cameraButton.KeyCode == keyCode) {
				masterCamera = cameraButton.cameraPriority;
				masterCamera.Priority = 11;
			}
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
		}
	}

	private void ResetMasterCamera() { 
		currentCharacter.ResetCamera();
		currentCharacter = null;
		masterCamera.Priority = 11;
	}
	
	
	[Serializable]
	public class CameraButtonMap {
		
		public KeyCode KeyCode;
		public CinemachineVirtualCamera cameraPriority;
	}
}
