using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CarDealership.behaviors;

public static class SmoothScrollBehavior
{
    public static readonly DependencyProperty EnableSmoothScrollingProperty =
        DependencyProperty.RegisterAttached(
            "EnableSmoothScrolling",
            typeof(bool),
            typeof(SmoothScrollBehavior),
            new PropertyMetadata(false, OnEnableSmoothScrollingChanged));

    public static void SetEnableSmoothScrolling(DependencyObject element, bool value) =>
        element.SetValue(EnableSmoothScrollingProperty, value);

    public static bool GetEnableSmoothScrolling(DependencyObject element) =>
        (bool)element.GetValue(EnableSmoothScrollingProperty);

    private static void OnEnableSmoothScrollingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ScrollViewer viewer) return;

        viewer.PreviewMouseWheel -= OnPreviewMouseWheel;
        viewer.PreviewKeyDown -= OnPreviewKeyDown;

        if ((bool)e.NewValue)
        {
            viewer.PreviewMouseWheel += OnPreviewMouseWheel;
            viewer.PreviewKeyDown += OnPreviewKeyDown;
        }
    }

    private static void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (sender is not ScrollViewer viewer) return;

        e.Handled = true;
        var targetOffset = viewer.VerticalOffset - e.Delta / 3.0;
        viewer.ScrollToVerticalOffset(targetOffset);
    }

    private static void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (sender is not ScrollViewer viewer) return;

        switch (e.Key)
        {
            case Key.Up:
                viewer.LineUp();
                e.Handled = true;
                break;
            case Key.Down:
                viewer.LineDown();
                e.Handled = true;
                break;
            case Key.PageUp:
                viewer.PageUp();
                e.Handled = true;
                break;
            case Key.PageDown:
                viewer.PageDown();
                e.Handled = true;
                break;
            case Key.Home:
                viewer.ScrollToTop();
                e.Handled = true;
                break;
            case Key.End:
                viewer.ScrollToBottom();
                e.Handled = true;
                break;
            default:
                return;
        }
    }
}
