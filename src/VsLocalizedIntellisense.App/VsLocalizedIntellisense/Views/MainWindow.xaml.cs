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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VsLocalizedIntellisense.Models.Mvvm;
using VsLocalizedIntellisense.Models.Mvvm.Message;
using VsLocalizedIntellisense.ViewModels.Message;

namespace VsLocalizedIntellisense.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Messenger = new ViewMessenger<Window>(this, m =>
            {
                m.Register<OpenFileDialogMessage>(a =>
                {
                    var dir = a.OpenDirectory(this);
                    a.ResultDirectory = dir;
                });
            });
        }

        #region property

        private ViewMessenger<Window> Messenger { get; set; }

        #endregion
    }
}
