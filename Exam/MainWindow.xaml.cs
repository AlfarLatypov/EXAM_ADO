using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Xml.Linq;
using Exam.Module;

namespace Exam
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WorksContainer db = new WorksContainer();
        public MainWindow()
        {
            InitializeComponent();

            SearchByCat.ItemsSource = db.WorkSet.Select(s => s.Category).ToList();

        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(NameOfCat.Text) && !string.IsNullOrEmpty(LinkToRss.Text))
            {
                Work work = new Work
                {
                    Category = NameOfCat.Text,
                    Rss = LinkToRss.Text
                };
                db.WorkSet.Add(work);
            }
            try
            {
                db.SaveChanges();
                MessageBox.Show("Данные добавлены успешно");
                NameOfCat.Text = "";
                LinkToRss.Text = "";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

        }

        private void Search_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void SearchByCat_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Work w = db.WorkSet.Find(SearchByCat.SelectedIndex+1);

            if (w != null)
            {
                XDocument xd = XDocument.Load(w.Rss);

                var t = xd.Element("rss").Element("channel")
                    .Elements().Where(item=>item.Name=="item").Select(s =>
                        new
                        {
                            title = s.Element("title").Value,
                            link = s.Element("link").Value,
                            description = s.Element("description").Value,
                            pubDate = DateTime.Parse(s.Element("pubDate").Value),
                            author = s.Element("author").Value
                        });
               

                //var t =xd.Element("rss").Element("channel")
                //   .Elements().Select(item => item.Element("item").Elements().Select(s =>
                //        new
                //        {
                //            title = s.Element("title").Value,
                //            link = s.Element("link").Value,
                //            description = s.Element("description").Value,
                //            pubDate = s.Element("pubDate").Value,
                //            author = s.Element("author").Value
                //        }
                //    ));

                listSearch.ItemsSource = t.ToList();


            }
        }
    }
}
