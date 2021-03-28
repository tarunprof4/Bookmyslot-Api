using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.SlotScheduler.ViewModels
{
    public class CancelledSlotInformationViewModel
    {
        public CancelledSlotViewModel CancelledSlotViewModel { get; set; }

        public CustomerViewModel CancelledByCustomerViewModel { get; set; }

        public static Response<IEnumerable<CancelledSlotInformationViewModel>> CreateCancelledSlotInformationViewModel(IEnumerable<CancelledSlotInformationModel> cancelledSlotInformationModels)
        {
            List<CancelledSlotInformationViewModel> cancelledSlotInformationViewModels = new List<CancelledSlotInformationViewModel>();
            foreach (var cancelledSlotInformationModel in cancelledSlotInformationModels)
            {
                var cancelledSlotInformationViewModel = new CancelledSlotInformationViewModel
                {
                    CancelledSlotViewModel = CancelledSlotViewModel.CreateCancelledSlotViewModel(cancelledSlotInformationModel.CancelledSlotModel),
                    CancelledByCustomerViewModel = new CustomerViewModel(cancelledSlotInformationModel.CancelledByCustomerModel.FirstName, cancelledSlotInformationModel.CancelledByCustomerModel.LastName,
               cancelledSlotInformationModel.CancelledByCustomerModel.BioHeadLine)
                };
                cancelledSlotInformationViewModels.Add(cancelledSlotInformationViewModel);
            }
           

            return new Response<IEnumerable<CancelledSlotInformationViewModel>> { Result = cancelledSlotInformationViewModels };
        }
    }
}
