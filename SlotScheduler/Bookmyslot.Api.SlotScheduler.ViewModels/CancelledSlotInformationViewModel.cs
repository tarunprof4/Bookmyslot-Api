using Bookmyslot.Api.SlotScheduler.Contracts;
using System;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.ViewModels
{
    public class CancelledSlotInformationViewModel
    {
        public CancelledSlotViewModel CancelledSlotViewModel { get; set; }

        public CustomerViewModel CancelledByCustomerViewModel { get; set; }

        public static IEnumerable<CancelledSlotInformationViewModel> CreateCancelledSlotInformationViewModel(IEnumerable<CancelledSlotInformationModel> cancelledSlotInformationModels, Func<string, string> encryptionFunction)
        {
            List<CancelledSlotInformationViewModel> cancelledSlotInformationViewModels = new List<CancelledSlotInformationViewModel>();
            foreach (var cancelledSlotInformationModel in cancelledSlotInformationModels)
            {
                var cancelledSlotInformationViewModel = new CancelledSlotInformationViewModel
                {
                    CancelledSlotViewModel = CancelledSlotViewModel.CreateCancelledSlotViewModel(cancelledSlotInformationModel.CancelledSlotModel),
                    CancelledByCustomerViewModel = CustomerViewModel.CreateCustomerViewModel(cancelledSlotInformationModel.CancelledByCustomerModel, encryptionFunction)
                };
                cancelledSlotInformationViewModels.Add(cancelledSlotInformationViewModel);
            }
           

            return cancelledSlotInformationViewModels;
        }
    }
}
