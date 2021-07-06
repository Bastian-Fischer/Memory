using System.Windows.Input;

namespace MemoryUI
{
    static class MenuCommands
    {
        public static readonly RoutedUICommand StartGame = new RoutedUICommand(
            "StartGame Command",
            "StartGame",
            typeof(MenuCommands),
            new InputGestureCollection() { new KeyGesture(Key.S, ModifierKeys.Control) });
        public static readonly RoutedUICommand Information = new RoutedUICommand(
            "Information Command",
            "Information", typeof(MenuCommands),
            new InputGestureCollection() { new KeyGesture(Key.I, ModifierKeys.Control) });
        public static readonly RoutedUICommand Return = new RoutedUICommand(
            "Return Command",
            "Return",
            typeof(MenuCommands),
            new InputGestureCollection() { new KeyGesture(Key.R, ModifierKeys.Control) });
    }
}
