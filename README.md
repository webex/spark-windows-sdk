# Cisco Spark Windows SDK
[![LICENSE](https://img.shields.io/github/license/ciscospark/spark-windows-sdk.svg)](https://github.com/ciscospark/spark-windows-sdk/blob/master/LICENSE)

> The Cisco Spark™ Windows SDK
 
The Cisco Spark Windows SDK makes it easy to integrate secure and convenient Cisco Spark calling and messaging features in your Windows applications.

This SDK is built with **Vistual Studio 2015** and requires:

- .NET Framework 4.5.2 or higher version
- Win8 or Win10

## Table of Contents
- [Install](#install)
- [Usage](#usage)
- [Contribute](#contribute)
- [License](#license)

## Install
In your Windows application for example WPF project, here are steps to integrate the Cisco Spark Windows SDK into your project:

1. Right click your project, and select "Manage NuGet Packages..."  
2. Search "Cisco.Spark.WindowsSDK" in the Browse tag  
3. Install the lastest stable version

## Usage
To use the SDK, you will need Cisco Spark integration credentials. If you do not already have a Cisco Spark account, visit the [Cisco Spark for Developers portal](https://developer.ciscospark.com/) to create your account and [register an integration](https://developer.ciscospark.com/authentication.html#registering-your-integration). Your app will need to authenticate users via an [OAuth](https://oauth.net/) grant flow for existing Cisco Spark users or a [JSON Web Token](https://jwt.io/) for guest users without a Cisco Spark account.

See the [Windows SDK area](https://developer.ciscospark.com/sdk-for-windows.html) of the Cisco Spark for Developers site for more information about this SDK.

### Examples
Here are some examples of how to use the Windows SDK in your application. More details can be found under [Windows SDK Demo app](https://github.com/ciscospark/spark-windows-sdk-example).

1. Create a new *Spark* instance using Spark ID authentication ([OAuth](https://oauth.net/)-based):  

    ``` c# 
    string clientId = "your client id";  
    string clientSecret = "your client secret";
    string redirectUri = "KitchenSink://response/";
    string scope = "spark:all";
    var auth = new OAuthAuthenticator(clientId, clientSecret, scope, redirectUri);
    // authCode(64 bits) can be extracted from url by loading auth.authorizationUrl with a WebBrowser
    var spark = new SPARK(auth);
    auth?.Authorize(authCode, result =>
    {
        if (result.Success)
        {
            System.Console.WriteLine("authorize success!");
        }
        else
        {
            System.Console.WriteLine("authorize failed!");
        }
    });
    ```

2. Create a new *Spark* instance using Guest ID authentication ([JWT](https://jwt.io/)-based):  

    ```c#
    var auth = new JWTAuthenticator();
    var spark = new SPARK(auth);
    auth?.AuthorizeWith(jwt, result =>
    {
        if (result.Success)
        {
            System.Console.WriteLine("authorize success!");
        }
        else
        {
            System.Console.WriteLine("authorize failed!");
        }
    });
    
    ```

3. Register the device to make or receive calls:  
 
    ``` c#
    spark?.Phone.Register(result =>
    {
        if (result.Success == true)
        {
            System.Console.WriteLine("spark cloud connected");
        }
        else
        {
            System.Console.WriteLine("spark cloud connect failed");
        }
    });
    ```
    
4. Make an outgoing call:  

    ```c#
    // dial
    spark?.Phone.Dial(calleeAddress, MediaOption.AudioVideoShare(curCallView.LocalViewHandle, curCallView.RemoteViewHandle, curCallView.RemoteShareViewHandle), result =>
    {
        if (result.Success)
        {
            currentCall = result.Data;
            RegisterCallEvent();
        }
        else
        {
            System.Console.WriteLine($"Error: {result.Error?.errorCode.ToString()} {result.Error?.reason}");
        }
    });
    
    // register call event handlers
    void RegisterCallEvent()
    {
        currentCall.onRinging += CurrentCall_onRinging;
        currentCall.onConnected += CurrentCall_onConnected;
        currentCall.onDisconnected += CurrentCall_onDisconnected;
        currentCall.onMediaChanged += CurrentCall_onMediaChanged;
        currentCall.onCapabilitiesChanged += CurrentCall_onCapabilitiesChanged;
        currentCall.onCallMembershipChanged += CurrentCall_onCallMembershipChanged;    
    }
    
    // when video window such as local/remote/sharing window is resized or hided, call corresponding updateView with the windows handle
    currentCall.UpdateLocalView(curCallView.LocalViewHandle);
    ```

5. Answer incoming call:

    ```c#
    // register incoming call event
    spark?.Phone.OnIncoming += Phone_onIncoming;
    
    // get call object
    void Phone_onIncoming(SparkSDK.Call obj)
    {
        currentCall = obj;
    }
    
    // register call event handler and answer the call
    RegisterCallEvent();
    
    // answer current call  
    currentCall?.Answer(MediaOption.audioVideo(curCallView.LocalViewHandle, curCallView.RemoteViewHandle), result =>
    {
        if (!result.Success)
        {
            System.Console.WriteLine($"Error: {result.Error?.errorCode.ToString()} {result.Error?.reason}");
        }
    });
    
    ```

6. Create a new Cisco Spark space, add a user to the space, and send a message:

    ```c#
    // Create a Cisco Spark room:
    SparkSDK.Room room = null;
    spark?.Rooms.Create("hello world", null, rsp =>
    {
        if (rsp.Success){
            room = rsp.Data;
            System.Console.WriteLine("create space successfully");
        }
    });
    
    // Add a user to the room
    spark?.Memberships.CreateByPersonEmail(room?.Id, "email address", false, rsp =>
    {
        if (rsp.Success)
        {
            System.Console.WriteLine("add user successfully");
        }
    });
    
    // send message to the room
    spark?.Messages.PostToRoom(room?.Id, "hello", null, rsp =>
    {
        if(rsp.Success)
        {
            System.Console.WriteLine("post message successfully");
        }
    });
    
    ```


## Contribute

Pull requests welcome. To suggest changes to the SDK, please fork this repository and submit a pull request with your changes. Your request will be reviewed by one of the project maintainers.

## License

&copy; 2016-2017 Cisco Systems, Inc. and/or its affiliates. All Rights Reserved.

See [LICENSE](https://github.com/ciscospark/spark-windows-sdk/blob/master/LICENSE) for details.
