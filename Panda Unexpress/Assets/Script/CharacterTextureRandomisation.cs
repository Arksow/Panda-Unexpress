using UnityEngine;

public class CharacterTextureRandomisation : MonoBehaviour
{
    public Material[] materials;
    public Renderer targetRenderer;

    void Start()
    {
        ApplyRandomMaterial();
    }

    void ApplyRandomMaterial()
    {
        if (materials.Length == 0) return;

        int index = Random.Range(0, materials.Length);

        targetRenderer.material = materials[index];
    }
}