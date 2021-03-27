using System;
using System.Collections.Generic;

namespace Faraboom.Framework.UI.FormBuilder.Components
{
    public class NavbarBrand
    {
        public string Text { get; set; }
        public Uri Uri { get; set; }
        public Uri ImageUri { get; set; }
    }

    public class Navbar
    {
        public Navbar()
        {
            Header = new NavbarHeader()
            {
                Brand = new NavbarBrand()
                {
                    Text = "Set Navbar.Brand.Text",
                    Uri = new Uri("/", UriKind.Relative)
                }
            };
            Components = new List<NavbarComponent>();

        }

        public NavbarHeader Header { get; set; }
        
        public List<NavbarComponent> Components { get; set; }

        public class NavbarHeader
        {
            public NavbarBrand Brand { get; set; }
        }

        public abstract class NavbarComponent
        {
            public virtual NavbarPosition? Position { get; set; }
            
            public class NavbarForm : NavbarComponent
            {
                private FormViewModel formViewModel;

                public NavbarForm()
                {
                    Form = new FormViewModel();
                }

                public FormViewModel Form
                {
                    get { return formViewModel; }
                    set
                    {
                        formViewModel.AdditionalClasses = "navbar-form";
                        formViewModel = value;
                    }
                }
            }

            public class Nav : NavbarComponent
            {
                public IList<NavbarNavItem> Items { get; set; }

                public abstract class NavbarNavItem
                {
                }

                public class Link : NavbarNavItem
                {
                    public string Text { get; set; }
                    public Uri Uri { get; set; }
                    public bool Active { get; set; }
                }

                public class Dropdown : NavbarNavItem
                {
                    public string Text { get; set; }
                    public IList<NavbarNavDropdownItem> Items { get; private set; }

                    public abstract class NavbarNavDropdownItem
                    {
                    }

                    public Dropdown(string text)
                    {
                        Text = text;
                    }

                    public class Link : NavbarNavDropdownItem
                    {
                        public string Text { get; set; }
                        public Uri Uri { get; set; }
                        public bool Active { get; set; }
                    }

                    public class Header : NavbarNavDropdownItem
                    {
                        public string Text { get; set; }
                    }

                    public class Separator : NavbarNavDropdownItem
                    {
                    }
                }
            }
            
            public class Button : NavbarComponent { }
            
            public class Text : NavbarComponent { public string Value { get; set; } }
        }

        public enum NavbarPosition
        {
            Left,
            Right
        }
    }
}