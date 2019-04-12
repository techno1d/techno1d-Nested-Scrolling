using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewHolder = Android.Support.V7.Widget.RecyclerView.ViewHolder;

namespace PreJobListingApp
{
    public class LoremIpsumViewHolder : ViewHolder
    {
        public LoremIpsumViewHolder(View itemView) : base(itemView)
        {
        }

        public LoremIpsumViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
    }
    public class LoremIpsumAdapter : RecyclerView.Adapter
    {
        private const int ITEM_COUNT = 5;

        private LayoutInflater mInflater;

        public override int ItemCount => ITEM_COUNT;

        public LoremIpsumAdapter(Context context)
        {
            mInflater = LayoutInflater.From(context);
        }

        public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return new LoremIpsumViewHolder(mInflater.Inflate(Resource.Layout.view_holder_item, parent, false));
        }

        public override void OnBindViewHolder(ViewHolder holder, int position)
        {

        }
    }
}