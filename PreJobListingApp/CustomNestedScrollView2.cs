using Android.Content;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;

namespace PreJobListingApp
{
    /**
     * A custom <see cref="NestedScrollView"/> that customizes the sample app's
     * nested scrolling behavior.
     */
    [Register("com.techno1d.app.CustomNestedScrollView2")]
    public class CustomNestedScrollView2 : NestedScrollView2
    {

        public CustomNestedScrollView2(Context context, IAttributeSet attrs) : base(context, attrs) { }

        public override void OnNestedPreScroll(View target, int dx, int dy, int[] consumed, int type)
        {
            if (!(target is RecyclerView rv)) return;

            if ((dy < 0 && IsRvScrolledToTop(rv)) || (dy > 0 && !IsNsvScrolledToBottom(this)))
            {
                // The NestedScrollView should steal the scroll event away from the
                // RecyclerView if: (1) the user is scrolling their finger down and the
                // RecyclerView is scrolled to the top of its content, or (2) the user
                // is scrolling their finger up and the NestedScrollView is not scrolled
                // to the bottom of its content.
                ScrollBy(0, dy);
                consumed[1] = dy;
                return;
            }

            base.OnNestedPreScroll(target, dx, dy, consumed, type);
        }

        /**
         * Returns true iff the {@link NestedScrollView} is scrolled to the bottom
         * of its content (i.e. the card is completely expanded).
         */
        private static bool IsNsvScrolledToBottom(NestedScrollView nsv)
        {
            return !nsv.CanScrollVertically(1);
        }

        /**
         * Returns true iff the {@link RecyclerView} is scrolled to the
         * top of its content (i.e. its first item is completely visible).
         */
        private static bool IsRvScrolledToTop(RecyclerView rv)
        {
            LinearLayoutManager lm = (LinearLayoutManager)rv.GetLayoutManager();

            return lm.FindFirstVisibleItemPosition() == 0
                && lm.FindViewByPosition(0).Top == 0;
        }
    }

}