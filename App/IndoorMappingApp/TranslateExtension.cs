using System;
using System.Globalization;
using System.Resources;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace IndoorMappingApp.Helpers
{
    [ContentProperty(nameof(Text))]
    public class TranslateExtension : IMarkupExtension<BindingBase>
    {
        // The key from AppResources.resx
        public string Text { get; set; }

        public BindingBase ProvideValue(IServiceProvider serviceProvider) =>
            new Binding($"[{Text}]",
                        source: LocalizationResourceManager.Instance,
                        mode: BindingMode.OneWay);

        object IMarkupExtension.ProvideValue(IServiceProvider provider) =>
            ProvideValue(provider);
    }
}
