using SpeechLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Client.Tool
{
   public class MP3Play
    {
        public static uint SND_ASYNC = 0x0001;
        public static uint SND_FILENAME = 0x00020000;
        public static string PATH= ConfigurationManager.AppSettings["VoicePath"];
        
        [DllImport("winmm.dll")]
        public static extern uint mciSendString(string lpstrCommand, string lpstrReturnString, uint uReturnLength, uint hWndCallback);
        public static void Play()
        {
            string cmd = "open \"" + PATH + "\" alias temp_alias";
            mciSendString(@"close temp_alias", null, 0, 0);
            mciSendString(@cmd, null, 0, 0); //音乐文件
            mciSendString("play temp_alias repeat", null, 0, 0);
        }

        public static void Play(string path)
        {
            string cmd = "open \"" + path + "\" alias temp_alias";
            mciSendString(@"close temp_alias", null, 0, 0);
            mciSendString(@cmd, null, 0, 0); //音乐文件
            mciSendString("play temp_alias repeat", null, 0, 0);
        }

        public static void StopT()
        {
           string TemStr = "";
            TemStr = TemStr.PadLeft(128, Convert.ToChar(" "));
            APIClass.mciSendString("close media", TemStr, 128, 0);
            APIClass.mciSendString("close all", TemStr, 128, 0);

        }

        public static void Shouyin(string num) { 
        SpeechVoiceSpeakFlags flag = SpeechVoiceSpeakFlags.SVSFlagsAsync;
            
        SpVoice voice = new SpVoice();
        string voice_txt = "收款成功"+ num + "元";
        voice.Voice = voice.GetVoices(string.Empty, string.Empty).Item(0);
            
        voice.Speak(voice_txt, flag);
        }

    }
}
