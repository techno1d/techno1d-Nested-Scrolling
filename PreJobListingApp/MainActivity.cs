using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.RecyclerView;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using FFImageLoading.Work;
using System;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace PreJobListingApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NestedScrollView.IOnScrollChangeListener
    {
        class OnScrollListener : RecyclerView.OnScrollListener
        {
            public OnScrollListener(MainActivity activity)
            {
                Activity = activity ?? throw new ArgumentNullException(nameof(activity));
            }

            MainActivity Activity { get; }
            View CardHeaderShadow => Activity.CardHeaderShadow;
            LinearLayoutManager lm => Activity.lm;
            bool IsShowingCardHeaderShadow
            {
                get
                {
                    return Activity.mIsShowingCardHeaderShadow;
                }
                set
                {
                    Activity.mIsShowingCardHeaderShadow = value;
                }
            }

            public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
            {
                // Animate the shadow view in/out as the user scrolls so that it
                // looks like the RecyclerView is scrolling beneath the card header.
                bool isRecyclerViewScrolledToTop = lm.FindFirstVisibleItemPosition() == 0
                        && lm.FindViewByPosition(0).Top == 0;

                if (!isRecyclerViewScrolledToTop && !IsShowingCardHeaderShadow)
                {
                    IsShowingCardHeaderShadow = true;
                    ShowOrHideView(CardHeaderShadow, true);
                }
                else if (isRecyclerViewScrolledToTop && IsShowingCardHeaderShadow)
                {
                    IsShowingCardHeaderShadow = false;
                    ShowOrHideView(CardHeaderShadow, false);
                }
            }
        }

        static readonly string TAG = nameof(MainActivity);
        public static MainActivity Instance { get; private set; }

        // True iff the shadow view between the card header and the RecyclerView
        // is currently showing.
        private bool mIsShowingCardHeaderShadow;
        LinearLayoutManager lm;

        public ImageViewAsync BackgroundImageView { get; private set; }        
        public RecyclerView RV { get; private set; }
        public View CardHeaderShadow { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            Instance = this;          
            
            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            BackgroundImageView = FindViewById<ImageViewAsync>(Resource.Id.background_image);

            ImageService.Instance.LoadFile(null).Into(BackgroundImageView);

            ImageService.Instance.LoadUrl("http://i.imgur.com/zKYUpWa.jpg")
                .WithPriority(LoadingPriority.High)
                .Retry(3, 200)
                .WithCache(FFImageLoading.Cache.CacheType.All)
                .Transform(new CropTransformation(1, 0, 0))
                .FadeAnimation(true)
                .Into(BackgroundImageView);


            RV = FindViewById<RecyclerView>(Resource.Id.card_recyclerview);
            lm = new LinearLayoutManager(this);
            RV.SetLayoutManager(lm);
            RV.SetAdapter(new LoremIpsumAdapter(this));
            RV.AddItemDecoration(new DividerItemDecoration(this, lm.Orientation));

            CardHeaderShadow = FindViewById(Resource.Id.card_header_shadow);

            RV.AddOnScrollListener(new OnScrollListener(this));

            NestedScrollView nsv = FindViewById<NestedScrollView>(Resource.Id.nestedscrollview);
            nsv.OverScrollMode = OverScrollMode.Never;
            nsv.SetOnScrollChangeListener(this);
        }

        public void OnScrollChange(NestedScrollView v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
            if (scrollY == 0 && oldScrollY > 0)
            {
                // Reset the RecyclerView's scroll position each time the card
                // returns to its starting position.
                RV.ScrollToPosition(0);
                CardHeaderShadow.Alpha = 0f;
                mIsShowingCardHeaderShadow = false;
            }
        }

        private static void ShowOrHideView(View view, bool shouldShow)
        {
            view.Animate().Alpha(shouldShow ? 1f : 0f)
                .SetDuration(100)
                .SetInterpolator(new Android.Views.Animations.DecelerateInterpolator());
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}