using Newtonsoft.Json;
using Physics.SelfStudy.Json;
using Physics.SelfStudy.Models;
using Physics.SelfStudy.Models.Contents.Abstract;
using Physics.SelfStudy.Viewers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Physics.SelfStudy.Editor
{
    public sealed partial class EditorHostControl : UserControl
    {
        private SerialDisposable _propertySubscription = new SerialDisposable();

        public EditorHostControl()
        {
            this.InitializeComponent();
            this.Unloaded += EditorHostControl_Unloaded;
        }

        public IContent EditedContent
        {
            get { return (IContent)GetValue(EditedContentProperty); }
            set { SetValue(EditedContentProperty, value); }
        }

        public static readonly DependencyProperty EditedContentProperty =
            DependencyProperty.Register("EditedContent", typeof(IContent), typeof(EditorHostControl), new PropertyMetadata(null, OnEditedContentChanged));

        private static void OnEditedContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editorHost = (EditorHostControl)d;
            if (e.NewValue is IContent content)
            {
                editorHost.SetupEditor(content);
                editorHost.UpdatePreview();
            }
        }

        private void EditorHostControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _propertySubscription?.Dispose();
        }

        private void SetupEditor(IContent content)
        {
            _propertySubscription.Disposable = 
                Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    h => content.PropertyChanged += h,
                    h => content.PropertyChanged -= h)                
                .Throttle(TimeSpan.FromMilliseconds(500))
                .ObserveOn(Dispatcher)
                .Subscribe(input => UpdatePreview());            
        }

        private void UpdatePreview()
        {
            var options = new JsonSerializerSettings()
            {
                Converters =
                {
                    new ContentConverter()
                }
            };
            var serialized = JsonConvert.SerializeObject(EditedContent);

            var deserialized = JsonConvert.DeserializeObject<IContent>(serialized, options);
            ViewerControl.Content = deserialized;
        }
    }
}
