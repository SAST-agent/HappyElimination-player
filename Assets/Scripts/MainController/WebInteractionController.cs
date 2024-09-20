using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using DataManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

/// <summary>
/// 通信控制器
/// </summary>
public class WebInteractionController : MonoBehaviour
{
    public string tokenB64;
    private static bool _loaded = false;

    public void Update()
    {
        // 这个函数被调用代表所有的 Awake() Start() 都启用了
        // 也即初始化已经完成
        // 本地调用直接打开会报错
        if (_loaded)
        {
            return;
        }
        _loaded = true;
        SendInitCompleteToFrontend();
    }

    // 通过前端网页连接 judger
    private void ConnectToJudger(string token)
    {
        if (Connect(token))
        {
            Debug.Log("Connected to Judger");
            ReplyConnectionSucceed(token);
        }
    }

    // 尝试通过前端网页与 judger 建立连接
    private bool Connect(string token)
    {
        try
        {
            var bytes = Convert.FromBase64String(token);
            var uri = Encoding.UTF8.GetString(bytes);
            Debug.Log(uri);
            GetComponent<PlatformFuncController>().SetPlayerId(int.Parse(uri[^1].ToString()));
            
            Connect_ws("wss://" + uri);
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            SendErrorToFrontend(e.Message);
            return false;
        }
    }

    // 通过前端网页对成功连接进行反馈
    private void ReplyConnectionSucceed(string token)
    {
        var info = new Info{
                request = "connect",
                token = token,
                content = null
            };
        tokenB64 = token;
        JsonSerializerSettings settings = new() { NullValueHandling = NullValueHandling.Ignore };
        var jsonString = JsonConvert.SerializeObject(info, settings);
        Debug.Log(jsonString);
        Write(jsonString);
    }

    // 通过前端网页发送信息给judger，发送的是information
    private void Write(string information)
    {
        try
        {
            Send_ws(information);
        }
        catch (Exception e)
        {
            Debug.Log($"Failed to send message: {e.Message}");
            SendErrorToFrontend(e.Message);
        }
    }

    // 向后端发送 action
    // 游戏UI逻辑需要使用
    public void SendAction(Operation action)
    {
        string sendAction = action.ToString();
        sendAction += '\n';
        var sendMessage = new Info{
            request = "action",
            token = tokenB64,
            content = sendAction
        };
        var jsonMessage = JsonConvert.SerializeObject(sendMessage);
        //Debug.Log($"Send message {jsonMessage}");
        Write(jsonMessage);
    }

    // 提供给前端网页使用
    // 接收后端信息
    public void ReceiveWebSocketMessage(string information)
    {
        try
        {
            Debug.Log($"Received message from websocket: {information}");
            var judgerData = JsonConvert.DeserializeObject<JudgerData>(information);
            if (judgerData.request == "action")
            {
                var backendData = JsonConvert.DeserializeObject<BackendData>(judgerData.content);
                var jsonData = BackendData.Convert(backendData);
                GetComponent<InteractController>().Interact(jsonData);
            }
            else
            {
                Debug.Log(judgerData.request);
                SendErrorToFrontend(judgerData.request);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            SendErrorToFrontend(e.Message);
        }
    }
    
    // 提供给前端网页使用
    // 接收网页信息
    public void HandleMessage(string buffer)
    {
        FrontendData msg;
        try
        {
            msg = JsonConvert.DeserializeObject<FrontendData>(buffer);
        }
        catch (Exception e)
        {
            SendErrorToFrontend(e.Message);
            return;
        }
        try
        {
            switch (msg.message)
            {
                case FrontendData.MsgType.init_player_player:
                    GetComponent<PlatformFuncController>().SwitchToInteractionMode();
                    ConnectToJudger(msg.token);
                    break;
                case FrontendData.MsgType.init_replay_player:
                    // 这里信息会由html文件一行一行传过来，不需要手动提供文件
                    GetComponent<PlatformFuncController>().SwitchToReplayMode(null);
                    int frameCount = Convert.ToInt32(msg.payload);
                    for (int i = 0; i < frameCount; i++)
                    {
                        Getoperation(i);
                    }
                    GetComponent<PlatformFuncController>().ReplayPlayerInited();
                    SendFrameCountToFrontend(frameCount - 1);
                    break;
                case FrontendData.MsgType.load_frame:
                    Debug.Log($"Load for the frame {msg.index + 1}");
                    GetComponent<PlatformFuncController>().LoadFrame(msg.index + 1);
                    break;
                case FrontendData.MsgType.load_next_frame:
                    Debug.Log("Load next frame");
                    GetComponent<PlatformFuncController>().LoadNextFrame();
                    break;
                case FrontendData.MsgType.load_players:
                    GetComponent<PlatformFuncController>().SetPlayerNames(msg.players[0], msg.players[1]);
                    break;
                case FrontendData.MsgType.play_speed:
                    GetComponent<PlatformFuncController>().SetAnimationSpeed(msg.speed);
                    Debug.Log("replay_speed is " + msg.speed);
                    break;
                default:
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    
    // 向网页发送回复信息
    private void SendToFrontend(FrontendReplyData reply)
    {
        Debug.Log("into send");
        string information = JsonConvert.SerializeObject(reply);
        Debug.Log(information);
        Send_frontend(information);
    }
    
    // 向网页报错
    private void SendErrorToFrontend(string message)
    {
        SendToFrontend(
            new FrontendReplyData()
            {
                message = FrontendReplyData.MsgType.error_marker,
                err_msg = message
            }
        );
    }
    
    // 告知网页总帧数
    private void SendFrameCountToFrontend(int count)
    {
        SendToFrontend(
            new FrontendReplyData
            {
                message = FrontendReplyData.MsgType.init_successfully,
                number_of_frames = count,
                init_result = true
            }
        );
    }

    private void SendInitCompleteToFrontend()
    {
        Debug.Log("aaa");
        // 告知前端网页unity已经初始化完成，接收队列中的信息
        SendToFrontend(new FrontendReplyData
            {
                message = FrontendReplyData.MsgType.loaded
            }
        );
    }
    
    /*
     * 这个函数是前端网页调用的，通过output.jslib与player.html协同实现通信
     * 逻辑为：unity调用Getoperation函数
     *       -> output.jslib中实现该函数，调用window.SendOperation
     *       -> player.html中实现window.SendOperation，调用Main Controller组件中的HandleOperation
     *       -> 到达该函数
     */
    public void HandleOperation(string information)
    {
        var backendData = JsonConvert.DeserializeObject<BackendData>(information);
        GetComponent<ReplayController>().AddDataToReplay(backendData);
    }

    // 下面的函数是定义在output.jslib中的库函数
    [DllImport("__Internal")]
    private static extern void Connect_ws(string address);

    [DllImport("__Internal")]
    private static extern void Send_ws(string strPayload);

    [DllImport("__Internal")]
    private static extern void Send_frontend(string json);

    [DllImport("__Internal")]
    private static extern void Getoperation(int index);
}


