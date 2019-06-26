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
        /// <summary>
        /// 给RichTextBox提供依赖属性DocumentProperty，默认Document属性不支持数据绑定
        /// </summary>
        public static readonly DependencyProperty DocumentProperty = DependencyProperty.RegisterAttached(
            "Document", typeof(FlowDocument), typeof(RichTextBoxExtensions),
            new PropertyMetadata(default(FlowDocument), OnDocumentChanged));

        private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FlowDocument document = (FlowDocument)e.NewValue;
            if (document.Parent is RichTextBox parent)
            {
                //这里很重要，WPF的控件元素有且仅有一个父对象（如果不这样设置，会报错：Document已属于另一RichTextBox）
                parent.Document = new FlowDocument();
            }
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
