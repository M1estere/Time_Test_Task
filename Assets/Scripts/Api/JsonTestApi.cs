using System;
using System.Collections;
using UnityEngine.Networking;

namespace Api
{
    public class JsonTestApi
    {
        public static IEnumerator GetRequest(string uri, Action<string> callback = null)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(uri);
            yield return uwr.SendWebRequest();

            if (uwr.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
            {
                callback?.Invoke(uwr.error);
                yield return uwr.error;
            }
            else
            {
                callback?.Invoke(uwr.downloadHandler.text);
                yield return uwr.downloadHandler.text;
            }
        }
    }
}