using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NUnit.Framework;
using Moq;
using FeatureInspection;
using System.Data;

namespace Feature_Inspection.UnitTests
{
  

    [TestFixture]
    public class InspectionEntryPresenterTests
    {
        private Mock<IInspectionView> viewMock;
        private Mock<IInspectionDataSource> modelMock;

        private InspectionPresenter systemUnderTest;

        DataTable jobInfo;

        //private ModelMockInspection dataTable;

        [SetUp]
        public void SetUp()
        {

            viewMock = new Mock<IInspectionView>(MockBehavior.Strict);
            modelMock = new Mock<IInspectionDataSource>(MockBehavior.Strict);

            //dataTable = new ModelMockInspection();

            systemUnderTest = new InspectionPresenter(viewMock.Object, modelMock.Object);

            jobInfo = new DataTable();
            jobInfo.Columns.Add("Part_Number", typeof(string));
            jobInfo.Columns.Add("Job_Number", typeof(string));
            jobInfo.Columns.Add("Operation_Number", typeof(string));
            jobInfo.Columns.Add("Act_Run_Qty", typeof(string));
        }

        /*
        [TestCase(0,1,10)]
        [TestCase(1,2,10)]
        [TestCase(5, 6, 10)]
        public void GoToNextPart__ListBoxIndexLessThanListBoxCount_ListBoxIndexPlus1(int beforeClickIndex, int afterClickIndex, int listBoxCount)
        {
            //Arrange

            viewMock.Setup(f => f.ListBoxCount).Returns(listBoxCount);
            viewMock.SetupProperty(g => g.ListBoxIndex, beforeClickIndex);

            //viewMock.SetupProperty(f => f.ListBoxCount, 20); --Cant set this property because only has get property in interface
            //viewMock.Setup(g => g.ListBoxIndex).Returns(1);  --This will force the property to return 1 regardless of what actual implementation is

            //viewMock.Setup(g => g.ListBoxIndex).Returns(2);  --This will force property to return 2 regradless of actual implementation in inspection presenter
           This is another test. Written from christian's computer

            //Act
            systemUnderTest.GotToNextPart();

            //Assert
            Assert.AreEqual(afterClickIndex, viewMock.Object.ListBoxIndex);

            viewMock.VerifyAll();
            modelMock.VerifyAll();

        }
        */

        [Test]
        public void OpKeyEntered_NonValidOpKey_InvalidOpKeyProcessExecuted()
        {
            //Arrange
            viewMock.Setup(foo => foo.OpKey).Returns(1);
            modelMock.Setup(f => f.GetInfoFromOpKeyEntry(1)).Returns(jobInfo);
            viewMock.Setup(foo => foo.InvalidOpKeyProcess()).Callback(() => Console.WriteLine("InvalidOpKey Process Called"));

            //Act
            systemUnderTest.OpKeyEntered();

            //Assert
            viewMock.Verify(foo => foo.InvalidOpKeyProcess(), Times.AtLeastOnce());

        }

        [Test]
        public void OpKeyEntered_ValidOpKeyNoInspectionNoFeatures_AlertNoFeatures()
        {
            //Arrange
            jobInfo.Rows.Add("386022", "15516", "150", "0");
            viewMock.Setup(f => f.SetJobInfoView(jobInfo)).Callback(() => Console.WriteLine("SetJobInfoView Called"));
            viewMock.Setup(foo => foo.OpKey).Returns(26266);
            viewMock.Setup(f => f.AlertNoFeatures()).Callback(() => Console.WriteLine("No Features. AlertNoFeatures Called"));
            viewMock.Setup(f => f.SmallInspectionPageClear()).Callback(() => Console.WriteLine("SmallInspectionPageClear Called"));

            modelMock.Setup(f => f.GetInfoFromOpKeyEntry(26266)).Returns(jobInfo);//Will allow for a true ValidOpkey
            modelMock.Setup(f => f.GetInspectionExistsOnOpKey(26266)).Returns(false);
            modelMock.Setup(f => f.FeaturesExist(26266)).Returns(false);

            //Act
            systemUnderTest.OpKeyEntered();

            //Assert
            viewMock.Verify(f => f.SmallInspectionPageClear(), Times.Once);
            viewMock.Verify(foo => foo.AlertNoFeatures(), Times.Once);

        }

        


        
        


    }
}
