using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomRight.ConsoleHelper
{
    public class ListChoice
    {

        public string Choice
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public ListChoice(string choice, string title)
        {
            this.Choice = choice;
            this.Title = title;
        }

    }
}
