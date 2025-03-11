using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TelefonLog.Utils;

namespace TelefonLog;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DBManager.CreateDatabase();
        PopulateCalls();
        DBManager.OnDBItemUpdate += PopulateCalls;
    }

    private void SaveCallInDB(object sender, RoutedEventArgs e)
    {
        CallLog cl = new(Name_txfld.Text, Message_txfld.Text, Time_txfld.Text);
        DBManager.InsertCallInDB(cl);
        Name_txfld.Text = "";
        Message_txfld.Text = "";
        Time_txfld.Text = "";
    }
    private void PopulateCalls()
    {
        RecentCalls_View.ItemsSource = DBManager.GetAllCallsFromDB();
    }

    private void RecentCalls_View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        
    }
}