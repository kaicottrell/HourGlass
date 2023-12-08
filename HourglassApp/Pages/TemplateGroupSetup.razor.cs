using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;

namespace HourglassApp.Pages
{
    public partial class TemplateGroupSetup
    {
        [Inject]
        private IUnitofWork? _unitofWork { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; } // Inject NavigationManager
        private IEnumerable<TemplateGroup>? TGList { get; set; }
        private bool isDeleteConfirmationVisible { get; set; } = false ;
        private int selectedTGId { get; set; } = -1;
        protected override void OnInitialized()
        {
            
            if (_unitofWork != null)
            {
                TGList = _unitofWork.TemplateGroup.List();
            }

        }
        protected int GetAssociatedTemplateCount(TemplateGroup tg)
        {
            int templateCount = 0;
            if(_unitofWork != null)
            {
                templateCount = _unitofWork.Template.List(template => template.TemplateGroupId == tg.Id).Count();
            }
            
            return templateCount;
        }
        protected void OpenCreateTemplateGroup()
        {
            Navigation.NavigateTo("UpsertTemplateGroup");
        }
        protected void OpenCreateTemplateGroup(int TemplateGroupId)
        {
            Navigation.NavigateTo($"UpsertTemplateGroup/{TemplateGroupId}");
        }
        private void HandleDeleteRequest(int templateGroupId)
        {
            isDeleteConfirmationVisible = true;
            selectedTGId = templateGroupId;
        }
        private void HandleCancelDelete()
        {
            // Hide the confirmation dialog
            isDeleteConfirmationVisible = false;
            selectedTGId = -1;
        }
        protected void HandleDeleteConfirmed()
        {

            if (_unitofWork != null && selectedTGId != -1)
            {
                var templateGroupToDelete = _unitofWork.TemplateGroup.GetById(selectedTGId);
                _unitofWork.TemplateGroup.Delete(templateGroupToDelete);
                _unitofWork.Commit();
                // Navigate to the current page to refresh the content
                Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
            }

        }
    }
}