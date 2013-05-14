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
using System.Net.Sockets;

using Bionte.Entities;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bionte.Entities.EntityPOSHostMessage mess;
                byte[] res = new byte[261];
                int i = 0;

                mess = new EntityPOSHostMessage();

                mess.HeaderVTOLListener.MessageLength = new byte[2] { 0x00, 0xC4 };
                mess.HeaderVTOLListener.VTOLVersion = new byte[2] { 0xF0, 0xF8 };
                mess.HeaderVTOLListener.BussinesFormat = new byte[2] { 0xD3, 0xE5 };
                mess.HeaderVTOLListener.StoreNumber = new byte[5] { 0xF0, 0xF0, 0xF0, 0xF7, 0xF1 };
                mess.HeaderVTOLListener.MessageLength = new byte[2] { 0x00, 0x00 };

                mess.Send();

                i = mess.VTOLSocket.Receive(res);
                i++;
            }
            catch (SocketException ee)
            {
                
                throw new Exception("", ee) ;
            }
        }
    }
}
