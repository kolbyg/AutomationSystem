using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation_Core.Media;

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
            else if(Variables.MediaPlayerType == "MPD")
            {
                MPDControl.Play();
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
            else if (Variables.MediaPlayerType == "MPD")
            {
                MPDControl.Pause();
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
            else if (Variables.MediaPlayerType == "MPD")
            {
                MPDControl.Stop();
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
            else if (Variables.MediaPlayerType == "MPD")
            {
                MPDControl.Next();
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
            else if (Variables.MediaPlayerType == "MPD")
            {
                MPDControl.Prev();
                return "Media Reverted";
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
            else if (Variables.MediaPlayerType == "MPD")
            {
                return "Function not supported by current media player, command ignored.";
            }
            return "Invalid Music Player";
        }
        public static string VolUp()
        {
            if (Variables.CurVolume == Variables.MaxVolume)
                return "Volume is at limit set by volume governer, cannot be set higher";
            if(Variables.CurVolume == 100)
            {
                return "Volume is at 100, cannot be set higher";
            }
            Variables.CurVolume++;
            SetVol(Variables.CurVolume);
            return "Volume turned up, current level: " + Variables.CurVolume;
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
            else if (Variables.MediaPlayerType == "MPD")
            {
                MPDControl.setup();
                return "Media player MPD has been started";
            }
            return "Invalid Music Player";
        }
        public static string VolDown()
        {
            if (Variables.CurVolume == 0)
                return "Volume is at 0, cannot be set any lower.";
            Variables.CurVolume--;
            SetVol(Variables.CurVolume);
            return "Volume turned down, current level: " + Variables.CurVolume;
        }
        public static string SetVol(int Value)
        {
            if (Value > 100 || Value < 0)
            {
                Value = 0;
                return "Chosen volume is invalid, acceptable values are from 0-100";
            }
            if (Value > Variables.MaxVolume)
            {
                Value = Variables.MaxVolume;
                return "Chosen volume is outside the allowable range set by the volume governer, acceptable values are 0-" + Variables.MaxVolume;
            }
            Variables.CurVolume = Value;
            if (Variables.MediaPlayerType == "INTERNAL")
            {
                Variables.musicPlayer.SetVol();
                return "Volume has been set to " + Variables.CurVolume;
            }
            else if (Variables.MediaPlayerType == "MPD")
            {
                MPDControl.SetVol();
                return "Volume has been set to " + Variables.CurVolume;
            }
            return "Invalid Music Player";
        }
        public static string LoadPL(string PLName)
        {
            if (Variables.MediaPlayerType == "INTERNAL")
            {
                return "Function not supported by current media player, command ignored.";
            }
            else if (Variables.MediaPlayerType == "MPD")
            {
                MPDControl.LoadPL(PLName);
                return "Playlist load queued";
            }
            return "Invalid Music Player";
        }
        public static int GetVol()
        {
            return Variables.CurVolume;
        }
        public static string GetSong()
        {
            if (Variables.MediaPlayerType == "INTERNAL")
            {
                return Variables.musicFiles[Variables.curMusicFile].Substring(Variables.musicFiles[Variables.curMusicFile].LastIndexOf('\\') + 1);
            }
            else if (Variables.MediaPlayerType == "MPD")
            {
                return MPDControl.GetSong();
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
            else if (Variables.MediaPlayerType == "MPD")
            {
                MPDControl.Rescan();
                return "Server rescan started.";
            }
            return "Invalid Music Player";
        }
        public static string Resume()
        {
            if (Variables.MediaPlayerType == "INTERNAL")
            {
                return Play();
            }
            else if (Variables.MediaPlayerType == "MPD")
            {

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
