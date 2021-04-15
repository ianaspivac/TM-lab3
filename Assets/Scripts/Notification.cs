using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    private Text notification;
    private float timeToAppear = 2f;
    private float timeWhenDisappear;
    void Start()
    {
        notification = GameObject.Find("Notification").GetComponent<Text>();
    }
    //We check every frame if the timer has expired and the text should disappear
void Update()
{
    if (notification.enabled && (Time.time >= timeWhenDisappear))
    {
        notification.enabled = false;
    }
}
    // Update is called once per frame
    public void Notify(string message)
    {
        notification.text = message;
        notification.enabled = true;
        timeWhenDisappear = Time.time + timeToAppear;
    }
}
