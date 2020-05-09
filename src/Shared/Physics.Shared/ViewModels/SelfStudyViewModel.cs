using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Physics.Shared.UI.Models.Navigation;

namespace Physics.Shared.ViewModels
{
    public class SelfStudyViewModel : ViewModelBase<SelfStudyNavigationModel>
    {
        private string _dataSourceFolderPath = null;
        private StorageFolder _sourceFolder;

        public SelfStudyViewModel()
        {

        }

        public override void Prepare(SelfStudyNavigationModel parameter)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            _dataSourceFolderPath = parameter.FolderPath;
        }

        public override Task Initialize()
        {
            IsLoading = true;
            //_sourceFolder = await StorageFolder.GetFolderFromPathAsync(_dataSourceFolderPath);
            IsLoading = false;
            return Task.CompletedTask;
        }

        public bool IsLoading { get; set; }
    }
}
