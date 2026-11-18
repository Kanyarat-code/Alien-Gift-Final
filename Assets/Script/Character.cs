using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Information")]
    [SerializeField] private string identityName;

    public string IdentityName => identityName;

    protected virtual void Start()
    {
        Debug.Log($"{IdentityName} initialized.");
    }
}