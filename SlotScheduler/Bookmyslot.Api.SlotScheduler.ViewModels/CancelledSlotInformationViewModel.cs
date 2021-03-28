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

        public static IEnumerable<CancelledSlotInformationViewModel> CreateCancelledSlotInformationViewModel(IEnumerable<CancelledSlotInformationModel> cancelledSlotInformationModels)
        {
            List<CancelledSlotInformationViewModel> cancelledSlotInformationViewModels = new List<CancelledSlotInformationViewModel>();
            foreach (var cancelledSlotInformationModel in cancelledSlotInformationModels)
            {
                var cancelledSlotInformationViewModel = new CancelledSlotInformationViewModel
                {
                    CancelledSlotViewModel = CancelledSlotViewModel.CreateCancelledSlotViewModel(cancelledSlotInformationModel.CancelledSlotModel),
                    CancelledByCustomerViewModel = CustomerViewModel.CreateCustomerViewModel(cancelledSlotInformationModel.CancelledByCustomerModel)
                };
                cancelledSlotInformationViewModels.Add(cancelledSlotInformationViewModel);
            }
           

            return cancelledSlotInformationViewModels;
        }
    }
}
