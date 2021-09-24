// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit360"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		_Progress("Progress", Range( 0 , 1)) = 0.2875291
		_Radius("Radius", Range( 0 , 1)) = 0.5728233
		_Color1("Color1", Color) = (0.0518868,0.6635301,1,1)
		_Color2("Color2", Color) = (1,1,1,1)
		_Texture("Texture", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		
		Stencil
		{
			Ref [_Stencil]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			CompFront [_StencilComp]
			PassFront [_StencilOp]
			FailFront Keep
			ZFailFront Keep
			CompBack Always
			PassBack Keep
			FailBack Keep
			ZFailBack Keep
		}


		Cull Off
		Lighting Off
		ZWrite On
		ZTest [_Always]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		
		Pass
		{
			Name "Default"
		CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform sampler2D _Texture;
			uniform float4 _Texture_ST;
			uniform float4 _Color1;
			uniform float _Radius;
			uniform float _Progress;
			uniform float4 _Color2;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.worldPosition = IN.vertex;
				
				
				OUT.worldPosition.xyz +=  float3( 0, 0, 0 ) ;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				float2 uv_Texture = IN.texcoord.xy * _Texture_ST.xy + _Texture_ST.zw;
				float4 tex2DNode47 = tex2D( _Texture, uv_Texture );
				float2 uv05 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 temp_cast_0 = (0.0).xx;
				float2 temp_cast_1 = (1.0).xx;
				float2 temp_cast_2 = (-1.0).xx;
				float2 temp_cast_3 = (1.0).xx;
				float2 temp_output_12_0 = (temp_cast_2 + (uv05 - temp_cast_0) * (temp_cast_3 - temp_cast_2) / (temp_cast_1 - temp_cast_0));
				float temp_output_34_0 = (( _Radius > length( temp_output_12_0 ) ) ? 1.0 :  0.0 );
				float2 temp_cast_4 = (0.0).xx;
				float2 temp_cast_5 = (1.0).xx;
				float2 temp_cast_6 = (-1.0).xx;
				float2 temp_cast_7 = (1.0).xx;
				float cos44 = cos( ( -0.5 * UNITY_PI ) );
				float sin44 = sin( ( -0.5 * UNITY_PI ) );
				float2 rotator44 = mul( temp_output_12_0 - float2( 0,0 ) , float2x2( cos44 , -sin44 , sin44 , cos44 )) + float2( 0,0 );
				float2 break23 = rotator44;
				float temp_output_29_0 = (( (0.0 + (atan2( break23.y , break23.x ) - ( -1.0 * UNITY_PI )) * (1.0 - 0.0) / (UNITY_PI - ( -1.0 * UNITY_PI ))) > ( 1.0 - _Progress ) ) ? 1.0 :  0.0 );
				
				half4 color = ( ( tex2DNode47 * _Color1 * temp_output_34_0 * temp_output_29_0 ) + ( _Color2 * ( 1.0 - temp_output_29_0 ) * temp_output_34_0 * tex2DNode47 ) );
				
				#ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17900
260;73;1231;603;-951.8119;74.12573;1.45612;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-1055.128,-6.429214;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-942.9695,295.8433;Inherit;False;Constant;_Float1;Float 1;0;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-934.9695,528.8433;Inherit;False;Constant;_Float3;Float 3;0;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-580.6908,647.026;Inherit;False;Constant;_Float7;Float 7;3;0;Create;True;0;0;False;0;-0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-938.9695,382.8433;Inherit;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-932.9695,450.8433;Inherit;False;Constant;_Float2;Float 2;0;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;12;-741.9612,295.2658;Inherit;True;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,0;False;3;FLOAT2;0,0;False;4;FLOAT2;1,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PiNode;45;-395.949,664.7695;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;44;-434.8841,409.8435;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-85.92068,645.4313;Inherit;False;Constant;_Float5;Float 5;0;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;23;-225.8596,374.8705;Inherit;True;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ATan2OpNode;21;73.60122,382.5361;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;562.5127,482.0714;Inherit;False;Property;_Progress;Progress;0;0;Create;True;0;0;False;0;0.2875291;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;28;44.27931,732.3314;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;26;85.2793,652.5311;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;97.07924,849.6307;Inherit;False;Constant;_Float4;Float 4;0;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;32;838.3093,491.7133;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;24;382.7791,651.53;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;647.8702,402.3816;Inherit;False;Constant;_Float6;Float 6;0;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;562.7296,46.38753;Inherit;False;Property;_Radius;Radius;1;0;Create;True;0;0;False;0;0.5728233;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;11;607.2323,150.6516;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareGreater;29;1135.862,585.1938;Inherit;True;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;55;1509.484,577.8007;Inherit;False;Property;_Color2;Color2;3;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;56;1549.403,785.3671;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;36;1542.856,262.1768;Inherit;False;Property;_Color1;Color1;2;0;Create;True;0;0;False;0;0.0518868,0.6635301,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCCompareGreater;34;1142.39,317.4899;Inherit;True;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;47;1474.787,65.5298;Inherit;True;Property;_Texture;Texture;4;0;Create;False;0;0;False;0;-1;64e7766099ad46747a07014e44d0aea1;64e7766099ad46747a07014e44d0aea1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;1856.761,597.7586;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;2044.429,280.2645;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;59;2306.489,415.4725;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;54;1162.865,107.2091;Inherit;False;Constant;_Always;Always;5;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;53;2555.541,286.2552;Float;False;True;-1;2;ASEMaterialInspector;0;4;Unlit360;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;True;2;False;-1;True;True;True;True;True;0;True;-9;True;True;0;True;-5;255;True;-8;255;True;-7;0;True;-4;0;True;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;0;True;54;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;12;0;5;0
WireConnection;12;1;14;0
WireConnection;12;2;13;0
WireConnection;12;3;15;0
WireConnection;12;4;16;0
WireConnection;45;0;46;0
WireConnection;44;0;12;0
WireConnection;44;2;45;0
WireConnection;23;0;44;0
WireConnection;21;0;23;1
WireConnection;21;1;23;0
WireConnection;26;0;27;0
WireConnection;32;0;31;0
WireConnection;24;0;21;0
WireConnection;24;1;26;0
WireConnection;24;2;28;0
WireConnection;24;3;25;0
WireConnection;11;0;12;0
WireConnection;29;0;24;0
WireConnection;29;1;32;0
WireConnection;29;2;30;0
WireConnection;56;0;29;0
WireConnection;34;0;35;0
WireConnection;34;1;11;0
WireConnection;34;2;30;0
WireConnection;58;0;55;0
WireConnection;58;1;56;0
WireConnection;58;2;34;0
WireConnection;58;3;47;0
WireConnection;33;0;47;0
WireConnection;33;1;36;0
WireConnection;33;2;34;0
WireConnection;33;3;29;0
WireConnection;59;0;33;0
WireConnection;59;1;58;0
WireConnection;53;0;59;0
ASEEND*/
//CHKSM=862B324549F25F080C2748C075949783950B1607