using UnityEngine;

public class CharacterTextureRandomisation : MonoBehaviour
{
    public Texture[] textures;
    public Renderer targetRenderer;

    void Start()
    {
        ApplyRandomTexture();
    }

    void ApplyRandomTexture()
    {
        if (textures.Length == 0) return;

        int index = Random.Range(0, textures.Length);

        targetRenderer.material.mainTexture = textures[index];
    }
}
