using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnim : MonoBehaviour
{
    public static SkeletonAnim Instance { get; private set; }
    [SerializeField] private List<GameObject> skelPrefabs;
    private Animator animator;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy the game object itself
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        FindAnimator(); // Find the Animator component
        SetSkelAnim();
    }

    private void FindAnimator()
    {
        animator = FindObjectOfType<Animator>(); // Find the Animator component in the scene
        if (animator == null)
        {
            Debug.LogError("Animator component not found.");
        }
    }

    private void SetSkelAnim()
    {
        if (animator == null)
        {
            return;
        }

        foreach (GameObject prefab in skelPrefabs)
        {
            int number = GetPrefabNumber(prefab);
            if (number != -1)
            {
                animator.SetInteger("Skel_" + number, number); // Set the animation parameter using the Animator component
            }
        }
    }

    int GetPrefabNumber(GameObject obj)
    {
        string[] parts = obj.name.Split('_'); // Assuming the name follows the format "PrefabName_Number"
        if (parts.Length > 1)
        {
            int number;
            if (int.TryParse(parts[1], out number))
            {
                return number;
            }
        }
        return -1; // Return -1 if the number cannot be determined
    }
}