using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Ookii.Dialogs.Wpf;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<User> users = new ObservableCollection<User>();
        private ObservableCollection<User> myproj = new ObservableCollection<User>();
        string selected;
        string file_selected;
        public MainWindow()
        {
            InitializeComponent();
            lbUsers.ItemsSource = users;
            Projects.ItemsSource = myproj;
        }
        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            List<string> tester = new List<string>();
            
            {
                try { file_selected =(Projects.SelectedItem as User).Name; }
                catch( NullReferenceException wee)
                {
                    
                    Console.WriteLine(wee);
                }
                
            }
            //file_selected = (Projects.SelectedItem as User).Name;
             tester = GetTools.GetDependencies(file_selected);
            users.Clear();
            foreach (string thing in tester)
            {
                users.Add(new User() { Name = thing });
                Console.WriteLine(thing);
            }

        }
        private void btnTest2_Click(object sender, RoutedEventArgs e)
        {
            List<string> tester = new List<string>();
            try { tester = GetTools.GetChildren((Projects.SelectedItem as User).Name); } catch(NullReferenceException help) { Console.WriteLine(help); }
            users.Clear();
            foreach (string thing in tester)
            {
                users.Add(new User() { Name = thing });
                Console.WriteLine(thing);
            }

        }
        private void btngetDirec_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog();
            
                dialog.ShowDialog();
           
                selected = dialog.SelectedPath;
            
                
            List<string> temp = GetTools.GetDirectories(selected);
            foreach (string thing in temp)
            {
                myproj.Add(new User() { Name = thing });
                Console.WriteLine(thing);
            }
            Console.WriteLine(selected);
                            
            
        }
        public class User : INotifyPropertyChanged
        {
            private string name;
            public string Name
            {
                get { return this.name; }
                set
                {
                    if (this.name != value)
                    {
                        this.name = value;
                        this.NotifyPropertyChanged("Name");
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public void NotifyPropertyChanged(string propName)
            {
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
