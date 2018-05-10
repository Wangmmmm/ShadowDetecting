using UnityEngine;
using System.Collections;

    /// <summary>
    /// 创建depth相机
    /// </summary>
	[RequireComponent(typeof(Camera))]
    public class CameraForShadow : MonoBehaviour
    {

		public static Camera ShadowCamera;
        public Shader shader;
        void Awake()
        {
           
			ShadowCamera=GetComponent<Camera>();
           GetComponent<Camera>().SetReplacementShader(shader, "RenderType");
        }


        /// <summary>
        /// OnPostRender is called after a camera finishes rendering the scene.
        /// </summary>
     

    }
