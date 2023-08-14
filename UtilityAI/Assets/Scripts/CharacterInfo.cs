using System.Collections;
using System.Collections.Generic;
using CharacterData;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UtilityAI;
using Action = System.Action;

public class CharacterInfo : MonoBehaviour {

   public Action OnCloseRequested;
   [SerializeField] Button closeButton;
   [SerializeField] private TMP_Text actionText;
   [SerializeField] private List<StatsInfo> stats;
   [SerializeField] private GameObject panelInfo;
   [SerializeField] private HealthInfo healthInfo;
   [SerializeField] private Canvas canvas;
   private Character currCharacter;
   
   

   private void Awake() {
      canvas.worldCamera = Camera.current; 
      closeButton.onClick.AddListener(() => {
         OnCloseRequested?.Invoke();
         DeInit();
      });
      panelInfo.SetActive(false);
      var thinkers = FindObjectsOfType<Thinker>();
      foreach (var thinker in thinkers) {
         thinker.OnObjectSatisfyRequested += TryUpdateActionName;
      }
   }

   [Button]
   public void Init(Character character) {
      currCharacter = character;
      panelInfo.SetActive(true);
      foreach (var stat in stats) {
         stat.Init(currCharacter);
      }
      healthInfo.Init(currCharacter);
   }

   private void DeInit() {
      currCharacter = null;
      foreach (var stat in stats) {
         stat.DeInit();
      }
      healthInfo.DeInit();
      panelInfo.SetActive(false);
   }


   private void TryUpdateActionName(UtilityAI.Action action, Thinker thinker) {
      if(currCharacter == null)
         return;
      bool isSameCharacter = currCharacter == thinker.GetComponent<Character>();
      if (!isSameCharacter) {
         return;
      }
      UpdateActionName(action.Title);
   }
   
   public void UpdateActionName(string nameAction) => actionText.text = nameAction;
   
   
   
}
