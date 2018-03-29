using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Windows.Forms;
using Moq;
using FeatureInspection;

namespace Feature_Inspection.UnitTests
{
   
    [TestFixture]
    class FeatureCreationPresenterTests
    {
        private Mock<IFeatureCreationView> mockView;
        private Mock<IFeaturesDataSource> mockModel;
        private FeatureCreationPresenter sut;
        
        [SetUp]
        public void SetUp()
        {
            mockView = new Mock<IFeatureCreationView>(MockBehavior.Strict);
            mockModel = new Mock<IFeaturesDataSource>(MockBehavior.Strict);

            sut = new FeatureCreationPresenter(mockView.Object, mockModel.Object);
        }

        [Test]
        public void OnEnterKeyPartNumber_ValidPartNumber_ReturnsTrue()
        {
            string partNumber;
            bool isValidPartNumber;
            mockView.Setup(f => f.PartNumber).Returns("386022");
            partNumber = mockView.Object.PartNumber;
            mockView.Setup(f => f.ClearForNewPartOpSearch()).Verifiable();
            mockView.Setup(f => f.SelectOpTextBox()).Verifiable();
            mockModel.Setup(f => f.PartNumberExists(partNumber)).Returns(true);

            isValidPartNumber = sut.OnEnterKeyPartNumber(partNumber);

            Assert.IsTrue(isValidPartNumber);
        }

        [Test]
        public void SetUpViewAfterPartEntry_EnterValidPartNumber_ReturnsTrue()
        {
            
            //Arrange
            bool validPartNumber;

            //Property set up: mock.Setup(foo => foo.Name).Returns("bar");
            mockView.Setup(f => f.PartNumber).Returns("386022");
            string partNumber = mockView.Object.PartNumber;
            mockView.Setup(g => g.SelectOpTextBox()).Verifiable(); //Verify view method gets called
            mockModel.Setup(f => f.PartNumberExists(partNumber)).Returns(true); //Configure model method to return true

            //Act
            validPartNumber = sut.SetUpViewAfterPartEntry(partNumber); //Method under test


            //Assert
            Assert.IsTrue(validPartNumber);
        }

        [Test]
        public void SetUpViewAfterPartEntry_EnterInvalidPartNumber_ReturnFalse()
        {
            //Arrange
            bool validPartNumber;

            //Property set up: mock.Setup(foo => foo.Name).Returns("bar");
            mockView.Setup(f => f.PartNumber).Returns("----");
            string partNumber = mockView.Object.PartNumber;
            mockView.Setup(g => g.ClearAfterPartNumberEntry("Part Number does not exist")).Verifiable(); //Verify view method gets called
            mockModel.Setup(f => f.PartNumberExists(partNumber)).Returns(false); //Configure model method to return true

            //Act
            validPartNumber = sut.SetUpViewAfterPartEntry(partNumber); //Method under test

            Assert.IsFalse(validPartNumber);
        }

        [Test]
        public void SetUpViewAfterPartEntry_EnterEmptyString_ReturnFalse()
        {
            //Arrange
            bool validPartNumber;

            //Property set up: mock.Setup(foo => foo.Name).Returns("bar");
            mockView.Setup(f => f.PartNumber).Returns("");
            string partNumber = mockView.Object.PartNumber;
            mockView.Setup(g => g.ClearAfterPartNumberEntry("Please Enter a Part Number")).Verifiable(); //Verify view method gets called
            mockModel.Setup(f => f.PartNumberExists(partNumber)).Returns(false); //Configure model method to return true

            //Act
            validPartNumber = sut.SetUpViewAfterPartEntry(partNumber); //Method under test

            Assert.IsFalse(validPartNumber);
        }

        //[Test]
        public void Ctor_PassingInModelAndView_ObjectsNotNull()
        {
                    
            //Assert
            Assert.That(sut, Is.Not.Null);
                       
        }

        //[Test]
        public void Ctor_PassingInModelAndView_ModelAndViewInstantiated()
        {
            mockView.VerifyAll();
            mockModel.VerifyAll();        
        }

        //[Test]
        public void CheckEnterKeyPressed_PassWhiteSpace_ShouldSuppressKey()
        {
            bool tabPressed = false;

            
            //KeyEventArgs e = new KeyEventArgs(Keys.Tab);

           //tabPressed = sut.OnEnterKeyInitializeDataGridView();

            Assert.That(tabPressed, Is.True);
        }
        
        //[Test]
        public void checkPartNumberExists_PartExists_FocusOpBox()
        {
            
            string partNumber = "1234";
            var opTextBox = new TextBox();
            opTextBox.Enabled = true;
            opTextBox.Visible = true;

            
            mockView.Setup(p => p.FeatureOpTextBox).Returns(opTextBox);
            mockModel.Setup(x => x.PartNumberExists(partNumber)).Returns(true);

            sut = new FeatureCreationPresenter(mockView.Object, mockModel.Object);

            sut.SetUpViewAfterPartEntry(partNumber);

           
            mockModel.VerifyAll();
            mockView.VerifyAll();

            
           Assert.That(opTextBox.Focused, Is.True);

        }
        

        /*
        [TestCase(Keys.Space, true)]
        [TestCase(Keys.X, false)]
        [TestCase(Keys.Return, true)]
        [TestCase(Keys.Space, true)]
        */

        //[Test]
        public void SuppressKeyIfSpace_TextInTextBox_ReturnTrue([Values]Keys a)
        {
                      
            KeyEventArgs e = new KeyEventArgs(a);
            //sut.SuppressKeyIfWhiteSpaceChar(e);

            if (char.IsWhiteSpace((char)e.KeyCode))
            {
                Assert.That(e.SuppressKeyPress, Is.EqualTo(true));
            }
            //else
              //  Assert.That(e.SuppressKeyPress, Is.EqualTo(false));
            
        }



       


        

       

    }
}
