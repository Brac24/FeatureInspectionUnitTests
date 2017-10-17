using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Windows.Forms;
using Moq;


namespace Feature_Inspection.UnitTests
{
    [TestFixture]
    class FeatureCreationViewTests
    {
        /*
        [Test]
        public void Event_TestingEditClicked_EventFired()
        {
            //Arrange
            var view = new FeatureCreationTableMock();

            var wasFired = false;

            view.EditClicked += (o, e) => wasFired = true;

            
            Assert.That(wasFired, Is.True);


        }
        */
    }


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
        public void Ctor_PassingInModelAndView_ObjectsNotNull()
        {
                    
            //Assert
            Assert.That(sut, Is.Not.Null);
                       
        }

        [Test]
        public void Ctor_PassingInModelAndView_ModelAndViewInstantiated()
        {
            mockView.VerifyAll();
            mockModel.VerifyAll();        
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

            sut.CheckPartNumberExists(partNumber);

           
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

        [Test]
        public void SuppressKeyIfSpace_TextInTextBox_ReturnTrue([Values]Keys a)
        {
                      
            KeyEventArgs e = new KeyEventArgs(a);
            sut.SuppressKeyIfWhiteSpaceChar(e);

            if (char.IsWhiteSpace((char)e.KeyCode))
            {
                Assert.That(e.SuppressKeyPress, Is.EqualTo(true));
            }
            //else
              //  Assert.That(e.SuppressKeyPress, Is.EqualTo(false));
            
        }



       


        

       

    }
}
