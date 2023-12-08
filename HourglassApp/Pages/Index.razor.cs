using ApplicationCore.Models;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Components;

namespace HourglassApp.Pages
{
    public partial class Index
    {
        [Inject]
        private IUnitofWork? _unitofWork { get; set; }
        private IEnumerable<Template>? TemplateList { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; } // Inject NavigationManager

        private void OpenTemplateSession(int templateID)
        {
            Navigation.NavigateTo($"/TemplateSession/{templateID}");
        }
        private void OpenCreateTemplate()
        {
            Navigation.NavigateTo("/UpsertTemplate/");
        }
        protected void EditTemplate(int templateId)
        {
            Navigation.NavigateTo($"/UpsertTemplate/{templateId}");
        }
        protected override void OnInitialized()
        {
            // Gather all of the templates associated with the user
            // Can utilize a predicate function to gather the templates you want
            TemplateList = _unitofWork.Template.List();
        }
        bool IsDarkColor(string color)
        {
            if (string.IsNullOrEmpty(color) || color.Length < 6 || !color.StartsWith("#"))
            {
                return false; // Return default value (considering the color invalid)
            }

            var hexColor = color.TrimStart('#');
            if (hexColor.Length != 6)
            {
                return false; // Return default value (considering the color invalid)
            }

            var r = Convert.ToInt32(hexColor.Substring(0, 2), 16);
            var g = Convert.ToInt32(hexColor.Substring(2, 2), 16);
            var b = Convert.ToInt32(hexColor.Substring(4, 2), 16);

            // Calculate the perceived brightness
            var perceivedBrightness = (r * 299 + g * 587 + b * 114) / 1000;

            return perceivedBrightness < 70; // Adjust this threshold for what you consider "dark"
        }

        string GetTransitionColor(string color, bool isDark)
        {
            var originalColor = System.Drawing.ColorTranslator.FromHtml(color);
            int delta = isDark ? 70 : -70; // Adjusted delta value for dark and light transitions
            int r = Clamp(originalColor.R + delta);
            int g = Clamp(originalColor.G + delta);
            int b = Clamp(originalColor.B + delta);
            return $"#{r:X2}{g:X2}{b:X2}";
        }

        int Clamp(int value)
        {
            return Math.Clamp(value, 0, 255); // Clamps the value between 0 and 255
        }





    }
}