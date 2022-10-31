#version 330 core

out vec4 outputColor;

struct Material 
{
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;    
    float shininess;
}; 

struct SpotLight
{
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    vec3 position;
    vec3 direction;
    float cutOff;
    float fallOff;
    float outerCutOff;
};

struct PointLight
{
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    vec3 position;
    float fallOff;
};

struct DirectionalLight
{
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    vec3 direction;
};

uniform sampler2D texture0;
uniform vec3 lightPos;
uniform vec3 viewPos;
uniform Material material;
//uniform int pointLightCount;
#define COUNT 10
uniform DirectionalLight dirLights;
uniform SpotLight spotLights[1];
uniform PointLight pointLights[2];

in vec2 texCoord;
in vec3 Normal;
in vec3 FragPos;

vec3 CalcDirLight(DirectionalLight light, vec3 normal, vec3 viewDir);
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir);

void main()
{
   
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);

    vec3 result = vec3(0.0);

    result += CalcDirLight(dirLights,norm,viewDir);

    for(int i =0;i<pointLights.length();i++)
    {
        result += CalcPointLight(pointLights[i],norm,FragPos,viewDir);
    }

    for(int i =0;i<spotLights.length();i++)
    {
        result += CalcSpotLight(spotLights[i],norm,FragPos,viewDir);
    }

    outputColor = vec4(result,1.0) * texture(texture0,texCoord);

}

vec3 CalcDirLight(DirectionalLight light,vec3 normal,vec3 viewDir)
{
    vec3 lightDir = normalize(-light.direction);
     float diff = max(dot(normal, lightDir), 0.0);
    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // combine results
    vec3 ambient = light.ambient;
    vec3 diffuse = light.diffuse * diff;
    vec3 specular = light.specular * spec;


    return (ambient + diffuse + specular);
}

vec3 CalcSpotLight(SpotLight light,vec3 normal,vec3 fragPos,vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);
    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // attenuation
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / pow(distance,light.fallOff);
    // spotlight intensity
    float theta = dot(lightDir, normalize(-light.direction)); 
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);
    // combine results
    vec3 ambient = light.ambient;
    vec3 diffuse = light.diffuse * diff;
    vec3 specular = light.specular * spec;
    ambient *= attenuation * intensity;
    diffuse *= attenuation * intensity;
    specular *= attenuation * intensity;
    return (ambient + diffuse + specular);

    


    //return vec3(1.0);
}
vec3 CalcPointLight(PointLight light,vec3 normal,vec3 fragPos,vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);
    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // attenuation
    float distance = length(light.position - fragPos);
    //float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));    
    float attenuation = 1.0 / pow(distance,light.fallOff);
    // combine results
    vec3 ambient = light.ambient;
    vec3 diffuse = light.diffuse * diff ;
    vec3 specular = light.specular * spec;
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;
    return (ambient + diffuse + specular);
    return vec3(1.0);
}