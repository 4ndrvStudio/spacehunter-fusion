using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

using Suinet.Rpc;
using Suinet.Rpc.Http;
using Suinet.Rpc.JsonRpc;
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
public class UnityWebRequestRpcClient : IRpcClient
{
    public Uri Endpoint { get; private set; }

    public UnityWebRequestRpcClient(string url)
    {
        Endpoint = new Uri(url);
    }

    public async Task<RpcResult<T>> SendAsync<T>(JsonRpcRequest request)
    {
      
        var requestJson = JsonConvert.SerializeObject(request, new Newtonsoft.Json.Converters.StringEnumConverter());
         Debug.Log(requestJson);
        try
        {
            var requestData = Encoding.UTF8.GetBytes(requestJson);
            Debug.Log(Endpoint);
            using (var unityWebRequest = new UnityWebRequest(Endpoint, "POST"))
            {
                unityWebRequest.uploadHandler = new UploadHandlerRaw(requestData);
                unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
                unityWebRequest.SetRequestHeader("Content-Type", "application/json");
                unityWebRequest.SendWebRequest();

                while (!unityWebRequest.isDone)
                {
                    await Task.Yield();
                }

                var result = HandleResult<T>(unityWebRequest.downloadHandler, request);


                result.RawRpcRequest = requestJson;

                return result;
            }
        }
        catch (Exception e)
        {

            var result = new RpcResult<T>
            {
                ErrorMessage = e.Message,
                RawRpcRequest = requestJson
            };
            var errorMessage = $"SendAsync Caught exception: {e.Message}";
            Debug.LogError(errorMessage);

            return result;
        }
    }

    private RpcResult<T> HandleResult<T>(DownloadHandler downloadHandler, JsonRpcRequest request)
    {   
        Debug.Log(downloadHandler.text);
  
        var result = new RpcResult<T>();
        try
        {
            result.RawRpcResponse = downloadHandler.text;
            var res = JsonConvert.DeserializeObject<JsonRpcValidResponse<T>>(downloadHandler.text);
            
    
            if (res.Result != null)
            {
                result.Result = res.Result;
                result.IsSuccess = true;
            }
            else
            {
                var errorRes = JsonConvert.DeserializeObject<JsonRpcErrorResponse>(result.RawRpcResponse);
                if (errorRes != null)
                {
                    result.ErrorMessage = errorRes.Error.Message;
                }
                else
                {
                    result.ErrorMessage = "Something wrong happened.";
                }
            }
        }
        catch (JsonException e)
        {
            Debug.LogError($"HandleResult Caught exception: {e.Message}");
            result.IsSuccess = false;
            result.ErrorMessage = "Unable to parse json.";
        }

        return result;
    }



}
