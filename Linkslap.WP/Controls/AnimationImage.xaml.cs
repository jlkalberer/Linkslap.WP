namespace Linkslap.WP.Controls
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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
                        ProgressRing.Visibility = Visibility.Visible;
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
                this.ProgressRing.Visibility = Visibility.Collapsed;
                this.ErrorIcon.Visibility = Visibility.Visible;

                return;
            }

            try
            {
                var speeds = new List<TimeSpan>();
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
                        var delayPropertyDictionary = await frame.BitmapProperties.GetPropertiesAsync(new List<string> { "/grctlext/Delay" });

                        if (delayPropertyDictionary.ContainsKey("Delay"))
                        {
                            speeds.Add(TimeSpan.FromMilliseconds(double.Parse(delayPropertyDictionary["Delay"].Value.ToString()) * 10));
                        }
                        else
                        {
                            speeds.Add(TimeSpan.FromMilliseconds(100));
                        }

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
                BuildStoryBoard(speeds);

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
                if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                {
                    this.ProgressRing.Visibility = Visibility.Collapsed;
                    this.ErrorIcon.Visibility = Visibility.Visible;
                }
            }
        }

        async private Task LoadImage()
        {
            if (string.IsNullOrEmpty(ImageUrl) || !ImageUrl.ToLower().EndsWith(".gif"))
            {
                this.ProgressRing.Visibility = Visibility.Collapsed;
                this.ErrorIcon.Visibility = Visibility.Visible;

                return;
            }

            try
            {
                var speeds = new List<TimeSpan>();
                using (var webClient = new HttpClient())
                {
                    using (var response = await webClient.GetAsync(ImageUrl))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            this.ProgressRing.Visibility = Visibility.Collapsed;
                            this.ErrorIcon.Visibility = Visibility.Visible;
                            return;
                        }

                        using (var imageStream = await response.Content.ReadAsStreamAsync())
                        {
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
                                var totalWidth = 0;

                                // Extract each frame and create a WriteableBitmap for each of these (store them in an internal list)
                                for (uint frameIndex = 0; frameIndex < FrameCount; frameIndex++)
                                {
                                    var frame = await imageDecoder.GetFrameAsync(frameIndex);
                                    var propertyDictionary =
                                        await
                                        frame.BitmapProperties.GetPropertiesAsync(
                                            new List<string>
                                                {
                                                    "/imgdesc/Width",
                                                    "/imgdesc/Height",
                                                    "/imgdesc/Left",
                                                    "/imgdesc/Top",
                                                    "/grctlext/Delay"
                                                });
                                    var test =
                                        await
                                        frame.BitmapProperties.GetPropertiesAsync(new List<string> { "/grctlext" });
                                    var test2 =
                                        await
                                        (test["/grctlext"].Value as BitmapPropertiesView).GetPropertiesAsync(
                                            new List<string>());
                                    var list = test2.ToList();

                                    speeds.Add(
                                        TimeSpan.FromMilliseconds(
                                            double.Parse(propertyDictionary["/grctlext/Delay"].Value.ToString()) * 10));

                                    var top = int.Parse(propertyDictionary["/imgdesc/Top"].Value.ToString());
                                    var left = int.Parse(propertyDictionary["/imgdesc/Left"].Value.ToString());
                                    var width = int.Parse(propertyDictionary["/imgdesc/Width"].Value.ToString());
                                    var height = int.Parse(propertyDictionary["/imgdesc/Height"].Value.ToString());

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

                                    if (lastFrame != null)
                                    {
                                        for (var j = 0; j < height; j++)
                                        {
                                            for (var i = 0; i < width; i++)
                                            {
                                                var offset = (i + (j * width)) * 4;
                                                if (pixels[offset + 3] == 0)
                                                {
                                                    continue;
                                                }

                                                var overallOffset = (((top + j) * totalWidth) + i + left) * 4;

                                                lastFrame[overallOffset] = pixels[offset];
                                                lastFrame[overallOffset + 1] = pixels[offset + 1];
                                                lastFrame[overallOffset + 2] = pixels[offset + 2];
                                                lastFrame[overallOffset + 3] = pixels[offset + 3];
                                            }
                                        }
                                    }

                                    if (lastFrame == null)
                                    {
                                        lastFrame = pixels;
                                        totalWidth = int.Parse(propertyDictionary["/imgdesc/Width"].Value.ToString());
                                    }

                                    using (var bitmapStream = writeableBitmap.PixelBuffer.AsStream())
                                    {
                                        bitmapStream.Write(lastFrame, 0, lastFrame.Length);
                                    }

                                    // Finally we have a frame (WriteableBitmap) that can internally be stored.
                                    this.bitmapFrames.Add(writeableBitmap);
                                }
                            }
                        }

                    }
                }

                var speedToSet = speeds[0] != TimeSpan.Zero ? speeds[0] : TimeSpan.FromMilliseconds(100);

                for (var i = 0; i < speeds.Count; i += 1)
                {
                    if (speeds[i] == TimeSpan.Zero)
                    {
                        speeds[i] = speedToSet;
                    }
                }

                //  Fill out the story board for the animation magic
                BuildStoryBoard(speeds);

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
                this.ProgressRing.Visibility = Visibility.Collapsed;
                this.ErrorIcon.Visibility = Visibility.Visible;
            }

            ProgressRing.Visibility = Visibility.Collapsed;
        }

        private void BuildStoryBoard(List<TimeSpan> speeds)
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

            // Create each DiscreteObjectKeyFrame and advance the KeyTime by 100 ms (=10 fps) and add it to 
            // the storyboard.
            for (int frameIndex = 0; frameIndex < this.bitmapFrames.Count; frameIndex++)
            {
                var keyFrame = new DiscreteObjectKeyFrame();

                keyFrame.KeyTime = KeyTime.FromTimeSpan(ts);
                keyFrame.Value = this.bitmapFrames[frameIndex];

                ts = ts.Add(speeds[frameIndex]);
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

        /// <summary>
        /// Finalizes an instance of the <see cref="AnimationImage"/> class. 
        /// </summary>
        ~AnimationImage()
        {
            this.bitmapFrames.Clear();
        }
    }
}
