using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_Core.Control
{
    class Media
    { 
        public static string Play()
        {
            if(Variables.MediaPlayerType == "INTERNAL")
            {
                Variables.musicPlayer.Play();
                return "Media Playing";
            }
            return "Invalid Music Player";
        }
        public static string Pause()
        {
            if (Variables.MediaPlayerType == "INTERNAL")
            {
                Variables.musicPlayer.Pause();
                return "Media Paused";
            }
            return "Invalid Music Player";
        }
        public static string Stop()
        {
            if (Variables.MediaPlayerType == "INTERNAL")
            {
                Variables.musicPlayer.Stop();
                return "Media Stopped";
            }
            return "Invalid Music Player";
        }
        public static string Next()
        {
            if (Variables.MediaPlayerType == "INTERNAL")
            {
                Variables.musicPlayer.Next();
                return "Media Advanced";
            }
            return "Invalid Music Player";
        }
        public static string Prev()
        {
            if (Variables.MediaPlayerType == "INTERNAL")
            {
                return "Function not supported by current media player, command ignored.";
            }
            return "Invalid Music Player";
        }
        public static string ResetRandomization()
        {
            if (Variables.MediaPlayerType == "INTERNAL")
            {
                Variables.musicPlayer.ResetRandomization();
                return "Randomization reset";
            }
            return "Invalid Music Player";
        }
        public static string VolUp()
        {
            if (Variables.MediaPlayerType == "INTERNAL")
            {
                if (Variables.musicPlayer.Volume >= 100)
                {
                    return "Volume is at maximum";
                }
                if (Variables.musicPlayer.Volume >= Variables.MaxVolume)
                {
                    return "Volume is at maximum, limit set to " + Variables.MaxVolume.ToString() + " by volume governer";
                }
                Variables.musicPlayer.Volume += 5;
                if (Variables.musicPlayer.Volume > 100) Variables.musicPlayer.Volume = 100;
                if (Variables.musicPlayer.Volume > Variables.MaxVolume) Variables.musicPlayer.Volume = Variables.MaxVolume;
                Variables.musicPlayer.SetVol();
                return "Volume turned up, current level: " + Variables.musicPlayer.Volume;

            }
            return "Invalid Music Player";
        }
        public static string InitPlayer()
        {
            if (Variables.MediaPlayerType == "INTERNAL")
            {
                Variables.musicPlayer = new Automation_Core.Media.MusicPlayer();
                Variables.musicPlayer.setup();
                Variables.musicPlayer.BeginPlay();
                return "Media player INTERNAL has been started";
            }
            return "Invalid Music Player";
        }
        public static string VolDown()
        {
            if (Variables.MediaPlayerType == "INTERNAL")
            {
                if (Variables.musicPlayer.Volume <= 0)
                {
                    return "Volume is at minimum";
                }
                Variables.musicPlayer.Volume -= 5;
                if (Variables.musicPlayer.Volume < 0) Variables.musicPlayer.Volume = 0;
                Variables.musicPlayer.SetVol();
                return "Volume turned down, current level: " + Variables.musicPlayer.Volume;
            }
            return "Invalid Music Player";
        }
        public static string SetVol(int Value)
        {
            if (Variables.MediaPlayerType == "INTERNAL")
            {
                if (Convert.ToInt32(Value) > 100 || Convert.ToInt32(Value) < 0)
                    return "Chosen volume is invalid, acceptable values are from 0-100";
                if (Convert.ToInt32(Value) > Variables.MaxVolume)
                    return "Chosen volume is outside the allowable range set by the volume governer, acceptable values are 0-" + Variables.MaxVolume;
                Variables.musicPlayer.Volume = Convert.ToInt32(Value);
                Variables.musicPlayer.SetVol();
                return "Volume has been set to " + Variables.musicPlayer.Volume;
            }
            return "Invalid Music Player";
        }
        public static int GetVol()
        {
            if (Variables.MediaPlayerType == "INTERNAL")
            {
                return Variables.musicPlayer.Volume;
            }
            Variables.logger.LogLine("Invalid Music Player");
            return 0;
        }
        public static string GetSong()
        {
            if (Variables.MediaPlayerType == "INTERNAL")
            {
                return Variables.musicFiles[Variables.curMusicFile].Substring(Variables.musicFiles[Variables.curMusicFile].LastIndexOf('\\') + 1);
            }
            return "Invalid Music Player";
        }
        public static string Rescan()
        {
            if (Variables.MediaPlayerType == "INTERNAL")
            {
                Variables.musicPlayer.refreshPlayList();
                return "All media folders have been rescanned.";
            }
            return "Invalid Music Player";
        }
        public static string Resume()
        {
            if (Variables.MediaPlayerType == "INTERNAL")
            {
                return Play();
            }
            return "Invalid Music Player";
        }
        public static string Shuffle()
        {
            if (Variables.MediaPlayerType == "INTERNAL")
            {
                return "Function not supported by current media player, command ignored.";
            }
            return "Invalid Music Player";
        }
    }
}
