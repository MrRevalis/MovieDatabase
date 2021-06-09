﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Forms;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace MovieDatabase.ViewModels
{
    [QueryProperty("Url", "url")]
    [QueryProperty("Type", "type")]
    public class PlayerViewModel : BaseViewModel
    {
        private string url;
        public string Url
        {
            get => url;
            set => SetProperty(ref url, value);
        }

        private string type;
        public string Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }

        private MediaSource streamVideo;
        public MediaSource StreamVideo
        {
            get => streamVideo;
            set => SetProperty(ref streamVideo, value);
        }

        public ICommand MediaOpenedCommand { get; private set; }
        public ICommand MediaFailerCommand { get; private set; }
        public ICommand MediaEndedCommand { get; private set; }
        public ICommand AppearingCommand { get; private set; }
        public ICommand CloseButton { get; private set; }

        public PlayerViewModel()
        {
            AppearingCommand = new Command(async () => await OnAppearing());
        }

        private async Task OnAppearing()
        {
            if (String.IsNullOrEmpty(Url))
                return;
            IsBusy = true;
            if (Type.Equals("youtube"))
            {
                string link = "https://www.youtube.com/watch?v=" + Url;
                YoutubeClient youtube = new YoutubeClient();
                Video video = await youtube.Videos.GetAsync(link);

                StreamManifest streamManifest = await youtube.Videos.Streams.GetManifestAsync(link);
                IVideoStreamInfo streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();

                if (streamInfo != null)
                {
                    Stream stream = await youtube.Videos.Streams.GetAsync(streamInfo);
                    StreamVideo = streamInfo.Url;
                }
            }
            else
            {
                StreamVideo = Url;
            }

            IsBusy = false;
        }
    }
}
