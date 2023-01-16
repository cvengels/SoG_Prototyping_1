
using System;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType
{
    Idle,
    Walk,
    Run,
    Jump,
    Fall,
    Wall,
    Interact
}

public enum CharType
{
    Cat = 0,
    Mouse = 1
}

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance { get; private set; }

    public static List<AnimationClip> animationsCat;
    public static List<AnimationClip> animationsMouse;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        
        animationsCat = new List<AnimationClip>();
        animationsMouse = new List<AnimationClip>();
    }

    private void Start()
    {
        // Get all animations for Cat an Mouse
        foreach (AnimationClip animationClip in Resources.LoadAll("Animations", typeof(AnimationClip)))
        {
            if (animationClip.name.Contains("Cat_"))
            {
                animationsCat.Add(animationClip);
            }
            else if (animationClip.name.Contains("Mouse_"))
            {
                animationsMouse.Add(animationClip);
            }
        }
    }

    public static string GetAnimationName(CharType character, AnimationType animationType)
    {
        if (character.Equals(CharType.Cat))
        {
            return animationsCat[(int)animationType].name;
        }
        else if (character.Equals(CharType.Mouse))
        {
            return animationsMouse[(int)animationType].name;
        }
        else
        {
            throw new Exception("Animation not found");
        }
        
    }
    

}
