using System;
using Windows.UI.Xaml.Controls;

using MyScript.Certificate;
using MyScript.Atk.MathWidget;
using System.Diagnostics;
using Windows.UI.Popups;
using Windows.UI.Xaml;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MainPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MathWidget mathWidget;
        public MainPage()
        {
            this.InitializeComponent();

            mathWidget = new MathWidget();

            if (!mathWidget.RegisterCertificate((byte[])(Array)MyCertificate.Bytes))
            {
                ShowCertificateError();
                return;
            }
            // References resources copied as Content
            var resourcesDir = System.IO.Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "Resources", "conf");
            mathWidget.AddSearchDir(resourcesDir);

            MyMathWidget.WritingBeginHandler += OnWritingBegin;
            MyMathWidget.WritingEndHandler += OnWritingEnd;

            MyMathWidget.ConfigurationBeginHandler += OnConfigurationBegin;
            MyMathWidget.ConfigurationEndHandler += OnConfigurationEnd;
            MyMathWidget.ConfigurationFailedHandler += OnConfigurationError;

            MyMathWidget.RecognitionBeginHandler += OnRecognitionBegin;
            MyMathWidget.RecognitionEndHandler += OnRecognitionEnd;

            MyMathWidget.UndoRedoStateChangedHandler += OnUndoRedoStateChanged;

            MyMathWidget.UsingAngleUnitChangedHandler += OnChangeUsingAngleUnit;

            MyMathWidget.EraseGestureHandler += OnEraseGesture;

            this.Loaded += MainPage_Loaded;

            // The configuration is an asynchronous operation. Callbacks are provided to
            // monitor the beginning and end of the configuration process.
            //
            // "math" references the math bundle name in conf/math.conf file in your resources.
            // "standard" references the configuration name in math.conf
            MyMathWidget.Configure("math", "standard");
        }
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            commandBar.DataContext = MyMathWidget;
        }
 
        private async void ShowCertificateError()
        {
            var dialog = new MessageDialog("Please use a valid certificate.", "Invalid Certificate");
            await dialog.ShowAsync();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
        #region notifications

        private void OnWritingBegin(object sender, Object obj)
        {
            System.Diagnostics.Debug.WriteLine("Writing begin");
        }

        private void OnWritingEnd(object sender, Object obj)
        {
            System.Diagnostics.Debug.WriteLine("Writing end");
        }

        private void OnConfigurationBegin(object sender, Object obj)
        {
            System.Diagnostics.Debug.WriteLine("Configuration started");
        }

        private void OnConfigurationEnd(object sender, Object obj)
        {
            System.Diagnostics.Debug.WriteLine("Configuration done");
        }

        private async void OnConfigurationError(object sender, string error)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async () =>
            {
                var dialog = new MessageDialog("Configuration error", error);
                await dialog.ShowAsync();
            });

            System.Diagnostics.Debug.WriteLine("Configuration failed. Error = {0}", error);
        }

        private void OnRecognitionBegin(object sender, Object obj)
        {
            System.Diagnostics.Debug.WriteLine("Recognition started");
        }

        private void OnRecognitionEnd(object sender, Object obj)
        {
            System.Diagnostics.Debug.WriteLine("Recognition ended");
        }

        private void OnChangeUsingAngleUnit(object sender, bool used)
        {
            System.Diagnostics.Debug.WriteLine("Using angle unit has changed. Current value is used = {0}", used);
        }

        private void OnEraseGesture(object sender, bool partial)
        {
            System.Diagnostics.Debug.WriteLine("Erase gesture done. Erase gesture is partial = {0}", partial);
        }

        private void OnUndoRedoStateChanged(object sender, Object obj)
        {
            System.Diagnostics.Debug.WriteLine("Undo redo state has changed");
        }

        #endregion
    }
}