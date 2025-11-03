using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CarDealership.util
{
    public static class KeyboardShortcuts
    {
        private static bool _registered;

        public static void Register()
        {
            if (_registered) return;
            _registered = true;
            EventManager.RegisterClassHandler(typeof(Window), Keyboard.PreviewKeyDownEvent,
                new KeyEventHandler(OnPreviewKeyDown), true);
        }

        private static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is not Window win) return;

            // F1 — Help
            if (e.Key == Key.F1 || (e.Key == Key.H && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) || (e.Key == Key.Oem2 && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control))
            {
                e.Handled = true;
                ShowHelp();
                return;
            }

            // Tab / Shift+Tab — let WPF handle focus navigation
            if (e.Key == Key.Tab) return;

            // Enter — Accept / submit
            if (e.Key == Key.Enter)
            {
                // If inside multi-line TextBox, allow newline
                if (Keyboard.FocusedElement is TextBox tb && tb.AcceptsReturn)
                    return;

                if (ClickDefaultButton(win))
                {
                    e.Handled = true;
                    return;
                }
            }

            // Esc — Cancel / Back
            if (e.Key == Key.Escape)
            {
                if (ClickCancelButton(win))
                {
                    e.Handled = true;
                    return;
                }

                var frame = FindChild<Frame>(win);
                if (frame != null && frame.CanGoBack)
                {
                    frame.GoBack();
                    e.Handled = true;
                    return;
                }
            }
            // Other keys — ignored by this handler
        }

        private static void ShowHelp()
        {
            var msg = "Гарячі клавіші:\n" +
                      "F1 — допомога\n" +
                      "Enter — підтвердження (кнопка за замовчуванням)\n" +
                      "Esc — відміна/назад\n" +
                      "Tab — наступне поле, Shift+Tab — попереднє";
            MessageBox.Show(msg, "Довідка", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static bool ClickDefaultButton(Window win)
        {
            // Prefer Button with IsDefault=true
            var btn = FindChild<Button>(win, b => b.IsDefault);
            if (btn == null)
            {
                // Fallback by common content
                btn = FindChild<Button>(win, b => HasCaption(b, "OK", "ОК", "Підтвердити", "Зберегти", "Так", "Продовжити"));
            }
            if (btn != null && btn.IsEnabled)
            {
                btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                return true;
            }
            return false;
        }

        private static bool ClickCancelButton(Window win)
        {
            var btn = FindChild<Button>(win, b => b.IsCancel);
            if (btn == null)
            {
                btn = FindChild<Button>(win, b => HasCaption(b, "Cancel", "Відміна", "Скасувати", "Ні", "Назад", "Закрити"));
            }
            if (btn != null && btn.IsEnabled)
            {
                btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                return true;
            }
            return false;
        }

        private static bool HasCaption(Button b, params string[] texts)
        {
            var s = b.Content?.ToString()?.Trim();
            if (string.IsNullOrEmpty(s)) return false;
            return texts.Any(t => string.Equals(s, t, StringComparison.OrdinalIgnoreCase));
        }

        private static T? FindChild<T>(DependencyObject? parent, Func<T, bool>? predicate = null) where T : DependencyObject
        {
            if (parent == null) return null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T t)
                {
                    if (predicate == null || predicate(t)) return t;
                }
                var sub = FindChild<T>(child, predicate);
                if (sub != null) return sub;
            }
            return null;
        }
    }
}

