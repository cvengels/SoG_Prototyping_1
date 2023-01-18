using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance { get; private set; }
    
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
        
        animationsCat = new Dictionary<AnimationType, string>();
        animationsMouse = new Dictionary<AnimationType, string>();
    }

    private void Start()
    {
        // Get all animations for Cat an Mouse
        Object[] animations = Resources.LoadAll("Animations", typeof(AnimationClip));
        
        // search in animation clips for cat or mouse animations
        foreach (AnimationClip animationClip in animations)
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
        print("Cat animations found: " + animationsCat.Count);
        print("Mouse animations found: " + animationsMouse.Count);
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
