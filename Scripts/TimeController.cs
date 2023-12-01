using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class TimeController : MonoBehaviour{
    // Variables
    // // How fast the time passes
    [SerializeField]
    private float timeMultiplier;
    // // The hour the game starts at
    [SerializeField]
    private float startHour;
    // // TextMeshPro object to display the time
    [SerializeField]
    private TextMeshProUGUI timeText;
    // // Variable to keep track of the current in-game time
    public DateTime currentTime;

    // // Variables for the sun and moon
    [SerializeField]
    private Light sunLight;
    [SerializeField]
    private Light moonLight;

    // // The hour the sun rises and sets
    [SerializeField]
    private float sunriseHour;
    [SerializeField]
    private float sunsetHour;

    // Timespan values to convert sunrise and sunset hours to use in calculations
    private TimeSpan sunriseTime;
    private TimeSpan sunsetTime;

    // // Variables for the ambient light
    [SerializeField]
    private Color dayAmbientLight;
    [SerializeField]
    private Color nightAmbientLight;

    // // Adding a curve to the light change
    [SerializeField]
    private AnimationCurve lightChangeCurve;

    // // Variables for the light intensity
    [SerializeField]
    private float maxSunLightIntensity;
    [SerializeField]
    private float maxMoonLightIntensity;

    // // Variable for the winning time
    [SerializeField]
    private float winningHour;
    [SerializeField]
    private float winningMinute;


    // Start is called before the first frame update
    void Start(){
        // Setting current time to the desired start time
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
        
        // Converting sunrise and sunset hours to timespan values
        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
    }

    // Update is called once per frame
    void Update(){
        // Update the time of day
        UpdateTimeOfDay();
        // Rotate the sun
        RotateSun();
        // Update the light settings
        UpdateLightSettings();

        // Check if the player has won the game
        if(currentTime.ToString("HH:mm") == winningHour.ToString("00") + ":" + winningMinute.ToString("00")){ 
            // Win the game
            FindObjectOfType<GameManagerScript>().EndGameWin();
        }
    }

    // Function to increase the current time
    private void UpdateTimeOfDay() {
        // Adding to the current time 
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

        // Display current time on the UI
        if(timeText != null) {
            // Display time in 24 hour format
            timeText.text = currentTime.ToString("HH:mm");
        }
    }

    // Function to rotate the sun
    private void RotateSun() {
        // Variable to hold the rotation of the sun
        float sunLightRotation;

        // Check if we're in daytime
        if(currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime) {
            // Calculate the time difference between sunrise and sunset
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);

            // Calculate how much time has passed since sunrise
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            // Calculate the percentage of time that has passed since sunrise
            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            // Calculate the rotation of the sun
            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        } else {    // Else we're in nighttime
            // Calculate the time difference between sunset and sunrise
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);

            // Calculate how much time has passed since sunset
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            // Calculate the percentage of time that has passed since sunset
            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            // Calculate the rotation of the sun
            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        // Apply rotation to the sunlight
        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    // Function to calculate the difference between two times
    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime) {
        // Calculate the difference between the two times
        TimeSpan difference = toTime - fromTime;

        // If the difference is negative, add 24 hours to it
        return (difference.TotalSeconds < 0) ? (difference += TimeSpan.FromHours(24)) : difference;
    }

    // Function to update the light settings
    private void UpdateLightSettings() {
        // Calculate the dot product of the sun's forward vector and the vector pointing down
        // This will give us a value between -1 and 1
        // 1= sun pointing directly down, 0 = sun pointing directly horizontal, -1 = sun pointing directly up
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);

        // Change the intensity of the sun based on the dot product
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct));

        // Change the intensity of the moon based on the dot product
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));

        // Set the ambient light
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }
}
