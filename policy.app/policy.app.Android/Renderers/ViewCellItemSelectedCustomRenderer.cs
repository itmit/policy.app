﻿using policy.app.Controls;
using policy.app.Droid.Renderers;
using policy.app.Views.ViewCell;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CategoryViewCell), typeof(ViewCellItemSelectedCustomRenderer))]

namespace policy.app.Droid.Renderers
{
    public class ViewCellItemSelectedCustomRenderer : ViewCellRenderer
    {
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, Android.Views.ViewGroup parent, Android.Content.Context context)
        {
            var cell = base.GetCellCore(item, convertView, parent, context);

            cell.SetBackgroundResource(Resource.Drawable.ViewCellBackground);

            return cell;
        }
    }
}