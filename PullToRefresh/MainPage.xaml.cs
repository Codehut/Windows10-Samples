using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PullToRefresh
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private ObservableCollection<string> feed = new ObservableCollection<string>();
        private int lastValue = 0;
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
            Window.Current.SizeChanged += Current_SizeChanged;
            PopulateFeed();
            InnerCustomPanel.SizeChanged += InnerCustomPanel_SizeChanged;
        }

        private void InnerCustomPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            PTRScrollViewer.ChangeView(null, 100.0, null, true);
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            InnerCustomPanel.InvalidateMeasure();
        }

        //For first time
        private void PopulateFeed()
        {
            feed.Clear();

            feed.Add("Jan");
            feed.Add("Feb");
            feed.Add("Mar");
            feed.Add("Apr");
            feed.Add("May");
            feed.Add("Jun");
            contentListView.ItemsSource = feed;
            lastValue = 6;
        }

        //Update in each pull
        private void UpdateFeed()
        {
            if (lastValue == 12)
            {
                PopulateFeed();
            }
            else
            {
                feed.Add("Jul");
                feed.Add("Aug");
                feed.Add("Sep");
                feed.Add("Oct");
                feed.Add("Nov");
                feed.Add("Dec");
                lastValue = 12;
            }
            PTRScrollViewer.ChangeView(null, 0, null, true);
            VisualStateManager.GoToState(this, "PullToRefresh", false);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Hide the refresh indicator (PTRScrollViewer is the outer ScrollViewer)
            PTRScrollViewer.ChangeView(null, 100, null);
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ScrollViewer sv = sender as ScrollViewer;
            if (sv.VerticalOffset == 0)
            {
                PTRScrollViewer.DirectManipulationCompleted += PTRScrollViewer_DirectManipulationCompleted;
                VisualStateManager.GoToState(this, "Refreshing", false);
            }
        }

        private void PTRScrollViewer_DirectManipulationCompleted(object sender, object e)
        {
            PTRScrollViewer.DirectManipulationCompleted -= PTRScrollViewer_DirectManipulationCompleted;
            UpdateFeed();
        }

    }
}
