using NUnit.Framework;
using ModuleHelper.ViewModels;
using ModuleHelper.Utility;

namespace ModuleHelper.Tests
{
    [TestFixture]
    public class MainWindowViewModelTests
    {
        private IDialogService _dialogService;
        private MainWindowViewModel _mainWindowViewModel;

        [SetUp]
        public void SetUp()
        {
            _dialogService = new UnitTestDialogService();
            _mainWindowViewModel = new MainWindowViewModel(_dialogService);
        }

        [TestCase]
        public void Modulo_NegativeNumberAs1stParam_PositiveResult()
        {
            Assert.AreEqual(MainWindowViewModel.Modulo(-3, 3), 0);
        }
    }
}