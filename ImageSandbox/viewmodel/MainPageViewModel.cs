using System.ComponentModel;

namespace ImageSandbox.viewmodel
{
    /// <summary>
    ///     Handles communications between the view and model in a testable fashion.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        #region Constructors

        #endregion

        #region Methods

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}