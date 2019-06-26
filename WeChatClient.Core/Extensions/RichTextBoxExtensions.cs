using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace WeChatClient.Core.Extensions
{
    public class RichTextBoxExtensions
    {
        public static readonly DependencyProperty DocumentProperty = DependencyProperty.RegisterAttached(
            "Document", typeof(FlowDocument), typeof(RichTextBoxExtensions),
            new PropertyMetadata(default(FlowDocument), OnDocumentChanged));

        private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FlowDocument document = (FlowDocument)e.NewValue;
            document.LineHeight = 1;

            Binding width_bind = new Binding
            {
                RelativeSource = new RelativeSource
                {
                    Mode = RelativeSourceMode.FindAncestor,
                    AncestorType = typeof(RichTextBox)
                },
                Path = new PropertyPath("ActualWidth"),
                Mode = BindingMode.OneWay
            };
            document.SetBinding(FlowDocument.PageWidthProperty, width_bind);

            RichTextBox rtb = (RichTextBox)d;
            rtb.Document = document;
        }

        public static void SetDocument(DependencyObject element, FlowDocument value)
        {
            element.SetValue(DocumentProperty, value);
        }

        public static FlowDocument GetDocument(DependencyObject element)
        {
            return (FlowDocument)element.GetValue(DocumentProperty);
        }
    }
}
