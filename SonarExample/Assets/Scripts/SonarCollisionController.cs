using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarCollisionController : MonoBehaviour
{
    System.Random random;
    void Start() {
        //used for random numbers;
        random = new System.Random(Guid.NewGuid().GetHashCode());
    }

    //This is the only useful function here.
    private void OnTriggerEnter(Collider other)
    {
        SonarReaction sonarReaction = other.GetComponent<SonarReaction>();
        if (sonarReaction != null) {
            //Hit something you wanna care about! You wanna give stuff a sonar reaction component for them to be detected.

            //Temp function you can remove that makes stuff green when hit.
            setColourOfHitStuffToRandomColour(other.gameObject);

            //prints the name and sonar reaction name here.
            Debug.Log($"{other.name} detected with sonar name: {sonarReaction.getName()}");
            // A switch would be a good thing to use here.
        }
    }

    //this is all for visuals
    private void setColourOfHitStuffToRandomColour(GameObject _hit_thing) {


        Color oldColour = _hit_thing.GetComponent<Renderer>().material.color;

        //picks a random colour
        Color newColor = random.Next(0,6) switch {
            0 => Color.green,
            1 => Color.magenta,
            2 => Color.yellow,
            3 => Color.cyan,
            4 => Color.black,
            5 => Color.red,
            _ => Color.white
        };

        //some spicy recursion to prevent it becoming the same colour
        if (oldColour == newColor) {setColourOfHitStuffToRandomColour(_hit_thing); return;}

        //sets hit object to new colour with fade
        StartCoroutine(colourTransition(oldColour,newColor,_hit_thing));
    }

    private IEnumerator colourTransition(Color _old, Color _new, GameObject _thing) {
        float duration = 0.4f;
        float eTime = 0;
        while(eTime < duration) {
            Color newColor = Color.Lerp(_old,_new,(eTime/duration));
            _thing.GetComponent<Renderer>().material.color = newColor;
            eTime += Time.deltaTime;
            yield return null;
        }
        _thing.GetComponent<Renderer>().material.color = _new;
    }
}
