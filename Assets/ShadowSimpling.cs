using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    /// <summary>
    /// 负责阴影的采样
    /// </summary>
public class ShadowSimpling : MonoBehaviour {

	/// <summary>
	/// 阴影检测点
	/// </summary>
	[SerializeField]
	private Transform shadowCheckPoint;

	/// <summary>
	/// 需要采样的深度图
	/// </summary>
	public RenderTexture depthTexture;


	/// <summary>
	/// 阴影摄像机
	/// </summary>
	[HideInInspector]
	public Camera shadowCamera;
	/// <summary>
	/// 判断是否在阴影中的接口
	/// </summary>
	public bool isInShadow;

	//用于将rendertexture加载到内存,unity不能直接对rt进行采样，需要转换为Texture2D
	Texture2D tex;

	void Start () {
		
	
		shadowCamera=   CameraForShadow.ShadowCamera.GetComponent<Camera>();
		tex=getTexture2d(depthTexture);

	}
	void Update()
	{
		
		//将监测点坐标空间转换到裁剪空间
		Vector4 Pwolrd=new Vector4(shadowCheckPoint.position.x,shadowCheckPoint.position.y,shadowCheckPoint.position.z,1);
		Vector4 Pcamera=shadowCamera.worldToCameraMatrix*Pwolrd;
		Vector4 pos= shadowCamera.projectionMatrix*Pcamera;
		double depth=pos.z/pos.w;
		


		

		
		
		//根据监测点在裁剪空间下的横纵坐标采样RT，获取摄像机获取到的深度值
		int width = depthTexture.width;
        int height = depthTexture.height;
		Vector3 screenpos=new Vector3(pos.x/pos.w*width/2+width/2,pos.y/pos.w*height/2+height/2,pos.z/pos.w*0.5f+0.5f);
        RenderTexture.active = depthTexture;
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
		Color color= tex.GetPixel((int) screenpos.x,(int) screenpos.y);
		float sample=color.r;


		//进行采样对比
		if((screenpos.z-sample)>0.003f)isInShadow=true;
		else isInShadow=false;

	}

	/// <summary>
	/// 将rt转换为texture2D
	/// </summary>
	/// <param name="renderT"></param>
	/// <returns></returns>
  public Texture2D getTexture2d(RenderTexture renderT)
    {
        if (renderT == null)
            return null;

        int width = renderT.width;
        int height = renderT.height;
        Texture2D tex2d = new Texture2D(width, height, TextureFormat.ARGB32, false);
        RenderTexture.active = renderT;
        tex2d.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex2d.Apply();

    
        return tex2d;
    }

	



}