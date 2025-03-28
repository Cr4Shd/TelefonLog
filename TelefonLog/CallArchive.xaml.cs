using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TelefonLog.Utils;

namespace TelefonLog
{
    /// <summary>
    /// Interaktionslogik für CallArchive.xaml
    /// </summary>
    public partial class CallArchive : Window
    {
        public CallArchive()
        {
            
            InitializeComponent();
            this.CallsArchive_ListView.ItemsSource = DBManager.GetAllCallsFromArchivesDB();
            DBManager.OnDBItemUpdate += PopulateCalls;
        }

        private void Retrieve_From_DB(object sender, RoutedEventArgs e)
        {
            var x = (CallLog)CallsArchive_ListView.SelectedItem;
            DBManager.InsertCallInDB(x);
            DBManager.RemoveCallFromHistoryDB(x.CallID);
            DBManager.OnItemUpdate();
        }
        private void PopulateCalls()
        {
            this.CallsArchive_ListView.ItemsSource = DBManager.GetAllCallsFromArchivesDB();
        }
        private void Move_Call_BackToActive(object sender, MouseButtonEventArgs e)
        {
            var x = (ListView)sender;
            var y = (CallLog)x.SelectedItem;

            if (MessageBox.Show(y.Text, "Soll der Anruf wieder in die aktive Datenbank geschrieben werden?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DBManager.InsertCallInDB(y);
                DBManager.RemoveCallFromHistoryDB(y.CallID);
            }
        }
    }
}
