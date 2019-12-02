using UnityEngine;

namespace DilmerGames.Core.Utilities
{
    public static class MaterialUtils 
    {
        public static Material CreateMaterialWithRandomColor(string name, string shaderName = "Sprites/Default")
        {
            Material material = new Material(Shader.Find(shaderName));
            material.name = name;
            material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            return material;
        }
    }
}