using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

using LinqLanguageEditor2022.Extensions;

namespace LinqLanguageEditor2022.Options
{
    /// <summary>
    /// Interaction logic for AdvancedOptions.xaml
    /// </summary>
    public partial class AdvancedOptions : UserControl
    {
        public AdvancedOptions()
        {
            InitializeComponent();
        }
        internal LinqAdvancedOptionPage advancedOptionsPage;

        public void Initialize()
        {
            cmbResultCodeColor.ItemsSource = typeof(Brushes).GetProperties();
            cmbResultColor.ItemsSource = typeof(Brushes).GetProperties();
            cmbRunningQueryMsgColor.ItemsSource = typeof(Brushes).GetProperties();
            cmbExceptionAdditionMsgColor.ItemsSource = typeof(Brushes).GetProperties();
            cmbResultsEqualMsgColor.ItemsSource = typeof(Brushes).GetProperties();
            advanceOptionText.Text = Constants.AdvanceOptionText;
            tbResultCodeColor.Text = Constants.ResultsCodeTextColor;
            tbResultColor.Text = Constants.ResultColor;
            tbResultsEqualMsgColor.Text = Constants.QueryEqualsMsgColor;
            tbRunningQueryMsgColor.Text = Constants.RunningSelectQueryMsgColor;
            tbExceptionAdditionMsgColor.Text = Constants.ExceptionAdditionMsgColor;
            tbExceptionAdditionMsgColor.Text = Constants.ExceptionAdditionMsgColor;
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                //Update Values in the Settings Store.
                LinqAdvancedOptions linqAdvancedOptions = await LinqAdvancedOptions.GetLiveInstanceAsync();
                if (linqAdvancedOptions.LinqResultsColor != null || linqAdvancedOptions.LinqResultsColor != "")
                {
                    //Settings Store Values to load.
                    await linqAdvancedOptions.LoadAsync();
                    cbOpenInVSPreviewTab.IsChecked = linqAdvancedOptions.OpenInVSPreviewTab;
                    cbEnableToolWindowResults.IsChecked = linqAdvancedOptions.EnableToolWindowResults;
                    cmbResultCodeColor.SelectedIndex = LinqEnumExtensions.EnumIndexFromString<ResultsColorOptions>(linqAdvancedOptions.LinqCodeResultsColor);
                    cmbResultColor.SelectedIndex = LinqEnumExtensions.EnumIndexFromString<ResultsColorOptions>(linqAdvancedOptions.LinqResultsColor);
                    cmbResultsEqualMsgColor.SelectedIndex = LinqEnumExtensions.EnumIndexFromString<ResultsColorOptions>(linqAdvancedOptions.LinqResultsEqualMsgColor);
                    cmbRunningQueryMsgColor.SelectedIndex = LinqEnumExtensions.EnumIndexFromString<ResultsColorOptions>(linqAdvancedOptions.LinqRunningSelectQueryMsgColor);
                    cmbExceptionAdditionMsgColor.SelectedIndex = LinqEnumExtensions.EnumIndexFromString<ResultsColorOptions>(linqAdvancedOptions.LinqExceptionAdditionMsgColor);
                    LinqAdvancedOptions.Instance.OpenInVSPreviewTab = linqAdvancedOptions.OpenInVSPreviewTab;
                    LinqAdvancedOptions.Instance.EnableToolWindowResults = linqAdvancedOptions.EnableToolWindowResults;
                    LinqAdvancedOptions.Instance.LinqCodeResultsColor = linqAdvancedOptions.LinqCodeResultsColor;
                    LinqAdvancedOptions.Instance.LinqResultsColor = linqAdvancedOptions.LinqResultsColor;
                    LinqAdvancedOptions.Instance.LinqResultsEqualMsgColor = linqAdvancedOptions.LinqResultsEqualMsgColor;
                    LinqAdvancedOptions.Instance.LinqRunningSelectQueryMsgColor = linqAdvancedOptions.LinqRunningSelectQueryMsgColor;
                    LinqAdvancedOptions.Instance.LinqExceptionAdditionMsgColor = linqAdvancedOptions.LinqExceptionAdditionMsgColor;
                    await LinqAdvancedOptions.Instance.SaveAsync();
                }
                else
                {
                    //Default Values to save to Settings Store.
                    linqAdvancedOptions.EnableToolWindowResults = true;
                    linqAdvancedOptions.OpenInVSPreviewTab = true;
                    linqAdvancedOptions.LinqRunningSelectQueryMsgColor = "LightBlue";
                    linqAdvancedOptions.LinqResultsColor = "Yellow";
                    linqAdvancedOptions.LinqCodeResultsColor = "LightGreen";
                    linqAdvancedOptions.LinqResultsEqualMsgColor = "LightBlue";
                    linqAdvancedOptions.LinqExceptionAdditionMsgColor = "Red";
                    await linqAdvancedOptions.SaveAsync();
                    cbOpenInVSPreviewTab.IsChecked = linqAdvancedOptions.OpenInVSPreviewTab;
                    cbEnableToolWindowResults.IsChecked = linqAdvancedOptions.EnableToolWindowResults;
                    cmbResultCodeColor.SelectedIndex = LinqEnumExtensions.EnumIndexFromString<ResultsColorOptions>(linqAdvancedOptions.LinqCodeResultsColor);
                    cmbResultColor.SelectedIndex = LinqEnumExtensions.EnumIndexFromString<ResultsColorOptions>(linqAdvancedOptions.LinqResultsColor);
                    cmbResultsEqualMsgColor.SelectedIndex = LinqEnumExtensions.EnumIndexFromString<ResultsColorOptions>(linqAdvancedOptions.LinqResultsEqualMsgColor);
                    cmbRunningQueryMsgColor.SelectedIndex = LinqEnumExtensions.EnumIndexFromString<ResultsColorOptions>(linqAdvancedOptions.LinqRunningSelectQueryMsgColor);
                    cmbExceptionAdditionMsgColor.SelectedIndex = LinqEnumExtensions.EnumIndexFromString<ResultsColorOptions>(linqAdvancedOptions.LinqExceptionAdditionMsgColor);
                }
            }).FireAndForget();


        }

        private void cbOpenInVSPreviewTab_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                LinqAdvancedOptions.Instance.OpenInVSPreviewTab = (bool)cbOpenInVSPreviewTab.IsChecked;
                await LinqAdvancedOptions.Instance.SaveAsync();
                //Update Values in the Settings Store.
                LinqAdvancedOptions linqAdvancedOptions = await LinqAdvancedOptions.GetLiveInstanceAsync();
                linqAdvancedOptions.OpenInVSPreviewTab = (bool)cbOpenInVSPreviewTab.IsChecked;
                await linqAdvancedOptions.SaveAsync();
            }).FireAndForget();
        }

        private void cbEnableToolWindowResults_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                LinqAdvancedOptions.Instance.EnableToolWindowResults = (bool)cbEnableToolWindowResults.IsChecked;
                await LinqAdvancedOptions.Instance.SaveAsync();
                //Update Values in the Settings Store.
                LinqAdvancedOptions linqAdvancedOptions = await LinqAdvancedOptions.GetLiveInstanceAsync();
                linqAdvancedOptions.EnableToolWindowResults = (bool)cbEnableToolWindowResults.IsChecked;
                await linqAdvancedOptions.SaveAsync();
            }).FireAndForget();

        }

        private void cbOpenInVSPreviewTab_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                LinqAdvancedOptions.Instance.OpenInVSPreviewTab = (bool)cbOpenInVSPreviewTab.IsChecked;
                await LinqAdvancedOptions.Instance.SaveAsync();
                //Update Values in the Settings Store.
                LinqAdvancedOptions linqAdvancedOptions = await LinqAdvancedOptions.GetLiveInstanceAsync();
                linqAdvancedOptions.OpenInVSPreviewTab = (bool)cbOpenInVSPreviewTab.IsChecked;
                await linqAdvancedOptions.SaveAsync();
            }).FireAndForget();

        }

        private void cbEnableToolWindowResults_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                LinqAdvancedOptions.Instance.EnableToolWindowResults = (bool)cbEnableToolWindowResults.IsChecked;
                await LinqAdvancedOptions.Instance.SaveAsync();
                //Update Values in the Settings Store.
                LinqAdvancedOptions linqAdvancedOptions = await LinqAdvancedOptions.GetLiveInstanceAsync();
                linqAdvancedOptions.EnableToolWindowResults = (bool)cbEnableToolWindowResults.IsChecked;
                await linqAdvancedOptions.SaveAsync();
            }).FireAndForget();
        }

        private void cmbResultCodeColor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                Brush selectedColor = (Brush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
                LinqAdvancedOptions.Instance.LinqCodeResultsColor = LinqEnumExtensions.GetEnumValueFromDescription<ResultsColorOptions>(selectedColor.ToString()).ToString();
                await LinqAdvancedOptions.Instance.SaveAsync();
                //Update Values in the Settings Store.
                LinqAdvancedOptions linqAdvancedOptions = await LinqAdvancedOptions.GetLiveInstanceAsync();
                linqAdvancedOptions.LinqCodeResultsColor = LinqEnumExtensions.GetEnumValueFromDescription<ResultsColorOptions>(selectedColor.ToString()).ToString();
                await linqAdvancedOptions.SaveAsync();
            }).FireAndForget();
        }

        private void cmbResultColor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                Brush selectedColor = (Brush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
                LinqAdvancedOptions.Instance.LinqResultsColor = LinqEnumExtensions.GetEnumValueFromDescription<ResultsColorOptions>(selectedColor.ToString()).ToString();
                await LinqAdvancedOptions.Instance.SaveAsync();
                //Update Values in the Settings Store.
                LinqAdvancedOptions linqAdvancedOptions = await LinqAdvancedOptions.GetLiveInstanceAsync();
                linqAdvancedOptions.LinqResultsColor = LinqEnumExtensions.GetEnumValueFromDescription<ResultsColorOptions>(selectedColor.ToString()).ToString();
                await linqAdvancedOptions.SaveAsync();
            }).FireAndForget();
        }

        private void cmbResultsEqualMsgColor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                Brush selectedColor = (Brush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
                LinqAdvancedOptions.Instance.LinqResultsEqualMsgColor = LinqEnumExtensions.GetEnumValueFromDescription<ResultsColorOptions>(selectedColor.ToString()).ToString();
                await LinqAdvancedOptions.Instance.SaveAsync();
                //Update Values in the Settings Store.
                LinqAdvancedOptions linqAdvancedOptions = await LinqAdvancedOptions.GetLiveInstanceAsync();
                linqAdvancedOptions.LinqResultsEqualMsgColor = LinqEnumExtensions.GetEnumValueFromDescription<ResultsColorOptions>(selectedColor.ToString()).ToString();
                await linqAdvancedOptions.SaveAsync();
            }).FireAndForget();
        }

        private void cmbRunningQueryMsgColor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                Brush selectedColor = (Brush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
                LinqAdvancedOptions.Instance.LinqRunningSelectQueryMsgColor = LinqEnumExtensions.GetEnumValueFromDescription<ResultsColorOptions>(selectedColor.ToString()).ToString();
                await LinqAdvancedOptions.Instance.SaveAsync();
                //Update Values in the Settings Store.
                LinqAdvancedOptions linqAdvancedOptions = await LinqAdvancedOptions.GetLiveInstanceAsync();
                linqAdvancedOptions.LinqRunningSelectQueryMsgColor = LinqEnumExtensions.GetEnumValueFromDescription<ResultsColorOptions>(selectedColor.ToString()).ToString();
                await linqAdvancedOptions.SaveAsync();
            }).FireAndForget();
        }
        private void cmbExceptionAdditionMsgColor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                Brush selectedColor = (Brush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
                LinqAdvancedOptions.Instance.LinqExceptionAdditionMsgColor = LinqEnumExtensions.GetEnumValueFromDescription<ResultsColorOptions>(selectedColor.ToString()).ToString();
                await LinqAdvancedOptions.Instance.SaveAsync();
                //Update Values in the Settings Store.
                LinqAdvancedOptions linqAdvancedOptions = await LinqAdvancedOptions.GetLiveInstanceAsync();
                linqAdvancedOptions.LinqExceptionAdditionMsgColor = LinqEnumExtensions.GetEnumValueFromDescription<ResultsColorOptions>(selectedColor.ToString()).ToString();
                await linqAdvancedOptions.SaveAsync();
            }).FireAndForget();
        }
        public enum ResultsColorOptions
        {
            [Description("#FFF0F8FF")]
            AliceBlue = 0,
            [Description("#FFFAEBD7")]
            AntiqueWhite = 1,
            [Description("#FF00FFFF")]
            Aqua = 2,
            [Description("#FF7FFFD4")]
            Aquamarine = 3,
            [Description("#FFF0FFFF")]
            Azure = 4,
            [Description("#FFF5F5DC")]
            Beige = 5,
            [Description("#FFFFE4C4")]
            Bisque = 6,
            [Description("#FF000000")]
            Black = 7,
            [Description("#FFFFEBCD")]
            BlanchedAlmond = 8,
            [Description("#FF0000FF")]
            Blue = 9,
            [Description("#FF8A2BE2")]
            BlueViolet = 10,
            [Description("#FFA52A2A")]
            Brown = 11,
            [Description("#FFDEB887")]
            BurlyWood = 12,
            [Description("#FF5F9EA0")]
            CadetBlue = 13,
            [Description("#FF7FFF00")]
            Chartreuse = 14,
            [Description("#FFD2691E")]
            Chocolate = 15,
            [Description("#FFFF7F50")]
            Coral = 16,
            [Description("#FF6495ED")]
            CornflowerBlue = 17,
            [Description("#FFFFF8DC")]
            Cornsilk = 18,
            [Description("#FFDC143C")]
            Crimson = 19,
            [Description("#FF00FFFF")]
            Cyan = 20,
            [Description("#FF00008B")]
            DarkBlue = 21,
            [Description("#FF008B8B")]
            DarkCyan = 22,
            [Description("#FFB8860B")]
            DarkGoldenrod = 23,
            [Description("#FFA9A9A9")]
            DarkGray = 24,
            [Description("#FF006400")]
            DarkGreen = 25,
            [Description("#FFBDB76B")]
            DarkKhaki = 26,
            [Description("#FF8B008B")]
            DarkMagenta = 27,
            [Description("#FF556B2F")]
            DarkOliveGreen = 28,
            [Description("#FFFF8C00")]
            DarkOrange = 29,
            [Description("#FF9932CC")]
            DarkOrchid = 30,
            [Description("#FF8B0000")]
            DarkRed = 31,
            [Description("#FFE9967A")]
            DarkSalmon = 32,
            [Description("#FF8FBC8F")]
            DarkSeaGreen = 33,
            [Description("#FF483D8B")]
            DarkSlateBlue = 34,
            [Description("#FF2F4F4F")]
            DarkSlateGray = 35,
            [Description("#FF00CED1")]
            DarkTurquoise = 36,
            [Description("#FF9400D3")]
            DarkViolet = 37,
            [Description("#FFFF1493")]
            DeepPink = 38,
            [Description("#FF00BFFF")]
            DeepSkyBlue = 39,
            [Description("#FF696969")]
            DimGray = 40,
            [Description("#FF1E90FF")]
            DodgerBlue = 41,
            [Description("#FFB22222")]
            Firebrick = 42,
            [Description("#FFFFFAF0")]
            FloralWhite = 43,
            [Description("#FF228B22")]
            ForestGreen = 44,
            [Description("#FFFF00FF")]
            Fuchsia = 45,
            [Description("#FFDCDCDC")]
            Gainsboro = 46,
            [Description("#FFF8F8FF")]
            GhostWhite = 47,
            [Description("#FFFFD700")]
            Gold = 48,
            [Description("#FFDAA520")]
            Goldenrod = 49,
            [Description("#FF808080")]
            Gray = 50,
            [Description("#FF008000")]
            Green = 51,
            [Description("#FFADFF2F")]
            GreenYellow = 52,
            [Description("#FFF0FFF0")]
            Honeydew = 53,
            [Description("#FFFF69B4")]
            HotPink = 54,
            [Description("#FFCD5C5C")]
            IndianRed = 55,
            [Description("#FF4B0082")]
            Indigo = 56,
            [Description("#FFFFFFF0")]
            Ivory = 57,
            [Description("#FFF0E68C")]
            Khaki = 58,
            [Description("#FFE6E6FA")]
            Lavender = 59,
            [Description("#FFFFF0F5")]
            LavenderBlush = 60,
            [Description("#FF7CFC00")]
            LawnGreen = 61,
            [Description("#FFFFFACD")]
            LemonChiffon = 62,
            [Description("#FFADD8E6")]
            LightBlue = 63,
            [Description("#FFF08080")]
            LightCoral = 64,
            [Description("#FFE0FFFF")]
            LightCyan = 65,
            [Description("#FFFAFAD2")]
            LightGoldenrodYellow = 66,
            [Description("#FFD3D3D3")]
            LightGray = 67,
            [Description("#FF90EE90")]
            LightGreen = 68,
            [Description("#FFFFB6C1")]
            LightPink = 69,
            [Description("#FFFFA07A")]
            LightSalmon = 70,
            [Description("#FF20B2AA")]
            LightSeaGreen = 71,
            [Description("#FF87CEFA")]
            LightSkyBlue = 72,
            [Description("#FF778899")]
            LightSlateGray = 73,
            [Description("#FFB0C4DE")]
            LightSteelBlue = 74,
            [Description("#FFFFFFE0")]
            LightYellow = 75,
            [Description("#FF00FF00")]
            Lime = 76,
            [Description("#FF32CD32")]
            LimeGreen = 77,
            [Description("#FFFAF0E6")]
            Linen = 78,
            [Description("#FFFF00FF")]
            Magenta = 79,
            [Description("#FF800000")]
            Maroon = 80,
            [Description("#FF66CDAA")]
            MediumAquamarine = 81,
            [Description("#FF0000CD")]
            MediumBlue = 82,
            [Description("#FFBA55D3")]
            MediumOrchid = 83,
            [Description("#FF9370DB")]
            MediumPurple = 84,
            [Description("#FF3CB371")]
            MediumSeaGreen = 85,
            [Description("#FF7B68EE")]
            MediumSlateBlue = 86,
            [Description("#FF00FA9A")]
            MediumSpringGreen = 87,
            [Description("#FF48D1CC")]
            MediumTurquoise = 88,
            [Description("#FFC71585")]
            MediumVioletRed = 89,
            [Description("#FF191970")]
            MidnightBlue = 90,
            [Description("#FFF5FFFA")]
            MintCream = 91,
            [Description("#FFFFE4E1")]
            MistyRose = 92,
            [Description("#FFFFE4B5")]
            Moccasin = 93,
            [Description("#FFFFDEAD")]
            NavajoWhite = 94,
            [Description("#FF000080")]
            Navy = 95,
            [Description("#FFFDF5E6")]
            OldLace = 96,
            [Description("#FF808000")]
            Olive = 97,
            [Description("#FF6B8E23")]
            OliveDrab = 98,
            [Description("#FFFFA500")]
            Orange = 99,
            [Description("#FFFF4500")]
            OrangeRed = 100,
            [Description("#FFDA70D6")]
            Orchid = 101,
            [Description("#FFEEE8AA")]
            PaleGoldenrod = 102,
            [Description("#FF98FB98")]
            PaleGreen = 103,
            [Description("#FFAFEEEE")]
            PaleTurquoise = 104,
            [Description("#FFDB7093")]
            PaleVioletRed = 105,
            [Description("#FFFFEFD5")]
            PapayaWhip = 106,
            [Description("#FFFFDAB9")]
            PeachPuff = 107,
            [Description("#FFCD853F")]
            Peru = 108,
            [Description("#FFFFC0CB")]
            Pink = 109,
            [Description("#FFDDA0DD")]
            Plum = 110,
            [Description("#FFB0E0E6")]
            PowderBlue = 111,
            [Description("#FF800080")]
            Purple = 112,
            [Description("#FFFF0000")]
            Red = 113,
            [Description("#FFBC8F8F")]
            RosyBrown = 114,
            [Description("#FF4169E1")]
            RoyalBlue = 115,
            [Description("#FF8B4513")]
            SaddleBrown = 116,
            [Description("#FFFA8072")]
            Salmon = 117,
            [Description("#FFF4A460")]
            SandyBrown = 118,
            [Description("#FF2E8B57")]
            SeaGreen = 119,
            [Description("#FFFFF5EE")]
            SeaShell = 120,
            [Description("#FFA0522D")]
            Sienna = 121,
            [Description("#FFC0C0C0")]
            Silver = 122,
            [Description("#FF87CEEB")]
            SkyBlue = 123,
            [Description("#FF6A5ACD")]
            SlateBlue = 124,
            [Description("#FF708090")]
            SlateGray = 125,
            [Description("#FFFFFAFA")]
            Snow = 126,
            [Description("#FF00FF7F")]
            SpringGreen = 127,
            [Description("#FF4682B4")]
            SteelBlue = 128,
            [Description("#FFD2B48C")]
            Tan = 129,
            [Description("#FF008080")]
            Teal = 130,
            [Description("#FFD8BFD8")]
            Thistle = 131,
            [Description("#FFFF6347")]
            Tomato = 132,
            [Description("#00FFFFFF")]
            Transparent = 133,
            [Description("#FF40E0D0")]
            Turquoise = 134,
            [Description("#FFEE82EE")]
            Violet = 135,
            [Description("#FFF5DEB3")]
            Wheat = 136,
            [Description("#FFFFFFFF")]
            White = 137,
            [Description("#FFF5F5F5")]
            WhiteSmoke = 138,
            [Description("#FFFFFF00")]
            Yellow = 139,
            [Description("#FF9ACD32")]
            YellowGreen = 140,
        }
    }
}
