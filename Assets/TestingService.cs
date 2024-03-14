using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingService : MonoBehaviour
{
  /* 
   <?xml version = "1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android" xmlns:tools="http://schemas.android.com/tools" package="com.blocky.runner.free" android:versionCode="1" android:versionName="1.0">
  <application android:label="@string/app_name" android:icon="@drawable/app_icon">
    <!-- The MessagingUnityPlayerActivity is a class that extends
         UnityPlayerActivity to work around a known issue when receiving
         notification data payloads in the background. -->
    <activity android:name= "com.google.firebase.MessagingUnityPlayerActivity" android:configChanges= "fontScale|keyboard|keyboardHidden|locale|mnc|mcc|navigation|orientation|screenLayout|screenSize|smallestScreenSize|uiMode|touchscreen" >
      < intent - filter >
        < action android:name= "android.intent.action.MAIN" />
        < category android:name= "android.intent.category.LAUNCHER" />
      </ intent - filter >
      < meta - data android:name= "unityplayer.UnityActivity" android:value= "true" />
      < meta - data android:name= "unityplayer.ForwardNativeEventsToDalvik" android:value= "true" />

    </ activity >
    < activity android:name= "com.chartboost.sdk.CBImpressionActivity" android:excludeFromRecents= "true" android:hardwareAccelerated= "true" android:theme= "@android:style/Theme.Translucent.NoTitleBar.Fullscreen" android:configChanges= "keyboardHidden|orientation|screenSize" />
    < service android:name= "com.google.firebase.messaging.MessageForwardingService" android:exported= "false" />
    < meta - data android:name= "com.google.android.gms.ads.APPLICATION_ID" android:value= "ca-app-pub-6518156732552985~1270107387" />
        < meta - data android:name= "firebase_messaging_auto_init_enabled"
             android:value= "true" />
  < meta - data android:name= "firebase_analytics_collection_enabled"
             android:value= "false" />
    < activity android:name= "com.facebook.unity.FBUnityLoginActivity" android:configChanges= "fontScale|keyboard|keyboardHidden|locale|mnc|mcc|navigation|orientation|screenLayout|screenSize|smallestScreenSize|uiMode|touchscreen" android:theme= "@android:style/Theme.Translucent.NoTitleBar.Fullscreen" />
    < activity android:name= "com.facebook.unity.FBUnityDialogsActivity" android:configChanges= "fontScale|keyboard|keyboardHidden|locale|mnc|mcc|navigation|orientation|screenLayout|screenSize|smallestScreenSize|uiMode|touchscreen" android:theme= "@android:style/Theme.Translucent.NoTitleBar.Fullscreen" />
    < activity android:name= "com.facebook.unity.FBUnityAppLinkActivity" android:exported= "true" />
    < activity android:name= "com.facebook.unity.FBUnityDeepLinkingActivity" android:exported= "true" />
    < activity android:name= "com.facebook.unity.FBUnityGameRequestActivity" />
    < activity android:name= "com.facebook.unity.FBUnityCreateGameGroupActivity" />
    < activity android:name= "com.facebook.unity.FBUnityJoinGameGroupActivity" />
    < activity android:name= "com.facebook.unity.AppInviteDialogActivity" />
    < meta - data android:name= "com.facebook.sdk.ApplicationId" android:value= "fb1967536380042623" />
    < meta - data android:name= "com.facebook.sdk.AutoLogAppEventsEnabled" android:value= "true" />
    < meta - data android:name= "com.facebook.sdk.AdvertiserIDCollectionEnabled" android:value= "true" />
    < provider android:name= "com.facebook.FacebookContentProvider" android:authorities= "com.facebook.app.FacebookContentProvider1967536380042623" android:exported= "true" />
    < meta - data android:name= "com.google.android.gms.version" android:value= "@integer/google_play_services_version" />
      < activity android:name= "com.facebook.ads.AudienceNetworkActivity" android:configChanges= "keyboardHidden|orientation|screenSize" />
  </ application >
    < uses - permission android:name= "com.android.vending.BILLING" />
     < uses - permission android:name= "android.permission.ACCESS_WIFI_STATE" />
    < uses - permission android:name= "android.permission.WRITE_EXTERNAL_STORAGE" />
    < !--Allows the SDK to handle video playback when interrupted by a call -->
    <uses-permission android:name= "android.permission.READ_PHONE_STATE" />
  < uses - permission android:name= "android.permission.READ_PHONE_STATE" tools:node= "remove" />
  < uses - feature android:glEsVersion= "0x00020000" />
  < uses - feature android:name= "android.hardware.touchscreen" android:required= "false" />
  < uses - feature android:name= "android.hardware.touchscreen.multitouch" android:required= "false" />
  < uses - feature android:name= "android.hardware.touchscreen.multitouch.distinct" android:required= "false" />
</ manifest > 
             */

}
