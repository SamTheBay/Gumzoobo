
sampler s;
float player1x;
float player1y;
float player2x;
float player2y;
float player3x;
float player3y;
float player4x;
float player4y;
float textureW;
float textureH;




float4 PixelShaderFunction(float2 uv : TEXCOORD0) : COLOR0
{
	// defines for the behavior 	
	float hardRadius = 50;
	float softRadius = 80;

	// get the current pixel
	float4 color = tex2D(s, uv);
	
	// setup the variables
	float2 currentPix = float2(uv.x * textureW, uv.y * textureH);
	float2 playerloc1 = float2(player1x, player1y);
	float2 playerloc2 = float2(player2x, player2y);
	float2 playerloc3 = float2(player3x, player3y);
	float2 playerloc4 = float2(player4x, player4y);

	// calculate distances
	float distance1 = sqrt(pow((playerloc1.x - currentPix.x),2) + pow((playerloc1.y -currentPix.y),2));
	float distance2 = sqrt(pow((playerloc2.x - currentPix.x),2) + pow((playerloc2.y -currentPix.y),2));
	float distance3 = sqrt(pow((playerloc3.x - currentPix.x),2) + pow((playerloc3.y -currentPix.y),2));
	float distance4 = sqrt(pow((playerloc4.x - currentPix.x),2) + pow((playerloc4.y -currentPix.y),2));
	
	
	if (distance1 < hardRadius ||
	    distance2 < hardRadius ||
	    distance3 < hardRadius ||
	    distance4 < hardRadius)
	{
	    color.a = 0;
	}
	else
	{
		if(distance1 < softRadius)
		{
			float fade = (1 / ((softRadius - hardRadius + 1) - (distance1 - hardRadius))) * 6;
			if (color.a > fade)
			{
				color.a = fade;
			}
		}
		if(distance2 < softRadius)
		{
			float fade = (1 / ((softRadius - hardRadius + 1) - (distance2 - hardRadius))) * 6;
			if (color.a > fade)
			{
				color.a = fade;
			}
		}
		if(distance3 < softRadius)
		{
			float fade = (1 / ((softRadius - hardRadius + 1) - (distance3 - hardRadius))) * 6;
			if (color.a > fade)
			{
				color.a = fade;
			}
		}
		if(distance4 < softRadius)
		{
			float fade = (1 / ((softRadius - hardRadius + 1) - (distance4 - hardRadius))) * 6;
			if (color.a > fade)
			{
				color.a = fade;
			}
		}
	}

	// return pixel color
    return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
