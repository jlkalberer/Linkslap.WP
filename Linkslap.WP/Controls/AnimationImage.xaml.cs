namespace Linkslap.WP.Controls
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Threading.Tasks;

    using Windows.Graphics.Imaging;
    using Windows.Storage;
    using Windows.Storage.Streams;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media.Animation;
    using Windows.UI.Xaml.Media.Imaging;

    public sealed partial class AnimationImage : UserControl
    {
        #region Private Fields

        private static readonly DependencyProperty ImageUrlDependencyProperty = DependencyProperty.Register(
            "ImageUrl",
            typeof(string),
            typeof(AnimationImage),
            new PropertyMetadata(String.Empty, ImageUrlPropertyChanged));

        private readonly List<WriteableBitmap> bitmapFrames = new List<WriteableBitmap>();

        /// <summary>
        /// The play on load.
        /// </summary>
        private bool playOnLoad = true;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the url of the image e.g. "/Assets/MyAnimation.gif"
        /// </summary>
        public string ImageUrl
        {
            get
            {
                return (string)GetValue(ImageUrlDependencyProperty);
            }
            set
            {
                this.SetValue(ImageUrlDependencyProperty, value);
            }
        }

        /// <summary>
        /// Gets the number of frames in the gif animation
        /// </summary>
        public uint FrameCount { get; private set; }

        public bool PlayOnLoad
        {
            get
            {
                return this.playOnLoad;
            }
            set
            {
                this.playOnLoad = value;
            }
        }

        #endregion

        #region Constructors

        public AnimationImage()
        {
            this.InitializeComponent();
            
            this.IsEnabledChanged += async (sender, args) =>
                {
                    if (this.IsEnabled)
                    {
                        await this.LoadImage();
                    }
                    else
                    {
                        this.storyboard.Stop();
                        this.storyboard.Children.Clear();
                        this.bitmapFrames.Clear();
                    }
                };
        }

        #endregion

        #region Private Methods

        private async void LoadImageLocal()
        {
            if (String.IsNullOrEmpty(ImageUrl))
            {
                return;
            }

            try
            {
                // Get the file e.g. "/Assets/MyAnimation.gif"
                var storageFile =
                    await StorageFile.GetFileFromApplicationUriAsync(new Uri(String.Format("ms-appx://{0}", ImageUrl)));

                using (var res = await storageFile.OpenAsync(FileAccessMode.Read))
                {
                    // Get the GIF decoder, to perform the magic
                    var decoder = await BitmapDecoder.CreateAsync(BitmapDecoder.GifDecoderId, res);

                    // Now we know the number of frames
                    FrameCount = decoder.FrameCount;

                    byte[] firstFrame = null;

                    //  Extract each frame and create a WriteableBitmap for each of these (store them in an internal list)
                    for (uint frameIndex = 0; frameIndex < FrameCount; frameIndex++)
                    {
                        var frame = await decoder.GetFrameAsync(frameIndex);

                        var writeableBitmap = new WriteableBitmap(
                            (int)decoder.OrientedPixelWidth,
                            (int)decoder.OrientedPixelHeight);

                        //  Extract the pixel data and fill the WriteableBitmap with them
                        var bitmapTransform = new BitmapTransform();
                        var pixelDataProvider =
                            await
                            frame.GetPixelDataAsync(
                                BitmapPixelFormat.Bgra8,
                                decoder.BitmapAlphaMode,
                                bitmapTransform,
                                ExifOrientationMode.IgnoreExifOrientation,
                                ColorManagementMode.DoNotColorManage);
                        var pixels = pixelDataProvider.DetachPixelData();

                        if (frameIndex == 0)
                        {
                            firstFrame = pixels;
                        }
                        else
                        {
                            for (var i = 0; i < pixels.Length; i += 1)
                            {
                                pixels[i] &= firstFrame[i];
                            }
                        }

                        using (var stream = writeableBitmap.PixelBuffer.AsStream())
                        {
                            stream.Write(pixels, 0, pixels.Length);
                        }

                        //  Finally we have a frame (WriteableBitmap) that can internally be stored.
                        this.bitmapFrames.Add(writeableBitmap);
                    }
                }

                //  Fill out the story board for the animation magic
                BuildStoryBoard();

                //  Start the animation if needed and fire the event
                if (PlayOnLoad)
                {
                    storyboard.Begin();

                    if (ImageLoaded != null)
                    {
                        ImageLoaded(this, null);
                    }
                }
            }
            catch (Exception exception)
            {
                //  Yeah, I know this is kinda' "cowboyish" - but hey, I don't want it to fail in the designer!
                if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                {
                    throw;
                }
            }
        }

        async private Task LoadImage()
        {
            if (string.IsNullOrEmpty(ImageUrl) || !ImageUrl.ToLower().EndsWith(".gif"))
            {
                return;
            }

            try
            {
                using (var webClient = new HttpClient())
                {
                    using (var response = await webClient.GetAsync(ImageUrl))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            return;
                        }

                        var imageStream = await response.Content.ReadAsStreamAsync();

                        BitmapDecoder imageDecoder;
                        using (var randomAccessStream = new InMemoryRandomAccessStream())
                        {
                            await imageStream.CopyToAsync(randomAccessStream.AsStreamForWrite());
                            imageDecoder =
                                await BitmapDecoder.CreateAsync(BitmapDecoder.GifDecoderId, randomAccessStream);

                            FrameCount = imageDecoder.FrameCount;
                            // Clear old bitmaps in case the control is being reused
                            bitmapFrames.Clear();

                            byte[] lastFrame = null;

                            // Extract each frame and create a WriteableBitmap for each of these (store them in an internal list)
                            for (uint frameIndex = 0; frameIndex < FrameCount; frameIndex++)
                            {
                                var frame = await imageDecoder.GetFrameAsync(frameIndex);
                                var writeableBitmap = new WriteableBitmap(
                                    (int)imageDecoder.OrientedPixelWidth,
                                    (int)imageDecoder.OrientedPixelHeight);

                                // Extract the pixel data and fill the WriteableBitmap with them
                                var bitmapTransform = new BitmapTransform();

                                var pixelDataProvider =
                                    await
                                    frame.GetPixelDataAsync(
                                        BitmapPixelFormat.Bgra8,
                                        imageDecoder.BitmapAlphaMode,
                                        bitmapTransform,
                                        ExifOrientationMode.IgnoreExifOrientation,
                                        ColorManagementMode.DoNotColorManage);

                                var pixels = pixelDataProvider.DetachPixelData();

                                if (lastFrame != null && lastFrame.Length == pixels.Length)
                                {
                                    for (var i = 0; i < pixels.Length; i += 4)
                                    {
                                        if (pixels[i + 3] != 0)
                                        {
                                            continue;
                                        }

                                        pixels[i] = lastFrame[i];
                                        pixels[i + 1] = lastFrame[i + 1];
                                        pixels[i + 2] = lastFrame[i + 2];
                                        pixels[i + 3] = lastFrame[i + 3];
                                    }
                                }

                                if (lastFrame == null || lastFrame.Length == pixels.Length)
                                {
                                    lastFrame = pixels;

                                    using (var bitmapStream = writeableBitmap.PixelBuffer.AsStream())
                                    {
                                        bitmapStream.Write(pixels, 0, pixels.Length);
                                    }

                                    // Finally we have a frame (WriteableBitmap) that can internally be stored.
                                    bitmapFrames.Add(writeableBitmap);
                                }
                            }
                        }
                    }
                }

                //  Fill out the story board for the animation magic
                BuildStoryBoard();

                //  Start the animation if needed and fire the event
                if (PlayOnLoad)
                {
                    storyboard.Begin();

                    if (ImageLoaded != null)
                    {
                        ImageLoaded(this, null);
                    }
                }
            }
            catch (Exception ex)
            {
                var v = 0;
            }
        }

        private void BuildStoryBoard()
        {
            //  Clear the story board, if it has previously been filled
            if (storyboard.Children.Count > 0)
            {
                storyboard.Children.Clear();
            }

            //  Now create the animation as a set of ObjectAnimationUsingKeyFrames (I love this name!)
            var anim = new ObjectAnimationUsingKeyFrames();
            anim.BeginTime = TimeSpan.FromSeconds(0);

            var ts = new TimeSpan();
            var speed = TimeSpan.FromMilliseconds(100); // Standard GIF framerate 10 fps?

            // Create each DiscreteObjectKeyFrame and advance the KeyTime by 100 ms (=10 fps) and add it to 
            // the storyboard.
            for (int frameIndex = 0; frameIndex < this.bitmapFrames.Count; frameIndex++)
            {
                var keyFrame = new DiscreteObjectKeyFrame();

                keyFrame.KeyTime = KeyTime.FromTimeSpan(ts);
                keyFrame.Value = this.bitmapFrames[frameIndex];

                ts = ts.Add(speed);
                anim.KeyFrames.Add(keyFrame);
            }

            //  Connect the image control with the story board

            Storyboard.SetTarget(anim, image);
            Storyboard.SetTargetProperty(anim, "Source");

            //  And finally add the animation-set to the storyboard
            storyboard.Children.Add(anim);
        }

        private static void ImageUrlPropertyChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            return;

            var control = (AnimationImage)sender;

            if (!control.IsEnabled)
            {
                return;
            }

            control.LoadImage();
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Fired whenever the image has loaded
        /// </summary>
        public EventHandler ImageLoaded;

        #endregion
    }
}
