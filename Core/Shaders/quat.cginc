// #version 150
// in vec4 gxl3d_Position;
// in vec4 gxl3d_TexCoord0;
// in vec4 gxl3d_Color;
// out vec4 Vertex_UV;
// out vec4 Vertex_Color;
// uniform mat4 gxl3d_ViewProjectionMatrix;

// struct Transform
// {
//   vec4 position;
//   vec4 axis_angle;
// };
// uniform Transform T;


float4 quat_from_axis_angle(float4 axis, float angle)
{ 
    float4 qr;
    float half_angle = (angle * 0.5) * 3.14159 / 180.0;
    qr.x = axis.x * sin(half_angle);
    qr.y = axis.y * sin(half_angle);
    qr.z = axis.z * sin(half_angle);
    qr.w = cos(half_angle);
    return qr;
}

float4 quat_conjugate(float4 q) // quat_inverse
{ 
    return float4(-q.x, -q.y, -q.z, q.w); 
}
  
float4 quat_multiply(float4 q1, float4 q2)
{ 
    float4 qr;
    qr.x = (q1.w * q2.x) + (q1.x * q2.w) + (q1.y * q2.z) - (q1.z * q2.y);
    qr.y = (q1.w * q2.y) - (q1.x * q2.z) + (q1.y * q2.w) + (q1.z * q2.x);
    qr.z = (q1.w * q2.z) + (q1.x * q2.y) - (q1.y * q2.x) + (q1.z * q2.w);
    qr.w = (q1.w * q2.w) - (q1.x * q2.x) - (q1.y * q2.y) - (q1.z * q2.z);
    return qr;
}

float4 rotate_vertex_position(float4 position, float4 axis, float angle)
{ 
    float4 qr = quat_from_axis_angle(axis, angle);
    float4 qr_conj = quat_conjugate(qr);
    float4 q_pos = float4(position.x, position.y, position.z, 0);

    float4 q_tmp = quat_multiply(qr, q_pos);
    qr = quat_multiply(q_tmp, qr_conj);

    return float4(qr.x, qr.y, qr.z);
}

float3 rotate_vertex_position(float3 position, float3 axis, float angle)
{ 
  float4 q = quat_from_axis_angle(axis, angle);
  float3 v = position.xyz;
  return v + 2.0 * cross(q.xyz, cross(q.xyz, v) + q.w * v);
}

// void main()
// {
//   float4 P = rotate_vertex_position(gxl3d_Position.xyz, T.axis_angle.xyz, T.axis_angle.w);
//   P += T.position.xyz;
//   gl_Position = gxl3d_ViewProjectionMatrix * float4(P, 1);
//   Vertex_UV = gxl3d_TexCoord0;
//   Vertex_Color = gxl3d_Color;
// }