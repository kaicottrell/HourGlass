using Microsoft.Maui.Controls;
using ApplicationCore.Models;
using ApplicationCore.Interfaces;
using System;
using Microsoft.AspNetCore.Components;

namespace HourglassApp.Pages
{
    public partial class TemplateSession
    {
        [Parameter]
        public int TemplateID { get; set; }
        [Parameter]
        public int? SessionID { get; set; }
        public Template? SessionTemplate { get; set; }
        [Inject]
        private IUnitofWork? _unitofWork { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; } // Inject NavigationManager
        private DateTime StartTime { get; set; } = DateTime.Now;
        private DateTime EndTime { get; set; } = DateTime.Now;
        private String TextColor { get; set; } = "text-white";
        private String GradientEndColor { get; set; } = "#FFF";
        protected override void OnInitialized()
        {
            if (_unitofWork != null)
            {
                // Get the template
                SessionTemplate = _unitofWork.Template.GetById(TemplateID);

                if (SessionID != null && SessionID >0)
                {
                    var CurrentSession = _unitofWork.Session.GetById(SessionID.Value);
                    StartTime = CurrentSession.SessionStart;
                    EndTime = CurrentSession.SessionEnd;
                }
                else if (SessionID == null)
                {
                    StartTime = GetRoundedToMinute(DateTime.Now);
                    EndTime = StartTime.AddMinutes(15);
                }

            }
            //Template coloring:

            var template = _unitofWork.Template.GetById(TemplateID);
            bool isDark = IsDarkColor(template.TemplateColor);
            TextColor = isDark ? "text-white" : "text-black";

            GradientEndColor = GetTransitionColor(template.TemplateColor, isDark);

        }
        private DateTime GetRoundedToMinute(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
        }
        private bool IsValidTimeRange()
        {
            return StartTime < EndTime && StartTime != EndTime; // Allows submission only if end time is after start time
        }
        private void HandleBackRequest()
        {
            //Go back the previous page
            if (SessionID == null || SessionID <= 0)
            {
                NavigationManager.NavigateTo("");
            }
            else
            {
                NavigationManager.NavigateTo("/PastSessions/");
            }

        }
        private void HandleSubmit()
        {
            if (SessionID != null)
            {
                var CurrentSession = _unitofWork.Session.GetById(SessionID.Value);
                CurrentSession.SessionStart = GetRoundedToMinute(StartTime);
                CurrentSession.SessionEnd = GetRoundedToMinute(EndTime);
                CurrentSession.Duration = EndTime - StartTime;
                _unitofWork.Session.Update(CurrentSession);
                _unitofWork.Commit();
                NavigationManager.NavigateTo("/PastSessions/");
            }
            else if (SessionTemplate != null)
            {

                Session UserSession = new()
                {
                    Template = SessionTemplate,
                    SessionStart = GetRoundedToMinute(StartTime),
                    SessionEnd = GetRoundedToMinute(EndTime).AddMinutes(1),
                    Duration = EndTime - StartTime
                };
                _unitofWork.Session.Add(UserSession);
                _unitofWork.Commit();

                NavigationManager.NavigateTo("");

            }


        }
        //Gradient coloring
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
        private void HandleDeleteRequest(int SessionID)
        {
            var sessionToDelete = _unitofWork.Session.GetById(SessionID);
            _unitofWork.Session.Delete(sessionToDelete);
            _unitofWork.Commit();
            // Navigate to the current page to refresh the content
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);

        }

        int Clamp(int value)
        {
            return Math.Clamp(value, 0, 255); // Clamps the value between 0 and 255
        }

    }
}

