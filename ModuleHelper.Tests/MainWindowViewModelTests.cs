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

        [TestCase(-3,3,0)]
        [TestCase(-10,2,0)]
        [TestCase(-8,3,1)]
        [TestCase(-2,4,2)]
        public void Modulo_NegativeNumberAs1stParam_PositiveResult(int x, int m, int expectedResult)
        {
            Assert.AreEqual(MainWindowViewModel.Modulo(x, m), expectedResult);
        }

        [TestCase(8,4,0)]
        [TestCase(9,4,1)]
        [TestCase(0,3,0)]
        [TestCase(1,7,1)]
        public void Modulo_PositiveNumberAs1stParam_PositiveResult(int x, int m, int expectedResult)
        {
            Assert.AreEqual(MainWindowViewModel.Modulo(x, m), expectedResult);
        }
    }
}