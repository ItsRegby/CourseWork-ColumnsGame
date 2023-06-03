using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WMPLib;

namespace ColumnsGame.Controls
{
    public class MusicPlayer
    {
        private WindowsMediaPlayer player;

        public MusicPlayer()
        {
            player = new WindowsMediaPlayer();
        }

        public void Play(string fileName)
        {
            player.URL = $"D:\\Projects\\CourseWork-ColumnsGame\\Music\\{fileName}";
            player.controls.play();
            if (fileName == "Title.mp3")
            {
                player.settings.setMode("loop", true);
            }
        }

        public void Stop()
        {
            player.controls.stop();
        }

    }
}
