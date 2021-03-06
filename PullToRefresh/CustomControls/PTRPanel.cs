﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace PullToRefresh.CustomControls
{
    public class PTRPanel : Panel, IScrollSnapPointsInfo
    {

        EventRegistrationTokenTable<EventHandler<object>> _verticaltable = new EventRegistrationTokenTable<EventHandler<object>>();
        EventRegistrationTokenTable<EventHandler<object>> _horizontaltable = new EventRegistrationTokenTable<EventHandler<object>>();

        public bool AreHorizontalSnapPointsRegular
        {
            get
            {
                return false;
            }
        }

        public bool AreVerticalSnapPointsRegular
        {
            get
            {
                return false;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            // need to get away from infinity
            var parent = this.Parent as FrameworkElement;
            while (!(parent is PTRBorder))
            {
                parent = parent.Parent as FrameworkElement;
            }

            var ptrBorder = parent as PTRBorder;

            // Children[0] is the Border that comprises the refresh UI
            this.Children[0].Measure(ptrBorder.availableSize);
            // Children[1] is the ListView
            this.Children[1].Measure(new Size(ptrBorder.availableSize.Width, ptrBorder.availableSize.Height));
            return new Size(this.Children[1].DesiredSize.Width, this.Children[0].DesiredSize.Height + ptrBorder.availableSize.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            // need to get away from infinity
            var parent = this.Parent as FrameworkElement;
            while (!(parent is PTRBorder))
            {
                parent = parent.Parent as FrameworkElement;
            }

            var ptrBorder = parent as PTRBorder;

            // Children[0] is the Border that comprises the refresh UI
            this.Children[0].Arrange(new Rect(0, 0, this.Children[0].DesiredSize.Width, this.Children[0].DesiredSize.Height));
            // Children[1] is the ListView
            this.Children[1].Arrange(new Rect(0, this.Children[0].DesiredSize.Height, ptrBorder.finalSize.Width, ptrBorder.finalSize.Height));
            return finalSize;
        }
        

        public IReadOnlyList<float> GetIrregularSnapPoints(Orientation orientation, SnapPointsAlignment alignment)
        {
            if (orientation == Orientation.Vertical)
            {
                var l = new List<float>();
                l.Add((float)this.Children[0].DesiredSize.Height);
                return l;
            }
            else
            {
                return new List<float>();
            }
        }

        float IScrollSnapPointsInfo.GetRegularSnapPoints(Orientation orientation, SnapPointsAlignment alignment, out float offset)
        {
            throw new NotImplementedException();
        }

        event EventHandler<object> IScrollSnapPointsInfo.HorizontalSnapPointsChanged
        {
            add
            {
                return EventRegistrationTokenTable<EventHandler<object>>
                        .GetOrCreateEventRegistrationTokenTable(ref this._horizontaltable)
                        .AddEventHandler(value);

            }
            remove
            {
                EventRegistrationTokenTable<EventHandler<object>>
                    .GetOrCreateEventRegistrationTokenTable(ref this._horizontaltable)
                    .RemoveEventHandler(value);
            }
        }

        event EventHandler<object> IScrollSnapPointsInfo.VerticalSnapPointsChanged
        {
            add
            {
                return EventRegistrationTokenTable<EventHandler<object>>
                        .GetOrCreateEventRegistrationTokenTable(ref this._verticaltable)
                        .AddEventHandler(value);

            }
            remove
            {
                EventRegistrationTokenTable<EventHandler<object>>
                    .GetOrCreateEventRegistrationTokenTable(ref this._verticaltable)
                    .RemoveEventHandler(value);
            }
        }
    }
}
