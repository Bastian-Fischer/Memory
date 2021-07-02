using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MemoryUI
{
    static class MenuCommands
    {
        public static readonly RoutedUICommand StartGame = new RoutedUICommand("StartGame Command", "StartGame", typeof(MenuCommands), new InputGestureCollection() { new KeyGesture(Key.S, ModifierKeys.Control) });
        public static readonly RoutedUICommand Information = new RoutedUICommand("StartGame Command", "StartGame", typeof(MenuCommands), new InputGestureCollection() { new KeyGesture(Key.I, ModifierKeys.Control) });
    }
}
