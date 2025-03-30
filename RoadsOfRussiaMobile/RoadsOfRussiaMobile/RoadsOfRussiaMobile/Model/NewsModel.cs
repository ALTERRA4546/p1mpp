using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Xamarin.Forms;

namespace RoadsOfRussiaMobile.Model
{
    public class NewsModel
    {
        public int IDNews { get; set; }
        public ImageSource Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PositiveVote { get; set; }
        public int NegativeVote { get; set; }
        public string Date { get; set; }
    }
}
