using System;
using System.Collections;
using System.Collections.Generic;
using CharacterData;
using UnityEngine;
using UnityEngine.UI;

public class HealthInfo : MonoBehaviour {
   
   [SerializeField] Color normalColor;
   [SerializeField] private Color notUsedColor;
   [SerializeField] private List<Vector2> thresholds = new List<Vector2>();
   [SerializeField] private List<Image> imagesHealth = new List<Image>();
   private Character character;
   private bool isActive;

   private void Awake() {
   }

   public void Init(Character currCharacter) {
	   character = currCharacter;
       isActive = true;
   }

   public void DeInit() {
	   character = null;
       isActive = false;
   }

   public void Update() {
	   if (!isActive) {
		   return;
	   }

	   var healthCharacter = character.Health;
	   for (int i = 0; i < thresholds.Count; i++) {
		   var values = thresholds[i];
		   if (healthCharacter >= values.x && healthCharacter <= values.y) {
			   imagesHealth[i].color = normalColor;
			   UpdateOtherColors(i);
			   return;
		   }
	   }
   }


   private void UpdateOtherColors(int x) {
	   for (int i = 0; i < imagesHealth.Count; i++) {
		   if (i == x) {
			   continue;
		   }
		   imagesHealth[i].color = notUsedColor;
	   }
   }
}
