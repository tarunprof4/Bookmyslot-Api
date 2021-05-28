using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors
{
    public class CancelledSlotResponseAdaptor : ICancelledSlotResponseAdaptor
    {
        private readonly ICustomerResponseAdaptor customerResponseAdaptor;

        public CancelledSlotResponseAdaptor(ICustomerResponseAdaptor customerResponseAdaptor)
        {
            this.customerResponseAdaptor = customerResponseAdaptor;
        }



        public CancelledSlotViewModel CreateCancelledSlotViewModel(CancelledSlotModel cancelledSlotModel)
        {
            var cancelledSlotViewModel = new CancelledSlotViewModel
            {
                Title = cancelledSlotModel.Title,
                Country = cancelledSlotModel.Country,
                SlotStartZonedDateTime = cancelledSlotModel.SlotStartZonedDateTime,
                SlotStartTime = cancelledSlotModel.SlotStartTime,
                SlotEndTime = cancelledSlotModel.SlotEndTime,
            };

            return cancelledSlotViewModel;
        }

        public IEnumerable<CancelledSlotViewModel> CreateCancelledSlotViewModels(IEnumerable<CancelledSlotModel> cancelledSlotModels)
        {
            var cancelledSlotViewModels = new List<CancelledSlotViewModel>();

            foreach (var cancelledSlotModel in cancelledSlotModels)
            {
                cancelledSlotViewModels.Add(CreateCancelledSlotViewModel(cancelledSlotModel));
            }

            return cancelledSlotViewModels;
        }

        public IEnumerable<CancelledSlotInformationViewModel> CreateCancelledSlotInformationViewModel(IEnumerable<CancelledSlotInformationModel> cancelledSlotInformationModels)
        {
            List<CancelledSlotInformationViewModel> cancelledSlotInformationViewModels = new List<CancelledSlotInformationViewModel>();
            foreach (var cancelledSlotInformationModel in cancelledSlotInformationModels)
            {
                var cancelledSlotInformationViewModel = new CancelledSlotInformationViewModel
                {
                    CancelledSlotViewModel = this.CreateCancelledSlotViewModel(cancelledSlotInformationModel.CancelledSlotModel),
                    CancelledByCustomerViewModel = this.customerResponseAdaptor.CreateCustomerViewModel(cancelledSlotInformationModel.CancelledByCustomerModel)
                };
                cancelledSlotInformationViewModels.Add(cancelledSlotInformationViewModel);
            }


            return cancelledSlotInformationViewModels;
        }
    }
}
