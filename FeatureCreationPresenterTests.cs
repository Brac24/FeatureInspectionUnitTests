using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Windows.Forms;

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
        private FeatureCreationTableMock view;
        private FeatureCreationPresenter presenter;
        
        [SetUp]
        public void SetUp()
        {
            view = new FeatureCreationTableMock();


            //Act
            view.FeatureCreationTableMock_Load(new object(), new EventArgs());

            presenter = view.presenter;
        }

        [Test]
        public void Ctor_PassingInModelAndView_ObjectsNotNull()
        {
                    
            //Assert
            Assert.That(presenter, Is.Not.Null);
                       
        }

        

       
    }
}
