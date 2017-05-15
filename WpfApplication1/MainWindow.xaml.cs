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
using System.Globalization;

namespace SpeedBump
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
        SomeObjectClass obj = new SomeObjectClass();
        private GetTools getTools = new GetTools();
        private myFile json = new myFile();

        private TestOptions options = null;
        public MainWindow()
        {
            InitializeComponent();
            txtName.DataContext = obj;
            this.options = new TestOptions();
            this.options.BooleanProperty = true;
            this.options.EnumProperty = TestEnum.Option1;
            this.DataContext = this.options;
            if (getTools.lastdirect() != "")
            {
                List<string> temp = getTools.GetDirectories(getTools.lastdirect());
                selected = getTools.lastdirect();
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
                try { file_selected = (Projects.SelectedItem as User).Name; }
                catch (NullReferenceException wee)
                {

                    Console.WriteLine(wee);
                }

            }
            //file_selected = (Projects.SelectedItem as User).Name;
            tester = getTools.GetDependencies(file_selected);
            users.Clear();
            foreach (string thing in tester)
            {
                users.Add(new User() { Name = thing });
                Console.WriteLine(thing);
            }

        }
        private void Projects_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var myitem = (e.OriginalSource as FrameworkElement).DataContext as User;
            try { json = getTools.openJSON(myitem.Name); }
            catch (NullReferenceException) { }
            Version welp = getTools.getjsonVersion(json);
            obj.Name = welp.getVersion();
        }

        private void btnTest2_Click(object sender, RoutedEventArgs e)
        {
            List<string> tester = new List<string>();
            try { tester = getTools.GetChildren((Projects.SelectedItem as User).Name); } catch (NullReferenceException help) { Console.WriteLine(help); }
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


            List<string> temp = getTools.GetDirectories(selected);
            myproj.Clear();
            foreach (string thing in temp)
            {
                myproj.Add(new User() { Name = thing });
                Console.WriteLine(thing);
            }
            getTools.writeDirec(selected);
            Console.WriteLine(selected);


        }
        private void depverBump(object sender, RoutedEventArgs e)
        {

            //Version welp = getTools.getjsonVersion((Projects.SelectedItem as User).Name);
            //obj.Name = welp.getVersion();


        }
        private void allkidsBump(object sender, RoutedEventArgs e)
        {
            json = getTools.openJSON((Projects.SelectedItem as User).Name);
            help = getTools.getjsonVersion(json);
            help.bumpTrivial();

            obj.Name = help.getVersion();


        }
        private void jsonverBump(object sender, RoutedEventArgs e)
        {
            json = getTools.openJSON((Projects.SelectedItem as User).Name);
            help = getTools.getjsonVersion(json);

            bool matches;
            try {
                matches = (getTools.verify(json, selected));
                System.Windows.MessageBox.Show("Everything went okay", "Verify");
            }
            catch (Exception MismatchedVersion) { System.Windows.MessageBox.Show("There was an error", "Error"); }

           
        }
        private void bmptestBump(object sender, RoutedEventArgs e)
        {
            json = getTools.openJSON((Projects.SelectedItem as User).Name);
            help = getTools.getjsonVersion(json);

            int choice = (int)options.EnumProperty;
            if (choice == 0) { System.Windows.MessageBox.Show("Trivial Bump"); }
            else if (choice == 1) { System.Windows.MessageBox.Show("Minor Bump"); }
            else if (choice == 2) { System.Windows.MessageBox.Show("Major Bump"); }
            else if (choice == 3) { System.Windows.MessageBox.Show("Rewrite Bump"); }
            help.toArray();
            help.bumpTrivial();
            help.toString();
                List<string> test = getTools.GetDirectories(selected);

            try { help2 = getTools.bumpChildrenTrivial((Projects.SelectedItem as User).Name); } catch (System.IO.DirectoryNotFoundException) { Console.WriteLine(); }
                getTools.writejsonVersion(help);
                getTools.writechildVersion(help2);
                getTools.writeOtherDep(json, selected);
            //}
           // getTools.verify((Projects.SelectedItem as User).Name);

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
        public enum RadioButtons { RadioButton1, RadioButton2, RadioButton3, RadioButton4, None }
        public RadioButtons SelectedRadioButton { get; set; }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int choice = (int)options.EnumProperty;
            json = getTools.openJSON((Projects.SelectedItem as User).Name);
            help = getTools.getjsonVersion(json);
            List<string> direcs = getTools.GetDirectories(selected);
            StringBuilder message = new StringBuilder(@"Are you sure you would like to bump ");
            message.Append((Projects.SelectedItem as User).Name);
            message.Append(" to version ");


                 if (choice == 0) { help.bumpTrivial(); obj.Name = help.getVersion(); }
                else if (choice == 1) { help.bumpMinor(); obj.Name = help.getVersion(); }
                else if (choice == 2) { help.bumpMajor(); obj.Name = help.getVersion(); }
                else if (choice == 3) { help.bumpRewrite(); obj.Name = help.getVersion(); }

            message.Append(help.getVersion() + "?");
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(message.ToString(), "Bump Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {

            }
            else { help = getTools.getjsonVersion(json); obj.Name = help.getVersion(); }
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            List<string> temp = getTools.GetDirectories(getTools.lastdirect());
            selected = getTools.lastdirect();
            myproj.Clear();
            foreach (string thing in temp)
            {
                myproj.Add(new User() { Name = thing });
                Console.WriteLine(thing);
            }
        }

        private void Projects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
    public enum TestEnum
    {
        Option1,
        Option2,
        Option3,
        Option4
    }

    public class TestOptions
    {
        public TestEnum EnumProperty { get; set; }
        public bool BooleanProperty { get; set; }
    }
    public class RadioButtonCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return value.Equals(true) ? parameter : System.Windows.Data.Binding.DoNothing;
        }
    }
    class SomeObjectClass : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
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


