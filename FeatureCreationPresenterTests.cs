using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;




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
        [Test]
        public void Ctor_PassingInModelAndView_ObjectsNotNull()
        {
            //Arrange
            var view = new FeatureCreationTableMock();
            var model = new FeatureCreationModelMock();

            //Act
            var presenter = new FeatureCreationPresenter(view,model);
            

            //Assert
            Assert.That(presenter, Is.Not.Null);
                       
        }

       
        
        
    }
}
