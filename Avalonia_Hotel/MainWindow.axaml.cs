using Avalonia.Controls;
using Calendar = System.Globalization.Calendar;

namespace Avalonia_Hotel;

public partial class MainWindow : Window
{
    
    public MainWindow()
    {
        DataContext = new ViewModel(this);
        InitializeComponent();
    }
    
}