using UnityEngine;

public class CharacterTextureRandomisation : MonoBehaviour
{
    public Material[] materials;

    private SkinnedMeshRenderer[] meshRenderers;

    void Start()
    {
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        ApplyRandomMaterial();
    }

    void ApplyRandomMaterial()
    {
        if (materials.Length == 0) return;

        int index = Random.Range(0, materials.Length);
        Material randomMaterial = materials[index];

        foreach (SkinnedMeshRenderer renderer in meshRenderers)
        {
            renderer.material = randomMaterial;
        }
    }
}