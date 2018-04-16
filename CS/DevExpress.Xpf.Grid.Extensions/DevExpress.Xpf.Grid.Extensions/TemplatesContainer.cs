using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Core;

namespace BandedViewExtension {
    public class TemplatesContainer : Control {
        public static readonly DependencyProperty GridDataRowTemplateProperty =
            DependencyProperty.Register("GridDataRowTemplate", typeof(DataTemplate), typeof(TemplatesContainer), null);
        public static readonly DependencyProperty GridHeadersTemplateProperty =
            DependencyProperty.Register("GridHeadersTemplate", typeof(DataTemplate), typeof(TemplatesContainer), null);
        public static readonly DependencyProperty ColumnHeaderResourcesProperty =
            DependencyProperty.Register("ColumnHeaderResources", typeof(ResourceDictionary), typeof(TemplatesContainer), null);
        public static readonly DependencyProperty BandColumnHeaderTemplateProperty =
            DependencyProperty.Register("BandColumnHeaderTemplate", typeof(DataTemplate), typeof(TemplatesContainer), null);
        public DataTemplate GridDataRowTemplate {
            get { return (DataTemplate)GetValue(GridDataRowTemplateProperty); }
            set { SetValue(GridDataRowTemplateProperty, value); }
        }
        public DataTemplate GridHeadersTemplate {
            get { return (DataTemplate)GetValue(GridHeadersTemplateProperty); }
            set { SetValue(GridHeadersTemplateProperty, value); }
        }
        public DataTemplate BandColumnHeaderTemplate {
            get { return (DataTemplate)GetValue(BandColumnHeaderTemplateProperty); }
            set { SetValue(BandColumnHeaderTemplateProperty, value); }
        }
        [Browsable(false)]
        public ResourceDictionary ColumnHeaderResources {
            get { return (ResourceDictionary)GetValue(ColumnHeaderResourcesProperty); }
            set { SetValue(ColumnHeaderResourcesProperty, value); }
        }
        public TemplatesContainer() {
            DefaultStyleKey = typeof(TemplatesContainer);
        }
        [Browsable(false)]
        public Thickness GetTopColumnHeaderIndent() {
            return GetColumnHeaderIndentThemedResource("TopRowColumnIndent");
        }
        [Browsable(false)]
        public Thickness GetMiddleColumnHeaderIndent() {
            return GetColumnHeaderIndentThemedResource("MiddleRowColumnIndent");
        }
        [Browsable(false)]
        public Thickness GetBottomColumnHeaderIndent() {
            return GetColumnHeaderIndentThemedResource("BottomRowColumnIndent");
        }
        Thickness GetColumnHeaderIndentThemedResource(string resourceName) {
            double bottom = (double)(GetColumnHeaderThemedResource(resourceName) ?? 0d);
            return new Thickness() { Bottom = bottom };
        }
        object GetColumnHeaderThemedResource(string resourceName) {
            string themeName = ThemeManager.ApplicationThemeName;
#if SILVERLIGHT
            themeName = ThemeManager.GetThemeName(this);
            if(string.IsNullOrEmpty(themeName)) themeName = "LightGray";
#else
            if(ThemeManager.GetTreeWalker(this) != null)
                themeName = ThemeManager.GetTreeWalker(this).ThemeName;
            if(string.IsNullOrEmpty(themeName)) themeName = "DeepBlue";
#endif
            string themedResourceName = resourceName + themeName;
            if(ColumnHeaderResources == null || !ColumnHeaderResources.Contains(themedResourceName))
                return null;
            return ColumnHeaderResources[themedResourceName];
        }
    }
}
