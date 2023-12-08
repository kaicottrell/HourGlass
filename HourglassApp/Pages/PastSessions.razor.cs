using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::Microsoft.AspNetCore.Components;
using System.Net.Http;
using HourglassApp.Shared;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;

namespace HourglassApp.Pages
{
    public partial class PastSessions
    {
        
        [Inject]
        private IUnitofWork? _unitofWork { get; set; }
        
        [Inject]
        private NavigationManager Navigation { get; set; } // Inject NavigationManager
        private IEnumerable<Session>? SessionList { get; set; }


        protected override void OnInitialized()
        {
            // Gather all of the templates associated with the user
            // Can utilize a predicate function to gather the templates you want
            if(_unitofWork != null)
            {
                // Get the current date and time
                DateTime currentDate = DateTime.UtcNow;

                // Calculate the date one week ago from the current date
                DateTime oneWeekAgo = currentDate.AddDays(-7);

                SessionList = _unitofWork.Session.List(s => s.SessionStart >= oneWeekAgo && s.SessionStart <= currentDate, null,"Template");
            }
            
        }
        protected void UpdateSession(int templateId, int sessionId)
        {
            Navigation.NavigateTo($"/TemplateSession/{templateId}/{sessionId}");
        }
        private void HandleDeleteRequest(int SessionID)
        {
            var sessionToDelete = _unitofWork.Session.GetById(SessionID);
            _unitofWork.Session.Delete(sessionToDelete);
            _unitofWork.Commit();
            // Navigate to the current page to refresh the content
            Navigation.NavigateTo(Navigation.Uri, forceLoad: true);

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

        static int Clamp(int value)
        {
            return Math.Clamp(value, 0, 255); // Clamps the value between 0 and 255
        }
        /*
         * Mobile screens typically have a short width and longer length
         * Desktop screens have a long width and shorter height.
         * if width / height > 1, then screen is likely desktop.
         * if width / height < .8, then screen is likely mobile.
         *
         */
        public double mobilesize()
        {
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            return displayInfo.Width / displayInfo.Height;
        }
    }
}