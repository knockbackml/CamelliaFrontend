shader_type canvas_item;

uniform vec2 screen_size;
uniform float h_thickness;
uniform sampler2D pattern : source_color;
uniform vec4 size_offset;

void fragment() {
	if (VERTEX.x < h_thickness || screen_size.x - VERTEX.x <= h_thickness) {
		COLOR = texture(pattern, fract((VERTEX + screen_size / 2.0 + size_offset.zw) / size_offset.xy));
	}
}
