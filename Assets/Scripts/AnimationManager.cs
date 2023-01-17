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

    /*
    public static List<AnimationClip> animationsCat;
    public static List<AnimationClip> animationsMouse;
    */
    public static Dictionary<AnimationType, string> animationsCat;
    public static Dictionary<AnimationType, string> animationsMouse;
    

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
        
        /*
        animationsCat = new List<AnimationClip>();
        animationsMouse = new List<AnimationClip>();
        */
        animationsCat = new Dictionary<AnimationType, string>();
        animationsMouse = new Dictionary<AnimationType, string>();
    }

    private void Start()
    {
        // Get all animations for Cat an Mouse
        foreach (AnimationClip animationClip in Resources.LoadAll("Animations", typeof(AnimationClip)))
        {
            if (animationClip.name.Contains("Cat_"))
            {
                foreach (string possibleCatAnimation in Enum.GetNames(typeof(AnimationType)))
                {
                    if (animationClip.name.Contains(possibleCatAnimation))
                    {
                        AnimationType catAnimation;
                        Enum.TryParse(possibleCatAnimation, out catAnimation);
                        animationsCat.Add(catAnimation, animationClip.name);
                    }
                }
            }
            else if (animationClip.name.Contains("Mouse_"))
            {
                foreach (string possibleMouseAnimation in Enum.GetNames(typeof(AnimationType)))
                {
                    if (animationClip.name.Contains(possibleMouseAnimation))
                    {
                        AnimationType mouseAnimation;
                        Enum.TryParse(possibleMouseAnimation, out mouseAnimation);
                        animationsMouse.Add(mouseAnimation, animationClip.name);
                    }
                }
            }
        }
    }

    public static string GetAnimationName(CharType character, AnimationType animationType)
    {
        if (character.Equals(CharType.Cat))
        {
            return animationsCat[animationType];
        }
        else if (character.Equals(CharType.Mouse))
        {
            return animationsMouse[animationType];
        }
        else
        {
            throw new Exception("Animation not found");
        }
    }
    

}
