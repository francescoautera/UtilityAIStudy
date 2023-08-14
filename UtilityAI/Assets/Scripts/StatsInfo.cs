using System;
using System.Collections;
using System.Collections.Generic;
using CharacterData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum statType {
   Hunger,
   Thirst,
   Fun,
   Sleepy
}

public class StatsInfo : MonoBehaviour {
   
   [SerializeField] Image fillImage;
   [SerializeField] private TMP_Text updateValues;
   private Character character;
   private bool isActive;
   public statType StatType;

   private void Awake() {
      isActive = false;
   }


   public void Init(Character characterInit) {
      character = characterInit;
      isActive = true;
   }

   public void DeInit() {
      character = null;
      isActive = false;
   }

   private void Update() {
      if (!isActive) {
         return;
      }
      var value = StatType switch
         {
            statType.Hunger => Mathf.InverseLerp(0, character.MaxValueStat, character.Hunger),
            statType.Thirst => Mathf.InverseLerp(0, character.MaxValueStat, character.Thirst),
            statType.Fun => Mathf.InverseLerp(0,character.MaxValueStat,character.Fun),
            statType.Sleepy => Mathf.InverseLerp(0,character.MaxValueStat,character.Sleepy),
            _ => 0f
         };

         fillImage.fillAmount = value;
         updateValues.text = $"{(int) (value* 100)} / {character.MaxValueStat}";
   }

}
