shader_type canvas_item;

uniform sampler2D screen_tex : hint_screen_texture, repeat_disable, filter_nearest;
uniform vec4 alt_color : source_color;
uniform float y_offset;

varying vec2 obj_pos;

void vertex() {
	obj_pos = VERTEX;
}

void fragment() {
	vec3 deltas = obj_pos.yyy - (sin((obj_pos.xxx + cos(obj_pos.xxx / vec3(52.8, 78.2, 96.3)) * vec3(20.0, 28.0, 37.0) + TIME * vec3(8.3, -3.7, 1.3)) / vec3(37.0, 59.0, 61.0) + PI / vec3(11.0, 17.0, .7)) * vec3(20.0, 17.0, 23.0) + vec3(0.0, 7.0, 13.0) + y_offset);
	vec3 dists = abs(deltas);

	if (dists.x <= 0.5 || dists.y <= 0.5 || dists.z <= 0.5) {
		COLOR.rgb = mix(COLOR.rgb, alt_color.rgb, 1.0 - abs(deltas.x + deltas.y + deltas.z) * .02);
	}

	COLOR.a  = (deltas.x > 0.0 ? 0.2 : 0.0);
	COLOR.a += (deltas.y > 0.0 ? 0.4 : 0.0);
	COLOR.a += (deltas.z > 0.0 ? 0.4 : 0.0);

	COLOR.rgb = mix(textureLod(screen_tex, SCREEN_UV, 0.0).rgb * COLOR.rgb, COLOR.rgb, 0.5);
}
