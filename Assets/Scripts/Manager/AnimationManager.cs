using System;
using System.Collections.Generic;
using System.Linq;
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
        
        ParseAnimations();
    }
    
    
    public static void ParseAnimations()
    {
        // Get all animations for Cat an Mouse
        List<Object> animations = Resources.LoadAll("Animations", typeof(AnimationClip)).ToList();
        List<Object> animationsRemaining = new List<Object>(animations);
        
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
                        animationsRemaining.Remove(animationClip);
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
                        animationsRemaining.Remove(animationClip);
                    }
                }
            }
        }

        string catAnimationNames = "", mouseAnimationNames = "", notParsedAnimations = "";
        foreach (var ani in animationsCat)
        {
            catAnimationNames += ani.Value + "   ";
        }

        foreach (var ani in animationsMouse)
        {
            mouseAnimationNames += ani.Value + "   ";
        }

        foreach (var ani in animationsRemaining)
        {
            notParsedAnimations += ani.name + "   ";
        }
        
        print($"Cat animations found ({animationsCat.Count}):\t{catAnimationNames}");
        print($"Mouse animations found ({animationsMouse.Count}):\t{mouseAnimationNames}");
        print($"Not parsed animations ({animationsRemaining.Count}):\t{notParsedAnimations}");
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
