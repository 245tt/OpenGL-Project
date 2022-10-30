﻿#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;
layout(location = 2) in vec3 aNormal;

out vec2 texCoord;
out vec3 Normal;
out vec3 FragPos;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;



void main(void)
{
    texCoord = aTexCoord;
    Normal = mat3(transpose(inverse(model))) * aNormal;  
    FragPos = vec3(vec4(aPosition, 1.0) * model);
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
    
}