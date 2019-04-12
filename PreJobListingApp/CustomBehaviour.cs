using Android.Content;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Views;

namespace PreJobListingApp
{
    [Register("com.biiom.app.CustomBehavior")]
    class CustomBehavior : CoordinatorLayout.Behavior
    {
        static readonly string TAG = nameof(CustomBehavior);

        public CustomBehavior(Context context, IAttributeSet set) : base(context, set) { }

        public override bool LayoutDependsOn(CoordinatorLayout parent, Java.Lang.Object child, View dependency)
        {
            // List the toolbar container as a dependency to ensure that it will
            // always be laid out before the child (which depends on the toolbar
            // container's height in onLayoutChild() below).
            return dependency.Id == Resource.Id.toolbar_container; 
        }

        public override bool OnLayoutChild(CoordinatorLayout parent, Java.Lang.Object child, int layoutDirection)
        {
            if (!(child is NestedScrollView view))
                return base.OnLayoutChild(parent, child, layoutDirection);

            // First layout the child as normal.
            parent.OnLayoutChild(view, layoutDirection);

            // Center the FAB vertically along the top edge of the card.
            int fabHalfHeight = view.FindViewById(Resource.Id.fab).Height / 2;            
            SetTopMargin(view.FindViewById(Resource.Id.cardview), fabHalfHeight);

            // Give the RecyclerView a maximum height to ensure the card will never
            // overlap the toolbar as it scrolls.
            int rvMaxHeight =
                view.Height
                    - fabHalfHeight
                    - view.FindViewById(Resource.Id.card_title).Height
                    - view.FindViewById(Resource.Id.card_subtitle).Height;

            MaxHeightRecyclerView rv = view.FindViewById<MaxHeightRecyclerView>(Resource.Id.card_recyclerview);
            rv.SetMaxHeight(rvMaxHeight);

            // Give the card container top padding so that only the top edge of the card
            // initially appears at the bottom of the screen. The total padding will
            // be the distance from the top of the screen to the FAB's top edge.
            View cardContainer = view.FindViewById(Resource.Id.card_container);
            int toolbarContainerHeight = parent.GetDependencies(view)[0].Height;
            SetPaddingTop(cardContainer, rvMaxHeight - toolbarContainerHeight);

            // Offset the child's height so that its bounds don't overlap the
            // toolbar container.
            ViewCompat.OffsetTopAndBottom(view, toolbarContainerHeight);

            // Add the same amount of bottom padding to the RecyclerView so it doesn't
            // display its content underneath the navigation bar.
            SetPaddingBottom(rv, toolbarContainerHeight);

            // Return true so that the parent doesn't waste time laying out the
            // child again (any modifications made above will have triggered a second
            // layout pass anyway).
            return true;
        }

        private static void SetTopMargin(View v, int topMargin)
        {
            ViewGroup.MarginLayoutParams lp = (ViewGroup.MarginLayoutParams)v.LayoutParameters;

            if (lp.TopMargin != topMargin)
            {
                lp.TopMargin = topMargin;
                v.LayoutParameters = lp;
            }
        }

        private static void SetPaddingTop(View v, int top)
        {
            if (v.PaddingTop != top)
            {
                v.SetPadding(v.PaddingLeft, top, v.PaddingRight, v.PaddingBottom);
            }
        }

        private static void SetPaddingBottom(View v, int bottom)
        {
            if (v.PaddingBottom != bottom)
            {
                v.SetPadding(v.PaddingLeft, v.PaddingTop, v.PaddingRight, bottom);
            }
        }

        public override bool OnInterceptTouchEvent(CoordinatorLayout parent, Java.Lang.Object child, MotionEvent ev)
        {
            if (child is NestedScrollView view)
            {
                // Block all touch events that originate within the bounds of our
                // NestedScrollView but do *not* originate within the bounds of its
                // inner CardView and FloatingActionButton.
                return ev.ActionMasked == MotionEventActions.Down
                    && IsTouchInChildBounds(parent, view, ev)
                    && !IsTouchInChildBounds(parent, view.FindViewById(Resource.Id.cardview), ev)
                    && !IsTouchInChildBounds(parent, view.FindViewById(Resource.Id.fab), ev);
            }
            else
                return base.OnInterceptTouchEvent(parent, child, ev);
        }

        private static bool IsTouchInChildBounds(ViewGroup parent, View child, MotionEvent ev)
        {
            return ViewGroupUtils.IsPointInChildBounds(
                parent, child, (int)ev.GetX(), (int)ev.GetY());
        }
    }
}