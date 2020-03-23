using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CovidTracker.Localization
{
    [ContentProperty("Text")]
    public class LocalizeExtension : IMarkupExtension
    {
        const string ResourceId = "CovidTracker.Localization.LocResources";
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null) {
                return null;
            }
            ResourceManager resourceManager = new ResourceManager(ResourceId, typeof(LocalizeExtension).GetTypeInfo().Assembly);
            return resourceManager.GetString(Text, CultureInfo.CurrentCulture);
        }
    }
}