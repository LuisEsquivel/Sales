using System;
using System.Collections.Generic;
using System.Text;

namespace Sales.Infrastructure
{
    using ViewModels;


    public class InstanceLocator
    {
        //va a ser el Binding principal de todas las paginas
        public MainViewModel Main { get; set; }


        //constructor
        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
    }
}