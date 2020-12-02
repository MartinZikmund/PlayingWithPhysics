using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.UI.Services.Dialogs
{
    public interface IContentDialogHelper
    {
		Task ShowAsync(string title, string message);
	}
}
