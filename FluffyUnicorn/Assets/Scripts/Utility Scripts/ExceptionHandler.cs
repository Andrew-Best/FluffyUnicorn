using UnityEngine;
using System;
using System.IO;

public class ExceptionHandler : MonoBehaviour 
{
    private StreamWriter writer_;
    private int exceptionCount_ = 0;

	void Awake() 
    {
        Application.logMessageReceived += HandleException;
        writer_ = new StreamWriter(Path.Combine(Application.dataPath, "unityexceptions.txt"));
        writer_.AutoFlush = true;
	}

    private void HandleException(string condition, string stackTrace, LogType type)
    {
        if(type == LogType.Exception)
        {
            exceptionCount_++;
            writer_.WriteLine("{0}: {1}\n{2}", type, condition, stackTrace);
        }
    }
}