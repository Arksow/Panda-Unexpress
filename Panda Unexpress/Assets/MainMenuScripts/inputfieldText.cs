
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class inputfieldText : MonoBehaviour
{
    TMP_InputField field;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        field = GetComponent<TMP_InputField>();

        EventSystem.current.SetSelectedGameObject(null);
        field.DeactivateInputField();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
