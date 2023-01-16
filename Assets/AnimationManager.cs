using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance { get; private set; }
    
    public enum AnimationType
    {
        Idle,
        Walk,
        Run,
        Jump,
        Fall,
        Interact
    }

    public List<AnimationClip> animationsCat;
    public List<AnimationClip> animationsMouse;

    public Dictionary<string, string[]> animationLookup;
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
        animationLookup = new Dictionary<string, string[]>();
    }

    private void Start()
    {
        // Get all animations for Cat an Mouse

        foreach (var animation in Resources.LoadAll("Animations", typeof(AnimationClip)))
        {
            if (animation.name.ToLower().Contains("cat"))
            {
                animationsCat.Add((AnimationClip)animation);
            }
            else if (animation.name.ToLower().Contains("mouse"))
            {
                animationsMouse.Add((AnimationClip)animation);
            }
        }
        
        /*
        print("Cat animations (" + animationsCat.Count + "):");
        foreach (var animation in animationsCat)
        {
            print(animation.name);
        }
        print("---");
        
        print("Mouse animations (" + animationsMouse.Count + "):");
        foreach (var animation in animationsMouse)
        {
            print(animation.name);
        }
        print("---");
        */
        
        foreach (var action in Enum.GetValues(typeof(AnimationType)))
        {
            AnimationClip catAction = animationsCat.LastOrDefault(anim => anim.ToString().Contains(action.ToString()));
            AnimationClip mouseAction = animationsMouse.LastOrDefault(anim => anim.ToString().Contains(action.ToString()));

            if (catAction != null && mouseAction != null)
            {
                animationLookup.Add(action.ToString(), new []{catAction.ToString(), mouseAction.ToString()});
            }
        }

        //string dicString = animationLookup.Select(keyValue => keyValue.Key.ToString() + ": " + keyValue.Value[0].ToString() + ", " + keyValue.Value[1].ToString());
        print( animationLookup.ToList() );
/*
        foreach (var action in animationLookup)
        {
            print("Key: " + action.Key + ", Cat: " + action.Value[0] + ", Mouse: " + action.Value[1]);
        }
        */
    }
    
}
