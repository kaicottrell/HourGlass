using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;

using ApplicationCore.Models;
using ApplicationCore.Interfaces;

namespace HourglassApp.Pages
{
    public partial class Stats
    {
        [Inject]
        private IUnitofWork? _unitofWork { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; } // Inject NavigationManager
        private List<ExtendedTemplateGroup> ExtTGList { get; set; } = new List<ExtendedTemplateGroup>();

        protected override void OnInitialized()
        {
            if (_unitofWork != null)
            {
                //Copy over the values from the template groups and convert them to extend template groups
                var templateGroupList = _unitofWork.TemplateGroup.List();
                foreach (var TG in templateGroupList)
                {
                    var ETG = new ExtendedTemplateGroup
                    {
                        Id = TG.Id,
                        Name = TG.Name,
                        ShowTemplates = false
                    };
                    ExtTGList.Add(ETG);
                }


            }
        }
        public class ExtendedTemplateGroup : TemplateGroup
        {
            public bool ShowTemplates { get; set; }
        }
        public string GetHours(ExtendedTemplateGroup ETG)
        {
            TimeSpan total = TimeSpan.Zero; // Initialize total duration to zero
            var sessions = _unitofWork.Session.List(s => s.Template.TemplateGroupId == ETG.Id, null, "Template");

            foreach (var session in sessions)
            {
                total += session.Duration;
            }

            // Extract hours and minutes from the total duration
            int hours = (int)total.TotalHours;
            int minutes = total.Minutes;

            // Construct the string representation
            string durationString = $"{hours} {(hours == 1 ? "hour" : "hours")} and {minutes} {(minutes == 1 ? "minute" : "minutes")}";

            return durationString;
        }
        public string GetHours(Template template)
        {
            TimeSpan total = TimeSpan.Zero; // Initialize total duration to zero
            var sessions = _unitofWork.Session.List(s => s.Template.Id == template.Id, null, "Template");

            foreach (var session in sessions)
            {
                total += session.Duration;
            }

            // Extract hours and minutes from the total duration
            int hours = (int)total.TotalHours;
            int minutes = total.Minutes;

            // Construct the string representation
            string durationString = $"{hours} {(hours == 1 ? "hour" : "hours")} and {minutes} {(minutes == 1 ? "minute" : "minutes")}";

            return durationString;
        }

        public IEnumerable<Template> GetAssociatedTemplates(ExtendedTemplateGroup ETG)
        {
            if (_unitofWork != null)
            {
                var templateList = _unitofWork.Template.List(temp => temp.TemplateGroupId == ETG.Id, null, "TemplateGroup");
                return templateList;
            }

            return new List<Template>();


        }
        void ToggleDropdown(ExtendedTemplateGroup group)
        {
            group.ShowTemplates = !group.ShowTemplates;
        }

        //Gradients

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