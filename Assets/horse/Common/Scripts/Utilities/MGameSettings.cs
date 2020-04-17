using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MalbersAnimations
{
    
    public class MGameSettings : MonoBehaviour
    {
        public bool HideCursor = false;
        public bool ForceFPS = false;
        [ConditionalHide("ForceFPS",true,false)]
        public int GameFPS = 60;


#if UNITY_EDITOR
        [Space,Tooltip("The Scene must be added to the Build Settings!!!")]
        public UnityEditor.SceneAsset AddAdditiveScene;
#endif
       [HideInInspector] public string sceneName;

        void Awake()
        {
            DontDestroyOnLoad(this);

            if (HideCursor)
            {
                Cursor.lockState = HideCursor ? CursorLockMode.Locked : CursorLockMode.None;  // Lock or unlock the cursor.
                Cursor.visible = !HideCursor;
            }

            if (ForceFPS)
            {
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = GameFPS;
            }

            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            sceneName = AddAdditiveScene != null ?  AddAdditiveScene.name : string.Empty; //Save the scene name into the string ||EDITOR STUFF CANNOT BE LAUNCH ON 
        }
#endif
    }
}