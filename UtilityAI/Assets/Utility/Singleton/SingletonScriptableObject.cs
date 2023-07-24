using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

        /// <summary>
        /// Abstract class for making reload-proof singletons out of ScriptableObjects
        /// Returns the asset created on the editor, or null if there is none
        /// Based on https://www.youtube.com/watch?v=VBA1QCoEAX4
        /// </summary>
        /// <typeparam name="T">Singleton type</typeparam>
#if ODIN_INSPECTOR
        public abstract class SingletonScriptableObject<T> : SerializedScriptableObject where T : SerializedScriptableObject {
#else
        public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
        {
#endif
            private static T instance = null;
            public static T Instance {
                get {
                    if (!instance)
                        instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
#if UNITY_EDITOR
                    if (!instance)
                    {
                        string[] configsGUIDs = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
                        if (configsGUIDs.Length > 0)
                        {
                            instance = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(configsGUIDs[0]));
                        }
                    }
#endif
                    return instance;
                }
            }

#if UNITY_EDITOR
            public void OnEnable()
            {
                AddToPreloaded();
            }

            private void AddToPreloaded()
            {
                if (Instance != null)
                {
                    var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
                    if (!preloadedAssets.Contains(Instance))
                    {
                        preloadedAssets.Add(Instance);
                        PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
                    }
                }
            }
#endif
        }
  

