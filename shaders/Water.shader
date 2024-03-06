shader_type spatial;

render_mode cull_back, diffuse_burley, specular_toon;

uniform sampler2D nTexture : hint_black;

varying vec2 vertPos;
void vertex() {
	vec3 ns = texture(nTexture, UV).rgb * 1.6;
	vertPos = VERTEX.xz / 2.0;
	VERTEX.y = VERTEX.y + (ns.z*0.9 * NORMAL.y);
}

void fragment() {
	mediump vec3 ns = texture(nTexture, UV).rgb; //noise_texture(from Viewport)
	mediump vec3 fFac = smoothstep(vec3(0.8), vec3(0.4), ns); //foam_factor
	mediump float fresnel = sqrt(1.0 - dot(NORMAL, VIEW));
	mediump vec3 col_1 = vec3(0.0, 0.1, 0.3);
	mediump vec3 col_2 = vec3(0.1, 0.3, 0.4);
	mediump vec3 col_3 = vec3(0.4, 0.8, 0.8);
	mediump vec3 col_4 = vec3(0.8, 0.9, 0.9);
	//Color_ramp
	mediump vec3 col = mix(mix(col_1, col_2, ns*0.6), mix(col_3, col_4, ns*1.2), smoothstep(0.0, 1.0, ns));
	mediump vec3 fCol = clamp(mix(vec3(0.999), col, fFac), 0.0, 1.0); //final_color
	//fCol *= min(clamp(mix(vec3(1.0), vec3(0.2, 0.4, 0.6), fFac), 0.0, 1.0), 1.0);
	ALBEDO = min(fCol + (0.1 * fresnel), 1.0);
	SPECULAR = 0.2 * fresnel;
	ROUGHNESS = mix(0.9, 0.7, fFac.z) * (1.0 - fresnel*0.2);
	TRANSMISSION = vec3(0.2, 0.7, 0.3);
}