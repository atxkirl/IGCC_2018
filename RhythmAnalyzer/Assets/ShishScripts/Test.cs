﻿using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class Test : SingletonMonoBehaviour<Test>
{
    SerialPort sp = new SerialPort("COM5", 9600);
    public bool tap;

    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 1;
    }

    void Update()
    {
        try
        {
            if(sp.ReadByte() == 49)
            {
                //Debug.Log("kiki");
                //Debug.Log("I feel so unsure As I take your hand and lead you to the dance floor As the music dies, something in your eyes Calls to mind the silver screen And all its sad good - byes I'm never gonna dance again Guilty feet have got no rhythm Though it's easy to pretend I know you're not a fool Should've known better than to cheat a friend And waste the chance that I've been given So I'm never gonna dance again The way I danced with you Time can never mend The careless whispers of a good friend To the heart and mind Ignorance is kind There's no comfort in the truth Pain is all you'll find I'm never gonna dance again Guilty feet have got no rhythm Though it's easy to pretend I know you're not a fool I should've known better than to cheat a friend And waste the chance that I've been given So I'm never gonna dance again The way I danced with you Never without your love Tonight the music seems so loud I wish that we could lose this crowd Maybe it's better this way We'd hurt each other with the things we'd want to say We could have been so good together We could have lived this dance forever But no one's gonna dance with me Please stay And I'm never gonna dance again Guilty feet have got no rhythm Though it's easy to pretend I know you're not a fool Should've known better than to cheat a friend And waste the chance that I've been given So I'm never gonna dance again The way I danced with you Now that you're gone (Now that you're gone) What I did's so wrong, so wrong That you had to leave me alone");
                tap = true;
            }
        }
        catch (System.Exception ex)
        {
            //Debug.Log(ex.ToString());
            //Debug.Log("kek");
        }
    }
}