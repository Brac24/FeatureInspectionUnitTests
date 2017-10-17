using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Feature_Inspection.UnitTests
{
    [TestFixture]
    public class InspectionEntryPresenterTests
    {
        [Test]
        public void InspectionCtor_PassingInModelAndView_ObjectsNotNull()
        {
            //Arrange
            var view = new Feature_Inspection();
            var model = new FeatureCreationModelMock();

            //Act
            var presenter = new FeatureCreationPresenter(view, model);   

            //Assert
            Assert.That(presenter, Is.Not.Null);

        }
    }
}
