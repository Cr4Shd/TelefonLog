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
        }
    }
}
