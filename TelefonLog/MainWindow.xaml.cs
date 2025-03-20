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
        DBManager.CreateHistoryDB();
        PopulateCalls();
        DBManager.OnDBItemUpdate += PopulateCalls;
    }
    /// <summary>
    /// Button-Methode zum speichern eines aktiven Vorgangs
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SaveCallInDB(object sender, RoutedEventArgs e)
    {
        CallLog cl = new(Name_txfld.Text, Message_txfld.Text, Time_txfld.Text, TelNum_txfld.Text, DateTime.Now.ToString(), ConvertBoolToInt(), GetBounding());
        DBManager.InsertCallInDB(cl);
        Name_txfld.Text = "";
        Message_txfld.Text = "";
        Time_txfld.Text = "";
        TelNum_txfld.Text = "";
        MedicalChkBx.IsChecked = false;
        OutBoundChkBx.IsChecked = false;
    }
    /// <summary>
    /// Aktualisierungsmethode
    /// </summary>
    private void PopulateCalls()
    {
        RecentCalls_View.ItemsSource = DBManager.GetAllCallsFromDB();
    }
    /// <summary>
    /// Methode, um direkt aus der Liste die Eigenschaften eines Elementes anzuzeigen oder aber dieses Abzulegen (MessageBox)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    
    private void RecentCalls_View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var x = (ListView)sender;
        var y = (CallLog)x.SelectedItem;

        if(MessageBox.Show(y.Text,"Soll der Anruf abgelegt werden?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            // Das entsprechende Item in die HistoryDatenbank speichern
            // 1. Speichere Item in Datenbank 2. Sobald Datensatz abgespeichert ist.. 3. Lösche den Datensatz aus der aktiven DB 4. Aktualisiere die Liste
            
            DBManager.InsertCallInHistoryDB(y);
            DBManager.RemoveCallFromDB(y.CallID);


        }
    }
    /// <summary>
    /// Convertmethode für den boolschen Wert (-> SQL)
    /// </summary>
    /// <returns></returns>
    private int ConvertBoolToInt()
    {
        var ret = MedicalChkBx.IsChecked;
        if ((bool)ret && ret != null)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    /// <summary>
    /// Work in Progress - Anzeige noch nicht finalisiert
    /// </summary>
    /// <returns></returns>
    private string GetBounding()
    {
        var ret = OutBoundChkBx.IsChecked;
        if ((bool)ret && ret != null)
        {
            return "Ausgehend";
        }
        else
        {
            return "Eingehend";
        }
    }
    /// <summary>
    /// Button-Methode zum Öffnen des Archivs
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Open_Archive(object sender, RoutedEventArgs e)
    {
        CallArchive cal = new CallArchive();
        cal.Show();
    }
}