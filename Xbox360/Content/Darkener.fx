
sampler s;
float darkFactor;


float4 PixelShaderFunction(float2 uv : TEXCOORD0) : COLOR0
{
	// get the current pixel
	float4 color = tex2D(s, uv);
	
	// darken the color
	color.r = color.r - (color.r * darkFactor);
	color.g = color.g - (color.g * darkFactor);
	color.b = color.b - (color.b * darkFactor);

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
