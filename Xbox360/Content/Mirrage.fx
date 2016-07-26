
sampler s;
float strength;
float time;


float4 PixelShaderFunction(float2 uv : TEXCOORD0) : COLOR0
{
	// get the current pixel
	float4 color = tex2D(s, uv);
	
	

	// return pixel color
    return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
