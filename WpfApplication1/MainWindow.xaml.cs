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
        private ObservableCollection<Version> vers = new ObservableCollection<Version>();
        string selected;
        string file_selected;
        Version help;
        Dictionary<string, string> help2;
        public MainWindow()
        {
            InitializeComponent();
            if (GetTools.lastdirect() != "")
            {
                List<string> temp = GetTools.GetDirectories(GetTools.lastdirect());
                selected = GetTools.lastdirect();
                myproj.Clear();
                foreach (string thing in temp)
                {
                    myproj.Add(new User() { Name = thing });
                    Console.WriteLine(thing);
                }
            }
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
            myproj.Clear();
            foreach (string thing in temp)
            {
                myproj.Add(new User() { Name = thing });
                Console.WriteLine(thing);
            }
            GetTools.writeDirec(selected);
            Console.WriteLine(selected);
                            
            
        }
        private void depverBump(object sender, RoutedEventArgs e)
        {
            string test = (Projects.SelectedItem as User).Name + "\\" + (lbUsers.SelectedItem as User).Name;
            help = GetTools.getchildVersion(test);
            help.toArray();
            help.bumpMajor();
            help.toString();
            
            
        }
        private void allkidsBump(object sender, RoutedEventArgs e)
        {
            List<string> test = GetTools.GetDirectories(selected);
            bool find = GetTools.findOtherDep(test, (Projects.SelectedItem as User).Name);
            bool verif;
            verif = GetTools.verify((Projects.SelectedItem as User).Name);



        }
        private void jsonverBump(object sender, RoutedEventArgs e)
        {
            help = GetTools.getjsonVersion((Projects.SelectedItem as User).Name);
            help.toArray();
            help.bumpMajor();
            help.toString();
            GetTools.writejsonVersion(help);


        }
        private void bmptestBump(object sender, RoutedEventArgs e)
        {
            help = GetTools.getjsonVersion((Projects.SelectedItem as User).Name);
            help.toArray();
            help.bumpTrivial();
            help.toString();
            List<string> test = GetTools.GetDirectories(selected);

            help2 = GetTools.bumpChildrenTrivial((Projects.SelectedItem as User).Name);
            GetTools.writejsonVersion(help);
            GetTools.writechildVersion(help2);
            GetTools.findOtherDep(test, (Projects.SelectedItem as User).Name);
            GetTools.verify((Projects.SelectedItem as User).Name);


        }
        private void verdisp(object sender, RoutedEventArgs e)
        {
            User temp = new User();
            // ... Get label.
            try { temp = new User { Name = help.getVersion() }; }
            catch (NullReferenceException l) {
                Console.WriteLine(l);
            }

            var label = sender as System.Windows.Controls.Label;
                // ... Set date in content.
                label.Content = temp.Name;
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
        public partial class EntitiesView : System.Windows.Controls.UserControl, INotifyPropertyChanged
        {
            private string _name2;
            public string Name2
            {
                get { return _name2; }
                set
                {
                    _name2 = value;
                    RaisePropertyChanged("Name2");
                }
            }

            public EntitiesView()
            {
                Name2 = "abcdef";
                DataContext = this;
                }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void RaisePropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}
