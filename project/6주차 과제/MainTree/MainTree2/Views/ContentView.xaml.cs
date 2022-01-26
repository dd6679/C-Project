using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MainTree2.Models;
using MainTree2.ViewModels;
using VMS.Codec.Lib;

namespace MainTree2.Views
{
    /// <summary>
    /// ContentView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ContentView : UserControl
    {
        ContentViewVM _vm;
        ImageControl _imageControl;
        public ContentView()
        {
            InitializeComponent();

            this.DataContext = new ContentViewVM();
            _vm = this.DataContext as ContentViewVM;
            _imageControl = new ImageControl(viewer);
        }

        public ContentData ContentImage
        {
            get { return (ContentData)GetValue(ContentImageProperty); }
            set { SetValue(ContentImageProperty, value); }
        }

        public static readonly DependencyProperty ContentImageProperty = DependencyProperty.Register("ContentImage",
            typeof(ContentData), typeof(ContentView), new UIPropertyMetadata(new PropertyChangedCallback(OnContentImagePropertyChanged)));

        public static void OnContentImagePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((ContentView)obj).OnContentImagePropertyChanged(e);
        }

        protected void OnContentImagePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            var cd = _vm.ContentData;
            if (cd != null)
            {
                _imageControl.Update(cd.imgBuffer, cd.width, cd.height, PixelFormats.Bgr32);
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            var data = e.Data.GetFormats();
            foreach (var i in data)
            {     
                if (i.ToString() == "MainTree2.Models.LeafNodeItem")
                {
                    LeafNodeItem item = (LeafNodeItem)e.Data.GetData(i);
                    _vm.AddMedia();
                    _vm.DevNick = item.Name;
                }
            }
        }
    }
}
