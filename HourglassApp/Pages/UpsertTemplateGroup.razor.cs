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
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HourglassApp.Pages
{
   
    public partial class UpsertTemplateGroup
    {
        [Inject]
        private IUnitofWork? _unitofWork { get; set; }

        [Parameter]
        public int? TemplateGroupId { get; set; }
        protected string TemplateGroupName { get; set; } = string.Empty;
        [Inject]
        private NavigationManager Navigation { get; set; } // Inject NavigationManager
        protected override void OnInitialized()
        {
            

            if (TemplateGroupId != null && _unitofWork != null)
            {
                // Handle the case when a template group ID is present in the URL
                // Perform actions based on the provided templateId

                //Gather the associated template
                var templateGroupIdValue = TemplateGroupId.Value;
                var chosenTemplateGroup = _unitofWork.TemplateGroup.GetById(templateGroupIdValue);

                //Fill predefined values
                TemplateGroupName = chosenTemplateGroup.Name;
            }

        }
        private void HandleSubmit()
        {

            if (_unitofWork != null)
            {
                if (TemplateGroupId != null)
                {
                    //Handle case when editing the template group
                    var templategroupIdValue = TemplateGroupId.Value;

                    var templateGroup = _unitofWork.TemplateGroup.GetById(templategroupIdValue);

                    templateGroup.Name = TemplateGroupName;

                    _unitofWork.TemplateGroup.Update(templateGroup);

                }
                else
                {
                    // Handle case when creating a template group
                    TemplateGroup TemplateGroup = new()
                    {
                        Name = TemplateGroupName
                    };
                    _unitofWork.TemplateGroup.Add(TemplateGroup);

                }

                _unitofWork.Commit();

                Navigation.NavigateTo("TemplateGroupSetup");


            }


        }
        protected void HandleBackRequest()
        {
            Navigation.NavigateTo("TemplateGroupSetup");
        }
    }
    

}