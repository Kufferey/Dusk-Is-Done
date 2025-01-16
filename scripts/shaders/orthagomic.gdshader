shader_type spatial;

uniform sampler2D texture1 : source_color;
uniform sampler2D texture2 : source_color;
uniform sampler2D texture3 : source_color;

uniform vec2 speed1 = vec2(-0.2, 0.2);
uniform vec2 speed2 = vec2(0.15, -0.2);
uniform vec2 speed3 = vec2(0.0, 0);

void fragment() {
	vec2 texture1Cords = UV * 10.0 + speed1 * TIME;
	vec2 texture2Cords = UV * 5.5 + speed2 * TIME;
	vec2 texture3Cords = UV + speed3 * TIME;

	vec4 text1 = texture(texture1, texture1Cords);
	vec4 text2 = texture(texture2, texture2Cords);
	vec4 text3 = texture(texture3, texture3Cords);

	vec4 textComb = mix(text1, text2.aaaa, 0.6);
	textComb = mix(textComb, text3, 0.85);
	
	// Old
	//vec4 textComb = mix(text1, text1, 1);
	//textComb = mix(textComb, text2.aaaa, -9);

	ALBEDO = textComb.rgb;
}