using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
namespace PreJobListingApp
{
    /**
     * A <see cref="RecyclerView"/> with an optional maximum height.
     */
     [Register("com.techno1d.app.MaxHeightRecyclerView")]
    public class MaxHeightRecyclerView : RecyclerView
    {
        static readonly string TAG = nameof(MaxHeightRecyclerView);

        private int mMaxHeight = -1;

        public MaxHeightRecyclerView(Context context, IAttributeSet attrs) : base(context, attrs) { }

        protected override void OnMeasure(int widthSpec, int heightSpec)
        {
            var mode = MeasureSpec.GetMode(heightSpec);
            int height = MeasureSpec.GetSize(heightSpec);

            if (mMaxHeight >= 0 && (mode == Android.Views.MeasureSpecMode.Unspecified || height > mMaxHeight))
            {
                heightSpec = MeasureSpec.MakeMeasureSpec(mMaxHeight, Android.Views.MeasureSpecMode.AtMost);
            }

            base.OnMeasure(widthSpec, heightSpec);
        }

        /**
         * Sets the maximum height for this recycler view.
         */
        public void SetMaxHeight(int maxHeight)
        {
            if (mMaxHeight != maxHeight)
            {
                mMaxHeight = maxHeight;
                RequestLayout();
            }
        }
    }
}