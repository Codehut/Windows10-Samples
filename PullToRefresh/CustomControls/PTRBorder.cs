using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace PullToRefresh.CustomControls
{
    public class PTRBorder: Panel
    {
        public Size availableSize { get; set; }
        public Size finalSize { get; set; }

        protected override Size MeasureOverride(Size availableSize)
        {
            this.availableSize = availableSize;
            // Children[0] is the outer ScrollViewer
            this.Children[0].Measure(availableSize);
            return this.Children[0].DesiredSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            this.finalSize = finalSize;
            // Children[0] is the outer ScrollViewer
            this.Children[0].Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return finalSize;
        }
    }
}
