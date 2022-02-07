using System;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class PlayAudioFromPath : MonoBehaviour
{
    private const int SampleRate = 48000;
    
    [SerializeField] private AudioSource source;

    public async UniTask PlayClip(string fullPath)
    {
        await UniTask.SwitchToMainThread();
        
        var buffer = File.ReadAllBytes(fullPath);
        
        MemoryStream memoryStream = new MemoryStream();
        memoryStream.SetLength(0);
 
        int sampleBits = 16;
        int sampleBytes = sampleBits / 8;
        Encoding encoding = Encoding.ASCII;
 
        memoryStream.Write(encoding.GetBytes("RIFF"), 0, 4);
        memoryStream.Write(BitConverter.GetBytes(44 + buffer.Length - 8), 0, 4);
        memoryStream.Write(encoding.GetBytes("WAVE"), 0, 4);
        memoryStream.Write(encoding.GetBytes("fmt "), 0, 4);
        memoryStream.Write(BitConverter.GetBytes(16), 0, 4);
        memoryStream.Write(BitConverter.GetBytes((short)1), 0, 2);
        memoryStream.Write(BitConverter.GetBytes((short)1), 0, 2);
        memoryStream.Write(BitConverter.GetBytes(SampleRate), 0, 4);
        memoryStream.Write(BitConverter.GetBytes(SampleRate * sampleBytes), 0, 4);
        memoryStream.Write(BitConverter.GetBytes((short)sampleBytes), 0, 2);
        memoryStream.Write(BitConverter.GetBytes((short)sampleBits), 0, 2);
        memoryStream.Write(encoding.GetBytes("data"), 0, 4);
        memoryStream.Write(BitConverter.GetBytes(buffer.Length), 0, 4);
        
        memoryStream.Write(buffer, 0, buffer.Length);
        
        memoryStream.WriteTo(new FileStream(fullPath.Replace(".raw", ".wav"), FileMode.OpenOrCreate));
        
        memoryStream.Close();

        //AudioClip clip = AudioClip.Create("test", SampleRate * 1, 1, SampleRate, false, OnAudioRead);
        
         var request = UnityWebRequestMultimedia.GetAudioClip($"file://{fullPath.Replace(".raw", ".wav")}", AudioType.WAV);
        
         var response = await request.SendWebRequest();
        
         Debug.Log(response.error);
         Debug.Log(response.result);
         Debug.Log($"file://{fullPath}");
        
         Debug.Log(response.downloadHandler.data.Length);
        
        source.clip = DownloadHandlerAudioClip.GetContent(response);
        
        source.Play();
    }
}
