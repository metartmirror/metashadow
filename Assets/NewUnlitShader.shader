Shader "Unlit/InverseCullCubeMapShader"
{
    Properties
    {
        _CubeMap( "Cube Map", Cube ) = "white" {}
    }
    SubShader
    {
        Pass 
        {
            Tags { "DisableBatching" = "True" }

            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
        
            samplerCUBE _CubeMap;
        
            struct v2f 
            {
                float4 pos : SV_Position;
                half3 uv : TEXCOORD0;
            };
        
            v2f vert( appdata_img v )
            {
                v2f o;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.uv = v.vertex.xyz * half3(-1,1,1); // mirror so cubemap projects as expected
                return o;
            }
        
            fixed4 frag( v2f i ) : SV_Target 
            {
                return texCUBE( _CubeMap, i.uv );
            }
            ENDCG
        }
    }
}