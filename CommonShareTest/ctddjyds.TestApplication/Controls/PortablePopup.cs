using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

#if NETFX_CORE

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;

#else

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

#endif

namespace ctddjyds.TestApplication
{
    /// <summary>
    /// Layout aware page which can be used in SL, WinRT and WPF
    /// </summary>
    public class PortablePopup : ContentControl
    {
        private double CONSTPOPUPWIDTH = 400;

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
          DependencyProperty.Register("IsOpen",
          typeof(bool), typeof(PortablePopup), new PropertyMetadata(false, OnIsOpenChanged));

        public object HideOnPropertyChange
        {
            get { return (object)GetValue(HideOnPropertyChangeProperty); }
            set { SetValue(HideOnPropertyChangeProperty, value); }
        }

        public static readonly DependencyProperty HideOnPropertyChangeProperty =
          DependencyProperty.Register("HideOnPropertyChange",
          typeof(object), typeof(PortablePopup), new PropertyMetadata(false, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PortablePopup).HidePopup();
        }

        private void HidePopup()
        {
            if (popup != null)
            {
                ClosePopup();
            }
        }

        private void ClosePopup()
        {
            this.SetValue(IsOpenProperty, false);
        }

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((e.NewValue as bool?) == true)
            {
                (d as PortablePopup).OnPopupOpened();
            }
            else
            {
                (d as PortablePopup).OnPopupClosed();
            }
        }

        private void OnPopupClosed()
        {
        }

        private void OnPopupOpened()
        {
        }


        public PortablePopup()
        {

#if NETFX_CORE
            this.DefaultStyleKey = typeof(PortablePopup);
#else
            
#endif
        }

        static PortablePopup()
        {
#if NETFX_CORE

#else
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PortablePopup), new FrameworkPropertyMetadata(typeof(PortablePopup)));
#endif
        }

#if NETFX_CORE
        protected override void OnApplyTemplate()
        {
            InternalOnApplyTemplate();
        }
#else
        public override void OnApplyTemplate()
        {
            InternalOnApplyTemplate();
        }
#endif
        private ContentControl contentControl = null;
        private Popup popup = null;
        private Button closeButton = null;
        public void InternalOnApplyTemplate()
        {
            if (this == null)
                return;
            DependencyObject dObject = this.GetTemplateChild("PART_Popup");
            if(dObject!=null)
                popup = dObject as Popup;
            dObject = this.GetTemplateChild("PART_Content");
            if (dObject!=null)
                contentControl = dObject as ContentControl;
            dObject = this.GetTemplateChild("PART_CloseButton");
            if (dObject != null)
                closeButton = dObject as Button;

#if NETFX_CORE
            popup.PointerPressed += popup_PointerPressed;
            popup.Closed += popup_Closed;
            popup.Width = CONSTPOPUPWIDTH;
            popup.HorizontalOffset = -75;
            contentControl.Width = CONSTPOPUPWIDTH;
            contentControl.MaxHeight = Window.Current.Bounds.Height - 150;
#else
            if(popup!=null)
            {
                popup.Width = CONSTPOPUPWIDTH;
                popup.AllowsTransparency = true;
                popup.HorizontalOffset = -75;
                popup.Height = Application.Current.MainWindow.Height - 150;
            }
#endif
            if(closeButton!=null)
                closeButton.Click += CloseButton_Click;
            base.OnApplyTemplate();
        }

        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsOpen = false;
        }
        
#if NETFX_CORE
        private void popup_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.SetValue(IsOpenProperty, false);
        }
        void popup_Closed(object sender, object e)
        {
            this.SetValue(IsOpenProperty, false);
        }
#endif
    }
}
