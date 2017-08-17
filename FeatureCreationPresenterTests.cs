using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;




namespace Feature_Inspection.UnitTests
{
    [TestFixture]
    class FeatureCreationPresenterTests
    {
        [Test]
        public void Ctor_PassingInModelAndView_ObjectsNotNull()
        {
            var view = new FeatureCreationTableMock();
            var model = new FeatureCreationModelMock();

            var presenter = new FeatureCreationPresenter(view,model);

            Assert.That(presenter.ViewExists());
            
        }
        
    }
}
