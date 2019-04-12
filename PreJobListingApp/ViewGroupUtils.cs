using Android.Graphics;
using Android.Views;

namespace PreJobListingApp
{
    /**
     * A simple utility class copied from the design support library
     * source code (see j.mp/ViewGroupUtils).
     */
    public class ViewGroupUtils
    {
        private static Matrix Matrix { get; } = new Matrix();
        private static RectF RectF { get; } = new RectF();
        private static Matrix Identity { get; } = new Matrix();
        private static Rect Rect { get; } = new Rect();

        /**
         * Check if a given point in the parent's coordinates are within
         * the view bounds of the given direct child view.
         *
         * @param child child view to test
         * @param x     X coordinate to test, in the parent's coordinate system
         * @param y     Y coordinate to test, in the parent's coordinate system
         * @return true if the point is within the child's bounds, false otherwise
         */
        public static bool IsPointInChildBounds(ViewGroup parent, View child, int x, int y)
        {
            GetDescendantRect(parent, child, Rect);
            return Rect.Contains(x, y);
        }

        /**
         * Retrieve the transformed bounding rect of an arbitrary descendant view.
         * This does not need to be a direct child.
         *
         * @param descendant descendant view to reference
         * @param out        rect to set to the bounds of the descendant view
         */
        private static void GetDescendantRect(ViewGroup parent, View descendant, Rect @out)
        {
            @out.Set(0, 0, descendant.Width, descendant.Height);
            OffsetDescendantRect(parent, descendant, @out);
        }

        /**
         * This is a port of the common
         * {@link ViewGroup#offsetDescendantRectToMyCoords(View, Rect)} from the
         * framework, but adapted to take transformations into account. The result
         * will be the bounding rect of the real transformed rect.
         *
         * @param descendant view defining the original coordinate system of rect
         * @param rect       the rect to offset from descendant to this view's coordinate system
         */
        private static void OffsetDescendantRect(ViewGroup parent, View descendant, Rect rect)
        {
            Matrix.Set(Identity);
            OffsetDescendantMatrix(parent, descendant, Matrix);
            RectF.Set(rect);
            Matrix.MapRect(RectF);
            int left = (int)(RectF.Left + 0.5f);
            int top = (int)(RectF.Top + 0.5f);
            int right = (int)(RectF.Right + 0.5f);
            int bottom = (int)(RectF.Bottom + 0.5f);
            rect.Set(left, top, right, bottom);
        }

        private static void OffsetDescendantMatrix(IViewParent target, View view, Matrix m)
        {
            IViewParent parent = view.Parent;
            if ((parent is View vp) && parent != target)
            {
                OffsetDescendantMatrix(target, vp, m);
                m.PreTranslate(-vp.ScrollX, -vp.ScrollY);
            }
            m.PreTranslate(view.Left, view.Top);
            if (!view.Matrix.IsIdentity)
            {
                m.PreConcat(view.Matrix);
            }
        }
    }
}