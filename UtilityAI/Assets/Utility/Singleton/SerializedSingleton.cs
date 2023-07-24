using UnityEngine;
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

#if ODIN_INSPECTOR
        public class SerializedSingleton<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour
        {
            private static T instance;
            public static T Instance {
                get {
                    return instance;
                }
            }

            public void Awake()
            {
                if (!instance)
                {
                    if (typeof(T) != GetType())
                    {
                        Destroy(this);
                        throw new Exception("Singleton instance type mismatch!");
                    }
                    instance = this as T;
                }
                else
                {
                    Destroy(this);
                }
            }

        }
        
        
        
        public class SerializedSingletonDDOL<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour
        {
            private static T instance;
            public static T Instance {
                get {
                    return instance;
                }
            }

            public void Awake()
            {
                if (!instance)
                {
                    if (typeof(T) != GetType())
                    {
                        Destroy(this);
                        throw new Exception("Singleton instance type mismatch!");
                    }
                    instance = this as T;
                    DontDestroyOnLoad(this.gameObject);
                }
                else
                {
                    Destroy(this);
                }
            }

        }
#endif
 


