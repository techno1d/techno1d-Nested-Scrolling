using Android.Content;
using Android.Support.V4.View;
using Android.Support.V4.Widget;

using Android.Util;
using Android.Views;

namespace PreJobListingApp
{
    ///A <see cref="NestedScrollView"/> that implements the <see cref="INestedScrollingParent2"/> interface.        
    public class NestedScrollView2 : NestedScrollView, INestedScrollingParent2
    {
        private NestedScrollingParentHelper parentHelper;

        public override ScrollAxis NestedScrollAxes
        {
            get
            {
                return (ScrollAxis)parentHelper.NestedScrollAxes;
            }
        }

        public NestedScrollView2(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            parentHelper = new NestedScrollingParentHelper(this);
        }

        // NestedScrollingParent2 methods.
        public override bool OnStartNestedScroll(View child, View target, int axes, int type)
        {
            return (axes & ViewCompat.ScrollAxisVertical) != 0;
        }

        public override void OnNestedScrollAccepted(View child, View target, int axes, int type)
        {
            parentHelper.OnNestedScrollAccepted(child, target, axes);
            StartNestedScroll(ScrollAxis.Vertical, type);
        }

        public override void OnNestedPreScroll(View target, int dx, int dy, int[] consumed, int type)
        {
            DispatchNestedPreScroll(dx, dy, consumed, null, type);
        }

        public override void OnNestedScroll(View target, int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed, int type)
        {
            int oldScrollY = ScrollY;
            ScrollBy(0, dyUnconsumed);
            int myConsumed = ScrollY - oldScrollY;
            int myUnconsumed = dyUnconsumed - myConsumed;

            DispatchNestedScroll(0, myConsumed, 0, myUnconsumed, null, type);
        }

        public override void OnStopNestedScroll(View target, int type)
        {
            parentHelper.OnStopNestedScroll(target, type);
            StopNestedScroll(type);
        }

        // NestedScrollingParent methods. For the most part these methods delegate
        // to the NestedScrollingParent2 methods above, passing TYPE_TOUCH as the
        // type to maintain API compatibility.
        public override bool OnStartNestedScroll(View child, View target, ScrollAxis axes)
        {
            return OnStartNestedScroll(child, target, (int)axes, ViewCompat.TypeTouch);
        }

        public override void OnNestedScrollAccepted(View child, View target, ScrollAxis axes)
        {
            OnNestedScrollAccepted(child, target, (int)axes, ViewCompat.TypeTouch);
        }

        public override void OnNestedPreScroll(View target, int dx, int dy, int[] consumed)
        {
            OnNestedPreScroll(target, dx, dy, consumed, ViewCompat.TypeTouch);
        }

        public override void OnNestedScroll(View target, int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed)
        {
            OnNestedScroll(target, dxConsumed, dyConsumed, dxUnconsumed, dyUnconsumed, ViewCompat.TypeTouch);
        }

        public override void OnStopNestedScroll(View target)
        {
            OnStopNestedScroll(target, ViewCompat.TypeTouch);
        }
    }

}