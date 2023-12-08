using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Hourglass;
using Hourglass.Shared;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;

namespace Hourglass.Pages
{
    public partial class UpsertTemplate
    {
        [Parameter]
        public int? TemplateId { get; set; }
        [Inject]
        private IUnitofWork? _unitofWork { get; set; }
        protected string TemplateName { get; set; } = string.Empty;
        protected string SelectedIcon { get; set; } = string.Empty;
        protected string SelectedColor { get; set; } = string.Empty;
        protected string SelectedGroup { get; set; } = string.Empty;
        private bool isDeleteConfirmationVisible = false;
        [Inject]
        private NavigationManager Navigation { get; set; } // Inject NavigationManager
                                                           // List of available icons and groups
        private List<string> AvailableIcons { get; set; } = new List<string>
        {
            "fa-globe", // Global
            "fa-cog", // Gear
            "fa-chart-line", // Chart Line
            "fa-utensils", // Utensils
            "fa-glass-martini", // Martini Glass
            "fa-dumbbell", // Dumbbell
            "fa-heartbeat", // Heartbeat
            "fa-book", // Book
            "fa-briefcase", // Briefcase
            "fa-graduation-cap", // Graduation Cap
            "fa-gavel", // Gavel
            "fa-laptop", // Laptop
            "fa-bicycle", // Bicycle
            "fa-balance-scale", // Balance Scale
            "fa-shopping-cart", // Shopping Cart
            "fa-plane", // Plane
            "fa-bed", // Bed
            "fa-running", // Running
            "fa-music", // Music
            "fa-paint-brush", // Paint Brush
            "fa-camera", // Camera
            "fa-film", // Film
            "fa-lightbulb", // Lightbulb
            "fa-train", // Train
            "fa-bus" // Bus
        };


        private List<string> AvailableGroups { get; set; } = new List<string>();
        protected override void OnInitialized()
        {
            //Populate available groups and icons
            AvailableGroups = _unitofWork.TemplateGroup
                .List()
                .Select(group => group.Name)
                .ToList();

            if (TemplateId != null)
            {
                // Handle the case when a template ID is present in the URL
                // Perform actions based on the provided templateId

                //Gather the associated template
                var templateIdValue = TemplateId.Value;
                var chosenTemplate = _unitofWork.Template.Get(ct => ct.Id == templateIdValue, false, "TemplateGroup");

                //Gather the associated TemplateGroup

                var associatedGroup = _unitofWork.TemplateGroup.GetById(chosenTemplate.TemplateGroupId);

                //Fill predefined values
                TemplateName  = chosenTemplate.Name;
                SelectedIcon  = chosenTemplate.TemplateImage ?? "";
                SelectedColor = chosenTemplate.TemplateColor;
                SelectedGroup = associatedGroup.Name;
            }

        }
        private void HandleSubmit()
        {

            if (_unitofWork != null)
            {
                if (TemplateId != null)
                {
                    //Handle case when editing the template
                    var templateIdValue = TemplateId.Value;

                    var SelectedTemplateGroup = _unitofWork.TemplateGroup.Get(g => g.Name == SelectedGroup);
                    var chosenTemplate = _unitofWork.Template.Get(ct => ct.Id == templateIdValue);

                    chosenTemplate.Name = TemplateName;
                    chosenTemplate.TemplateImage = SelectedIcon;
                    chosenTemplate.TemplateColor = SelectedColor;
                    if (string.IsNullOrEmpty(SelectedColor))
                    {
                        SelectedColor = "#000000"; // Set a default black color when the value is empty or null
                    }
                    chosenTemplate.TemplateGroup = SelectedTemplateGroup;

                    _unitofWork.Template.Update(chosenTemplate);

                }
                else
                {
                    // Handle case when creating a template
                    var SelectedTemplateGroup = _unitofWork.TemplateGroup.Get(g => g.Name == SelectedGroup);
                    if (string.IsNullOrEmpty(SelectedColor))
                    {
                        SelectedColor = "#000000"; // Set a default black color when the value is empty or null
                    }
                    Template UserTemplate = new()
                    {
                        Name = TemplateName,
                        TemplateImage = SelectedIcon,
                        TemplateColor = SelectedColor,
                        TemplateGroup = SelectedTemplateGroup
                    };
                    _unitofWork.Template.Add(UserTemplate);
                    
                }

                _unitofWork.Commit();

                Navigation.NavigateTo("");


            }


        }
        private void SelectIcon(string icon)
        {
            SelectedIcon = icon; // Update the SelectedIcon property on selection
        }
        private void HandleBackRequest()
        {
            //Go back the previous page
            Navigation.NavigateTo("");
        }
        private void HandleDeleteRequest()
        {
            isDeleteConfirmationVisible = true;
        }
        private void HandleCancelDelete()
        {
            // Hide the confirmation dialog
            isDeleteConfirmationVisible = false;
        }
        protected void HandleDeleteConfirmed()
        {
            
            if (_unitofWork != null && TemplateId.HasValue)
            {
                var templateIdValue = TemplateId.Value;
                var chosenTemplate = _unitofWork.Template.Get(ct => ct.Id == templateIdValue);
                _unitofWork.Template.Delete(chosenTemplate);
                _unitofWork.Commit();
                isDeleteConfirmationVisible = false;
                Navigation.NavigateTo("");
            }
                
        }
    }
}