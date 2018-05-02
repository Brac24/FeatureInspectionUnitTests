using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using FeatureInspection;
using System.Data;

namespace Feature_Inspection.UnitTests
{
	[TestFixture]
	class FinalInspectionPresenterTests
	{
		private Mock<IFinalInspection> mockView;
		private Mock<IFinalInspectionModel> mockModel;

		private FinalInspectionPresenter sut;
		private FinalInspectionModel _modelImpl;

		string jobNumber;
		private DataTable jobInfo;

		[SetUp]
		public void SetUp()
		{

			mockView = new Mock<IFinalInspection>(MockBehavior.Strict);
			mockModel = new Mock<IFinalInspectionModel>(MockBehavior.Strict);

			sut = new FinalInspectionPresenter(mockView.Object, mockModel.Object);

			jobNumber = "2123FLKDL";

			jobInfo = new DataTable();

		}

		[Test]
		public void jobTextBoxKeyDown_ValidJobNumber_SetJobProperty()
		{
			//Arrange
			mockModel.Setup(f => f.JobNumberExists(jobNumber)).Returns(true);
			mockModel.Setup(f => f.GetJobInfo(jobNumber)).Returns(jobInfo); //Returns empty table be do not care for this test
			mockView.Setup(f => f.SetJobInfo(jobInfo));
	
			//Act
			sut.jobTextBoxKeyDown(jobNumber);

			//Assert
			Assert.AreEqual(jobNumber, sut.JobNumber);
		}

		[Test]
		public void jobTextBoxKeyDown_InvalidJobNumber_JobNumberEmpty_ClearInfo_ShowInvalidDialogBox()
		{
			//Arrange
			mockModel.Setup(f => f.JobNumberExists(jobNumber)).Returns(false);
			mockView.Setup(f => f.ClearPage_JobNumberDoesNotExist()).Verifiable();

			//Act
			sut.jobTextBoxKeyDown(jobNumber);

			//Assert
			Assert.IsEmpty(sut.JobNumber);

		}

		[Test]
		public void IntegrationTest_jobTextBoxKeyDown_ValidJobNumber_GetAndSetJobInfoView()
		{
			//Arrange
			jobNumber = "30715";
			_modelImpl = new FinalInspectionModel(); //This essentially makes it an integration test because we call the server 
			mockModel.Setup(f => f.JobNumberExists(jobNumber)).Returns(true);
			mockModel.Setup(f => f.GetJobInfo(jobNumber)).Returns(_modelImpl.GetJobInfo(jobNumber)).Verifiable(); //Accessing DB
			mockView.Setup(f => f.SetJobInfo(mockModel.Object.GetJobInfo(jobNumber))).Verifiable();



			//Act 
			sut.jobTextBoxKeyDown(jobNumber);
			mockModel.Verify(f => f.GetJobInfo(jobNumber));
			mockView.Verify(f => f.SetJobInfo(mockModel.Object.GetJobInfo(jobNumber)));

			//Assert
			Assert.AreEqual(jobNumber, sut.JobNumber);

		}


		[Test]
		public void SetJobProperty_InvalidJobNumber_JobNumberPropertyNotSet()
		{
			string jobNumber = "2123FLKDL";

			//Arrange
			mockModel.Setup(f => f.JobNumberExists(jobNumber)).Returns(false);

			//Act
			bool response = sut.SetJobProperty(jobNumber);

			//Assert
			Assert.IsEmpty(sut.JobNumber);
			Assert.IsFalse(response);

		}

		[Test]
		public void SetJobProperty_ValidJobNumber_SetJobInfoCalled_JobNumberPropertySet()
		{
			mockModel.Setup(f => f.JobNumberExists(jobNumber)).Returns(true);

			bool response = sut.SetJobProperty(jobNumber);

			Assert.AreEqual(jobNumber, sut.JobNumber);
			Assert.IsTrue(response);
		}

		[TestCase("ASF4515")]
		public void IntegrationTest_SetJobProperty_InvalidJobNumber_JobPropertyEmpty(string jobNumber)
		{
			//Arrange
			_modelImpl = new FinalInspectionModel();
			mockModel.Setup(f => f.JobNumberExists(jobNumber)).Returns(_modelImpl.JobNumberExists(jobNumber));

			//Act
			sut.SetJobProperty(jobNumber);

			//Assert
			Assert.IsEmpty(sut.JobNumber);
		}

		


	}
}
