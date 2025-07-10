Shader "Custom/PinballTracker"
{
    Properties
    {
        _MainTex ("Tracking Texture", 2D) = "black" {}
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        Pass
        {
            ZWrite Off
            ZTest Always
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            float4 _PinballPositions[32];
            float4 _PreviousPositions[32];
            int _PinballCount;
            float2 _TextureSize;
            float _TrackingRadius;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 pixelPos = i.uv * _TextureSize;
                
                // Get old pixel color
                fixed4 existing = tex2D(_MainTex, i.uv);
                fixed4 result = existing;
                
                // Check if any pinball affects this pixel
                for (int j = 0; j < _PinballCount && j < 32; j++)
                {
                    // Skip inactive pinballs (active flag)
                    if (_PinballPositions[j].z <= 0.0) 
                        continue;
                    
                    float2 currentPos = _PinballPositions[j].xy;
                    float ballID = _PinballPositions[j].w;
                    bool shouldMark = false;
                    
                    // Find previous position by ball ID
                    float2 previousPos = float2(0, 0);
                    bool hasValidPrevious = false;
                    
                    for (int k = 0; k < 32; k++)
                    {
                        if (_PreviousPositions[k].z > 0.5 && _PreviousPositions[k].w == ballID)
                        {
                            previousPos = _PreviousPositions[k].xy;
                            hasValidPrevious = true;
                            break;
                        }
                    }
                    
                    if (hasValidPrevious)
                    {
                        // Draw line from previous to current position
                        float2 lineVec = currentPos - previousPos;
                        float lineLength = length(lineVec);
                        
                        if (lineLength < 0.1)
                        {
                            // Very small movement, just mark current position
                            shouldMark = length(pixelPos - currentPos) <= _TrackingRadius;
                        }
                        else
                        {
                            // Draw line between previous and current
                            float2 lineDir = lineVec / lineLength;
                            float2 toPixel = pixelPos - previousPos;
                            float projection = dot(toPixel, lineDir);
                            projection = clamp(projection, 0.0, lineLength);
                            float2 closestPoint = previousPos + lineDir * projection;
                            shouldMark = length(pixelPos - closestPoint) <= _TrackingRadius;
                        }
                    }
                    else
                    {
                        float distance = length(pixelPos - currentPos);
                        shouldMark = distance <= _TrackingRadius;
                    }
                    
                    // Pinball affects this pixel
                    if (shouldMark)
                    {
                        result = fixed4(1, 1, 1, 1);
                        break; // No need to check other pinballs
                    }
                }
                
                return result;
            }
            ENDCG
        }
    }
}
