/* 
Basic sci-fi pulse post-processing effect.
Required to be put on a quad mesh that is rendered in the scene.
Required associated script to increase the radius over time.
Video tutorial on YouTube: https://youtu.be/x1dIJdz8Uj8
Written by Michael Watt
*/

shader_type spatial;
render_mode unshaded;

// Settings to play with
uniform vec3 start_point = vec3(0.0);
uniform float pulse_width = 2.0;
uniform vec4 color :hint_color;

// Updated by Script
uniform float radius = 0.0;

// Not sure this is actually needed, but used in Documentation.
varying mat4 CAMERA;

void vertex() {
	// Set the Quad to cover the entire screen.
	// If it doesn't, resize the quad (x,y) to (2,2)
	//POSITION = vec4(VERTEX, 1.0);
	
	// Again, from the docs, we need the interpolated camera matrix
	// Which I think you can just get in the fragment shader?
	CAMERA = CAMERA_MATRIX;
}

void fragment() {
	// Get the original screen rendered texture at the screen uv coordinates.
	vec4 original = texture(SCREEN_TEXTURE, SCREEN_UV);
	
	// Get the depth value form the depth buffer. Stored in the x value
	// of the depth texture.
	float depth = texture(DEPTH_TEXTURE, SCREEN_UV).x;
	
	// Normalized Device Coordinates (clip space... mostly)
	vec3 ndc = vec3(SCREEN_UV, depth) * 2.0 - 1.0;
	
	// Convert to world space from NDC
	vec4 world = CAMERA_MATRIX * INV_PROJECTION_MATRIX * vec4(ndc, 1.0);
	vec3 world_position = world.xyz / world.w;
	
	// Calculate how far the fragment (in world space) is from the start point.
	// If you want a cylinder effect not a sphere use the code below instead
	//	float dist = distance(world_position.xz, start_point.xz);
	float dist = distance(world_position, start_point);
	float mix_ratio = 0.0;
	
	if (dist < radius && dist > radius - pulse_width){
		mix_ratio = 1.0 - (radius - dist);
		
		// Clamp the mix ratio because you can get some unintential visuals
		
		mix_ratio = clamp(mix_ratio, 0.0, 1.0);
	}
	
	ALBEDO = mix(original.rgb, color.rgb, mix_ratio);
}
