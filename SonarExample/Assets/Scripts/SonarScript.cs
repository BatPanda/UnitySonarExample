using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarScript : MonoBehaviour
{

    [Header("Sonar Settings")]
    [SerializeField] private float sonarDuration;
    [SerializeField] private float sonarMaxRadius;
    [SerializeField] private GameObject sonarPrefab;
    [SerializeField] private Transform sonarPrefabParent;

    [Header("Sonar Fade Settings")]

    [SerializeField] private float fadeDuration;

    private GameObject sonarSphere;

    //Dont be scared by this, its a property. This means that it can only have
    //its value set in this script, but can be read by any script.
    public bool onCooldown {get; private set;} = false;

    private void updateSonarRadius(float _radius) {
        sonarSphere.transform.localScale = new Vector3(_radius,_radius,_radius);
    }

    private IEnumerator PulseSonar() {
        //Turn on cooldown to prevent multiple pulses.
        onCooldown = true;

        //Create sonar first
        sonarSphere = Instantiate(sonarPrefab,sonarPrefabParent);
        
        //get current radius of the sphere.
        float currentRadius = sonarSphere.transform.localScale.x;
        
        //This value will increase over time
        float elapsedTime = 0f;

        while (elapsedTime < sonarDuration) {
            //Make time actually increase.
            elapsedTime += Time.deltaTime;

            //Using the max radius and the current radius we can figure out the radius based on the elapsed time over the duration.
            float newRadius = Mathf.Lerp(currentRadius,sonarMaxRadius,(elapsedTime/sonarDuration));

            //So we update our sphere with it.
            updateSonarRadius(newRadius);

            //Then pause the code here until the next frame.
            yield return null;
        }
        //This code is exectured after the sonarDuration has been reached.
        //We made sure to set the radius to the maximum desired (this should be the size already, but precision errors can occure so we made sure.)
        updateSonarRadius(sonarMaxRadius);
        
        //Now we dont just want our pulse to just vanish, so we need to make it fade.
        Renderer sonarRenderer = sonarSphere.GetComponent<Renderer>();

        //This will cause it to fade out
        float elapsedFadeTime = 0f;
        while(elapsedFadeTime < fadeDuration) {
            elapsedFadeTime+=Time.deltaTime;
            float alpha = Mathf.Lerp(1,0,(elapsedFadeTime/fadeDuration));
            sonarRenderer.material.color = new Color(sonarRenderer.material.color.r,sonarRenderer.material.color.g,sonarRenderer.material.color.b,alpha);
            yield return null;
        }
        sonarRenderer.material.color = new Color(sonarRenderer.material.color.r,sonarRenderer.material.color.g,sonarRenderer.material.color.b,0);
        
        //Destroy the current sphere
        Destroy(sonarSphere);

        //And its done! so we can turn the cooldown off.
        onCooldown = false;
    }

    public void pingSonar() {
        if (onCooldown) return;
        //Will only pulse if its not on cooldown!
        StartCoroutine(PulseSonar());
    }
}
